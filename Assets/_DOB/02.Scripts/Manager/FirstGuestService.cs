// FirstGuestService — 공용 Gameplay의 입력·손님·응대를 Restaurant의 자리와 연결한다.
// 의사코드: 식당이 준비되면 입구에서 응대 위치까지 손님을 보낸다. WASD로 이동하고 가까이서 E를 누르면
// 플레이어 위치와 무관하게 손님 얼굴 정면으로 카메라를 천천히 줌인하고, 줌인이 끝난 뒤에 응대를 연다.
// 응대가 닫히면 전경으로 줌아웃한다. 줌인·응대·줌아웃·전환 동안 이동과 E를 잠근다.
// 수락 시 따라오고 빈 자리 가까이서 E를 누르면 자리를 선점하고 의자로 걷는다.
// 착석한 방문을 한 번 알린다. 돌려보내기 시 문으로 돌아간다. 씬 종료 시 좌석·카메라·화면을 정리한다.
using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DOB.Manager
{
    /// <summary>한 방문의 공간 이동과 입력을 담당한다. 손님 표시 데이터는 응대 Presenter가 소유한다.</summary>
    public sealed class FirstGuestService : MonoBehaviour
    {
        private enum Stage { Entering, Waiting, Following, ApproachingSeat, Seated, ReturningHome, SentHome, ToDungeon, InDungeon }

        [Header("Gameplay 씬 연결")]
        [SerializeField] private Transform player;
        [SerializeField] private Transform guest;
        [SerializeField] private Camera worldCamera;
        [SerializeField] private Text guidance;
        [SerializeField] private FirstGuestDialogue dialogue;

        [Header("그레이박스 이동 조정")]
        [Min(0.1f), SerializeField] private float guestSpeed = 3f;
        [Min(0.1f), SerializeField] private float interactionRange = 1.35f;
        [Min(0.1f), SerializeField] private float followDistance = 0.8f;

        /// <summary>전경과 얼굴 클로즈업 사이를 오가는 블렌드 시간(초).</summary>
        [Header("응대 줌인 조정 (가안)")]
        [Min(0.05f), SerializeField] private float zoomSeconds = 0.6f;
        /// <summary>클로즈업의 직교 크기(화면 세로 절반, 미터). 전경 카메라가 원근이면 쓰이지 않는다.</summary>
        [Min(0.05f), SerializeField] private float closeUpSize = 0.45f;
        /// <summary>손님 얼굴에서 클로즈업 카메라까지의 거리(미터).</summary>
        [Min(0.05f), SerializeField] private float closeUpDistance = 0.5f;

        private CharacterController controller;
        private FirstGuestControls controls;
        private PlayerMovementController movement;
        private Transform guestFace;
        private CinemachineCamera closeUp;
        private float zoomTimer;
        private bool pendingOpen;
        private RestaurantLayout layout;
        private Scene restaurantScene;
        private Scene uiScene;
        private int seatIndex = -1;
        private Stage stage;
        private string guestId;
        private bool observing;

        /// <summary>매 영업 진입 시 새로 부여하는 첫 손님의 방문 ID.</summary>
        public string VisitId { get; private set; }
        /// <summary>입장을 마쳐 손님 가까이에서 응대를 열 수 있는 상태.</summary>
        public bool IsWaitingToGreet => layout && stage == Stage.Waiting;
        /// <summary>손님이 실제로 배정된 의자에 도착했는지 확인한다.</summary>
        public bool IsSeated => layout && stage == Stage.Seated;
        /// <summary>착석 완료한 방문의 좌석 기준점.</summary>
        public Transform SeatedAt => IsSeated ? layout.GetSeat(seatIndex) : null;
        /// <summary>착석이 완료될 때 방문 ID와 실제 좌석을 한 번 전달한다.</summary>
        public event Action<string, Transform> GuestSeated;
        /// <summary>같은 Restaurant 씬의 주방 상호작용 기준점.</summary>
        public Transform Kitchen => layout ? layout.Kitchen : null;
        /// <summary>착석 후의 E 입력 처리. 주방·서빙 담당이 성공 여부를 반환한다.</summary>
        public Func<bool> MealInteraction { get; set; }
        /// <summary>현재 조리·서빙 단계에 맞는 안내 문구를 조회한다.</summary>
        public Func<string> MealGuidance { get; set; }
        /// <summary>주방 메뉴가 열려 있으면 플레이어와 손님 이동을 멈춘다.</summary>
        public Func<bool> KitchenIsOpen { get; set; }

        /// <summary>BasementUI가 준비되면 조합 담당이 서비스 활성화 전에 HUD와 응대 Presenter를 전달한다.</summary>
        public void ConnectHud(Text label, FirstGuestDialogue presenter)
        {
            guidance = label;
            dialogue = presenter;
        }

        private void OnEnable()
        {
            if (!player || !guest || !worldCamera || !guidance || !dialogue)
            {
                Debug.LogError("[DOB] 첫 손님 응대에 필요한 Gameplay 연결 또는 프로젝트 입력이 없습니다.", this);
                enabled = false;
                return;
            }

            controller = player.GetComponent<CharacterController>();
            controls = player.GetComponent<FirstGuestControls>();
            movement = player.GetComponent<PlayerMovementController>();
            // 공용 캐릭터 프리팹의 모델 교체와 독립적인 앵커 계약이다.
            guestFace = guest.Find("Anchors/FacePoint");
            if (!controller || !controls || !movement || !guestFace)
            {
                Debug.LogError("[DOB] 캐릭터 프리팹의 이동·입력 컴포넌트와 FacePoint를 확인하세요.", this);
                enabled = false;
                return;
            }
            BuildCameraRig();

            dialogue.BindActions(RequestFollow, RequestSendHome);
            uiScene = dialogue.gameObject.scene;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            if (!TryBindRestaurant(gameObject.scene)
                && !TryBindRestaurant(SceneManager.GetSceneByName(BasementComposition.RestaurantScene)))
                TryBindRestaurant(SceneManager.GetSceneByName(SceneComposition.Restaurant));
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            if (dialogue)
                dialogue.BindActions(null, null);
            EndVisit();
            uiScene = default;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene == gameObject.scene || scene.name == BasementComposition.RestaurantScene || scene.name == SceneComposition.Restaurant)
                TryBindRestaurant(scene);
        }

        private void OnSceneUnloaded(Scene scene)
        {
            if (scene == uiScene)
                enabled = false;
            else if (scene == restaurantScene)
                EndVisit();
        }

        private bool TryBindRestaurant(Scene scene)
        {
            if (!scene.IsValid() || !scene.isLoaded || VisitId != null)
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

                layout = candidate;
                restaurantScene = scene;
                VisitId = Guid.NewGuid().ToString("N");
                guestId = dialogue.FirstGuestId;
                stage = Stage.Entering;
                controller.enabled = false;
                player.SetPositionAndRotation(layout.PlayerSpawn.position, layout.PlayerSpawn.rotation);
                controller.enabled = true;
                movement.EnterRestaurant(worldCamera, layout, CanMovePlayer);
                guest.SetPositionAndRotation(layout.GuestEntrance.position, layout.GuestEntrance.rotation);
                guest.gameObject.SetActive(true);
                return true;
            }
            return false;
        }

        private void EndVisit()
        {
            if (movement)
                movement.LeaveRestaurant();
            if (VisitId != null && dialogue)
                dialogue.CancelDialogue();
            if (layout)
                layout.Release(seatIndex, VisitId);
            RestoreCamera();
            zoomTimer = 0f;
            pendingOpen = false;
            if (guidance)
                guidance.text = string.Empty;
            layout = null;
            VisitId = null;
            guestId = null;
            seatIndex = -1;
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
                    // 줌인을 마친 뒤에야 응대 화면을 연다. 열 수 없으면 전경으로 되돌린다.
                    if (!dialogue.TryOpenGuest(guestId))
                        RestoreCamera();
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
                RestoreCamera();
                return;
            }
            AdvanceGuest();
            RefreshGuidance();

            if (controls.InteractPressed)
                TryInteract();
        }

        // 이동 방향이 카메라 정면 기준이라 카메라가 블렌드되는 동안에도 이동을 막는다.
        private bool CanMovePlayer() =>
            layout && !SceneLoadService.IsLoading && zoomTimer <= 0f && !observing && !dialogue.BlocksWorldInput
            && KitchenIsOpen?.Invoke() != true;

        private void AdvanceGuest()
        {
            switch (stage)
            {
                case Stage.Entering:
                    if (WalkGuest(layout.GreetingPoint.position))
                        stage = Stage.Waiting;
                    break;
                case Stage.Following:
                    var offset = guest.position - player.position;
                    offset.y = 0f;
                    if (offset.magnitude > followDistance)
                        WalkGuest(player.position + offset.normalized * followDistance);
                    break;
                case Stage.ApproachingSeat:
                    var seat = layout.GetSeat(seatIndex);
                    if (!seat)
                    {
                        layout.Release(seatIndex, VisitId);
                        seatIndex = -1;
                        stage = Stage.Following;
                    }
                    else if (WalkGuest(seat.position))
                    {
                        guest.rotation = seat.rotation;
                        stage = Stage.Seated;
                        GuestSeated?.Invoke(VisitId, seat);
                    }
                    break;
                case Stage.ReturningHome:
                    if (WalkGuest(layout.GuestEntrance.position))
                    {
                        stage = Stage.SentHome;
                        guest.gameObject.SetActive(false);
                    }
                    break;
                case Stage.ToDungeon:
                    if (WalkGuest(layout.StairsExit.position))
                    {
                        stage = Stage.InDungeon;
                        guest.gameObject.SetActive(false);
                    }
                    break;
            }
        }

        private bool WalkGuest(Vector3 target)
        {
            var direction = target - guest.position;
            direction.y = 0f;
            if (direction.sqrMagnitude > 0.0001f)
                guest.rotation = Quaternion.LookRotation(direction);
            guest.position = Vector3.MoveTowards(guest.position, target, guestSpeed * Time.deltaTime);
            return guest.position == target;
        }

        /// <summary>E와 같은 진입점. 대기 손님이나 안내 중인 빈 자리 근처에서만 성공한다.</summary>
        public bool TryInteract()
        {
            if (!isActiveAndEnabled || !layout || Time.timeScale <= 0f || SceneLoadService.IsLoading || dialogue.BlocksWorldInput
                || zoomTimer > 0f || observing || KitchenIsOpen?.Invoke() == true)
                return false;
            if (stage == Stage.Seated)
                return MealInteraction?.Invoke() == true;
            if (stage == Stage.Waiting && Near(player, guest, interactionRange))
            {
                ObserveGuest();
                guidance.text = string.Empty;
                return true;
            }
            if (stage != Stage.Following)
                return false;

            int nearest = layout.FindNearbySeat(player.position, interactionRange);
            var seat = layout.GetSeat(nearest);
            if (!seat || !Near(guest, seat, interactionRange + followDistance) || !layout.TryAssign(nearest, VisitId))
                return false;
            seatIndex = nearest;
            stage = Stage.ApproachingSeat;
            return true;
        }

        private string RequestFollow(string id)
        {
            if (!CanRespond(id))
                return "지금은 이 손님을 안내할 수 없어요.";
            if (!layout.HasFreeSeat())
                return "지금은 빈 자리가 없어요.";
            stage = Stage.Following;
            return null;
        }

        private string RequestSendHome(string id)
        {
            if (!CanRespond(id))
                return "지금은 이 손님을 돌려보낼 수 없어요.";
            stage = Stage.ReturningHome;
            return null;
        }

        /// <summary>현재 방문의 서빙 성공에만 응답해 좌석을 해제하고 손님을 계단으로 보낸다.</summary>
        public bool TryDepartForDungeon(string visitId)
        {
            if (!IsSeated || VisitId != visitId || !layout.StairsExit)
                return false;
            layout.Release(seatIndex, VisitId);
            seatIndex = -1;
            stage = Stage.ToDungeon;
            return true;
        }

        /// <summary>현재 플레이어가 지정된 주방·좌석의 월드 상호작용 반경에 있는지 검사한다.</summary>
        public bool IsNearPlayer(Transform target) => layout && target && Near(player, target, interactionRange);

        private bool CanRespond(string id) => layout && Time.timeScale > 0f && !SceneLoadService.IsLoading
            && stage == Stage.Waiting && !string.IsNullOrEmpty(id) && id == guestId && Near(player, guest, interactionRange);

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
            brain.DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.EaseInOut, zoomSeconds);

            // 전경은 지금의 메인 카메라 그대로다. 평소 화면이 달라지지 않는다.
            var lens = LensSettings.FromCamera(worldCamera);
            var overview = NewCamera("OverviewCamera", lens);
            overview.transform.SetPositionAndRotation(worldCamera.transform.position, worldCamera.transform.rotation);

            // 직교↔원근은 부드럽게 블렌드되지 않으므로 투영 방식은 전경과 같게 두고 크기·위치만 좁힌다.
            lens.OrthographicSize = closeUpSize;
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
            // 플레이어가 선 곳과 무관하게 항상 손님 얼굴 정면에서 바라본다.
            closeUp.transform.SetPositionAndRotation(
                guestFace.position + guest.forward * closeUpDistance, Quaternion.LookRotation(-guest.forward));
            // 우선순위가 같으면 나중에 켜진 카메라가 선택되어 Brain이 전경에서 블렌드한다.
            closeUp.gameObject.SetActive(true);
            observing = true;
            pendingOpen = true;
            zoomTimer = zoomSeconds;
        }

        private void RestoreCamera()
        {
            if (!observing)
                return;
            if (closeUp)
                closeUp.gameObject.SetActive(false);
            observing = false;
            pendingOpen = false;
            zoomTimer = zoomSeconds;
        }

        private void RefreshGuidance()
        {
            switch (stage)
            {
                case Stage.Entering:
                case Stage.Waiting:
                    guidance.text = stage == Stage.Waiting && Near(player, guest, interactionRange)
                        ? "E를 눌러 손님을 맞이하세요."
                        : "WASD로 움직여 입구의 손님에게 다가가세요.";
                    break;
                case Stage.Following:
                    int nearest = layout.FindNearbySeat(player.position, interactionRange);
                    var seat = layout.GetSeat(nearest);
                    if (!seat)
                        guidance.text = "손님을 빈 자리로 안내하세요.";
                    else if (layout.IsOccupied(nearest))
                        guidance.text = "이미 사용 중인 자리예요. 빈 자리로 안내하세요.";
                    else if (!Near(guest, seat, interactionRange + followDistance))
                        guidance.text = "손님이 따라올 때까지 기다리세요.";
                    else
                        guidance.text = $"E를 눌러 {nearest + 1}번 자리에 손님을 안내하세요.";
                    break;
                case Stage.ApproachingSeat:
                    guidance.text = "손님이 자리로 가고 있어요.";
                    break;
                case Stage.Seated:
                    string mealHint = MealGuidance?.Invoke();
                    guidance.text = !string.IsNullOrEmpty(mealHint) ? mealHint : $"손님이 {seatIndex + 1}번 자리에 앉았어요.";
                    break;
                case Stage.ReturningHome:
                case Stage.SentHome:
                    guidance.text = "손님을 돌려보냈어요.";
                    break;
                case Stage.ToDungeon:
                    guidance.text = "식사를 마친 손님이 던전으로 내려가고 있어요.";
                    break;
                case Stage.InDungeon:
                    guidance.text = "손님이 던전으로 떠났어요.";
                    break;
            }
        }
    }
}
