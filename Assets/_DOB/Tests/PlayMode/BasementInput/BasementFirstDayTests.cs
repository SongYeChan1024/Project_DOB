#if UNITY_EDITOR
// 격리한 저장 경로에서 실제 Title 씬과 포인터 레이캐스트·키 입력을 사용해 새 게임, 이어하기, 첫 방문, 서빙, 다음 날 준비까지 연다.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DOB.Manager;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace DOB.Tests
{
    public sealed class BasementFirstDayTests
    {
        private const string SaveVariable = "DOB_BASEMENT_SAVE_DIR";
        private const string TitleScene = "Title";
        private const string TitlePath = "Assets/_DOB/01.Scenes/Title.unity";
        private static readonly List<RaycastResult> RaycastHits = new List<RaycastResult>();
        private readonly List<string> loadedScenes = new List<string>();
        private string previousDirectory;
        private string directory;
        private BasementTestKeyboard input;
        private float previousTimeScale;

        [SetUp]
        public void SetUp()
        {
            previousDirectory = Environment.GetEnvironmentVariable(SaveVariable);
            directory = Path.Combine(Path.GetTempPath(), "DOB-FirstDayTest-" + Guid.NewGuid().ToString("N"));
            Environment.SetEnvironmentVariable(SaveVariable, directory);
            previousTimeScale = Time.timeScale;
            Time.timeScale = 1f;
            loadedScenes.Clear();
            SceneManager.sceneLoaded += RecordScene;
            input = new BasementTestKeyboard();
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            SceneManager.sceneLoaded -= RecordScene;
            try
            {
                yield return SceneManager.LoadSceneAsync(TitlePath, LoadSceneMode.Single);
            }
            finally
            {
                Time.timeScale = previousTimeScale;
                input?.Dispose();
                Environment.SetEnvironmentVariable(SaveVariable, previousDirectory);
                if (Directory.Exists(directory)) Directory.Delete(directory, true);
            }
        }

        [UnityTest]
        public IEnumerator FreshStartWithoutSaveHidesContinue()
        {
            yield return LoadTitle();
            var continueButton = TitleButton("ContinueButton");
            Assert.That(continueButton, Is.Not.Null, "Title에 ContinueButton이 있어야 한다");
            Assert.That(continueButton.gameObject.activeInHierarchy, Is.False, "저장이 없으면 이어하기를 숨긴다");
            Assert.That(FindActiveButton("NewGameButton"), Is.Not.Null, "새 게임 버튼은 보인다");
            Assert.That(Directory.Exists(directory) && Directory.GetFiles(directory, "*.json").Length > 0, Is.False,
                "타이틀 표시만으로 저장 슬롯을 만들지 않는다");
        }

        [UnityTest]
        public IEnumerator NewGameGoesThroughLoadingToBasementRestaurantAndUi()
        {
            yield return StartFromTitle();
            Assert.That(SceneManager.GetSceneByName(TitleScene).isLoaded, Is.False, "Title은 게임 진입 후 남지 않는다");
            Assert.That(SceneManager.GetSceneByName(BasementComposition.BaseScene).isLoaded, Is.True);
            Assert.That(SceneManager.GetSceneByName(BasementComposition.RestaurantScene).isLoaded, Is.True);
            Assert.That(SceneManager.GetSceneByName(BasementComposition.UiScene).isLoaded, Is.True);
        }

        [UnityTest]
        public IEnumerator NewGameServesHaruAndOpensTheAutosavedSecondDay()
        {
            yield return StartFromTitle();
            var service = UnityEngine.Object.FindAnyObjectByType<FirstGuestService>();
            var kitchen = UnityEngine.Object.FindAnyObjectByType<FirstGuestKitchen>();
            var hud = UnityEngine.Object.FindAnyObjectByType<BasementHud>();
            var layout = UnityEngine.Object.FindAnyObjectByType<RestaurantLayout>();
            var actors = SceneManager.GetSceneByName(BasementComposition.BaseScene).GetRootGameObjects()
                .Single(obj => obj.name == "RuntimeCharacters").transform;
            var player = actors.Find("PF_Player").GetComponent<CharacterController>();
            var guest = actors.Find("PF_Guest");
            string firstVisit = service.VisitId;
            Assert.That(kitchen.Gold, Is.EqualTo(50));
            Assert.That(kitchen.GarlicStock, Is.EqualTo(2));
            Assert.That(hud.Settle.IsInteractable(), Is.False);

            yield return Until(() => guest.position == layout.GreetingPoint.position, "하루 입장");
            Place(player, layout.GreetingPoint.position + Vector3.right * .9f);
            yield return Until(() => hud.Guidance.text.Contains("손님을 맞이"), "하루 입장 완료 및 E 응대 가능 안내");
            yield return Press(Key.E);
            yield return Until(() => hud.Dialogue.BlocksWorldInput, "E로 실제 응대 화면 생성");
            var fade = SceneManager.GetSceneByName(BasementComposition.UiScene).GetRootGameObjects()
                .SelectMany(root => root.GetComponentsInChildren<Image>(true)).Single(image => image.name == "FadeOverlay");
            yield return Until(() => FindActiveButton("FollowButton") && !fade.raycastTarget, "동행 버튼 입력 가능");
            yield return Click("FollowButton");
            yield return Until(() => !hud.Dialogue.BlocksWorldInput, "동행 시작");
            Transform seat = layout.GetSeat(0);
            Place(player, seat.position + Vector3.back * .6f);
            yield return Until(() => Vector3.Distance(guest.position, seat.position) < 1.6f, "손님이 자리 근처에 도착");
            yield return Press(Key.E);
            yield return Until(() => service.IsSeated, "하루 착석");

            Place(player, layout.Kitchen.position);
            yield return Press(Key.E);
            Assert.That(hud.RecipePanel.activeInHierarchy, Is.True);
            Assert.That(hud.Ingredients.text, Does.Contain("6초"));
            yield return Click("ConfirmCook");
            Assert.That(hud.RecipePanel.activeInHierarchy, Is.False);
            Assert.That(kitchen.GarlicStock, Is.EqualTo(1));
            Assert.That(service.TryInteract(), Is.False, "조리 중인 접시는 픽업할 수 없다");
            yield return Until(() => service.MealGuidance().Contains("접시 들기"), "6초 조리 완료");
            yield return Press(Key.E);
            var plate = player.transform.Find("Anchors/PlatePoint/FirstGuestPlateVisual");
            Assert.That(plate, Is.Not.Null);
            Assert.That(plate.gameObject.activeInHierarchy, Is.True, "접시를 들면 캡슐 앞에 빵이 보인다");
            Place(player, seat.position + Vector3.back * .6f);
            yield return Press(Key.E);
            Assert.That(plate.gameObject.activeInHierarchy, Is.False, "서빙한 접시를 손에 계속 표시하지 않는다");
            Assert.That(kitchen.HasServed, Is.True);
            Assert.That(kitchen.Gold, Is.EqualTo(59));
            Assert.That(kitchen.GarlicStock, Is.EqualTo(1));
            Assert.That(hud.Receipt.text, Does.Contain("+9금화"));
            yield return Until(() => hud.Settle.IsInteractable(), "정산 버튼 활성화");
            loadedScenes.Clear();
            yield return Click("SettleDay");
            yield return Until(() => ReadyDay(2), "2일차 로드");
            Assert.That(loadedScenes, Does.Contain("Loading"));
            Assert.That(UnityEngine.Object.FindAnyObjectByType<FirstGuestService>().VisitId, Is.Not.EqualTo(firstVisit));
            kitchen = UnityEngine.Object.FindAnyObjectByType<FirstGuestKitchen>();
            Assert.That(kitchen.Gold, Is.EqualTo(59));
            Assert.That(kitchen.GarlicStock, Is.EqualTo(1));
            Assert.That(kitchen.HasServed, Is.False);
            AssertCheckpoint("autosave", 2, 59, 1);
        }

        [UnityTest]
        public IEnumerator EscapeMenuCancelsSavesAndReloadsTheManualPreparationCheckpoint()
        {
            yield return StartFromTitle();
            string firstVisit = UnityEngine.Object.FindAnyObjectByType<FirstGuestService>().VisitId;
            yield return Press(Key.Escape);
            Assert.That(Time.timeScale, Is.Zero);
            yield return Click("Save");
            yield return Click("CancelSave");
            Assert.That(File.Exists(Path.Combine(directory, "manual.json")), Is.False);
            yield return Click("Save");
            yield return Click("ConfirmSave");
            AssertCheckpoint("manual", 1, 50, 2);

            yield return Click("Load");
            yield return Click("Manual");
            yield return Click("CancelLoad");
            Assert.That(UnityEngine.Object.FindAnyObjectByType<FirstGuestService>().VisitId, Is.EqualTo(firstVisit));
            yield return Click("Manual");
            loadedScenes.Clear();
            yield return Click("ConfirmLoad");
            yield return Until(() => ReadyDay(1) &&
                UnityEngine.Object.FindAnyObjectByType<FirstGuestService>().VisitId != firstVisit, "수동 저장에서 새 방문 복원");
            Assert.That(loadedScenes, Does.Contain("Loading"));
            var kitchen = UnityEngine.Object.FindAnyObjectByType<FirstGuestKitchen>();
            Assert.That(kitchen.Gold, Is.EqualTo(50));
            Assert.That(kitchen.GarlicStock, Is.EqualTo(2));
            Assert.That(Time.timeScale, Is.EqualTo(1f));
        }

        [UnityTest]
        public IEnumerator EscapeConfirmExitReturnsToTitleImmediately()
        {
            yield return StartFromTitle();
            yield return Press(Key.Escape);
            Assert.That(Time.timeScale, Is.Zero);
            yield return Click("Exit");
            loadedScenes.Clear();
            yield return Click("ConfirmExit");
            // 추가 확인 없이 타이틀로 돌아가야 하므로 이 대기 중에는 아무 입력도 보내지 않는다.
            yield return Until(() => TitleIsReady() && !SceneManager.GetSceneByName(BasementComposition.BaseScene).isLoaded,
                "ConfirmExit 후 실제 Title 복귀");
            Assert.That(loadedScenes, Does.Contain(TitleScene));
            Assert.That(BasementComposition.Current, Is.Null);
            Assert.That(Time.timeScale, Is.EqualTo(1f), "타이틀 복귀 후 일시정지가 남지 않는다");
            Assert.That(FindActiveButton("ContinueButton"), Is.Not.Null, "새 게임 자동 저장이 있으므로 이어하기가 보인다");
        }

        [UnityTest]
        public IEnumerator ContinueLoadsTheSavedSecondDay()
        {
            SeedAutosave(2, 59, 1);
            yield return LoadTitle();
            yield return Until(() => FindActiveButton("ContinueButton"), "저장이 있으면 이어하기 표시");
            loadedScenes.Clear();
            yield return Click("ContinueButton");
            yield return Until(() => ReadyDay(2), "이어하기로 2일차 진입");
            Assert.That(loadedScenes, Does.Contain("Loading"));
            var kitchen = UnityEngine.Object.FindAnyObjectByType<FirstGuestKitchen>();
            Assert.That(kitchen.Gold, Is.EqualTo(59));
            Assert.That(kitchen.GarlicStock, Is.EqualTo(1));
            Assert.That(kitchen.HasServed, Is.False);
        }

        [UnityTest]
        public IEnumerator NewGameWithExistingSaveRequiresConfirmationAndPreservesTheCheckpoint()
        {
            SeedAutosave(2, 59, 1);
            string autosave = Path.Combine(directory, "autosave.json");
            byte[] original = File.ReadAllBytes(autosave);
            yield return LoadTitle();

            yield return Click("NewGameButton");
            yield return Until(() => FindActiveButton("YesButton") && FindActiveButton("NoButton"), "덮어쓰기 확인 표시");
            yield return Click("NoButton");
            yield return Until(() => !FindActiveButton("YesButton"), "취소 후 확인 창 닫힘");
            Assert.That(BasementComposition.Current, Is.Null, "취소하면 게임에 들어가지 않는다");
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo(TitleScene));
            Assert.That(File.ReadAllBytes(autosave), Is.EqualTo(original), "취소하면 기존 저장을 유지한다");
            Assert.That(FindActiveButton("ContinueButton"), Is.Not.Null);

            yield return Click("NewGameButton");
            yield return Until(() => FindActiveButton("YesButton"), "다시 덮어쓰기 확인 표시");
            loadedScenes.Clear();
            yield return Click("YesButton");
            yield return Until(() => ReadyDay(1), "확인 후 새 게임 1일차 진입");
            Assert.That(loadedScenes, Does.Contain("Loading"));
            var kitchen = UnityEngine.Object.FindAnyObjectByType<FirstGuestKitchen>();
            Assert.That(kitchen.Gold, Is.EqualTo(50));
            Assert.That(kitchen.GarlicStock, Is.EqualTo(2));
            Assert.That(File.ReadAllBytes(autosave), Is.EqualTo(original), "새 게임의 첫 정산 전까지 직전 자동 저장을 유지한다");
        }

        private IEnumerator LoadTitle()
        {
            yield return SceneManager.LoadSceneAsync(TitlePath, LoadSceneMode.Single);
            yield return Until(TitleIsReady, "실제 Title 씬 로드");
            // TitlePresenter.Start가 저장 유무로 메뉴를 고친 뒤에 버튼 표시를 검사한다.
            yield return null;
        }

        private IEnumerator StartFromTitle()
        {
            yield return LoadTitle();
            yield return Click("NewGameButton");
            yield return Until(() => ReadyDay(1), "타이틀에서 첫 영업 조합 진입");
            var expected = new[] { TitleScene, "Loading", BasementComposition.BaseScene, BasementComposition.RestaurantScene, BasementComposition.UiScene };
            CollectionAssert.AreEqual(expected, loadedScenes.Where(expected.Contains).ToArray());
            Assert.That(SceneManager.GetSceneByName("Loading").isLoaded, Is.False);
            AssertCheckpoint("autosave", 1, 50, 2);
        }

        private static bool TitleIsReady() =>
            SceneManager.GetActiveScene().name == TitleScene && SceneManager.GetActiveScene().isLoaded && EventSystem.current;

        private static bool ReadyDay(int day) => BasementComposition.Current && BasementComposition.Current.IsReady &&
            BasementComposition.Current.Day == day;

        // 숨긴 버튼도 찾기 위해 비활성 오브젝트를 포함해 Title 씬 안에서만 찾는다.
        private static Button TitleButton(string name) => SceneManager.GetSceneByPath(TitlePath).GetRootGameObjects()
            .SelectMany(root => root.GetComponentsInChildren<Button>(true)).SingleOrDefault(button => button.name == name);

        // 이동은 독립 키보드 테스트에서 검사한다. 이 테스트는 상호작용 반경까지 배치한 후 실제 E를 누른다.
        private static void Place(CharacterController player, Vector3 position)
        {
            player.enabled = false;
            player.transform.position = position;
            player.enabled = true;
        }

        private IEnumerator Press(Key key)
        {
            var keyboard = input.Keyboard;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(key));
            yield return Until(() => keyboard[key].isPressed, key + " 누름이 PlayMode 키보드에 전달됨");
            yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return Until(() => !keyboard[key].isPressed, key + " 뗌이 PlayMode 키보드에 전달됨");
            yield return null;
        }

        private static Button FindActiveButton(string name) =>
            UnityEngine.Object.FindObjectsByType<Button>().SingleOrDefault(button => button.name == name);

        // 버튼 중심에서 EventSystem 레이캐스트의 최상단 클릭 대상이 그 버튼일 때만 실제 포인터 이벤트로 누른다.
        // 새로 만든 UI는 한 번 그려진 뒤에야 레이캐스트 깊이가 생기므로 프레임을 넘기며 기다린다.
        private static IEnumerator Click(string name)
        {
            Button button = null;
            RaycastResult hit = default;
            yield return Until(() =>
            {
                button = FindActiveButton(name);
                return button && button.IsInteractable() && PointerHits(button, out hit);
            }, name + " 버튼이 활성화되고 포인터 레이캐스트 최상단에 있음");

            var data = new PointerEventData(EventSystem.current)
            {
                button = PointerEventData.InputButton.Left,
                position = hit.screenPosition,
                pointerCurrentRaycast = hit,
                pointerPressRaycast = hit,
                rawPointerPress = hit.gameObject,
                pointerPress = button.gameObject,
                eligibleForClick = true,
                clickCount = 1,
            };
            ExecuteEvents.Execute(button.gameObject, data, ExecuteEvents.pointerDownHandler);
            ExecuteEvents.Execute(button.gameObject, data, ExecuteEvents.pointerUpHandler);
            ExecuteEvents.Execute(button.gameObject, data, ExecuteEvents.pointerClickHandler);
        }

        private static bool PointerHits(Button button, out RaycastResult hit)
        {
            hit = default;
            var events = EventSystem.current;
            var canvas = button.GetComponentInParent<Canvas>();
            if (!events || !canvas)
                return false;
            Canvas.ForceUpdateCanvases();
            canvas = canvas.rootCanvas;
            var rect = (RectTransform)button.transform;
            Camera camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            Vector2 point = RectTransformUtility.WorldToScreenPoint(camera, rect.TransformPoint(rect.rect.center));
            RaycastHits.Clear();
            events.RaycastAll(new PointerEventData(events) { position = point }, RaycastHits);
            if (RaycastHits.Count == 0)
                return false;
            hit = RaycastHits[0];
            return ExecuteEvents.GetEventHandler<IPointerClickHandler>(hit.gameObject) == button.gameObject;
        }

        [Serializable]
        private sealed class SavedCheckpoint
        {
            public int day = -1, gold = -1, garlicStock = -1;
            public string stage = "", slot = "";
        }

        // 테스트 어셈블리는 DOB.Systems를 참조하지 않으므로 저장소의 DataContract JSON 형식을 직접 쓴다(멤버 이름순).
        private void SeedAutosave(int day, int gold, int garlic)
        {
            Directory.CreateDirectory(directory);
            string savedAt = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture);
            File.WriteAllText(Path.Combine(directory, "autosave.json"),
                "{\"day\":" + day + ",\"garlicStock\":" + garlic + ",\"gold\":" + gold + ",\"savedAtUtc\":\"" + savedAt +
                "\",\"schemaVersion\":1,\"slot\":\"autosave\",\"stage\":\"Preparation\"}");
        }

        private void AssertCheckpoint(string slot, int day, int gold, int garlic)
        {
            var path = Path.Combine(directory, slot + ".json");
            Assert.That(File.Exists(path), Is.True, "격리된 저장 슬롯 " + slot);
            var save = JsonUtility.FromJson<SavedCheckpoint>(File.ReadAllText(path));
            Assert.That(save.slot, Is.EqualTo(slot));
            Assert.That(save.stage, Is.EqualTo("Preparation"));
            Assert.That(save.day, Is.EqualTo(day));
            Assert.That(save.gold, Is.EqualTo(gold));
            Assert.That(save.garlicStock, Is.EqualTo(garlic));
        }

        private void RecordScene(Scene scene, LoadSceneMode mode) => loadedScenes.Add(scene.name);

        private static IEnumerator Until(Func<bool> condition, string step)
        {
            float deadline = Time.realtimeSinceStartup + 40f;
            while (!condition() && Time.realtimeSinceStartup < deadline) yield return null;
            Assert.That(condition(), Is.True, step);
        }
    }
}
#endif
