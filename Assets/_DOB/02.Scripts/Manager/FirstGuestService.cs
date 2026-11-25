// FirstGuestService — 공용 Gameplay의 입력·카메라·응대를 Restaurant의 자리와 그날 손님들(GuestManager)에 연결한다.
// 호출자: GameManager(ConnectHud, 영업 단계에서 켜고 끔), FirstGuestKitchen(식사 상호작용·안내 위임, 좌석 조회).
// 호출  : GuestManager(명단 열기·닫기, 방문 단계 요청), FirstGuestDialogue(응대 화면), RestaurantLayout(좌석·기준점),
//         PlayerMovementController(이동 잠금), Cinemachine(클로즈업).
// 상태  : 식당 배치, 응대 중인 손님, 따라오는 손님(한 번에 하나), 카메라 줌 타이머, 마지막 퇴장 안내.
// 의사코드:
//   식당이 준비되면 GuestManager로 그날 명단을(GameManager가 넘긴 입소문 추가 인원 포함) 열어 첫 손님을 입구에 세운다.
//   이후 입장은 GameManager의 Tick이 만든다.
//   매 프레임 손님마다 목표를 정해 걷게 한다: 입구 대기는 줄 위치(GreetingPoint에서 입구 쪽으로 줄 간격씩 뒤),
//   따라오기는 플레이어 뒤, 좌석 안내는 의자, 끝난 방문은 문(돌려보냄·대기 이탈) 또는 계단(파견).
//   E: ① 따라오는 손님이 빈 자리 가까이 있으면 자리를 선점하고 의자로 보낸다. ② 줄에 선 가장 가까운 손님을 명단에서
//   응대로 바꾸고, 그 얼굴 정면으로 줌인한 뒤 응대를 연다. ③ 그 밖은 주방(MealInteraction)에 맡긴다.
//   응대가 안내·돌려보냄 없이 닫히면 그 손님을 입구 대기로 되돌리고 전경으로 줌아웃한다. 줌·응대·전환 동안 이동과 E를 잠근다.
//   동행은 다른 손님이 따라오는 중이면 거절한다. 착석하면 명단에 알리고 GuestSeated를 한 번 보낸다.
//   명단이 방문을 끝내면(서빙 후 파견·돌려보냄·대기 이탈) 좌석을 풀고 계단 또는 문으로 보낸다.
//   씬 종료 시 좌석·카메라·화면·손님 오브젝트를 정리한다.
// 단일 손님 조회(VisitId·CurrentVisit·IsSeated 등)는 아직 끝나지 않은 가장 먼저 입장한 손님 기준이다.
using System;
using DOB.Define;
using DOB.Systems;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DOB.Manager
{
    /// <summary>손님들의 공간 이동과 플레이어 입력을 담당한다. 방문 단계는 명단이, 손님 표시 데이터는 응대 Presenter가 소유한다.</summary>
    public sealed class FirstGuestService : MonoBehaviour
    {
        [Header("Gameplay 씬 연결")]
        [SerializeField] private Transform player;
        /// <summary>그날 명단과 손님 오브젝트를 소유하는 손님 총괄.</summary>
        [SerializeField] private GuestManager guests;
        [SerializeField] private Camera worldCamera;
        [SerializeField] private Text guidance;
        [SerializeField] private FirstGuestDialogue dialogue;

        [Header("그레이박스 이동 조정")]
        [Min(0.1f), SerializeField] private float guestSpeed = 3f;
        [Min(0.1f), SerializeField] private float interactionRange = 1.35f;
        [Min(0.1f), SerializeField] private float followDistance = 0.8f;

        /// <summary>응대 줌인의 시간과 클로즈업 크기·거리 기준값(08.Data).</summary>
        [Header("기획 데이터")]
        [SerializeField] private GuestCloseUpSettings zoom;

        private CharacterController controller;
        private FirstGuestControls controls;
        private PlayerMovementController movement;
        private CinemachineCamera closeUp;
        private float zoomTimer;
        private bool pendingOpen;
        private RestaurantLayout layout;
        private Scene restaurantScene;
        private Scene uiScene;
        private GuestAgent greeting;
        private GuestAgent following;
        private string notice;
        private bool observing;
        private int serviceDay;
        // 그 일차의 입소문 추가 인원. 명단을 열 때 GuestManager에 넘긴다.
        private int extraGuests;

        private bool CanSendHome => serviceDay > 1 || dialogue.FirstGuestCanSendHome;
        private ServiceRoster Roster => guests.Roster;
        /// <summary>그날 명단과 손님 오브젝트.</summary>
        public GuestManager Guests => guests;
        // 아직 끝나지 않은 가장 먼저 입장한 손님. 모두 끝났으면 마지막 손님.
        private GuestAgent Primary
        {
            get
            {
                if (!layout || Roster == null) return null;
                foreach (var agent in guests.Agents)
                    if (Roster.Find(agent.VisitId)?.Stage != ServiceVisitStage.Ended)
                        return agent;
                return guests.Agents.Count > 0 ? guests.Agents[guests.Agents.Count - 1] : null;
            }
        }
        /// <summary>단일 손님 조회: 기준 손님의 방문 원고.</summary>
        public GuestDialogueVisit CurrentVisit => Primary?.Visit;
        /// <summary>단일 손님 조회: 기준 손님의 방문 ID. 영업 전에는 null.</summary>
        public string VisitId => Primary?.VisitId;
        /// <summary>단일 손님 조회: 기준 손님의 고정 ID. 방명록·호감은 이 값으로 구분한다.</summary>
        public string GuestId => Primary?.GuestId;
        /// <summary>단일 손님 조회: 기준 손님이 줄에 서서 응대를 열 수 있다.</summary>
        public bool IsWaitingToGreet => Primary is GuestAgent a && a.Move == GuestAgent.Movement.Queued
            && Roster.Find(a.VisitId).Stage == ServiceVisitStage.AtEntrance;
        /// <summary>단일 손님 조회: 기준 손님이 배정된 의자에 도착했다.</summary>
        public bool IsSeated => Primary?.Move == GuestAgent.Movement.Seated;
        /// <summary>단일 손님 조회: 기준 손님의 좌석 기준점.</summary>
        public Transform SeatedAt => IsSeated ? layout.GetSeat(Primary.SeatIndex) : null;
        /// <summary>착석이 완료될 때 방문 ID와 실제 좌석을 한 번 전달한다.</summary>
        public event Action<string, Transform> GuestSeated;
        /// <summary>같은 Restaurant 씬의 주방 상호작용 기준점.</summary>
        public Transform Kitchen => layout ? layout.Kitchen : null;
        /// <summary>맞이·좌석 안내가 아닌 E 입력 처리. 주방·서빙 담당이 성공 여부를 반환한다.</summary>
        public Func<bool> MealInteraction { get; set; }
        /// <summary>현재 조리·서빙 단계에 맞는 안내 문구를 조회한다.</summary>
        public Func<string> MealGuidance { get; set; }
        /// <summary>주방 메뉴가 열려 있으면 플레이어와 손님 이동을 멈춘다.</summary>
        public Func<bool> KitchenIsOpen { get; set; }

        /// <summary>BasementUI가 준비되면 GameManager가 서비스 활성화 전에 HUD와 응대 Presenter, 일차, 그 일차의 입소문 추가 인원을 전달한다.</summary>
        public void ConnectHud(Text label, FirstGuestDialogue presenter, int day, int wordOfMouthBonus = 0)
        {
            guidance = label;
            dialogue = presenter;
            serviceDay = day;
            extraGuests = wordOfMouthBonus;
        }

        /// <summary>그 방문이 의자에 앉아 있으면 좌석 기준점, 아니면 null.</summary>
        public Transform SeatOf(string visitId) =>
            guests.Find(visitId) is GuestAgent a && a.Move == GuestAgent.Movement.Seated ? layout.GetSeat(a.SeatIndex) : null;

        /// <summary>플레이어 상호작용 반경 안의 가장 가까운 착석 손님의 방문 ID. 없으면 null.</summary>
        public string NearestSeatedVisit()
        {
            GuestAgent nearest = null;
            float best = float.MaxValue;
            foreach (var agent in guests.Agents)
            {
                var seat = SeatOf(agent.VisitId);
                if (!seat || !Near(player, seat, interactionRange)) continue;
                float distance = (seat.position - player.position).sqrMagnitude;
                if (distance < best) { best = distance; nearest = agent; }
            }
            return nearest ? nearest.VisitId : null;
        }

        private void OnEnable()
        {
            if (!player || !guests || !worldCamera || !guidance || !dialogue || !zoom)
            {
                Debug.LogError("[DOB] 손님 응대에 필요한 Gameplay 연결, 클로즈업 데이터 또는 프로젝트 입력이 없습니다.", this);
                enabled = false;
                return;
            }

            controller = player.GetComponent<CharacterController>();
            controls = player.GetComponent<FirstGuestControls>();
            movement = player.GetComponent<PlayerMovementController>();
            if (!controller || !controls || !movement)
            {
                Debug.LogError("[DOB] 플레이어 프리팹의 이동·입력 컴포넌트를 확인하세요.", this);
                enabled = false;
                return;
            }
            BuildCameraRig();

            dialogue.BindActions(RequestFollow, RequestSendHome);
            uiScene = dialogue.gameObject.scene;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            if (!TryBindRestaurant(gameObject.scene))
                TryBindRestaurant(SceneManager.GetSceneByName(BasementComposition.RestaurantScene));
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            if (dialogue)
                dialogue.BindActions(null, null);
            EndService();
            uiScene = default;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene == gameObject.scene || scene.name == BasementComposition.RestaurantScene)
                TryBindRestaurant(scene);
        }

        private void OnSceneUnloaded(Scene scene)
        {
            if (scene == uiScene)
                enabled = false;
            else if (scene == restaurantScene)
                EndService();
        }

        private bool TryBindRestaurant(Scene scene)
        {
            if (!scene.IsValid() || !scene.isLoaded || layout)
                return false;

            // 씬을 가로지르는 참조는 직렬화할 수 없으므로 지정된 씬의 루트 계약만 한 번 해석한다.
            foreach (var root in scene.GetRootGameObjects())
            {
                if (!root.TryGetComponent<RestaurantLayout>(out var candidate))
                    continue;
                if (!candidate.IsReady)
                {
                    Debug.LogError("[DOB] RestaurantLayout의 배치 기준점·바닥 연결을 확인하세요.", candidate);
                    return false;
                }
                if (!guests.BeginService(serviceDay, candidate.GuestEntrance, extraGuests))
                    return false;

                guests.VisitEnded += OnVisitEnded;
                layout = candidate;
                restaurantScene = scene;
                notice = null;
                controller.enabled = false;
                player.SetPositionAndRotation(layout.PlayerSpawn.position, layout.PlayerSpawn.rotation);
                controller.enabled = true;
                movement.EnterRestaurant(worldCamera, layout, CanMovePlayer);
                return true;
            }
            return false;
        }

        private void EndService()
        {
            if (movement)
                movement.LeaveRestaurant();
            if (layout && dialogue)
                dialogue.CancelDialogue();
            if (guests)
            {
                guests.VisitEnded -= OnVisitEnded;
                if (layout)
                    foreach (var agent in guests.Agents)
                        if (agent) layout.Release(agent.SeatIndex, agent.VisitId);
                guests.EndService();
            }
            RestoreCamera();
            zoomTimer = 0f;
            pendingOpen = false;
            if (guidance)
                guidance.text = string.Empty;
            layout = null;
            greeting = null;
            following = null;
            restaurantScene = default;
        }

        private void Update()
        {
            if (!layout || Time.timeScale <= 0f || SceneLoadService.IsLoading)
                return;
            if (zoomTimer > 0f)
            {
                zoomTimer -= Time.deltaTime;
                guidance.text = string.Empty;
                if (zoomTimer > 0f)
                    return;
                if (pendingOpen)
                {
                    pendingOpen = false;
                    // 줌인을 마친 뒤에야 응대 화면을 연다. 열 수 없으면 손님을 줄로 되돌리고 전경으로 돌아간다.
                    if (!dialogue.TryOpenVisit(greeting.Visit, CanSendHome))
                    {
                        EndGreeting();
                        RestoreCamera();
                    }
                    return;
                }
            }
            if (dialogue.BlocksWorldInput || KitchenIsOpen?.Invoke() == true)
            {
                guidance.text = string.Empty;
                return;
            }
            if (observing)
            {
                // 응대가 닫혔다. 줌아웃이 끝날 때까지 한 번 더 잠근다.
                EndGreeting();
                RestoreCamera();
                return;
            }
            AdvanceGuests(Time.deltaTime);
            RefreshGuidance();

            if (controls.InteractPressed)
                TryInteract();
        }

        // 안내·돌려보냄 없이 닫힌 응대면 명단이 그 손님을 입구 대기로 되돌린다. 이미 다른 단계면 거절되어 그대로다.
        private void EndGreeting()
        {
            if (greeting)
                Roster?.TryCancelGreeting(greeting.VisitId);
            greeting = null;
        }

        // 이동 방향이 카메라 정면 기준이라 카메라가 블렌드되는 동안에도 이동을 막는다.
        private bool CanMovePlayer() =>
            layout && !SceneLoadService.IsLoading && zoomTimer <= 0f && !observing && !dialogue.BlocksWorldInput
            && KitchenIsOpen?.Invoke() != true;

        private void AdvanceGuests(float dt)
        {
            int queueIndex = 0;
            foreach (var agent in guests.Agents)
            {
                switch (agent.Move)
                {
                    case GuestAgent.Movement.Entering:
                    case GuestAgent.Movement.Queued:
                        bool arrived = agent.WalkTo(QueuePosition(queueIndex++), guestSpeed, dt);
                        agent.Move = arrived ? GuestAgent.Movement.Queued : GuestAgent.Movement.Entering;
                        break;
                    case GuestAgent.Movement.Following:
                        var offset = agent.transform.position - player.position;
                        offset.y = 0f;
                        if (offset.magnitude > followDistance)
                            agent.WalkTo(player.position + offset.normalized * followDistance, guestSpeed, dt);
                        break;
                    case GuestAgent.Movement.ToSeat:
                        var seat = layout.GetSeat(agent.SeatIndex);
                        if (!seat)
                        {
                            layout.Release(agent.SeatIndex, agent.VisitId);
                            agent.SeatIndex = -1;
                            agent.Move = GuestAgent.Movement.Following;
                            if (!following) following = agent;
                        }
                        else if (agent.WalkTo(seat.position, guestSpeed, dt))
                        {
                            agent.transform.rotation = seat.rotation;
                            agent.Move = GuestAgent.Movement.Seated;
                            Roster.TrySeat(agent.VisitId);
                            GuestSeated?.Invoke(agent.VisitId, seat);
                        }
                        break;
                    case GuestAgent.Movement.ToExit:
                        if (agent.WalkTo(layout.GuestEntrance.position, guestSpeed, dt))
                            Hide(agent);
                        break;
                    case GuestAgent.Movement.ToStairs:
                        if (agent.WalkTo(layout.StairsExit.position, guestSpeed, dt))
                            Hide(agent);
                        break;
                }
            }
        }

        private static void Hide(GuestAgent agent)
        {
            agent.Move = GuestAgent.Movement.Gone;
            agent.gameObject.SetActive(false);
        }

        // shortcut: 줄 위치를 GreetingPoint에서 입구 쪽으로 줄 간격씩 뒤로 계산한다. Restaurant에 줄 기준점이 생기면 그 Transform으로 바꾼다.
        private Vector3 QueuePosition(int index)
        {
            var front = layout.GreetingPoint.position;
            var back = layout.GuestEntrance.position - front;
            back.y = 0f;
            return index == 0 || back.sqrMagnitude < 0.0001f ? front : front + back.normalized * (guests.QueueSpacing * index);
        }

        /// <summary>E와 같은 진입점. 좌석 안내·손님 맞이·주방 상호작용 중 하나가 성공하면 true.</summary>
        public bool TryInteract()
        {
            if (!isActiveAndEnabled || !layout || Time.timeScale <= 0f || SceneLoadService.IsLoading || dialogue.BlocksWorldInput
                || zoomTimer > 0f || observing || KitchenIsOpen?.Invoke() == true)
                return false;

            if (following)
            {
                int nearest = layout.FindNearbySeat(player.position, interactionRange);
                var seat = layout.GetSeat(nearest);
                if (seat && Near(following.transform, seat, interactionRange + followDistance) && layout.TryAssign(nearest, following.VisitId))
                {
                    following.SeatIndex = nearest;
                    following.Move = GuestAgent.Movement.ToSeat;
                    following = null;
                    return true;
                }
            }

            var waiting = NearestQueued();
            if (waiting && Roster.TryBeginGreeting(waiting.VisitId))
            {
                greeting = waiting;
                ObserveGuest();
                guidance.text = string.Empty;
                return true;
            }
            return MealInteraction?.Invoke() == true;
        }

        // 줄 위치에 도착해 맞이를 기다리는 손님 중 플레이어에게 가장 가까운 손님.
        private GuestAgent NearestQueued()
        {
            GuestAgent nearest = null;
            float best = float.MaxValue;
            foreach (var agent in guests.Agents)
            {
                if (agent.Move != GuestAgent.Movement.Queued || Roster.Find(agent.VisitId).Stage != ServiceVisitStage.AtEntrance
                    || !Near(player, agent.transform, interactionRange))
                    continue;
                float distance = (agent.transform.position - player.position).sqrMagnitude;
                if (distance < best) { best = distance; nearest = agent; }
            }
            return nearest;
        }

        private string RequestFollow(string id)
        {
            if (!CanRespond(id))
                return "지금은 이 손님을 안내할 수 없어요.";
            if (following)
                return "다른 손님을 먼저 자리로 안내하세요.";
            if (!layout.HasFreeSeat())
                return "지금은 빈 자리가 없어요.";
            if (!Roster.TryGuide(greeting.VisitId))
                return "지금은 이 손님을 안내할 수 없어요.";
            greeting.Move = GuestAgent.Movement.Following;
            following = greeting;
            return null;
        }

        private string RequestSendHome(string id)
        {
            if (!CanRespond(id) || !CanSendHome || !Roster.TryEnd(greeting.VisitId, ServiceVisitEnd.SentHome))
                return "지금은 이 손님을 돌려보낼 수 없어요.";
            return null;
        }

        private void OnVisitEnded(string visitId, ServiceVisitEnd end)
        {
            notice = end == ServiceVisitEnd.Dispatched ? "식사를 마친 손님이 던전으로 내려갔어요."
                : end == ServiceVisitEnd.SentHome ? "손님을 돌려보냈어요." : "기다리다 지친 손님이 떠났어요.";
            if (end == ServiceVisitEnd.Dispatched)
            {
                TryDepartForDungeon(visitId);
                return;
            }
            var agent = guests.Find(visitId);
            if (!agent || agent.Move >= GuestAgent.Movement.ToExit)
                return;
            layout.Release(agent.SeatIndex, visitId);
            agent.SeatIndex = -1;
            if (following == agent) following = null;
            agent.Move = GuestAgent.Movement.ToExit;
        }

        /// <summary>착석한 그 방문의 좌석을 해제하고 계단으로 보낸다. 앉아 있지 않으면 false.</summary>
        public bool TryDepartForDungeon(string visitId)
        {
            var agent = guests.Find(visitId);
            if (!layout || !agent || agent.Move != GuestAgent.Movement.Seated || !layout.StairsExit)
                return false;
            layout.Release(agent.SeatIndex, visitId);
            agent.SeatIndex = -1;
            agent.Move = GuestAgent.Movement.ToStairs;
            return true;
        }

        /// <summary>현재 플레이어가 지정된 주방·좌석의 월드 상호작용 반경에 있는지 검사한다.</summary>
        public bool IsNearPlayer(Transform target) => layout && target && Near(player, target, interactionRange);

        private bool CanRespond(string id) => layout && Time.timeScale > 0f && !SceneLoadService.IsLoading && greeting
            && Roster.Find(greeting.VisitId).Stage == ServiceVisitStage.Greeting && !string.IsNullOrEmpty(id)
            && id == greeting.GuestId && Near(player, greeting.transform, interactionRange);

        private static bool Near(Transform first, Transform second, float range)
        {
            var offset = first.position - second.position;
            return offset.x * offset.x + offset.z * offset.z <= range * range;
        }

        // ponytail: 리그를 런타임에 만들어 씬·프리팹을 건드리지 않는다. 씬에서 샷을 직접 잡아야 하면
        // 두 CinemachineCamera를 씬에 두고 직렬화 참조로 받도록 바꾼다.
        private void BuildCameraRig()
        {
            if (closeUp)
                return;
            if (!worldCamera.TryGetComponent<CinemachineBrain>(out var brain))
                brain = worldCamera.gameObject.AddComponent<CinemachineBrain>();
            brain.DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.EaseInOut, zoom.ZoomSeconds);

            // 전경은 지금의 메인 카메라 그대로다. 평소 화면이 달라지지 않는다.
            var lens = LensSettings.FromCamera(worldCamera);
            var overview = NewCamera("OverviewCamera", lens);
            overview.transform.SetPositionAndRotation(worldCamera.transform.position, worldCamera.transform.rotation);

            // 직교↔원근은 부드럽게 블렌드되지 않으므로 투영 방식은 전경과 같게 두고 크기·위치만 좁힌다.
            lens.OrthographicSize = zoom.OrthographicSize;
            // 얼굴 바로 앞에 서므로 기본 근평면이면 얼굴이 잘린다.
            lens.NearClipPlane = 0.01f;
            closeUp = NewCamera("GuestCloseUpCamera", lens);
            closeUp.gameObject.SetActive(false);
        }

        private CinemachineCamera NewCamera(string cameraName, LensSettings lens)
        {
            var shot = new GameObject(cameraName).AddComponent<CinemachineCamera>();
            shot.transform.SetParent(transform, false);
            shot.Lens = lens;
            return shot;
        }

        private void ObserveGuest()
        {
            // 플레이어가 선 곳과 무관하게 항상 손님 얼굴 정면에서 바라본다. 얼굴 기준점이 없으면 손님 루트를 쓴다.
            var body = greeting.transform;
            var face = greeting.Face ? greeting.Face : body;
            closeUp.transform.SetPositionAndRotation(face.position + body.forward * zoom.Distance, Quaternion.LookRotation(-body.forward));
            // 우선순위가 같으면 나중에 켜진 카메라가 선택되어 Brain이 전경에서 블렌드한다.
            closeUp.gameObject.SetActive(true);
            observing = true;
            pendingOpen = true;
            zoomTimer = zoom.ZoomSeconds;
        }

        private void RestoreCamera()
        {
            if (!observing)
                return;
            if (closeUp)
                closeUp.gameObject.SetActive(false);
            observing = false;
            pendingOpen = false;
            zoomTimer = zoom.ZoomSeconds;
        }

        private void RefreshGuidance()
        {
            if (following)
            {
                int nearest = layout.FindNearbySeat(player.position, interactionRange);
                var seat = layout.GetSeat(nearest);
                if (!seat)
                    guidance.text = "손님을 빈 자리로 안내하세요.";
                else if (layout.IsOccupied(nearest))
                    guidance.text = "이미 사용 중인 자리예요. 빈 자리로 안내하세요.";
                else if (!Near(following.transform, seat, interactionRange + followDistance))
                    guidance.text = "손님이 따라올 때까지 기다리세요.";
                else
                    guidance.text = $"E를 눌러 {nearest + 1}번 자리에 손님을 안내하세요.";
                return;
            }
            if (NearestQueued())
            {
                guidance.text = "E를 눌러 손님을 맞이하세요.";
                return;
            }
            string mealHint = MealGuidance?.Invoke();
            if (!string.IsNullOrEmpty(mealHint))
            {
                guidance.text = mealHint;
                return;
            }
            guidance.text = notice ?? string.Empty;
            foreach (var agent in guests.Agents)
            {
                if (agent.Move <= GuestAgent.Movement.Queued)
                {
                    guidance.text = "WASD로 움직여 입구의 손님에게 다가가세요.";
                    return;
                }
                if (agent.Move == GuestAgent.Movement.ToSeat)
                    guidance.text = "손님이 자리로 가고 있어요.";
                else if (agent.Move == GuestAgent.Movement.Seated)
                    guidance.text = $"손님이 {agent.SeatIndex + 1}번 자리에 앉았어요.";
            }
        }
    }
}
