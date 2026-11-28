#if UNITY_EDITOR
// 귀환 보고 화면이 정산 Manager에 이어졌을 때(UIManager.ConnectDay): 손님 보고를 펼치면 확정을 요청하고,
// 공개된 층 위험을 「수첩에 적은 층 위험」 칸에, 새로 연 층을 전날 정산 창에 보이는지 (FeatureManagerMap F9 홍준우 인계).
using System.Collections;
using System.Linq;
using DOB.Define;
using DOB.Manager;
using DOB.Systems;
using DOB.UI;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace DOB.Tests
{
    public sealed class ReturnReportFloorTests
    {
        private const string ReportPrefab = "Assets/_DOB/03.Prefabs/UI/PF_ReturnReport.prefab";
        private const string RewardsAsset = "Assets/_DOB/08.Data/SO_ExpeditionRewards.asset";
        private GameObject screen;
        private GameObject managers;
        private SettlementManager settlement;
        private ReturnReportView view;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            managers = new GameObject("TestManagers");
            managers.SetActive(false);
            settlement = managers.AddComponent<SettlementManager>();
            var serialized = new SerializedObject(settlement);
            serialized.FindProperty("rewards").objectReferenceValue = AssetDatabase.LoadAssetAtPath<ExpeditionRewards>(RewardsAsset);
            serialized.ApplyModifiedPropertiesWithoutUndo();
            managers.SetActive(true);

            screen = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(ReportPrefab));
            view = screen.GetComponentInChildren<ReturnReportView>(true);
            Assert.That(view, Is.Not.Null, "PF_ReturnReport에 ReturnReportView가 있어야 한다");
            yield return null;
        }

        [TearDown]
        public void TearDown()
        {
            UIManager.DisconnectDay(settlement);
            Object.Destroy(screen);
            Object.Destroy(managers);
        }

        // 1층(독 · 체력)에 다녀와 체력이 모자랐던 성공 귀환 두 건 → 첫 보고가 체력 · 독을 공개, 두 번째 성공으로 2층이 열린다
        private void BeginDayWithTwoSuccesses()
        {
            var log = new VisitResultLog();
            var shortHealth = new[] { 0, 0, 0, 0, 1, 0 };
            var risks = new[] { 1, 0, 0, 0, 1, 0 };
            Assert.That(log.TryRecord("g1", "v1", "하루", 1, "구운 빵", false, shortHealth, risks, new[] { 1, 0, 0, 0, 0, 0 }, out _), Is.True);
            Assert.That(log.TryRecord("g2", "v2", "보리", 1, "구운 빵", false, shortHealth, risks, new[] { 1, 0, 0, 0, 0, 0 }, out _), Is.True);
            Assert.That(settlement.BeginDay(log.Results, new MaterialStock()), Is.True, "보상 · 층 기준으로 하루를 열 수 있어야 한다");
            UIManager.ConnectDay(settlement, null, null);
            screen.GetComponentInChildren<ReturnReportPresenter>(true).Bind(() => settlement.Reports, settlement.IsNewMaterial);
        }

        private bool ShowsText(string part) =>
            screen.GetComponentsInChildren<TMP_Text>(true).Any(t => t.gameObject.activeInHierarchy && t.text != null && t.text.Contains(part));

        [UnityTest]
        public IEnumerator OpeningTheReportConfirmsTheFirstGuestAndWritesRevealedFloorRisks()
        {
            BeginDayWithTwoSuccesses();
            view.Show(new MorningReportData(2, 50, 0));
            yield return null;

            Assert.That(settlement.IsReportConfirmed("v1"), Is.True, "처음 펼친 손님 보고는 확정을 요청한다");
            Assert.That(settlement.IsReportConfirmed("v2"), Is.False, "안 펼친 보고는 아직 확정하지 않는다");
            Assert.That(settlement.FloorOutcomeOf("v1").Revealed, Is.EquivalentTo(new[] { "체력", "독" }));
            Assert.That(ShowsText("1층 슬라임 숲"), Is.True, "수첩 칸에 다녀온 층 이름이 보인다");
            Assert.That(ShowsText("체력 ■") && ShowsText("독 ■"), Is.True, "공개된 위험이 수첩 칸에 보인다");
        }

        [UnityTest]
        public IEnumerator SettlementConfirmsTheRestAndAnnouncesTheOpenedFloorOnce()
        {
            BeginDayWithTwoSuccesses();
            view.Show(new MorningReportData(2, 50, 0));
            yield return null;
            var openSettlement = screen.GetComponentsInChildren<Button>(true).First(b => b.name == "OpenSettlement");
            openSettlement.onClick.Invoke();
            yield return null;

            Assert.That(settlement.IsReportConfirmed("v2"), Is.True, "[최종 정산]은 안 펼친 보고도 확정을 요청한다");
            Assert.That(settlement.OpenFloors, Is.EqualTo(2));
            Assert.That(ShowsText("2층 곰팡이 동굴</color>에 갈 수 있게 됐어요"), Is.True, "새로 연 층이 정산 창에 보인다");

            // 다시 열어도(재열기) 확정은 한 번뿐이고 층은 더 열리지 않는다
            view.Hide();
            view.Show(new MorningReportData(2, 50, 0));
            yield return null;
            Assert.That(settlement.OpenFloors, Is.EqualTo(2), "보고를 다시 열어도 층 진행이 다시 바뀌지 않는다");
            Assert.That(ShowsText("1층 슬라임 숲"), Is.True, "이미 확정된 보고는 다시 열 때도 수첩 칸이 보인다");
        }

        [UnityTest]
        public IEnumerator WithoutTheDayConnectionTheReportLeavesReportsUnconfirmed()
        {
            BeginDayWithTwoSuccesses();
            UIManager.DisconnectDay(settlement);
            view.Show(new MorningReportData(2, 50, 0));
            yield return null;
            Assert.That(settlement.IsReportConfirmed("v1"), Is.False, "연결 전에는 예전처럼 화면이 확정하지 않는다 (준비로 넘어갈 때 백엔드가 확정)");
        }
    }
}
#endif
