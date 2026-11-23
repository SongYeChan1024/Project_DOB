// FirstGuestServiceTests — 실제 식당·Gameplay 구성에서 첫 방문에 대한 공간 상호작용을 검증한다.
// 의사코드: 모든 자리를 점유하고 먼 거리와 빈 자리 없는 응대를 거절한다. 하나를 비운 뒤 같은 방문으로 재시도한다.
// 응대 페이드/카메라 복귀, 점유석과 손님의 도착 거리, 정확한 좌석 하나와 한 번의 착석 알림·퇴장을 확인한다.
using System;
using System.Collections;
using System.Linq;
using DOB.Manager;
using NUnit.Framework;
#if UNITY_EDITOR
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
            var input = new BasementTestKeyboard();
            var keyboard = input.Keyboard;
            float initialTimeScale = Time.timeScale;
            try
            {
                var moveAction = InputSystem.actions.FindAction("Player/Move", true);
                Assert.That(moveAction.enabled, Is.True, "런타임 Player/Move 액션 활성화");
                Assert.That(InputSystem.actions.FindAction("Player/Interact", true).enabled, Is.True,
                    "런타임 Player/Interact 액션 활성화");
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
                input.Dispose();
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
            var overhead = new Pose(camera.position, camera.rotation);
            yield return Until(() => service.IsWaitingToGreet && guest.position == layout.GreetingPoint.position, "입장");

            Assert.That(service.TryInteract(), Is.False, "멀리서 손님을 열 수 없어야 함");
            for (int i = 0; i < 3; i++)
                Assert.That(layout.TryAssign(i, "other_visit_" + i), Is.True);
            Assert.That(layout.HasFreeSeat(), Is.False);

            PlacePlayer(player, guest.position + Vector3.right * 0.9f);
            Assert.That(service.IsNearPlayer(guest), Is.True, "플레이어와 대기 손님이 응대 거리 안에 있어야 함");
            Assert.That(presenter.FirstGuestId, Is.EqualTo("tutorial_guest_01"), "응대 프로필 연결");
            Assert.That(service.TryInteract(), Is.True, "대기 손님 앞에서 응대를 여는 E 요청");
            Assert.That(service.TryInteract(), Is.False, "페이드 중 E 연타");
            var follow = DialogueElement("FollowButton").GetComponent<Button>();
            var close = DialogueElement("CloseButton").GetComponent<Button>();
            var fade = DialogueElement("FadeOverlay").GetComponent<Image>();
            yield return Until(() => follow.isActiveAndEnabled && !fade.raycastTarget, "첫 응대 화면 열기");
            Assert.That(Vector3.Distance(overhead.position, camera.position), Is.GreaterThan(0.5f));

            follow.onClick.Invoke();
            Assert.That(DialogueElement("NoticeText").gameObject.activeInHierarchy, Is.True, "만석 거절 안내");
            close.onClick.Invoke();
            yield return Until(() => !presenter.BlocksWorldInput, "닫기");
            yield return null;
            Assert.That(Vector3.Distance(overhead.position, camera.position), Is.LessThan(0.001f));
            Assert.That(Quaternion.Angle(overhead.rotation, camera.rotation), Is.LessThan(0.01f));

            layout.Release(1, "stale_visit");
            Assert.That(layout.IsOccupied(1), Is.True);
            layout.Release(1, "other_visit_1");
            Assert.That(layout.TryAssign(1, "other_visit_0"), Is.False, "한 방문에 좌석 두 개를 배정할 수 없음");

            Assert.That(service.TryInteract(), Is.True);
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
