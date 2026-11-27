// TasteTests — 입맛 채점(taste-scoring) 명세의 태그 고정·이전 저장 보충·접시 채점 시나리오를 검증한다.
// 의사코드: Resources의 SO_TasteRules(💡 기획 표 값)를 읽어 TasteDraw·TasteJudge·GuestLedger를 직접 부른다.
using System.Linq;
using DOB.Define;
using DOB.Systems;
using NUnit.Framework;
using UnityEngine;

namespace DOB.Tests
{
    public sealed class TasteTests
    {
        private static TasteRules Rules()
        {
            var rules = Resources.Load<TasteRules>(TasteRules.ResourceName);
            Assert.That(rules, Is.Not.Null, "Resources/SO_TasteRules");
            return rules;
        }

        [Test]
        public void TheTasteRulesAssetIsValid() => Assert.That(Rules().Validate(), Is.Null);

        [Test]
        public void ARaceWithASignatureKeepsItAndAddsADifferentTaste()
        {
            for (int i = 0; i < 50; i++)
            {
                var tastes = TasteDraw.Draw(Rules(), "guest" + i, "하플링");
                Assert.That(tastes[0], Is.EqualTo("달콤"));
                Assert.That(tastes[1], Is.Not.EqualTo("달콤"));
                Assert.That(Rules().Flavors, Does.Contain(tastes[1]));
            }
        }

        [Test]
        public void AHumanGetsTwoDifferentRandomTastes()
        {
            for (int i = 0; i < 50; i++)
            {
                var tastes = TasteDraw.Draw(Rules(), "guest" + i, "인간");
                Assert.That(tastes.Length, Is.EqualTo(2));
                Assert.That(tastes[0], Is.Not.EqualTo(tastes[1]));
            }
        }

        [Test]
        public void TheSameGuestIdAlwaysGetsTheSameTastes() =>
            Assert.That(TasteDraw.Draw(Rules(), "abc123", "인간"), Is.EqualTo(TasteDraw.Draw(Rules(), "abc123", "인간")));

        [Test]
        public void ALedgerFillsMissingTastesOfAnOlderSaveOnce()
        {
            var old = new GuestRecord("g1", "하루", "수인", "사제", null, 3);
            var first = new GuestLedger(new[] { old }, Rules()).Find("g1");
            var again = new GuestLedger(new[] { old }, Rules()).Find("g1");
            Assert.That(first.Tastes.Count, Is.EqualTo(2));
            Assert.That(first.Tastes[0], Is.EqualTo("고소"), "수인 대표 맛");
            Assert.That(again.Tastes, Is.EqualTo(first.Tastes), "다시 읽어도 같다");
            Assert.That(first.Affinity, Is.EqualTo(3), "장기 상태는 그대로");
        }

        [Test]
        public void ExistingTastesAreKeptAndSurviveSettlement()
        {
            var rules = ScriptableObject.CreateInstance<AffinityRules>();
            try
            {
                var guest = new GuestRecord("g1", "하루", "인간", "전사", null, 0, false, 1, new[] { "쌉쌀", "쿰쿰" });
                var ledger = new GuestLedger(new[] { guest }, Rules());
                ledger.SettleDay(rules, 1, null, new[] { new VisitAffinity("g1", "v1", ServiceVisitEnd.Dispatched, 0, 0, 0, 2) }, null);
                Assert.That(ledger.Find("g1").Tastes, Is.EqualTo(new[] { "쌉쌀", "쿰쿰" }));
                Assert.That(ledger.Find("g1").Affinity, Is.EqualTo(2));
            }
            finally { Object.DestroyImmediate(rules); }
        }

        [TestCase(new[] { "고소", "향긋" }, true, 2)]
        [TestCase(new[] { "고소", "달콤" }, false, 0)]
        [TestCase(new[] { "진한", "달콤" }, false, -2)]
        public void TasteScoreCountsTheGuestsTastesInTheDish(string[] guest, bool garlic, int expected)
        {
            var dish = TasteJudge.DishFlavors(Rules(), "구운 빵", garlic ? new[] { "마늘" } : null);
            Assert.That(TasteJudge.Score(guest, dish), Is.EqualTo(expected));
        }

        [Test]
        public void ANewGuestDrawnForTheDayHasTastes()
        {
            var settings = ScriptableObject.CreateInstance<ServiceRosterSettings>();
            var pool = Resources.Load<GuestArrivalPool>(GuestArrivalPool.ResourceName);
            try
            {
                var roster = DailyRoster.Compose(settings, pool, 1, null, null, 0, new System.Random(3), Rules());
                Assert.That(roster, Is.Not.Empty);
                Assert.That(roster.All(g => g.Tastes.Count == 2), Is.True);
                Assert.That(roster[0].Tastes, Is.EqualTo(TasteDraw.Draw(Rules(), roster[0].GuestId, roster[0].Race)));
            }
            finally { Object.DestroyImmediate(settings); }
        }
    }
}
