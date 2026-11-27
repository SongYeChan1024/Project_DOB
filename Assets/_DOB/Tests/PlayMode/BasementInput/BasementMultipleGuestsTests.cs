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
using DOB.UI;
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
                $"{{\"firstArrivalSeconds\":0,\"baseGuests\":{guests},\"arrivalMinSeconds\":{arrivalSeconds},\"arrivalMaxSeconds\":{arrivalSeconds}," +
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

        /// <summary>데모 엔딩 기준(서빙 인원)만 바꾼 하루 기준을 쓴다.</summary>
        public static void UseDemoEnding(int servedGuests)
        {
            var settings = ScriptableObject.CreateInstance<DayCycleSettings>();
            JsonUtility.FromJsonOverwrite("{\"demoEndingServedGuests\":" + servedGuests + "}", settings);
            GameManager.SettingsOverride = settings;
        }

        public static void Clear()
        {
            if (GameManager.SettingsOverride)
                Object.DestroyImmediate(GameManager.SettingsOverride);
            GameManager.SettingsOverride = null;
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
        public IEnumerator IngredientChoiceKeepsEveryClockRunningWhilePauseStopsAll()
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

            // 레시피(재료 선택) 패널을 연다. 시간은 ESC에서만 멈추므로 조리·대기·입장 시계가 모두 흐른다 (F2).
            s.Hud.RecipePanel.SetActive(true);
            yield return null;
            float wait = visit.WaitSeconds, arrival = s.Guests.Roster.NextArrivalSeconds, cooking = book.Cooking.RemainingSeconds;
            yield return new WaitForSeconds(0.5f);
            Assert.That(book.Cooking.RemainingSeconds, Is.LessThan(cooking), "재료 선택 중에도 조리는 흐른다");
            Assert.That(visit.WaitSeconds, Is.GreaterThan(wait), "재료 선택 중에도 대기 시간은 흐른다");
            Assert.That(s.Guests.Roster.NextArrivalSeconds, Is.LessThan(arrival), "재료 선택 중에도 입장 간격은 흐른다");
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
            // 대기 점수가 2점에 머물도록 점수 간격을 길게 두고, 방문 최대를 4로 낮춘다.
            // 탐험 등급과 입맛(−2~+2)은 손님마다 무작위라 고정할 수 없으므로, 테스트 기준에서는 모든 등급을 +3으로 두어 방문당 3점 이상(75% 문턱)이 되게 한다.
            TestGuestRoster.Use(guests: 3, scoreStep: 600f);
            // 대실패한 손님이 부상으로 쉬면 다음 날 인원이 달라지므로 이 테스트에서는 휴식 일수를 0으로 둔다.
            var rules = TestGuestRoster.UseAffinityRules("{\"visitMaxPoints\":4,\"greatSuccessPoints\":3,\"successPoints\":3,\"failurePoints\":3,\"greatFailurePoints\":3,\"injuryRestDays\":0}");
            yield return StartFromTitle();
            var s = FindScene();
            yield return Until(() => s.Guests.Agents.Count == 3 && s.Guests.Agents.All(a => a.Move == GuestAgent.Movement.Queued), "세 손님 입장");
            var agents = s.Guests.Agents.ToArray();
            var firstDay = agents.Select(a => a.GuestId + ":" + a.Visit.GuestName).ToArray();
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
            var tastes = s.Guests.Roster.Visits.Select(v => v.TasteScore).ToArray();
            Assert.That(tastes, Has.All.Not.Null, "서빙한 방문마다 입맛 점수가 기록된다");
            int sum = grades.Sum(g => 2 + rules.GradePoints(g)) + tastes.Sum(t => t.Value);
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
            // 잘 대접한 전날 손님 셋이 같은 손님 ID·이름으로 다시 오고, 나머지는 입소문 신규다.
            Assert.That(relationship.Guests.Count, Is.EqualTo(3));
            Assert.That(relationship.Guests.All(g => !g.Rejected && g.Affinity >= 3 && g.ReturnDay == 2), Is.True);
            var secondDay = RosterKey();
            CollectionAssert.IsSubsetOf(firstDay, secondDay.Select(k => k.Substring(0, k.LastIndexOf('/'))).ToArray(), "재방문 손님의 ID·이름 유지");
            Assert.That(secondDay.Length, Is.EqualTo(3 + expected));

            // 2일차 시작 자동 저장을 이어하기로 다시 불러와도 같은 기록과 예정 인원을 얻는다.
            yield return LoadTitle();
            yield return Click("ContinueButton");
            yield return Until(() => ReadyDay(2), "이어하기로 2일차 진입");
            relationship = Object.FindAnyObjectByType<RelationshipManager>();
            Assert.That(relationship.WordOfMouthBonus, Is.EqualTo(expected));
            CollectionAssert.AreEqual(yesterday, relationship.YesterdayAffinities.Select(a => (a.VisitId, a.Total)).ToArray());
            yield return OpenServiceAndExpectPlanned(3 + expected);
            CollectionAssert.AreEqual(secondDay, RosterKey(), "같은 스냅샷이면 신규 손님의 신원과 전체 입장 순서가 같다");
        }

        // 그날 명단을 입장 순서대로 「손님ID:이름/종족·직업」으로 적는다.
        private static string[] RosterKey() => Object.FindAnyObjectByType<GuestManager>().TodaysGuests
            .Select(g => g.GuestId + ":" + g.Name + "/" + g.Race + "·" + g.Job).ToArray();

        [UnityTest]
        public IEnumerator SendingEveryoneHomeRejectsThemAndTheNextDayKeepsTheMinimum()
        {
            TestGuestRoster.Use(guests: 2);
            yield return StartFromTitle();
            var s = FindScene();
            yield return Until(() => s.Guests.Agents.Count == 2 && s.Guests.Agents.All(a => a.Move == GuestAgent.Movement.Queued), "두 손님 입장");
            var sentHome = s.Guests.Agents.Select(a => a.GuestId).ToArray();
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
            Assert.That(relationship.YesterdayTally, Is.EqualTo((-8, 2)), "돌려보낸 방문도 입소문 N에 1명·S에 −4로 한 번씩 들어간다 (2026-11-25 확정)");
            Assert.That(relationship.WordOfMouthBonus, Is.Zero);
            // 처음 온 손님을 돌려보내면 누적 −4로 영구 거절된다. 다시 오는 손님도 입소문 신규도 없으므로 💡 최소 인원(1명)만 온다.
            Assert.That(relationship.Guests.Select(g => (g.GuestId, g.Rejected)), Is.EquivalentTo(sentHome.Select(id => (id, true))));
            Assert.That(relationship.GuestsComingToday, Is.Empty);
            yield return OpenServiceAndExpectPlanned(1);
            CollectionAssert.DoesNotContain(sentHome, Object.FindAnyObjectByType<GuestManager>().TodaysGuests.Single().GuestId, "돌려보낸 손님은 다시 오지 않는다");
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

        [UnityTest]
        public IEnumerator AManualSaveDuringServiceResumesTheSameVisitsSeatAndCooking()
        {
            TestGuestRoster.Use(guests: 2);
            yield return StartFromTitle();
            var s = FindScene();
            yield return Until(() => s.Guests.Agents.Count == 2 && s.Guests.Agents.All(a => a.Move == GuestAgent.Movement.Queued), "두 손님 입장");
            var first = s.Guests.Agents[0];
            var second = s.Guests.Agents[1];
            yield return GreetAndSeat(s, first, 0);
            yield return CookAndServe(s, first);
            yield return GreetAndSeat(s, second, 2);
            yield return StartCooking(s);

            // 같은 프레임에서 저장하고 값을 적어 둔다(시계가 흐르기 전).
            BasementComposition.Current.Game.SaveManual();
            var cooking = s.Kitchen.Orders.Single(o => o.State == FirstGuestOrderState.Cooking);
            Assert.That(cooking.VisitId, Is.EqualTo(second.VisitId));
            float remaining = cooking.RemainingSeconds;
            int gold = s.Kitchen.Gold, garlic = s.Kitchen.GarlicStock;
            var visits = s.Guests.Roster.Visits.Select(v => v.VisitId).ToArray();

            yield return LoadTitle();
            yield return Click("ContinueButton");
            yield return Until(() => ReadyDay(1) && FindScene().Kitchen.Orders.Count > 0, "수동 저장의 영업 진행 복원");
            s = FindScene();
            CollectionAssert.AreEqual(visits, s.Guests.Roster.Visits.Select(v => v.VisitId).ToArray(), "같은 방문 ID로 이어진다");
            Assert.That(s.Guests.Roster.Find(visits[0]).End, Is.EqualTo(ServiceVisitEnd.Dispatched));
            Assert.That(s.Guests.Find(visits[0]) == null || !s.Guests.Find(visits[0]).gameObject.activeSelf, Is.True, "떠난 손님은 다시 보이지 않는다");
            var seated = s.Guests.Find(visits[1]);
            Assert.That((seated.Move, seated.SeatIndex), Is.EqualTo((GuestAgent.Movement.Seated, 2)), "앉아 있던 손님은 같은 자리");
            var order = s.Kitchen.Orders.Single(o => o.VisitId == visits[1]);
            Assert.That(order.State, Is.EqualTo(FirstGuestOrderState.Cooking));
            Assert.That(order.RemainingSeconds, Is.LessThanOrEqualTo(remaining).And.GreaterThan(remaining - 1f), "조리 남은 시간 이어짐");
            Assert.That((s.Kitchen.Gold, s.Kitchen.GarlicStock), Is.EqualTo((gold, garlic)));

            // 이어서 서빙하고 마감하면 앞서 파견한 방문은 한 번만 정산된다.
            Place(s.Player, s.Layout.Kitchen.position);
            yield return Until(() => s.Service.MealGuidance().Contains("접시 들기"), "조리 완료");
            Assert.That(s.Service.TryInteract(), Is.True, "접시 들기");
            Place(s.Player, s.Service.SeatOf(visits[1]).position);
            Assert.That(s.Service.TryInteract(), Is.True, "서빙");
            yield return Until(() => s.Hud.Settle.IsInteractable(), "마감");
            CollectionAssert.AreEquivalent(visits, Object.FindAnyObjectByType<SettlementManager>().TodayResults.Select(r => r.VisitId));
            Assert.That(s.Guests.Roster.ServedGuestCount, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator AManualSaveDuringTheMorningReportKeepsWhatWasReceivedAndPaysEachReportOnce()
        {
            // FeatureManagerMap 「저장 · F13」 보고 A·B 사례 2·3.
            var a = new VisitResult("guest-a", "visit-a", "하루", 1, "구운 빵", true, new[] { 0, 0, 0, 0, 0, 0 }, ExpeditionGrade.Success, new[] { "들풀", "들풀" });
            var b = new VisitResult("guest-b", "visit-b", "바람", 1, "구운 빵", true, new[] { 0, 0, 0, 0, 0, 0 }, ExpeditionGrade.Success, new[] { "숲버섯" });
            new FirstServiceSaveStore(directory).SavePreparation(FirstServiceSaveSlot.Auto, 2, 59, Garlic(1), null, new[] { a, b });
            yield return LoadTitle();
            yield return Click("ContinueButton");
            yield return Until(() => ReadyDay(2), "2일차 아침");
            var settlement = Object.FindAnyObjectByType<SettlementManager>();
            Assert.That(settlement.ConfirmReport("visit-a"), Is.EqualTo(ReportConfirmResult.Confirmed));
            BasementComposition.Current.Game.SaveManual();
            Assert.That(settlement.ConfirmReport("visit-b"), Is.EqualTo(ReportConfirmResult.Confirmed), "저장 뒤 B까지 받는다");

            yield return LoadTitle();
            yield return Click("ContinueButton");
            yield return Until(() => ReadyDay(2), "수동 저장으로 2일차 아침");
            settlement = Object.FindAnyObjectByType<SettlementManager>();
            var economy = Object.FindAnyObjectByType<EconomyManager>();
            Assert.That(BasementComposition.Current.Game.Phase, Is.EqualTo(DayPhase.MorningReport));
            Assert.That((settlement.IsReportConfirmed("visit-a"), settlement.IsReportConfirmed("visit-b")), Is.EqualTo((true, false)));
            Assert.That((economy.Materials.Count("들풀"), economy.Materials.Count("숲버섯")), Is.EqualTo((2, 0)), "A만 받은 재고");
            yield return Click("ConfirmMorningReport");
            Assert.That((economy.Materials.Count("들풀"), economy.Materials.Count("숲버섯")), Is.EqualTo((2, 1)), "B는 한 번만 더해진다");
            // 층 진행도 보고마다 한 번씩: 1층 성공 A·B 두 번으로 2층이 열리고, A가 1층의 미확인 축(독·체력)을 공개했다.
            Assert.That(settlement.OpenFloors, Is.EqualTo(2), "다시 불러와도 A를 두 번 세지 않는다");
            Assert.That(settlement.FloorOutcomeOf("visit-a").Revealed, Is.EqualTo(new[] { "독", "체력" }));
            Assert.That((settlement.FloorOutcomeOf("visit-b").Revealed.Count, settlement.FloorOutcomeOf("visit-b").OpenedFloor), Is.EqualTo((0, (int?)2)));

            // 준비 단계에서 저장하고 불러오면 준비 단계에서 이어지고 다시 받지 않는다.
            BasementComposition.Current.Game.SaveManual();
            yield return LoadTitle();
            yield return Click("ContinueButton");
            yield return Until(() => ReadyDay(2), "수동 저장으로 2일차 준비");
            economy = Object.FindAnyObjectByType<EconomyManager>();
            Assert.That(BasementComposition.Current.Game.Phase, Is.EqualTo(DayPhase.Preparation));
            Assert.That((economy.Materials.Count("들풀"), economy.Materials.Count("숲버섯")), Is.EqualTo((2, 1)));
        }

        [UnityTest]
        public IEnumerator SpicesBoughtInPreparationAndFloorProgressCarryToTheNextDay()
        {
            // shop-economy·floor-progress 명세: 준비 단계에서 산 양념과 아침 보고의 층 진행이 정산을 거쳐 다음 일차 자동 슬롯에 남는다.
            TestGuestRoster.Use(guests: 1);
            var report = new VisitResult("guest-a", "visit-a", "하루", 1, "구운 빵", true, new[] { 0, 0, 0, 0, 1, 0 }, ExpeditionGrade.Success, new string[0]);
            new FirstServiceSaveStore(directory).SavePreparation(FirstServiceSaveSlot.Auto, 2, 59, Garlic(1), null, new[] { report });
            yield return LoadTitle();
            yield return Click("ContinueButton");
            yield return Until(() => ReadyDay(2), "2일차 아침");
            var economy = Object.FindAnyObjectByType<EconomyManager>();
            var salt = new System.Collections.Generic.Dictionary<string, int> { ["salt"] = 2 };
            Assert.That(economy.Buy(salt).Result, Is.EqualTo(PurchaseResult.MarketClosed), "아침 보고 중에는 시장이 닫혀 있다");
            yield return Click("ConfirmMorningReport");
            var bought = economy.Buy(salt);
            Assert.That((bought.Result, bought.Gold, economy.SpiceCount("salt")), Is.EqualTo((PurchaseResult.Purchased, 55, 2)));
            Assert.That(FindScene().Kitchen.Gold, Is.EqualTo(55), "주방은 경제의 금화를 그대로 본다");

            yield return Click("StartService");
            Assert.That(economy.Buy(salt).Result, Is.EqualTo(PurchaseResult.MarketClosed), "영업 중 구매");
            var s = FindScene();
            yield return Until(() => s.Guests.Agents.Count == 1 && s.Guests.Agents[0].Move == GuestAgent.Movement.Queued, "손님 입장");
            yield return GreetAndSeat(s, s.Guests.Agents[0], 0);
            yield return CookAndServe(s, s.Guests.Agents[0]);
            int goldAfterService = economy.Gold;
            Assert.That(goldAfterService, Is.GreaterThan(55));
            yield return Until(() => s.Hud.Settle.IsInteractable(), "마감");
            yield return Click("SettleDay");
            yield return Until(() => ReadyDay(3), "3일차");

            Assert.That(new FirstServiceSaveStore(directory).TryLoad(FirstServiceSaveSlot.Auto, out var next), Is.True);
            Assert.That((next.Day, next.Gold), Is.EqualTo((3, goldAfterService)));
            Assert.That(next.Spices.Single(x => x.Id == "salt").Count, Is.EqualTo(2), "산 소금(영업에서 쓰지 않음)");
            Assert.That((next.Floors.OpenFloors, next.Floors.DeepestWins), Is.EqualTo((1, 1)));
            Assert.That(next.Floors.Revealed.Single().Axes, Is.EqualTo(new[] { "독", "체력" }), "부족했던 체력부터, 남은 칸은 독");
            Assert.That(Object.FindAnyObjectByType<EconomyManager>().SpiceCount("salt"), Is.EqualTo(2), "다음 날 경제가 저장된 양념으로 열린다");
            Assert.That(Object.FindAnyObjectByType<SettlementManager>().IsRiskRevealed(1, "체력"), Is.True);
        }

        [UnityTest]
        public IEnumerator TheDemoEndingKeepsTheDayStartAndReplayingItOverwritesNoSlot()
        {
            TestGuestRoster.Use(guests: 1);
            TestGuestRoster.UseDemoEnding(1);
            yield return StartFromTitle();
            yield return ServeTheOnlyGuestAndSettle();
            yield return Until(() => BasementComposition.Current.Game.Phase == DayPhase.DemoEnding, "데모 엔딩");
            var saves = new FirstServiceSaveStore(directory);
            Assert.That(saves.TryLoad(FirstServiceSaveSlot.DemoComplete, out var demo), Is.True, "엔딩을 본 일차의 시작 보관");
            Assert.That((demo.Day, demo.Gold, demo.Progress), Is.EqualTo((1, 50, (DayProgress)null)));
            Assert.That(saves.TryLoad(FirstServiceSaveSlot.Auto, out var auto) && auto.Day == 2, Is.True, "정산은 평소처럼 다음 일차를 자동 저장");
            string autosave = System.IO.Path.Combine(directory, "autosave.json");
            byte[] autoBytes = System.IO.File.ReadAllBytes(autosave);

            // 데모 완료 슬롯으로 그 하루를 다시 플레이하면 마감 뒤 다음 일차 없이 끝나고 어떤 슬롯도 바뀌지 않는다.
            yield return LoadTitle();
            Assert.That(BasementComposition.EnterDay(demo), Is.True);
            yield return Until(() => ReadyDay(1), "데모 완료 슬롯으로 1일차");
            Assert.That(FindScene().Kitchen.Gold, Is.EqualTo(50));
            Assert.Throws<System.IO.IOException>(() => BasementComposition.Current.Game.SaveManual());
            yield return ServeTheOnlyGuestAndSettle();
            yield return Until(() => BasementComposition.Current.Game.Phase == DayPhase.DemoEnding, "다시 플레이한 하루도 엔딩");
            CollectionAssert.AreEqual(autoBytes, System.IO.File.ReadAllBytes(autosave), "자동 슬롯 그대로");
            Assert.That(System.IO.File.Exists(System.IO.Path.Combine(directory, "manual.json")), Is.False);
            Assert.That(BasementComposition.Current.Game.Day, Is.EqualTo(1));
        }

        private IEnumerator ServeTheOnlyGuestAndSettle()
        {
            var s = FindScene();
            yield return Until(() => s.Guests.Agents.Count == 1 && s.Guests.Agents[0].Move == GuestAgent.Movement.Queued, "손님 입장");
            yield return GreetAndSeat(s, s.Guests.Agents[0], 0);
            yield return CookAndServe(s, s.Guests.Agents[0]);
            yield return Until(() => s.Hud.Settle.IsInteractable(), "마감");
            yield return Click("SettleDay");
        }

        [UnityTest]
        public IEnumerator ServiceScreenFeedsReadTheRealOrdersBurnerMoodsAndPrompt()
        {
            // service-presentation-feeds 명세: 화면 Presenter 없이 UIManager에 연결된 제공자 값을 직접 읽는다.
            TestGuestRoster.Use(guests: 2);
            yield return StartFromTitle();
            var s = FindScene();
            var status = UIManager.ServiceStatus;
            var burners = UIManager.KitchenBurners;
            var orders = UIManager.KitchenOrders;
            var moods = UIManager.GuestMoods;
            var prompt = UIManager.InteractionPrompt;
            Assert.That(new object[] { status, burners, orders, moods, prompt }, Has.All.Not.Null, "영업 중 다섯 제공자 연결");
            Assert.That(status.Day, Is.EqualTo(1));
            yield return Until(() => s.Guests.Agents.Count == 2 && s.Guests.Agents.All(a => a.Move == GuestAgent.Movement.Queued), "두 손님 입장");
            var first = s.Guests.Agents[0];
            var second = s.Guests.Agents[1];
            Assert.That(status.GuestsArrived, Is.EqualTo(2));
            Assert.That(moods.StateAt(0), Is.EqualTo(GuestMoodState.Waiting));
            Assert.That(moods.PatienceLeftAt(0), Is.InRange(0.01f, 1f));
            Assert.That(moods.WaitScoreAt(0), Is.EqualTo(s.Guests.Roster.WaitScoreNow(s.Guests.Roster.Find(first.VisitId))));

            yield return GreetAndSeat(s, first, 0);
            yield return GreetAndSeat(s, second, 2);
            Assert.That(status.OrderCount, Is.EqualTo(2));
            Assert.That((status.OrderIdAt(0), status.OrderIdAt(1)), Is.EqualTo((first.VisitId, second.VisitId)), "착석 순서");
            Place(s.Player, s.Layout.Kitchen.position);
            Assert.That((prompt.Target, prompt.Action), Is.EqualTo((s.Layout.Kitchen, "재료 확인")));

            yield return StartCooking(s);
            Assert.That((status.StateAt(0), status.StateAt(1)), Is.EqualTo((OrderTicketState.Cooking, OrderTicketState.Waiting)));
            Assert.That((burners.Count, burners.StateAt(0), burners.DishIdAt(0), burners.CookSecondsAt(0)), Is.EqualTo((1, BurnerState.Cooking, "Bread", 6f)), "시작 화구는 한 칸");
            Assert.That(orders.OrderId, Is.EqualTo(first.VisitId), "빈 화구가 없으면 조리 중이 대기보다 먼저");
            int garlic = orders.GarlicStock;
            Assert.That(orders.TryStartCooking(second.VisitId, orders.FlourRequired, orders.GarlicRequired), Is.EqualTo(CookStartResult.BurnerBusy));
            Assert.That(orders.TryPickUp(first.VisitId), Is.EqualTo(PickUpResult.NotCooked));
            Assert.That((orders.GarlicStock, status.HeldOrderId), Is.EqualTo((garlic, (string)null)), "거절이면 그대로");

            yield return Until(() => burners.StateAt(0) == BurnerState.Done, "완성");
            Assert.That((orders.OrderId, orders.State), Is.EqualTo((first.VisitId, (FirstGuestOrderState?)FirstGuestOrderState.Ready)));
            Assert.That(prompt.Action, Is.EqualTo("접시 들기"));
            Assert.That(orders.TryPickUp(first.VisitId), Is.EqualTo(PickUpResult.PickedUp));
            Assert.That((status.HeldOrderId, status.HeldDishId), Is.EqualTo((first.VisitId, "Bread")));
            Assert.That(orders.TryPickUp(first.VisitId), Is.EqualTo(PickUpResult.AlreadyHolding));
            Assert.That(orders.TryStartCooking(second.VisitId, orders.FlourRequired - 1, orders.GarlicRequired), Is.EqualTo(CookStartResult.WrongIngredients));
            Assert.That(s.Kitchen.Orders.Single(o => o.VisitId == second.VisitId).State, Is.EqualTo(FirstGuestOrderState.Waiting));
            Assert.That(orders.TryStartCooking(second.VisitId, orders.FlourRequired, orders.GarlicRequired), Is.EqualTo(CookStartResult.Started), "접시를 들면 칸이 빈다");

            int gold = status.Gold;
            // 요리 화면의 예상 대금은 화면이 다루는 주문(둘째 손님) 기준이라, 서빙하는 첫 손님의 대금은 같은 식으로 따로 구한다.
            var market = MarketRules.Load();
            int price = PriceJudge.Payment(market, 8, s.Guests.FlavorScoreFor(market, s.Guests.Roster.Find(first.VisitId).GuestId, "구운 빵", new[] { "마늘" }), 1);
            Assert.That(orders.Price(true), Is.EqualTo(PriceJudge.Payment(market, 8, s.Guests.FlavorScoreFor(market, s.Guests.Roster.Find(second.VisitId).GuestId, "구운 빵", new[] { "마늘" }), 1)));
            var seat = s.Service.SeatOf(first.VisitId);
            Place(s.Player, seat.position);
            Assert.That((prompt.Target, prompt.Action), Is.EqualTo((seat, "서빙")));
            Assert.That(s.Service.TryInteract(), Is.True, "[E] 표시와 같은 판정으로 실제 서빙");
            Assert.That(status.Gold, Is.EqualTo(gold + price));
            Assert.That((status.OrderCount, status.OrderIdAt(0), status.HeldOrderId), Is.EqualTo((1, second.VisitId, (string)null)));
            Assert.That(moods.StateAt(0), Is.EqualTo(GuestMoodState.Served));
            yield return Until(() => moods.StateAt(0) != GuestMoodState.Served, "기쁨 표정 시간이 지남");
            Assert.That(moods.StateAt(0), Is.EqualTo(GuestMoodState.Leaving).Or.EqualTo(GuestMoodState.None));
            int taste = s.Guests.Roster.Find(first.VisitId).TasteScore ?? 0;
            Assert.That(moods.ReactionAt(0), Is.EqualTo(taste > 0 ? TasteReaction.Like : taste < 0 ? TasteReaction.Dislike : TasteReaction.Neutral));

            yield return LoadTitle();
            Assert.That(new object[] { UIManager.ServiceStatus, UIManager.KitchenBurners, UIManager.KitchenOrders, UIManager.GuestMoods, UIManager.InteractionPrompt },
                Has.All.Null, "영업이 꺼지면 모두 해제");
        }

        [UnityTest]
        public IEnumerator ASentHomeGuestLeavesDispleased()
        {
            TestGuestRoster.Use(guests: 2);
            yield return StartFromTitle();
            var s = FindScene();
            yield return Until(() => s.Guests.Agents.Count == 2 && s.Guests.Agents.All(a => a.Move == GuestAgent.Movement.Queued), "두 손님 입장");
            // 1일차 첫 손님은 돌려보낼 수 없으므로 둘째 손님을 돌려보낸다.
            yield return Greet(s, s.Guests.Agents[1]);
            Assert.That(UIManager.InteractionPrompt.Target, Is.Null, "대화 중에는 대상이 없다");
            yield return Click("SendHomeButton");
            yield return Click("YesButton");
            var moods = UIManager.GuestMoods;
            Assert.That((moods.StateAt(1), moods.ReactionAt(1)), Is.EqualTo((GuestMoodState.Leaving, TasteReaction.Dislike)));
        }

        // kitchen-burners: 시작 1칸, 준비 단계에서만 💡 금화 60으로 두 번째 화구를 산다. 산 화구는 수동 저장을 불러와도 남고, 영업에서 두 칸이 된다.
        [UnityTest]
        public IEnumerator ASecondBurnerIsBoughtInPreparationAndSurvivesAManualSave()
        {
            new FirstServiceSaveStore(directory).SavePreparation(FirstServiceSaveSlot.Auto, 2, 70, Garlic(1));
            yield return LoadTitle();
            yield return Click("ContinueButton");
            yield return Until(() => ReadyDay(2), "이어하기로 2일차 진입");
            var game = BasementComposition.Current.Game;
            var economy = Object.FindAnyObjectByType<EconomyManager>();
            Assert.That((game.OwnedBurners, game.NextBurnerPrice), Is.EqualTo((1, (int?)60)));
            Assert.That(game.TryBuyBurner(), Is.EqualTo(PurchaseResult.MarketClosed), "아침 보고 중에는 살 수 없다");
            yield return Click("ConfirmMorningReport");
            Assert.That(game.Phase, Is.EqualTo(DayPhase.Preparation));
            Assert.That(game.TryBuyBurner(), Is.EqualTo(PurchaseResult.Purchased));
            Assert.That((game.OwnedBurners, economy.Gold, game.NextBurnerPrice), Is.EqualTo((2, 10, (int?)null)));
            Assert.That(game.TryBuyBurner(), Is.EqualTo(PurchaseResult.NotForSale), "최대 화구");
            Assert.That(economy.Gold, Is.EqualTo(10));

            game.SaveManual();
            yield return LoadTitle();
            yield return Click("ContinueButton");
            yield return Until(() => ReadyDay(2), "수동 저장 이어하기");
            game = BasementComposition.Current.Game;
            Assert.That((game.Phase, game.OwnedBurners), Is.EqualTo((DayPhase.Preparation, 2)), "산 화구는 불러와도 남는다");
            yield return Click("StartService");
            yield return Until(() => UIManager.KitchenBurners != null, "영업 화구 표시 연결");
            Assert.That(UIManager.KitchenBurners.Count, Is.EqualTo(2));
            Assert.That(UIManager.KitchenBurners.BurnerAt(1), Is.Not.EqualTo(UIManager.KitchenBurners.BurnerAt(0)), "두 칸의 표시 위치가 겹치지 않는다");
        }

        // sound-playback: 성공 결과에만 한 번씩 울리고, 씬마다 곡은 하나이며, ESC 일시 정지 중 BGM은 💡 0.4배로 줄어든다.
        [UnityTest]
        public IEnumerator SoundsFollowOnlySuccessesAndEachSceneKeepsOneBgm()
        {
            var played = new System.Collections.Generic.List<SoundCue>();
            void Record(SoundCue cue) => played.Add(cue);
            int Count(SoundCue cue) => played.Count(c => c == cue);
            SoundManager.Played += Record;
            try
            {
                TestGuestRoster.Use(guests: 2);
                yield return StartFromTitle();
                Assert.That((SoundManager.CurrentTrack, SoundManager.BgmSourceCount), Is.EqualTo(("Service", 1)), "함께 열리는 Restaurant·BasementUI는 곡을 바꾸지 않는다");
                var s = FindScene();
                yield return Until(() => s.Guests.Agents.Count == 2 && s.Guests.Agents.All(a => a.Move == GuestAgent.Movement.Queued), "두 손님 입장");
                Assert.That(Count(SoundCue.GuestEntered), Is.EqualTo(2));

                yield return GreetAndSeat(s, s.Guests.Agents[0], 0);
                yield return CookAndCarry(s);
                Assert.That(Count(SoundCue.CookFinished), Is.EqualTo(1));
                Place(s.Player, s.Layout.GetSeat(2).position);
                Assert.That(s.Service.TryInteract(), Is.False, "빈 자리 서빙");
                Assert.That((Count(SoundCue.Served), Count(SoundCue.GoldEarned)), Is.EqualTo((0, 0)), "실패한 서빙은 소리 없음");
                Place(s.Player, s.Layout.GetSeat(0).position);
                Assert.That(s.Service.TryInteract(), Is.True);
                Assert.That((Count(SoundCue.Served), Count(SoundCue.GoldEarned)), Is.EqualTo((1, 1)));

                float bgm = GameSettings.GetVolume(GameSettings.Volume.Bgm);
                yield return Press(Key.Escape);
                Assert.That(Time.timeScale, Is.Zero);
                yield return null;
                Assert.That(SoundManager.BgmVolume, Is.EqualTo(bgm * SoundCatalog.Load().PauseBgmScale).Within(1e-4f));
                yield return Press(Key.Escape);
                yield return null;
                Assert.That(SoundManager.BgmVolume, Is.EqualTo(bgm).Within(1e-4f));
                Assert.That(SoundCatalog.Load().Validate(), Is.Null);
                yield return LoadTitle();
                Assert.That((SoundManager.CurrentTrack, SoundManager.BgmSourceCount), Is.EqualTo(("Title", 1)), "타이틀로 돌아가면 타이틀 곡");
            }
            finally
            {
                SoundManager.Played -= Record;
            }
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
