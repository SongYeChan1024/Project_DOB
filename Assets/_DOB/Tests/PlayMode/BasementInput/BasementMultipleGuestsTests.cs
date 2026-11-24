#if UNITY_EDITOR
// 여러 손님 통합 테스트 — 실제 Title → 새 게임 경로에서 손님 여러 명의 입장·응대·서빙·대기 이탈·정지 사유·마감을 확인한다.
// 의사코드: 테스트 기준값으로 손님 수·간격·줄·대기 한도를 줄인다. 손님마다 줄 옆에 플레이어를 세우고 E와 같은
// TryInteract로 맞이·좌석 안내·주방을 진행하며, 응대 화면 버튼은 실제 포인터 클릭으로 누른다.
using System.Collections;
using System.Linq;
using System.Reflection;
using DOB.Define;
using DOB.Manager;
using DOB.Systems;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace DOB.Tests
{
    /// <summary>통합 테스트용 영업 손님 기준값을 GuestManager에 넣고 뺀다.</summary>
    internal static class TestGuestRoster
    {
        public static void Use(int guests, float arrivalSeconds = 1f, int queue = 3, float leaveAfter = 600f, float scoreStep = 10f)
        {
            Clear();
            var settings = ScriptableObject.CreateInstance<ServiceRosterSettings>();
            JsonUtility.FromJsonOverwrite(
                $"{{\"baseGuests\":{guests},\"arrivalMinSeconds\":{arrivalSeconds},\"arrivalMaxSeconds\":{arrivalSeconds}," +
                $"\"queueCapacity\":{queue},\"waitStartScore\":2,\"scoreStepSeconds\":{scoreStep},\"leaveAfterSeconds\":{leaveAfter}}}", settings);
            GuestManager.SettingsOverride = settings;
        }

        /// <summary>씬의 호감·입소문 기준 대신 JSON으로 덮어쓴 기준을 쓴다. 적지 않은 값은 💡 시험값(기본값)이다.</summary>
        public static AffinityRules UseAffinityRules(string json)
        {
            var rules = ScriptableObject.CreateInstance<AffinityRules>();
            JsonUtility.FromJsonOverwrite(json, rules);
            RelationshipManager.RulesOverride = rules;
            return rules;
        }

        public static void Clear()
        {
            if (GuestManager.SettingsOverride)
                Object.DestroyImmediate(GuestManager.SettingsOverride);
            GuestManager.SettingsOverride = null;
            if (RelationshipManager.RulesOverride)
                Object.DestroyImmediate(RelationshipManager.RulesOverride);
            RelationshipManager.RulesOverride = null;
        }
    }

    public sealed partial class BasementFirstDayTests
    {
        private sealed class Dining
        {
            public FirstGuestService Service;
            public FirstGuestKitchen Kitchen;
            public GuestManager Guests;
            public RestaurantLayout Layout;
            public BasementHud Hud;
            public CharacterController Player;
        }

        private static Dining FindScene() => new Dining
        {
            Service = Object.FindAnyObjectByType<FirstGuestService>(),
            Kitchen = Object.FindAnyObjectByType<FirstGuestKitchen>(),
            Guests = Object.FindAnyObjectByType<GuestManager>(),
            Layout = Object.FindAnyObjectByType<RestaurantLayout>(),
            Hud = Object.FindAnyObjectByType<BasementHud>(),
            Player = SceneManager.GetSceneByName(BasementComposition.BaseScene).GetRootGameObjects()
                .Single(obj => obj.name == "RuntimeCharacters").transform.Find("PF_Player").GetComponent<CharacterController>()
        };

        [UnityTest]
        public IEnumerator ThreeGuestsArriveAndEndAsServedSentHomeAndWaitedOut()
        {
            TestGuestRoster.Use(guests: 3, leaveAfter: 25f);
            yield return StartFromTitle();
            var s = FindScene();
            var roster = s.Guests.Roster;
            Assert.That(roster.Planned, Is.EqualTo(3));
            yield return Until(() => s.Guests.Agents.Count == 3 && s.Guests.Agents.All(a => a.Move == GuestAgent.Movement.Queued),
                "세 손님이 간격을 두고 입장해 줄에 선다");
            var served = s.Guests.Agents[0];
            var sentHome = s.Guests.Agents[1];
            var waitedOut = s.Guests.Agents[2];
            Assert.That(s.Guests.Agents.Select(a => a.VisitId).Distinct().Count(), Is.EqualTo(3));

            yield return Greet(s, sentHome);
            yield return Click("SendHomeButton");
            yield return Click("YesButton");
            Assert.That(roster.Find(sentHome.VisitId).End, Is.EqualTo(ServiceVisitEnd.SentHome));

            yield return GreetAndSeat(s, served, 0);
            yield return CookAndServe(s, served);
            yield return Until(() => roster.Find(waitedOut.VisitId).End == ServiceVisitEnd.WaitedOut, "세 번째 손님 대기 이탈");
            yield return Until(() => !waitedOut.gameObject.activeSelf, "대기 이탈한 손님이 문으로 나감");

            yield return Until(() => s.Hud.Settle.IsInteractable(), "방명록 완성 후 마감");
            Assert.That(BasementComposition.Current.Game.Phase, Is.EqualTo(DayPhase.Closing));
            Assert.That(roster.AllEnded, Is.True);
            Assert.That(roster.Find(served.VisitId).End, Is.EqualTo(ServiceVisitEnd.Dispatched));
            Assert.That(roster.Find(served.VisitId).WaitScore, Is.Not.Null);
            Assert.That(roster.ServedGuestCount, Is.EqualTo(1));
            Assert.That(Object.FindAnyObjectByType<SettlementManager>().TodayResults.Single().VisitId, Is.EqualTo(served.VisitId));
        }

        [UnityTest]
        public IEnumerator PlatesOnlyGoToTheirOwnSeatAndEveryDispatchedVisitIsReportedNextMorning()
        {
            TestGuestRoster.Use(guests: 2);
            yield return StartFromTitle();
            var s = FindScene();
            yield return Until(() => s.Guests.Agents.Count == 2 && s.Guests.Agents.All(a => a.Move == GuestAgent.Movement.Queued), "두 손님 입장");
            var first = s.Guests.Agents[0];
            var second = s.Guests.Agents[1];
            yield return GreetAndSeat(s, first, 0);
            yield return GreetAndSeat(s, second, 2);

            // 먼저 앉은 손님의 주문부터 굽는다. 그 접시를 다른 손님 자리에 내면 거절되고 접시는 계속 든다.
            yield return CookAndCarry(s);
            Place(s.Player, s.Layout.GetSeat(2).position);
            Assert.That(s.Service.TryInteract(), Is.False, "다른 손님 자리 서빙");
            Assert.That(s.Kitchen.IsCarrying, Is.True);
            Place(s.Player, s.Layout.GetSeat(0).position);
            Assert.That(s.Service.TryInteract(), Is.True, "접시 주인 자리 서빙");
            var settlement = Object.FindAnyObjectByType<SettlementManager>();
            Assert.That(settlement.TodayResults.Single().VisitId, Is.EqualTo(first.VisitId), "그 방문의 탐험 결과만 기록");

            yield return CookAndServe(s, second);
            Assert.That(s.Kitchen.GarlicStock, Is.Zero, "두 주문이 마늘 2개를 나눠 쓴다");
            Assert.That(s.Guests.Roster.ServedGuestCount, Is.EqualTo(2));
            yield return Until(() => s.Hud.Settle.IsInteractable(), "두 손님 파견 후 마감");
            yield return Click("SettleDay");
            yield return Until(() => ReadyDay(2), "2일차 로드");
            var reports = Object.FindAnyObjectByType<SettlementManager>().Reports;
            CollectionAssert.AreEquivalent(new[] { first.VisitId, second.VisitId }, reports.Select(r => r.VisitId).ToArray(),
                "파견한 방문 수만큼 아침 보고");
        }

        [UnityTest]
        public IEnumerator IngredientChoiceKeepsCookingButStopsArrivalAndWaitWhilePauseStopsAll()
        {
            TestGuestRoster.Use(guests: 2, arrivalSeconds: 100f);
            yield return StartFromTitle();
            var s = FindScene();
            yield return Until(() => s.Guests.Agents.Count == 1 && s.Guests.Agents[0].Move == GuestAgent.Movement.Queued, "첫 손님 입장");
            var guest = s.Guests.Agents[0];
            yield return GreetAndSeat(s, guest, 0);
            yield return StartCooking(s);
            var book = (OrderBook)typeof(FirstGuestKitchen).GetField("book", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(s.Kitchen);
            var visit = s.Guests.Roster.Find(guest.VisitId);

            // 재료 선택 사유만 켠다. 레시피 패널이 열려 있으면 GameManager가 재료 선택으로 본다.
            s.Hud.RecipePanel.SetActive(true);
            yield return null;
            float wait = visit.WaitSeconds, arrival = s.Guests.Roster.NextArrivalSeconds, cooking = book.Cooking.RemainingSeconds;
            yield return new WaitForSeconds(0.5f);
            Assert.That(book.Cooking.RemainingSeconds, Is.LessThan(cooking), "재료 선택 중에도 조리는 흐른다");
            Assert.That(visit.WaitSeconds, Is.EqualTo(wait), "대기 시간은 그대로");
            Assert.That(s.Guests.Roster.NextArrivalSeconds, Is.EqualTo(arrival), "입장 간격은 그대로");
            s.Hud.RecipePanel.SetActive(false);

            yield return Press(Key.Escape);
            Assert.That(Time.timeScale, Is.Zero);
            wait = visit.WaitSeconds;
            arrival = s.Guests.Roster.NextArrivalSeconds;
            cooking = book.Cooking.RemainingSeconds;
            for (int i = 0; i < 10; i++)
                yield return null;
            Assert.That(book.Cooking.RemainingSeconds, Is.EqualTo(cooking), "일시 정지 중 조리 정지");
            Assert.That(visit.WaitSeconds, Is.EqualTo(wait), "일시 정지 중 대기 정지");
            Assert.That(s.Guests.Roster.NextArrivalSeconds, Is.EqualTo(arrival), "일시 정지 중 입장 정지");
            yield return Press(Key.Escape);
            Assert.That(Time.timeScale, Is.EqualTo(1f));
        }

        // ── 입소문: 1일차 방문 호감이 2일차 예정 인원을 정한다 ──

        [UnityTest]
        public IEnumerator WellServedGuestsBringMoreGuestsNextDayAndTheReloadedSaveAgrees()
        {
            // 대기 점수가 2점에 머물도록 점수 간격을 길게 두고, 입맛 0점에서도 문턱에 닿도록 방문 최대를 4로 낮춘다.
            // 탐험 등급은 손님 위험이 무작위라 고정할 수 없으므로, 테스트 기준에서는 실패·대실패도 +1로 두어 어떤 등급이든 방문당 3점 이상이 되게 한다.
            TestGuestRoster.Use(guests: 3, scoreStep: 600f);
            var rules = TestGuestRoster.UseAffinityRules("{\"visitMaxPoints\":4,\"failurePoints\":1,\"greatFailurePoints\":1}");
            yield return StartFromTitle();
            var s = FindScene();
            yield return Until(() => s.Guests.Agents.Count == 3 && s.Guests.Agents.All(a => a.Move == GuestAgent.Movement.Queued), "세 손님 입장");
            var agents = s.Guests.Agents.ToArray();
            int[] seats = { 0, 2, 0 };
            for (int i = 0; i < agents.Length; i++)
            {
                yield return GreetAndSeat(s, agents[i], seats[i]);
                yield return CookAndServe(s, agents[i]);
            }
            yield return Until(() => s.Hud.Settle.IsInteractable(), "세 손님 파견 후 마감");

            var grades = Object.FindAnyObjectByType<SettlementManager>().TodayResults.Select(r => r.Grade).ToArray();
            Assert.That(grades.Length, Is.EqualTo(3));
            Assert.That(s.Guests.Roster.Visits.Select(v => v.WaitScore), Has.All.EqualTo(2), "전제: 대기 점수 2");
            int sum = grades.Sum(g => 2 + rules.GradePoints(g));
            int expected = AffinityJudge.ExtraGuests(rules, sum, 3);
            Assert.That(expected, Is.GreaterThanOrEqualTo(2), "방문 최대 4 × 3명 = 12점, 방문당 3점 이상이면 75% 문턱까지 도달");

            yield return Click("SettleDay");
            yield return Until(() => ReadyDay(2), "2일차 로드");
            var relationship = Object.FindAnyObjectByType<RelationshipManager>();
            Assert.That(relationship.WordOfMouthBonus, Is.EqualTo(expected));
            Assert.That(relationship.YesterdayTally, Is.EqualTo((sum, 3)));
            var yesterday = relationship.YesterdayAffinities.Select(a => (a.VisitId, a.Total)).ToArray();
            CollectionAssert.AreEqual(agents.Select(a => a.VisitId), yesterday.Select(a => a.VisitId), "방문마다 기록 하나, 정산 순서");
            yield return OpenServiceAndExpectPlanned(3 + expected);

            // 2일차 시작 자동 저장을 이어하기로 다시 불러와도 같은 기록과 예정 인원을 얻는다.
            yield return LoadTitle();
            yield return Click("ContinueButton");
            yield return Until(() => ReadyDay(2), "이어하기로 2일차 진입");
            relationship = Object.FindAnyObjectByType<RelationshipManager>();
            Assert.That(relationship.WordOfMouthBonus, Is.EqualTo(expected));
            CollectionAssert.AreEqual(yesterday, relationship.YesterdayAffinities.Select(a => (a.VisitId, a.Total)).ToArray());
            yield return OpenServiceAndExpectPlanned(3 + expected);
        }

        [UnityTest]
        public IEnumerator SendingEveryoneHomeLeavesTheNextDayAtTheBaseCount()
        {
            TestGuestRoster.Use(guests: 2);
            yield return StartFromTitle();
            var s = FindScene();
            yield return Until(() => s.Guests.Agents.Count == 2 && s.Guests.Agents.All(a => a.Move == GuestAgent.Movement.Queued), "두 손님 입장");
            foreach (var guest in s.Guests.Agents.ToArray())
            {
                yield return Greet(s, guest);
                yield return Click("SendHomeButton");
                yield return Click("YesButton");
            }
            yield return Until(() => s.Hud.Settle.IsInteractable(), "모두 돌려보낸 뒤 마감");
            yield return Click("SettleDay");
            yield return Until(() => ReadyDay(2), "2일차 로드");
            var relationship = Object.FindAnyObjectByType<RelationshipManager>();
            Assert.That(relationship.YesterdayAffinities.Count, Is.EqualTo(2), "돌려보낸 방문도 기록은 남는다");
            Assert.That(relationship.YesterdayTally, Is.EqualTo((0, 0)), "💡 시험값: 돌려보낸 방문은 입소문에 넣지 않는다");
            Assert.That(relationship.WordOfMouthBonus, Is.Zero);
            yield return OpenServiceAndExpectPlanned(2);
        }

        // 아침 보고를 넘기고 영업을 시작해 명단이 열리면 예정 방문 수를 확인한다.
        private IEnumerator OpenServiceAndExpectPlanned(int planned)
        {
            yield return Click("ConfirmMorningReport");
            yield return Click("StartService");
            var guests = Object.FindAnyObjectByType<GuestManager>();
            yield return Until(() => guests.Roster != null, "영업 명단 열림");
            Assert.That(guests.Roster.Planned, Is.EqualTo(planned));
        }

        // 줄 방향의 옆에 서서 그 손님만 가장 가깝게 만든 뒤 응대를 연다. 줌아웃 중이면 E가 거절되므로 될 때까지 다시 누른다.
        private IEnumerator Greet(Dining s, GuestAgent guest)
        {
            var back = s.Layout.GuestEntrance.position - s.Layout.GreetingPoint.position;
            back.y = 0f;
            var side = Vector3.Cross(Vector3.up, back.normalized);
            if (side.sqrMagnitude < 0.01f) side = Vector3.right;
            yield return Until(() => !s.Hud.Dialogue.BlocksWorldInput, "이전 응대 닫힘");
            Place(s.Player, guest.transform.position + side * .9f);
            yield return Retry(s.Service.TryInteract, "손님 맞이 E");
            yield return Until(DialogueReady, "응대 화면 입력 가능");
            Assert.That(s.Guests.Roster.Find(guest.VisitId).Stage, Is.EqualTo(ServiceVisitStage.Greeting));
        }

        private IEnumerator GreetAndSeat(Dining s, GuestAgent guest, int seatIndex)
        {
            yield return Greet(s, guest);
            yield return Click("FollowButton");
            yield return Until(() => !s.Hud.Dialogue.BlocksWorldInput, "동행 시작");
            var seat = s.Layout.GetSeat(seatIndex);
            Place(s.Player, seat.position + Vector3.back * .6f);
            yield return Until(() => Vector3.Distance(guest.transform.position, seat.position) < 1.6f, "손님이 자리 근처에 도착");
            yield return Retry(s.Service.TryInteract, "자리 안내 E");
            yield return Until(() => guest.Move == GuestAgent.Movement.Seated, "착석");
        }

        private IEnumerator StartCooking(Dining s)
        {
            Place(s.Player, s.Layout.Kitchen.position);
            yield return Retry(s.Service.TryInteract, "주방 E");
            yield return Click("ConfirmCook");
        }

        private IEnumerator CookAndCarry(Dining s)
        {
            yield return StartCooking(s);
            yield return Until(() => s.Service.MealGuidance().Contains("접시 들기"), "조리 완료");
            Assert.That(s.Service.TryInteract(), Is.True, "접시 들기");
            Assert.That(s.Kitchen.IsCarrying, Is.True);
        }

        private IEnumerator CookAndServe(Dining s, GuestAgent guest)
        {
            yield return CookAndCarry(s);
            Place(s.Player, s.Service.SeatOf(guest.VisitId).position);
            Assert.That(s.Service.TryInteract(), Is.True, "서빙");
        }

        // 부수 효과가 있는 요청은 성공할 때까지 프레임마다 한 번씩만 보낸다.
        private static IEnumerator Retry(System.Func<bool> request, string step)
        {
            float deadline = Time.realtimeSinceStartup + 40f;
            bool done = false;
            while (!(done = request()) && Time.realtimeSinceStartup < deadline)
                yield return null;
            Assert.That(done, Is.True, step);
        }
    }
}
#endif
