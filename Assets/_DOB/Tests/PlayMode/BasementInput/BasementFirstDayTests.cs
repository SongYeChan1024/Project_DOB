#if UNITY_EDITOR
// 격리한 저장 경로에서 실제 Title 씬과 포인터 레이캐스트·키 입력을 사용해 새 게임, 이어하기, 첫 방문, 서빙, 다음 날 준비까지 연다.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DOB.Define;
using DOB.Manager;
using DOB.Systems;
using DOB.UI;
using NUnit.Framework;
using TMPro;
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
        public IEnumerator NewGameGoesThroughLoadingToRestaurantAndUi()
        {
            yield return StartFromTitle();
            Assert.That(SceneManager.GetSceneByName(TitleScene).isLoaded, Is.False, "Title은 게임 진입 후 남지 않는다");
            Assert.That(SceneManager.GetSceneByName(BasementComposition.BaseScene).isLoaded, Is.True);
            Assert.That(SceneManager.GetSceneByName(BasementComposition.RestaurantScene).isLoaded, Is.True);
            Assert.That(SceneManager.GetSceneByName(BasementComposition.UiScene).isLoaded, Is.True);
        }

        [UnityTest]
        public IEnumerator NewGameServesTheGeneratedGuestAndOpensTheAutosavedSecondDay()
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
            var visit = service.CurrentVisit;
            Assert.That(visit, Is.Not.Null);
            Assert.That(visit.VisitId, Is.EqualTo(firstVisit));
            Assert.That(visit.GuestId, Is.EqualTo(service.GuestId).And.Not.EqualTo(hud.Dialogue.FirstGuestId));
            Assert.That(visit.GuestName, Is.Not.Empty);
            string servedVisit = null, servedName = null, servedLine = null;
            kitchen.Served += meal => { servedVisit = meal.VisitId; servedName = meal.GuestName; servedLine = meal.Line; };
            Assert.That(kitchen.Gold, Is.EqualTo(50));
            Assert.That(kitchen.GarlicStock, Is.EqualTo(2));
            Assert.That(hud.Settle.IsInteractable(), Is.False);

            yield return Until(() => guest.position == layout.GreetingPoint.position, "손님 입장");
            Place(player, layout.GreetingPoint.position + Vector3.right * .9f);
            yield return Until(() => hud.Guidance.text.Contains("손님을 맞이"), "입장 완료 및 E 응대 가능 안내");
            var camera = Camera.main.transform;
            var overhead = new Pose(camera.position, camera.rotation);
            yield return Press(Key.E);
            yield return Until(() => hud.Dialogue.BlocksWorldInput, "E로 실제 응대 화면 생성");
            var fade = SceneManager.GetSceneByName(BasementComposition.UiScene).GetRootGameObjects()
                .SelectMany(root => root.GetComponentsInChildren<Image>(true)).Single(image => image.name == "FadeOverlay");
            yield return Until(() => FindActiveButton("FollowButton") && !fade.raycastTarget, "동행 버튼 입력 가능");
            Assert.That(FindActiveButton("SendHomeButton"), Is.Not.Null, "본영업에서는 첫날부터 손님을 돌려보낼 수 있다");
            var view = UnityEngine.Object.FindAnyObjectByType<GuestDialogueView>();
            Assert.That(view.GetComponentsInChildren<TMP_Text>().Single(t => t.name == "NameText").text, Is.EqualTo(visit.GuestName));
            var lineClue = visit.Clues.First(c => c.Channel == DialogueClueChannel.Dialogue);
            var observation = visit.Clues.First(c => c.Channel == DialogueClueChannel.Observation);
            var lineText = view.GetComponentInChildren<TextLinkClick>().GetComponent<TMP_Text>();
            Assert.That(visit.NotebookRisk(lineClue.Axis), Is.Null);
            yield return ClickPhrase(lineText, lineClue.Text);
            Assert.That(visit.IsDiscovered(lineClue.Key), Is.True);
            var marker = view.GetComponentsInChildren<ClueTooltip>().First();
            yield return ClickClue(marker);
            Assert.That(visit.IsDiscovered(observation.Key), Is.True);
            Assert.That(NotebookRecord.Session.Has(visit.GuestId, observation.Key), Is.True);
            int? known = visit.NotebookRisk(lineClue.Axis);

            yield return Click("CloseButton");
            yield return Until(() => !hud.Dialogue.BlocksWorldInput && hud.Guidance.text.Contains("손님을 맞이")
                && Vector3.Distance(overhead.position, camera.position) < 0.001f
                && Quaternion.Angle(overhead.rotation, camera.rotation) < 0.01f, "전경 복귀 및 E 응대 재활성");
            yield return Press(Key.E);
            yield return Until(DialogueReady, "같은 손님 응대 다시 열기");
            Assert.That(service.CurrentVisit, Is.SameAs(visit));
            view = UnityEngine.Object.FindAnyObjectByType<GuestDialogueView>();
            lineText = view.GetComponentInChildren<TextLinkClick>().GetComponent<TMP_Text>();
            Assert.That(lineText.text, Does.Contain("<u>").And.Contain(lineClue.Text));
            Assert.That(view.GetComponentsInChildren<ClueTooltip>().First().transform.Find("FoundMark").gameObject.activeSelf, Is.True);
            Assert.That(visit.NotebookRisk(lineClue.Axis), Is.EqualTo(known));
            yield return Click("FollowButton");
            yield return Until(() => !hud.Dialogue.BlocksWorldInput, "동행 시작");
            Transform seat = layout.GetSeat(0);
            Place(player, seat.position + Vector3.back * .6f);
            yield return Until(() => Vector3.Distance(guest.position, seat.position) < 1.6f, "손님이 자리 근처에 도착");
            yield return Press(Key.E);
            yield return Until(() => service.IsSeated, "같은 손님 착석");

            Place(player, layout.Kitchen.position);
            yield return Press(Key.E);
            Assert.That(hud.RecipePanel.activeInHierarchy, Is.True);
            Assert.That(hud.Ingredients.text, Does.Contain("6초"));
            Assert.That(hud.Ingredients.text, Does.Contain(visit.GuestName));
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
            Assert.That(servedVisit, Is.EqualTo(visit.VisitId));
            Assert.That(servedName, Is.EqualTo(visit.GuestName));
            Assert.That(hud.Receipt.text, Does.Contain(servedName).And.Contain(servedLine));
            var bread = UnityEditor.AssetDatabase.LoadAssetAtPath<FirstServiceRecipe>("Assets/_DOB/08.Data/SO_FirstService_Bread.asset");
            var shortfalls = ExpeditionJudge.Shortfalls(visit.ActualRisk, axis => bread.Protection(axis, true));
            int shortfall = ExpeditionJudge.Total(shortfalls);
            Assert.That(servedLine, Is.EqualTo(visit.TasteLine(true)));
            Assert.That(hud.Receipt.text, Does.Contain(visit.DepartureLine(shortfall)));
            // 서빙한 순간 그 방문의 탐험 결과가 한 번 확정되고, 등급은 출발 한마디와 같은 부족 합에서 나온다.
            var served = UnityEngine.Object.FindAnyObjectByType<SettlementManager>().TodayResults.Single();
            Assert.That(served.VisitId, Is.EqualTo(visit.VisitId));
            Assert.That(served.GuestId, Is.EqualTo(visit.GuestId));
            Assert.That(served.Shortfalls, Is.EqualTo(shortfalls));
            Assert.That(served.Grade, Is.EqualTo(ExpeditionJudge.Grade(shortfall)));
            // 대성공이면 30% 확률로 희귀 재료(숲 송로)가 하나 더 붙으므로 일반 재료만 센다.
            Assert.That(served.Loot.Count(material => material != "숲 송로"), Is.EqualTo(new[] { 3, 2, 1, 0 }[(int)served.Grade]));
            Assert.That(served.Loot.Count(material => material == "숲 송로"),
                Is.LessThanOrEqualTo(served.Grade == ExpeditionGrade.GreatSuccess ? 1 : 0));
            Assert.That(UnityEngine.Object.FindAnyObjectByType<EconomyManager>().Materials.All(), Is.Empty, "전리품은 다음 아침에 받는다");
            Canvas.ForceUpdateCanvases();
            var receipt = hud.Receipt;
            var settings = receipt.GetGenerationSettings(receipt.rectTransform.rect.size);
            settings.verticalOverflow = VerticalWrapMode.Overflow;
            using (var uncut = new TextGenerator())
            {
                uncut.Populate(receipt.text, settings);
                Assert.That(uncut.lineCount, Is.GreaterThanOrEqualTo(3));
                Assert.That(receipt.cachedTextGenerator.characterCountVisible, Is.EqualTo(uncut.characterCountVisible),
                    "영수증의 맛 소감·각오·마지막 금화 줄까지 모두 렌더링");
            }
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

            // 2일차는 아침보고 → 준비 → 영업 순서로 열리고, 영업 전에는 손님이 들어오지 않는다.
            var game = BasementComposition.Current.Game;
            Assert.That(game.Phase, Is.EqualTo(DOB.Define.DayPhase.MorningReport));
            Assert.That(UnityEngine.Object.FindAnyObjectByType<FirstGuestService>().enabled, Is.False);
            // 어제의 방문이 오늘 아침의 미처리 귀환 보고로 넘어오고, 보고가 끝나면 전리품이 한 번 반영된다.
            var settlement = UnityEngine.Object.FindAnyObjectByType<SettlementManager>();
            var economy = UnityEngine.Object.FindAnyObjectByType<EconomyManager>();
            var report = settlement.Reports.Single();
            Assert.That(report.VisitId, Is.EqualTo(firstVisit));
            Assert.That(report.Grade, Is.EqualTo(served.Grade));
            Assert.That(report.Loot, Is.EqualTo(served.Loot));
            Assert.That(settlement.IsReportConfirmed(firstVisit), Is.False);
            Assert.That(settlement.TodayResults, Is.Empty);
            Assert.That(economy.Materials.All(), Is.Empty);
            yield return Click("ConfirmMorningReport");
            Assert.That(game.Phase, Is.EqualTo(DOB.Define.DayPhase.Preparation));
            Assert.That(settlement.IsReportConfirmed(firstVisit), Is.True);
            Assert.That(economy.Materials.All().Sum(m => m.Count), Is.EqualTo(report.Loot.Count));
            Assert.That(settlement.ConfirmReport(firstVisit), Is.EqualTo(ReportConfirmResult.AlreadyConfirmed));
            Assert.That(economy.Materials.All().Sum(m => m.Count), Is.EqualTo(report.Loot.Count));
            Assert.That(kitchen.Gold, Is.EqualTo(59), "귀환 보고는 금화를 바꾸지 않는다");
            yield return Click("StartService");
            Assert.That(game.Phase, Is.EqualTo(DOB.Define.DayPhase.Service));
            yield return Until(() => UnityEngine.Object.FindAnyObjectByType<FirstGuestService>().VisitId != null, "2일차 영업에서 손님 입장");
        }

        [UnityTest]
        public IEnumerator AllFourSimultaneousObservationsCanBeFoundInTheRestaurantDialogue()
        {
            yield return StartFromTitle();
            var data = new GuestVisitInput
            {
                GuestId = Guid.NewGuid().ToString("N"), VisitId = "four-observations", GuestName = "관찰 시험",
                Day = 2, Floor = 1, Race = "수인", Job = "사제", IsTired = true, PersonalWeakness = "독"
            };
            var rules = Resources.Load<GuestDialogueRules>("SO_GuestDialogueRules");
            var visit = GuestDialogueGenerator.Generate(rules, data, new System.Random(9));
            var presenter = UnityEngine.Object.FindAnyObjectByType<FirstGuestDialogue>();
            Assert.That(presenter.TryOpenVisit(visit, true), Is.True);
            yield return Until(DialogueReady, "네 관찰 단서를 가진 응대 화면");
            var clues = visit.Clues.Where(c => c.Channel == DialogueClueChannel.Observation).ToArray();
            var view = UnityEngine.Object.FindAnyObjectByType<GuestDialogueView>();
            var markers = view.GetComponentsInChildren<ClueTooltip>();
            Assert.That(clues.Length, Is.EqualTo(4));
            Assert.That(markers.Length, Is.EqualTo(clues.Length));
            // 성표부터 보면 독은 미확인에서 확인된 0으로 바뀐다.
            int priest = Array.FindIndex(clues, c => c.Condition == DialogueCondition.Job);
            yield return ClickClue(markers[priest]);
            Assert.That(visit.NotebookRisk("독"), Is.EqualTo(0));
            Assert.That(view.GetComponentsInChildren<TMP_Text>().Any(t => t.text == "독 0"), Is.True);
            foreach (int i in Enumerable.Range(0, clues.Length).Where(i => i != priest))
                yield return ClickClue(markers[i]);
            Assert.That(clues.All(c => visit.IsDiscovered(c.Key)), Is.True);
            Assert.That(NotebookRecord.Session.CluesOf(visit.GuestId).Count, Is.EqualTo(4));
            Assert.That(visit.NotebookRisk("독"), Is.EqualTo(1), "수인 +1, 사제 -1, 개인 취약 +1");
            Assert.That(visit.NotebookRisk("체력"), Is.EqualTo(1));

            // 정적 SDF에서 '톡'과 '쏘'가 빠져 있던 수인 원고. 말과 초상은 같은 종족 원인이다.
            var scent = visit.Clues.Single(c => c.Channel == DialogueClueChannel.Dialogue && c.Condition == DialogueCondition.Race);
            Assert.That(scent.Text, Is.EqualTo("톡 쏘는 냄새"));
            yield return Click("ExpandTab");
            var dialogue = view.GetComponentInChildren<TextLinkClick>().GetComponent<TMP_Text>();
            yield return ClickPhrase(dialogue, scent.Text);
            Assert.That(visit.IsDiscovered(scent.Key), Is.True);
            Assert.That(visit.NotebookRisk("독"), Is.EqualTo(1), "같은 원인을 다른 채널에서 찾아도 중복 가산하지 않는다");
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
            yield return Until(() => TitleIsReady() && !SceneManager.GetSceneByName(BasementComposition.BaseScene).isLoaded && !SceneLoadService.IsLoading,
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
            var label = FindActiveButton("ContinueButton").transform.Find("Message").GetComponent<TMPro.TMP_Text>();
            Assert.That(label.text, Does.Contain("2일차 시작"), "타이틀의 이어하기 버튼에 저장된 일차를 표시한다");
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
        public IEnumerator ReloadingRestartsTheMorningReportAndReceivedLootCarriesToTheNextDay()
        {
            var report = new VisitResult("guest-1", "visit-1", "하루", 1, "구운 빵", true, new[] { 0, 0, 0, 0, 0, 0 },
                ExpeditionGrade.GreatSuccess, new[] { "들풀", "들풀", "숲버섯" });
            new FirstServiceSaveStore(directory).SavePreparation(FirstServiceSaveSlot.Auto, 2, 59, 1,
                new[] { new MaterialCount("숲버섯", 1) }, new[] { report });

            SettlementManager settlement = null;
            EconomyManager economy = null;
            // 보고 도중에는 저장하지 않는다. 다시 이어하면 같은 일차의 시작으로 돌아가 보고를 처음부터 받는다.
            for (int attempt = 0; attempt < 2; attempt++)
            {
                yield return LoadTitle();
                yield return Click("ContinueButton");
                yield return Until(() => ReadyDay(2), "이어하기로 2일차 진입 " + attempt);
                settlement = UnityEngine.Object.FindAnyObjectByType<SettlementManager>();
                economy = UnityEngine.Object.FindAnyObjectByType<EconomyManager>();
                Assert.That(settlement.Reports.Single().VisitId, Is.EqualTo("visit-1"));
                Assert.That(settlement.Reports.Single().Grade, Is.EqualTo(ExpeditionGrade.GreatSuccess));
                Assert.That(settlement.IsReportConfirmed("visit-1"), Is.False, "불러오면 보고는 미처리 " + attempt);
                Assert.That(economy.Materials.Count("숲버섯"), Is.EqualTo(1), "불러오면 재료는 일차 시작 값 " + attempt);
                Assert.That(economy.Materials.Count("들풀"), Is.Zero);

                // 첫 진입에서는 화면이 보고를 하나 먼저 확정한 경우를 흉내 낸다. 아침 보고를 끝내도 다시 지급되지 않는다.
                if (attempt == 0)
                    Assert.That(settlement.ConfirmReport("visit-1"), Is.EqualTo(ReportConfirmResult.Confirmed));
                yield return Click("ConfirmMorningReport");
                Assert.That(settlement.IsReportConfirmed("visit-1"), Is.True);
                Assert.That(economy.Materials.Count("숲버섯"), Is.EqualTo(2));
                Assert.That(economy.Materials.Count("들풀"), Is.EqualTo(2));
            }

            // 2일차 손님을 돌려보내면 탐험 결과가 남지 않고, 아침에 받은 재료는 정산을 거쳐 3일차로 넘어간다.
            yield return Click("StartService");
            var layout = UnityEngine.Object.FindAnyObjectByType<RestaurantLayout>();
            var hud = UnityEngine.Object.FindAnyObjectByType<BasementHud>();
            var actors = SceneManager.GetSceneByName(BasementComposition.BaseScene).GetRootGameObjects()
                .Single(obj => obj.name == "RuntimeCharacters").transform;
            var guest = actors.Find("PF_Guest");
            yield return Until(() => guest.position == layout.GreetingPoint.position, "2일차 손님 입장");
            Place(actors.Find("PF_Player").GetComponent<CharacterController>(), layout.GreetingPoint.position + Vector3.right * .9f);
            yield return Until(() => hud.Guidance.text.Contains("손님을 맞이"), "E 응대 가능 안내");
            yield return Press(Key.E);
            yield return Until(DialogueReady, "응대 화면 입력 가능");
            yield return Click("SendHomeButton");
            yield return Click("YesButton");
            yield return Until(() => hud.Settle.IsInteractable(), "돌려보낸 뒤 정산 버튼 활성화");
            Assert.That(settlement.TodayResults, Is.Empty, "돌려보낸 손님은 탐험 결과가 없다");
            yield return Click("SettleDay");
            yield return Until(() => ReadyDay(3), "3일차 로드");
            Assert.That(UnityEngine.Object.FindAnyObjectByType<SettlementManager>().Reports, Is.Empty);
            economy = UnityEngine.Object.FindAnyObjectByType<EconomyManager>();
            Assert.That(economy.Materials.Count("숲버섯"), Is.EqualTo(2));
            Assert.That(economy.Materials.Count("들풀"), Is.EqualTo(2));
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
            BasementComposition.Current.Game.Day == day;

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

        private static bool DialogueReady()
        {
            var view = UnityEngine.Object.FindAnyObjectByType<GuestDialogueView>();
            return view && view.IsOpen && !view.transform.Find("FadeOverlay").GetComponent<Image>().raycastTarget;
        }

        private static IEnumerator ClickPhrase(TMP_Text text, string phrase)
        {
            int index = -1;
            yield return Until(() =>
            {
                Canvas.ForceUpdateCanvases();
                text.ForceMeshUpdate();
                index = Array.FindIndex(text.textInfo.linkInfo, 0, text.textInfo.linkCount, item => item.GetLinkID() == phrase);
                return index >= 0;
            }, $"대사 원문의 링크 ID 생성: {phrase}");
            var link = text.textInfo.linkInfo[index];
            Assert.That(link.GetLinkText(), Is.EqualTo(phrase), "원문의 한글이 대체 글자 없이 그려져야 한다");
            var letter = text.textInfo.characterInfo[link.linkTextfirstCharacterIndex];
            var localPoint = (letter.bottomLeft + letter.topRight) * .5f;
            yield return ClickAt(text.gameObject, text.transform.TransformPoint(localPoint));
        }

        private static IEnumerator ClickClue(ClueTooltip marker)
        {
            var tab = FindActiveButton("ExpandTab");
            if (tab.GetComponentInChildren<TMP_Text>().text == "접기")
                yield return Click("ExpandTab");
            var rect = (RectTransform)marker.transform;
            yield return ClickAt(marker.gameObject, rect.TransformPoint(rect.rect.center));
        }

        private static IEnumerator ClickAt(GameObject target, Vector3 worldPoint)
        {
            var canvas = target.GetComponentInParent<Canvas>().rootCanvas;
            var camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            var point = RectTransformUtility.WorldToScreenPoint(camera, worldPoint);
            var data = new PointerEventData(EventSystem.current) { position = point, button = PointerEventData.InputButton.Left };
            yield return Until(() =>
            {
                RaycastHits.Clear();
                EventSystem.current.RaycastAll(data, RaycastHits);
                return RaycastHits.Count > 0 && ExecuteEvents.GetEventHandler<IPointerClickHandler>(RaycastHits[0].gameObject) == target;
            }, target.name + " 단서가 화면의 포인터를 받음");
            data.pointerPressRaycast = RaycastHits[0];
            data.pointerCurrentRaycast = RaycastHits[0];
            ExecuteEvents.Execute(target, data, ExecuteEvents.pointerClickHandler);
        }

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

        // 저장 로더가 읽는 기존 DataContract JSON 형식으로 이전 진행을 준비한다.
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
