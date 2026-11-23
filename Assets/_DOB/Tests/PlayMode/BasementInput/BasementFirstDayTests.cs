#if UNITY_EDITOR
// 격리한 저장 경로에서 타이틀과 실제 버튼·키 입력을 사용해 첫 방문, 서빙, 다음 날 준비까지 연다.
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DOB.Manager;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
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
        private const string DemoTitle = "Assets/_DOB/01.Scenes/BasementTitle.unity";
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
                yield return EditorSceneManager.LoadSceneAsyncInPlayMode(DemoTitle, new LoadSceneParameters(LoadSceneMode.Single));
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
            Click("FollowButton");
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
            Click("ConfirmCook");
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
            Click("SettleDay");
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
            Click("Save");
            Click("CancelSave");
            Assert.That(File.Exists(Path.Combine(directory, "manual.json")), Is.False);
            Click("Save");
            Click("ConfirmSave");
            AssertCheckpoint("manual", 1, 50, 2);

            Click("Load");
            Click("Manual");
            Click("CancelLoad");
            Assert.That(UnityEngine.Object.FindAnyObjectByType<FirstGuestService>().VisitId, Is.EqualTo(firstVisit));
            Click("Manual");
            loadedScenes.Clear();
            Click("ConfirmLoad");
            yield return Until(() => ReadyDay(1) &&
                UnityEngine.Object.FindAnyObjectByType<FirstGuestService>().VisitId != firstVisit, "수동 저장에서 새 방문 복원");
            Assert.That(loadedScenes, Does.Contain("Loading"));
            var kitchen = UnityEngine.Object.FindAnyObjectByType<FirstGuestKitchen>();
            Assert.That(kitchen.Gold, Is.EqualTo(50));
            Assert.That(kitchen.GarlicStock, Is.EqualTo(2));
            Assert.That(Time.timeScale, Is.EqualTo(1f));
        }

        private IEnumerator StartFromTitle()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(DemoTitle, new LoadSceneParameters(LoadSceneMode.Single));
            yield return null;
            Click("NewGame");
            yield return Until(() => ReadyDay(1), "타이틀에서 첫 영업 조합 진입");
            var expected = new[] { "BasementTitle", "Loading", "Basement", "BasementRestaurant", "BasementUI" };
            CollectionAssert.AreEqual(expected, loadedScenes.Where(expected.Contains).ToArray());
            Assert.That(SceneManager.GetSceneByName("Loading").isLoaded, Is.False);
            AssertCheckpoint("autosave", 1, 50, 2);
        }

        private static bool ReadyDay(int day) => BasementComposition.Current && BasementComposition.Current.IsReady &&
            BasementComposition.Current.Day == day;

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

        private static void Click(string name)
        {
            var button = FindActiveButton(name);
            Assert.That(button && button.IsInteractable(), Is.True, name + " 버튼 활성화");
            button.onClick.Invoke();
        }

        [Serializable]
        private sealed class SavedCheckpoint
        {
            public int day = -1, gold = -1, garlicStock = -1;
            public string stage = "", slot = "";
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
