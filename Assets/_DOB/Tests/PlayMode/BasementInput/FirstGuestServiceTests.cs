// FirstGuestServiceTests — 실제 식당·Gameplay 구성에서 첫 방문에 대한 공간 상호작용을 검증한다.
// 의사코드: 모든 자리를 점유하고 먼 거리와 빈 자리 없는 응대를 거절한다. 하나를 비운 뒤 같은 방문으로 재시도한다.
// 응대 페이드/카메라 복귀, 점유석과 손님의 도착 거리, 정확한 좌석 하나와 한 번의 착석 알림·퇴장을 확인한다.
// PF_Player 단독 검사: 손님 서비스 없이 프리팹만 생성해 PlayerInput·Player 맵·보이는 캡슐과 WASD 이동·정지를 확인한다.
// 키 입력은 PlayerInput 사용자를 테스트 키보드·마우스의 Keyboard&Mouse 스킴에만 짝지어 실제(또는 다른) 키보드가 이동을 가로채지 못하게 한다.
// 장치는 씬·프리팹보다 먼저 SetUp에서 만들고, TearDown에서 생성물·씬·장치를 모두 정리한다.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DOB.Manager;
using NUnit.Framework;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace DOB.Tests
{
    public sealed class FirstGuestServiceTests
    {
        private const string DemoPath = "Assets/_DOB/01.Scenes/Basement.unity";
        private const string PlayerPrefabPath = "Assets/_DOB/03.Prefabs/Characters/PF_Player.prefab";
        private const string PlayerActionsPath = "Assets/_DOB/08.Data/DOB_PlayerControls.inputactions";
        private const string PlayerMap = "Player";
        // 이동 판정용 허용 오차다. 기획 수치가 아니라 테스트가 방향·정지를 구분하는 기준이다.
        private const float MinimumStep = 0.25f;
        private const float DirectionDot = 0.9f;
        private const float StillTolerance = 0.001f;
        private const string KeyboardMouseScheme = "Keyboard&Mouse";

        private readonly List<GameObject> spawned = new List<GameObject>();
        private BasementTestKeyboard input;
        private Keyboard keyboard;
        private Mouse mouse;
        private Keyboard otherKeyboard;
        private float initialTimeScale;

        // 배치 실행에서도 PC와 같은 Keyboard&Mouse 조합과 장치 짝짓기를 검증하도록
        // 게임 씬이나 PF_Player를 켜기 전에 가상 키보드와 마우스를 먼저 만든다.
        [SetUp]
        public void SetUp()
        {
            initialTimeScale = Time.timeScale;
            spawned.Clear();
            input = new BasementTestKeyboard();
            // 기존 한 손님 흐름은 손님 1명·넉넉한 대기 한도로 검증한다. 여러 손님 테스트는 본문에서 다시 정한다.
            TestGuestRoster.Use(guests: 1);
            keyboard = input.Keyboard;
            mouse = InputSystem.AddDevice<Mouse>("TestMouse");
        }

        // 생성한 오브젝트와 로드한 씬을 Title로 정리한 뒤 추가한 장치와 입력 설정을 되돌린다.
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            try
            {
                foreach (var item in spawned)
                    if (item) UnityEngine.Object.Destroy(item);
                spawned.Clear();
                yield return SceneManager.LoadSceneAsync(SceneComposition.Title, LoadSceneMode.Single);
            }
            finally
            {
                Time.timeScale = initialTimeScale;
                if (otherKeyboard != null && otherKeyboard.added) InputSystem.RemoveDevice(otherKeyboard);
                if (mouse != null && mouse.added) InputSystem.RemoveDevice(mouse);
                input?.Dispose();
                TestGuestRoster.Clear();
                otherKeyboard = null;
                mouse = null;
                keyboard = null;
                input = null;
            }
        }

        // 의사코드:
        // 1. 빈 테스트 씬만 남기고 다른 씬을 내린다. 손님 서비스가 없음을 확인한다.
        // 2. 바닥 콜라이더를 깔고 PF_Player 프리팹을 그 위에 단독으로 생성한다.
        // 3. 루트의 CharacterController, PlayerInput(전용 액션·Player 맵), VisualRoot/ModelSlot 아래 보이는 렌더러를 확인한다.
        // 4. PlayerInput 사용자를 테스트 키보드·마우스의 Keyboard&Mouse 스킴에만 짝짓고 자동 장치 전환을 끈다.
        // 5. 다른 키보드의 D는 무시되는지, 테스트 키보드의 W는 +z, D는 +x로 움직이는지, 키를 떼면 몇 프레임 동안 멈춰 있는지 수평 이동량으로 확인한다.
        [UnityTest]
        public IEnumerator StandalonePlayerPrefabMovesWithItsOwnPlayerInput()
        {
            // 실제 키보드처럼 플레이어보다 먼저 존재하는 다른 키보드다. 자동 짝짓기가 이 장치를 고를 수 있는 상황을 만든다.
            otherKeyboard = InputSystem.AddDevice<Keyboard>("OtherKeyboardStandIn");
            var scene = SceneManager.CreateScene("StandalonePlayerTest");
            SceneManager.SetActiveScene(scene);
            for (int i = SceneManager.sceneCount - 1; i >= 0; i--)
            {
                var other = SceneManager.GetSceneAt(i);
                if (other != scene && other.isLoaded)
                    yield return SceneManager.UnloadSceneAsync(other);
            }
            Assert.That(UnityEngine.Object.FindAnyObjectByType<FirstGuestService>(), Is.Null, "손님 서비스 없이 단독 검사");

            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            spawned.Add(ground);
            ground.transform.localScale = new Vector3(10f, 1f, 10f);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PlayerPrefabPath);
            Assert.That(prefab, Is.Not.Null, PlayerPrefabPath);
            var player = UnityEngine.Object.Instantiate(prefab, new Vector3(0f, 0.05f, 0f), Quaternion.identity);
            spawned.Add(player);
            yield return null;

            Assert.That(player.GetComponent<CharacterController>(), Is.Not.Null, "루트 CharacterController");
            var playerInput = player.GetComponent<PlayerInput>();
            Assert.That(playerInput, Is.Not.Null, "루트 PlayerInput");
            AssertPlayerActions(playerInput);
            Assert.That(VisibleRenderer(player.transform), Is.Not.Null, "VisualRoot/ModelSlot 아래 보이는 캡슐 렌더러");

            PairWithTestDevices(playerInput);
            var standIn = otherKeyboard;
            Vector3 start = player.transform.position;
            InputSystem.QueueStateEvent(standIn, new KeyboardState(Key.D));
            yield return Until(() => standIn.dKey.isPressed, "다른 키보드의 D 전달");
            yield return null;
            yield return null;
            Assert.That(Horizontal(player.transform.position - start).magnitude, Is.LessThan(StillTolerance), "다른 키보드는 이동을 가로채지 못함");
            AssertPairedToTestDevices(playerInput, "다른 키보드 입력 뒤에도 짝지은 장치 유지");
            InputSystem.QueueStateEvent(standIn, new KeyboardState());
            yield return null;

            yield return PressAndMeasure(keyboard, Key.W, player.transform, Vector3.forward, "W 전진");
            yield return PressAndMeasure(keyboard, Key.D, player.transform, Vector3.right, "D 오른쪽");
            yield return PressAndMeasure(keyboard, Key.S, player.transform, Vector3.back, "S 후진");
            yield return PressAndMeasure(keyboard, Key.A, player.transform, Vector3.left, "A 왼쪽");
        }

        [UnityTest]
        public IEnumerator KeyboardWDAndEControlTheActualPlayerAndDialogue()
        {
            yield return LoadDemo();
            var layout = FindLayout();
            var player = SceneManager.GetSceneByName(BasementComposition.BaseScene).GetRootGameObjects().Single(root => root.name == "RuntimeCharacters")
                .transform.Find("PF_Player");
            var guest = player.parent.Find("PF_Guest");
            var dialogue = UnityEngine.Object.FindAnyObjectByType<FirstGuestDialogue>();
            var service = UnityEngine.Object.FindAnyObjectByType<FirstGuestService>();
            var controls = UnityEngine.Object.FindAnyObjectByType<FirstGuestControls>();
            AssertVisibleFromMainCamera(player);
            var playerInput = player.GetComponent<PlayerInput>();
            Assert.That(playerInput, Is.Not.Null, "게임 플레이어의 PlayerInput");
            AssertPlayerActions(playerInput);
            try
            {
                PairWithTestDevices(playerInput);
                var moveAction = playerInput.actions.FindAction("Player/Move", true);
                Assert.That(moveAction.enabled, Is.True, "플레이어 Player/Move 액션 활성화");
                Assert.That(playerInput.actions.FindAction("Player/Interact", true).enabled, Is.True,
                    "플레이어 Player/Interact 액션 활성화");
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W));
                yield return Until(() => keyboard.wKey.isPressed, "가상 키보드의 W 입력 전달");
                Assert.That(controls.Move.y, Is.GreaterThan(0.9f), "W 바인딩에서 전진 입력 읽기");
                Vector3 start = player.position;
                yield return Until(() => (player.position - start).magnitude > 0.25f, "W 키로 실제 이동");
                Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
                Assert.That(Vector3.Dot((player.position - start).normalized, forward), Is.GreaterThan(0.9f));
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.D));
                yield return Until(() => keyboard.dKey.isPressed, "가상 키보드의 D 입력 전달");
                Assert.That(controls.Move.x, Is.GreaterThan(0.9f), "D 바인딩에서 오른쪽 입력 읽기");
                start = player.position;
                yield return Until(() => (player.position - start).magnitude > 0.25f, "D 키로 실제 이동");
                Assert.That(Vector3.Dot((player.position - start).normalized, Vector3.Cross(Vector3.up, forward)),
                    Is.GreaterThan(0.9f));
                InputSystem.QueueStateEvent(keyboard, new KeyboardState());
                yield return null;

                yield return Until(() => service.IsWaitingToGreet && guest.position == layout.GreetingPoint.position, "입장");
                PlacePlayer(player.GetComponent<CharacterController>(), guest.position + Vector3.right * 0.9f);
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Escape));
                yield return Until(() => Time.timeScale == 0f, "ESC로 일시정지");
                start = player.position;
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.E, Key.W));
                yield return null;
                yield return null;
                Assert.That(dialogue.BlocksWorldInput, Is.False, "일시정지 중 E 입력");
                Assert.That(Vector3.Distance(start, player.position), Is.LessThan(0.001f), "일시정지 중 W 입력");
                InputSystem.QueueStateEvent(keyboard, new KeyboardState());
                yield return null;
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Escape));
                yield return Until(() => Time.timeScale == initialTimeScale, "ESC로 영업 복귀");
                InputSystem.QueueStateEvent(keyboard, new KeyboardState());
                yield return null;
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.E));
                yield return Until(() => dialogue.BlocksWorldInput, "E 키로 실제 응대 화면 열기");
                start = player.position;
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W));
                yield return null;
                yield return null;
                Assert.That(Vector3.Distance(start, player.position), Is.LessThan(0.001f), "응대 중 이동 잠금");
            }
            finally
            {
                Time.timeScale = initialTimeScale;
            }
            yield return SceneManager.LoadSceneAsync(SceneComposition.Title);
            yield return null;
            Assert.That(SceneManager.GetSceneByName(BasementComposition.UiScene).isLoaded, Is.False);
            Assert.That(SceneManager.GetSceneByName(BasementComposition.RestaurantScene).isLoaded, Is.False);
            Assert.That(UnityEngine.Object.FindAnyObjectByType<FirstGuestDialogue>(), Is.Null);
            yield return LoadDemo();
            var scenes = Enumerable.Range(0, SceneManager.sceneCount).Select(index => SceneManager.GetSceneAt(index).name);
            CollectionAssert.AreEquivalent(new[] { BasementComposition.BaseScene, BasementComposition.UiScene, BasementComposition.RestaurantScene }, scenes);
            Assert.That(UnityEngine.Object.FindObjectsByType<EventSystem>().Length, Is.EqualTo(1));
            Assert.That(UnityEngine.Object.FindObjectsByType<Camera>().Length, Is.EqualTo(1));
            Assert.That(UnityEngine.Object.FindAnyObjectByType<FirstGuestDialogue>().BlocksWorldInput, Is.False);
            yield return SceneManager.LoadSceneAsync(SceneComposition.Title);
        }

        // 입력 검수 (홍준우 2026-11-28): 영업 중 방명록 책이 덮고 있으면 E 안내가 숨으므로 E·W가 뒤에서 응대·이동을 하지 않는다.
        // Esc는 ESC 메뉴를 열지 않고 방명록만 닫고, 닫힌 뒤에는 E가 다시 응대를 연다.
        [UnityTest]
        public IEnumerator GuestbookOverServiceBlocksWorldInputAndEscClosesOnlyIt()
        {
            yield return LoadDemo();
            var layout = FindLayout();
            var player = SceneManager.GetSceneByName(BasementComposition.BaseScene).GetRootGameObjects().Single(root => root.name == "RuntimeCharacters")
                .transform.Find("PF_Player");
            var guest = player.parent.Find("PF_Guest");
            var dialogue = UnityEngine.Object.FindAnyObjectByType<FirstGuestDialogue>();
            var service = UnityEngine.Object.FindAnyObjectByType<FirstGuestService>();
            PairWithTestDevices(player.GetComponent<PlayerInput>());
            var book = UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_DOB/03.Prefabs/UI/PF_Guestbook.prefab"));
            spawned.Add(book);
            var bookView = book.GetComponentInChildren<DOB.UI.GuestbookBookView>(true);
            bookView.Closed += bookView.Hide; // DayFlowPresenter와 같은 연결

            yield return Until(() => service.IsWaitingToGreet && guest.position == layout.GreetingPoint.position, "입장");
            PlacePlayer(player.GetComponent<CharacterController>(), guest.position + Vector3.right * 0.9f);
            bookView.Show(new[] { new DOB.UI.GuestbookLineData("하루", "잘 먹었어요") });
            Assert.That(UIManager.IsScreenOpen, Is.True, "방명록이 화면으로 잡힘");
            Vector3 start = player.position;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.E, Key.W));
            for (int i = 0; i < 5; i++)
                yield return null;
            Assert.That(dialogue.BlocksWorldInput, Is.False, "방명록 뒤 E 입력");
            Assert.That(Vector3.Distance(start, player.position), Is.LessThan(StillTolerance), "방명록 뒤 W 입력");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null;

            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Escape));
            yield return Until(() => !bookView.IsOpen, "Esc로 방명록 닫기");
            Assert.That(BasementPauseMenu.IsPaused, Is.False, "같은 Esc로 ESC 메뉴를 열지 않음");
            Assert.That(Time.timeScale, Is.EqualTo(initialTimeScale));
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.E));
            yield return Until(() => dialogue.BlocksWorldInput, "닫은 뒤 E로 응대 열기");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return SceneManager.LoadSceneAsync(SceneComposition.Title);
        }

        private static IEnumerator LoadDemo()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(DemoPath, new LoadSceneParameters(LoadSceneMode.Single));
            yield return Until(() => SceneManager.GetSceneByName(BasementComposition.UiScene).isLoaded
                && SceneManager.GetSceneByName(BasementComposition.RestaurantScene).isLoaded && !SceneManager.GetSceneByName("Loading").isLoaded
                && UnityEngine.Object.FindAnyObjectByType<FirstGuestService>() is FirstGuestService service
                && service.isActiveAndEnabled && !string.IsNullOrEmpty(service.VisitId), "Basement 월드와 UI 조합 로드");
            yield return null;
        }

        [UnityTest]
        public IEnumerator FirstVisitCanOnlyTakeTheNearbyFreeSeatAndLeaveOnce()
        {
            yield return LoadDemo();

            var layout = FindLayout();
            var service = UnityEngine.Object.FindAnyObjectByType<FirstGuestService>();
            var presenter = UnityEngine.Object.FindAnyObjectByType<FirstGuestDialogue>();
            Assert.That(layout && service && presenter, Is.True, "공용 Gameplay의 첫 손님 연결");
            var runtime = SceneManager.GetSceneByName(BasementComposition.BaseScene).GetRootGameObjects()
                .Single(item => item.name == "RuntimeCharacters").transform;
            var player = runtime.Find("PF_Player").GetComponent<CharacterController>();
            var guest = runtime.Find("PF_Guest");
            var camera = Camera.main.transform;
            // 전경은 플레이어를 따라가는 추적 카메라다. 줌아웃 뒤 메인 카메라가 그 카메라와 같은 자리로 돌아오는지 본다.
            var overhead = GameObject.Find("FollowCamera").transform;
            yield return Until(() => service.IsWaitingToGreet && guest.position == layout.GreetingPoint.position, "입장");

            Assert.That(service.TryInteract(), Is.False, "멀리서 손님을 열 수 없어야 함");
            for (int i = 0; i < 3; i++)
                Assert.That(layout.TryAssign(i, "other_visit_" + i), Is.True);
            Assert.That(layout.HasFreeSeat(), Is.False);

            PlacePlayer(player, guest.position + Vector3.right * 0.9f);
            Assert.That(service.IsNearPlayer(guest), Is.True, "플레이어와 대기 손님이 응대 거리 안에 있어야 함");
            Assert.That(presenter.FirstGuestId, Is.EqualTo("tutorial_guest_01"), "응대 프로필 연결");
            Assert.That(service.TryInteract(), Is.True, "대기 손님 앞에서 응대를 여는 E 요청");
            Assert.That(service.TryInteract(), Is.False, "줌인 중 E 연타");
            Assert.That(presenter.BlocksWorldInput, Is.False, "줌인이 끝나기 전에는 응대 화면을 열지 않음");
            yield return Until(() => presenter.BlocksWorldInput, "줌인 후 응대 화면 생성");
            var follow = DialogueElement("FollowButton").GetComponent<Button>();
            var close = DialogueElement("CloseButton").GetComponent<Button>();
            var fade = DialogueElement("FadeOverlay").GetComponent<Image>();
            yield return Until(() => follow.isActiveAndEnabled && !fade.raycastTarget, "첫 응대 화면 열기");
            // 플레이어는 손님 옆에 서 있지만 카메라는 손님 얼굴 정면에서 얼굴을 바라봐야 한다.
            var toCamera = camera.position - guest.Find("Anchors/FacePoint").position;
            Assert.That(Vector3.Dot(toCamera, guest.forward), Is.GreaterThan(0.05f), "카메라가 손님 얼굴 앞");
            Assert.That(Vector3.Cross(toCamera, guest.forward).magnitude, Is.LessThan(0.01f), "카메라가 얼굴 정면 축 위");
            Assert.That(Vector3.Angle(camera.forward, -guest.forward), Is.LessThan(0.5f), "카메라가 얼굴을 바라봄");

            follow.onClick.Invoke();
            Assert.That(DialogueElement("NoticeText").gameObject.activeInHierarchy, Is.True, "만석 거절 안내");
            close.onClick.Invoke();
            yield return Until(() => !presenter.BlocksWorldInput, "닫기");
            Assert.That(service.TryInteract(), Is.False, "줌아웃 중 E");
            yield return Until(() => Vector3.Distance(overhead.position, camera.position) < 0.001f
                && Quaternion.Angle(overhead.rotation, camera.rotation) < 0.01f, "줌아웃 후 전경 복귀");
            var hud = UnityEngine.Object.FindAnyObjectByType<BasementHud>();
            yield return Until(() => hud.Guidance.text.Contains("손님을 맞이"), "줌아웃 입력 잠금 종료");

            layout.Release(1, "stale_visit");
            Assert.That(layout.IsOccupied(1), Is.True);
            layout.Release(1, "other_visit_1");
            Assert.That(layout.TryAssign(1, "other_visit_0"), Is.False, "한 방문에 좌석 두 개를 배정할 수 없음");

            Assert.That(service.TryInteract(), Is.True);
            yield return Until(() => presenter.BlocksWorldInput, "줌인 후 응대 다시 생성");
            follow = DialogueElement("FollowButton").GetComponent<Button>();
            fade = DialogueElement("FadeOverlay").GetComponent<Image>();
            yield return Until(() => follow.isActiveAndEnabled && !fade.raycastTarget, "응대 다시 열기");
            follow.onClick.Invoke();
            follow.onClick.Invoke();
            yield return Until(() => !presenter.BlocksWorldInput, "동행 수락 후 응대 닫기");

            var occupied = layout.GetSeat(0);
            var chosen = layout.GetSeat(1);
            PlacePlayer(player, chosen.position);
            Assert.That(service.TryInteract(), Is.False, "따라오는 손님이 먼 상태");
            PlacePlayer(player, occupied.position + Vector3.back * 0.6f);
            yield return Until(() => Vector3.Distance(guest.position, occupied.position) < 1.6f, "점유석 근처 도착");
            Assert.That(service.TryInteract(), Is.False, "다른 방문이 사용하는 자리");
            PlacePlayer(player, chosen.position + Vector3.back * 0.6f);
            yield return Until(() => Vector3.Distance(guest.position, chosen.position) < 1.6f, "빈 자리 근처 도착");

            int notifications = 0;
            service.GuestSeated += (visit, seat) =>
            {
                Assert.That(visit, Is.EqualTo(service.VisitId));
                Assert.That(seat, Is.SameAs(chosen));
                notifications++;
            };
            Assert.That(service.TryInteract(), Is.True);
            Assert.That(service.TryInteract(), Is.False, "의자로 걷는 동안 중복 착석");
            yield return Until(() => service.IsSeated, "의자에 도착");
            Assert.That(service.SeatedAt, Is.SameAs(chosen));
            Assert.That(Vector3.Distance(guest.position, chosen.position), Is.LessThan(0.01f));
            Assert.That(notifications, Is.EqualTo(1));

            Assert.That(service.TryDepartForDungeon("stale_visit"), Is.False);
            Assert.That(layout.IsOccupied(1), Is.True);
            string visitId = service.VisitId;
            Assert.That(service.TryDepartForDungeon(visitId), Is.True);
            Assert.That(service.TryDepartForDungeon(visitId), Is.False);
            Assert.That(layout.IsOccupied(1), Is.False);
            yield return Until(() => !guest.gameObject.activeSelf, "계단으로 퇴장");
            Assert.That(Vector3.Distance(guest.position, layout.StairsExit.position), Is.LessThan(0.01f));
            yield return SceneManager.LoadSceneAsync(SceneComposition.Title);
        }

        private static void AssertPlayerActions(PlayerInput playerInput)
        {
            var playerActions = AssetDatabase.LoadAssetAtPath<InputActionAsset>(PlayerActionsPath);
            Assert.That(playerActions, Is.Not.Null, PlayerActionsPath);
            Assert.That(playerInput.actions, Is.Not.Null, "PlayerInput 액션 자산 연결");
            Assert.That(playerInput.actions, Is.Not.SameAs(InputSystem.actions), "UI와 플레이어의 액션 수명을 분리한다");
            // PlayerInput은 여러 플레이어를 위해 자산을 복제할 수 있으므로 같은 원본의 맵 ID로 비교한다.
            Assert.That(playerInput.actions.FindActionMap(PlayerMap, true).id, Is.EqualTo(playerActions.FindActionMap(PlayerMap, true).id),
                "플레이어 전용 액션 사용");
            Assert.That(playerInput.defaultActionMap, Is.EqualTo(PlayerMap));
            Assert.That(playerInput.currentActionMap?.name, Is.EqualTo(PlayerMap));
        }

        // 테스트 키보드·마우스만 이 플레이어의 Keyboard&Mouse 장치로 남긴다. 자동 전환을 끄지 않으면 다른 키보드 입력이 장치를 바꿀 수 있다.
        private void PairWithTestDevices(PlayerInput playerInput)
        {
            Assert.That(playerInput.user.valid, Is.True, "PlayerInput user valid");
            playerInput.neverAutoSwitchControlSchemes = true;
            playerInput.SwitchCurrentControlScheme(KeyboardMouseScheme, keyboard, mouse);
            Assert.That(playerInput.currentControlScheme, Is.EqualTo(KeyboardMouseScheme));
            AssertPairedToTestDevices(playerInput, "테스트 키보드·마우스만 짝지음");
        }

        private void AssertPairedToTestDevices(PlayerInput playerInput, string step) =>
            CollectionAssert.AreEquivalent(new InputDevice[] { keyboard, mouse }, playerInput.user.pairedDevices.ToArray(), step);

        private static IEnumerator PressAndMeasure(Keyboard keyboard, Key key, Transform player, Vector3 direction, string step)
        {
            Vector3 start = player.position;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(key));
            yield return Until(() => keyboard[key].isPressed, step + " 키 전달");
            yield return Until(() => Horizontal(player.position - start).magnitude > MinimumStep, step + " 이동");
            Assert.That(Vector3.Dot(Horizontal(player.position - start).normalized, direction), Is.GreaterThan(DirectionDot), step + " 방향");
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return Until(() => !keyboard[key].isPressed, step + " 키 뗌");
            yield return null;
            Vector3 released = player.position;
            for (int i = 0; i < 5; i++)
                yield return null;
            Assert.That(Horizontal(player.position - released).magnitude, Is.LessThan(StillTolerance), step + " 뗀 뒤 정지");
        }

        private static Vector3 Horizontal(Vector3 delta) => new Vector3(delta.x, 0f, delta.z);

        private static Renderer VisibleRenderer(Transform player)
        {
            var slot = player.Find("VisualRoot/ModelSlot");
            return slot ? slot.GetComponentsInChildren<Renderer>().FirstOrDefault(item => item.enabled && item.gameObject.activeInHierarchy) : null;
        }

        // 시작 위치에서 캡슐이 메인 카메라 시야 안에 있고, 카메라와 캡슐 중심 사이를 다른 충돌체가 가리지 않는지 본다.
        private static void AssertVisibleFromMainCamera(Transform player)
        {
            var camera = Camera.main;
            Assert.That(camera, Is.Not.Null, "메인 카메라");
            var renderer = VisibleRenderer(player);
            Assert.That(renderer, Is.Not.Null, "게임 플레이어의 보이는 렌더러");
            Assert.That(GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), renderer.bounds), Is.True,
                "시작 위치의 플레이어가 카메라 시야 안");
            if (Physics.Linecast(camera.transform.position, renderer.bounds.center, out var hit, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                Assert.That(hit.transform.IsChildOf(player), Is.True, "카메라와 플레이어 사이를 가리는 물체: " + hit.collider.name);
        }

        private static void PlacePlayer(CharacterController controller, Vector3 position)
        {
            controller.enabled = false;
            controller.transform.position = position;
            controller.enabled = true;
        }

        private static Transform DialogueElement(string name) => SceneManager.GetSceneByName(BasementComposition.UiScene).GetRootGameObjects()
            .SelectMany(root => root.GetComponentsInChildren<Transform>(true)).Single(item => item.name == name);

        private static RestaurantLayout FindLayout() => SceneManager.GetSceneByName(BasementComposition.RestaurantScene).GetRootGameObjects()
            .Single(root => root.name == "LayoutAnchors").GetComponent<RestaurantLayout>();

        private static IEnumerator Until(Func<bool> condition, string step)
        {
            float deadline = Time.realtimeSinceStartup + 30f;
            while (!condition() && Time.realtimeSinceStartup < deadline)
                yield return null;
            Assert.That(condition(), Is.True, step);
        }
    }
}
#endif
