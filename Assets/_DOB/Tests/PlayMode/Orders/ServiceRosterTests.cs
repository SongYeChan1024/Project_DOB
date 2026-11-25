// ServiceRosterTests — 정지 사유와 하루 방문 명단(입장 일정·입구 줄·단계 전이·대기 시계·종료·서빙 인원)을 검증한다.
// 의사코드: JsonUtility로 시험 기준값(3명, 8~20초, 줄 2명, 2점, 10초, 30초)을 만들고, 시간·난수를 주입해
// service-roster·service-clocks 명세의 시나리오를 그대로 따라간다.
using System;
using System.Collections.Generic;
using DOB.Define;
using DOB.Systems;
using NUnit.Framework;
using UnityEngine;

namespace DOB.Tests
{
    public sealed class ServiceRosterTests
    {
        private readonly List<ServiceRosterSettings> created = new List<ServiceRosterSettings>();

        [TearDown]
        public void TearDown()
        {
            foreach (var settings in created)
                UnityEngine.Object.DestroyImmediate(settings);
            created.Clear();
        }

        private ServiceRosterSettings Settings(int guests = 3, float min = 8f, float max = 20f, int queue = 2)
        {
            var settings = ScriptableObject.CreateInstance<ServiceRosterSettings>();
            JsonUtility.FromJsonOverwrite(
                $"{{\"dailyGrowthMin\":0,\"dailyGrowthMax\":0,\"firstArrivalSeconds\":0,\"baseGuests\":{guests},\"arrivalMinSeconds\":{min},\"arrivalMaxSeconds\":{max},\"queueCapacity\":{queue}," +
                "\"waitStartScore\":2,\"scoreStepSeconds\":10,\"leaveAfterSeconds\":30}", settings);
            created.Add(settings);
            return settings;
        }

        private ServiceRoster Open(int guests = 3, float min = 8f, float max = 20f, int queue = 2, int seed = 1)
        {
            var roster = new ServiceRoster(Settings(guests, min, max, queue), new System.Random(seed));
            roster.Tick(0f);
            return roster;
        }

        [Test]
        public void BaseGuestsGrowByOneOrTwoEachDayAfterTheFirst()
        {
            var settings = Settings();
            JsonUtility.FromJsonOverwrite("{\"dailyGrowthMin\":1,\"dailyGrowthMax\":2}", settings);
            Assert.That(new ServiceRoster(settings, new System.Random(1), 0, 1).Planned, Is.EqualTo(3), "1일차는 늘지 않는다");
            int previous = 3;
            for (int day = 2; day <= 6; day++)
            {
                int planned = new ServiceRoster(settings, new System.Random(1), 0, day).Planned;
                Assert.That(planned - previous, Is.InRange(1, 2), $"{day}일차 증가");
                Assert.That(new ServiceRoster(settings, new System.Random(9), 0, day).Planned, Is.EqualTo(planned), "불러와도 같은 인원");
                previous = planned;
            }
            Assert.That(new ServiceRoster(settings, new System.Random(1), 2, 2).Planned, Is.InRange(6, 7), "입소문 추가와 따로 더한다");
        }

        [Test]
        public void FirstGuestWaitsForTheFirstArrivalDelay()
        {
            var settings = Settings();
            JsonUtility.FromJsonOverwrite("{\"firstArrivalSeconds\":3}", settings);
            var roster = new ServiceRoster(settings, new System.Random(1));
            roster.Tick(0f);
            Assert.That(roster.CanAdmit, Is.False, "영업을 열자마자는 들어오지 않는다");
            roster.Tick(2.9f);
            Assert.That(roster.CanAdmit, Is.False);
            roster.Tick(0.2f);
            Assert.That(roster.CanAdmit, Is.True, "첫 입장 대기가 지나면 들어온다");
        }

        // 맞이 → 동행 → 착석까지 단계를 옮긴다.
        private static void SeatVisit(ServiceRoster roster, string visit)
        {
            Assert.That(roster.TryBeginGreeting(visit), Is.True);
            Assert.That(roster.TryGuide(visit), Is.True);
            Assert.That(roster.TrySeat(visit), Is.True);
        }

        // ── 정지 사유 ──

        [Test]
        public void OverlappingReasonsKeepTheClockStoppedUntilAllAreOff()
        {
            var pauses = new ServicePauseReasons();
            pauses.Set(ServicePauseReason.Dialogue, true);
            pauses.Set(ServicePauseReason.Dialogue, true);
            pauses.Set(ServicePauseReason.Ingredients, true);
            pauses.Set(ServicePauseReason.Dialogue, false);
            Assert.That(pauses.StopsArrivalAndWait, Is.True, "재료 선택이 남아 있다");
            Assert.That(pauses.StopsCooking, Is.False, "재료 선택은 조리를 멈추지 않는다");
            pauses.Set(ServicePauseReason.Ingredients, false);
            Assert.That(pauses.StopsArrivalAndWait, Is.False);
            Assert.That(pauses.StopsCooking, Is.False);
        }

        [Test]
        public void PauseMenuAndTransitionStopEverything()
        {
            foreach (var reason in new[] { ServicePauseReason.PauseMenu, ServicePauseReason.Transition })
            {
                var pauses = new ServicePauseReasons();
                pauses.Set(reason, true);
                Assert.That(pauses.StopsArrivalAndWait && pauses.StopsCooking, Is.True, reason.ToString());
            }
            foreach (var reason in new[] { ServicePauseReason.Dialogue, ServicePauseReason.Notebook, ServicePauseReason.Ingredients })
            {
                var pauses = new ServicePauseReasons();
                pauses.Set(reason, true);
                Assert.That(pauses.StopsArrivalAndWait, Is.True, reason.ToString());
                Assert.That(pauses.StopsCooking, Is.False, reason.ToString());
            }
        }

        // ── 예정 수·입장 ──

        [Test]
        public void PlannedCountComesFromTheBaseGuestsAndTheFirstVisitArrivesImmediately()
        {
            var roster = new ServiceRoster(Settings(), new System.Random(1));
            Assert.That(roster.Planned, Is.EqualTo(3));
            Assert.That(roster.Entered, Is.Zero);
            roster.Tick(0f);
            Assert.That(roster.CanAdmit, Is.True, "첫 방문은 영업 시작 직후");
            Assert.That(roster.Admit("v1", "g1"), Is.True);
            Assert.That(roster.Entered, Is.EqualTo(1));
            Assert.That(roster.NextArrivalSeconds, Is.InRange(8f, 20f));
        }

        [Test]
        public void WordOfMouthExtraGuestsAddToThePlannedCount()
        {
            Assert.That(new ServiceRoster(Settings(), new System.Random(1), 1).Planned, Is.EqualTo(4));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ServiceRoster(Settings(), new System.Random(1), -1));
        }

        [Test]
        public void WaitScoreNowAndPatienceLeftFollowTheWaitClock()
        {
            var roster = Open();
            Assert.That(roster.Admit("v1", "g1"), Is.True);
            var visit = roster.Find("v1");
            Assert.That((roster.WaitScoreNow(visit), roster.PatienceLeft(visit)), Is.EqualTo((2, 30f)));
            roster.Tick(12f);
            Assert.That((roster.WaitScoreNow(visit), roster.PatienceLeft(visit)), Is.EqualTo((1, 18f)));
            roster.Tick(25f);
            Assert.That(visit.End, Is.EqualTo(ServiceVisitEnd.WaitedOut));
            Assert.That((roster.WaitScoreNow(visit), roster.PatienceLeft(visit)), Is.EqualTo((0, 0f)));
        }

        [Test]
        public void InvalidSettingsAreRejected()
        {
            Assert.Throws<ArgumentException>(() => new ServiceRoster(Settings(guests: 0), new System.Random(1)));
            Assert.Throws<ArgumentException>(() => new ServiceRoster(Settings(min: 20f, max: 8f), new System.Random(1)));
            Assert.Throws<ArgumentException>(() => new ServiceRoster(Settings(queue: 0), new System.Random(1)));
            Assert.That(Settings(guests: -1).Validate(), Is.Not.Null);
            Assert.That(Settings().Validate(), Is.Null);
        }

        [Test]
        public void TheNextVisitArrivesWhenTheIntervalPasses()
        {
            var roster = Open();
            roster.Admit("v1", "g1");
            float interval = roster.NextArrivalSeconds;
            roster.Tick(interval - 0.5f);
            Assert.That(roster.CanAdmit, Is.False);
            roster.Tick(0.5f);
            Assert.That(roster.CanAdmit, Is.True);
            Assert.That(roster.Admit("v1", "g2"), Is.False, "중복 방문 ID");
            Assert.That(roster.Admit("v2", "g2"), Is.True);
            Assert.That(roster.Find("v2"), Is.Not.Null);
        }

        [Test]
        public void TheSameSeedGivesTheSameIntervals()
        {
            float[] Intervals(int seed)
            {
                var roster = Open(seed: seed);
                var result = new float[2];
                roster.Admit("v1", "g1");
                result[0] = roster.NextArrivalSeconds;
                roster.TryBeginGreeting("v1");
                roster.Tick(result[0]);
                roster.Admit("v2", "g2");
                result[1] = roster.NextArrivalSeconds;
                return result;
            }
            Assert.That(Intervals(7), Is.EqualTo(Intervals(7)));
        }

        [Test]
        public void NoMoreVisitsArriveAfterThePlannedCount()
        {
            var roster = Open(guests: 1);
            roster.Admit("v1", "g1");
            roster.TryBeginGreeting("v1");
            roster.Tick(1000f);
            Assert.That(roster.CanAdmit, Is.False);
            Assert.That(roster.Admit("v2", "g2"), Is.False);
            Assert.That(roster.Entered, Is.EqualTo(1));
        }

        [Test]
        public void AFullQueueDefersTheNextArrivalUntilASpotOpens()
        {
            var roster = Open(min: 5f, max: 5f, queue: 2);
            roster.Admit("v1", "g1");
            roster.Tick(5f);
            roster.Admit("v2", "g2");
            roster.Tick(5f);
            Assert.That(roster.QueueCount, Is.EqualTo(2));
            Assert.That(roster.CanAdmit, Is.False, "줄이 차 있으면 기다린다");
            Assert.That(roster.TryBeginGreeting("v1"), Is.True);
            Assert.That(roster.CanAdmit, Is.True, "줄의 한 방문을 맞이하면 바로 입장");
            Assert.That(roster.Admit("v3", "g3"), Is.True);
        }

        [Test]
        public void AWaitedOutVisitFreesItsQueueSpot()
        {
            var roster = Open(min: 5f, max: 5f, queue: 2);
            roster.Admit("v1", "g1");
            roster.Tick(5f);
            roster.Admit("v2", "g2");
            roster.Tick(5f);
            Assert.That(roster.CanAdmit, Is.False);
            roster.Tick(20f); // v1은 30초째 이탈한다.
            Assert.That(roster.Find("v1").End, Is.EqualTo(ServiceVisitEnd.WaitedOut));
            Assert.That(roster.CanAdmit, Is.True);
        }

        // ── 단계·종료 ──

        [Test]
        public void TransitionsFollowTheOrderAndRejectUnknownIds()
        {
            var roster = Open();
            roster.Admit("v1", "g1");
            Assert.That(roster.TrySeat("v1"), Is.False, "입구에서 바로 착석");
            Assert.That(roster.TryBeginGreeting("v1"), Is.True);
            Assert.That(roster.TryServe("v1"), Is.False, "응대 중 서빙");
            Assert.That(roster.TryEnd("v1", ServiceVisitEnd.WaitedOut), Is.False, "응대 중 이탈");
            Assert.That(roster.TryCancelGreeting("v1"), Is.True);
            Assert.That(roster.Find("v1").Stage, Is.EqualTo(ServiceVisitStage.AtEntrance));
            Assert.That(roster.TryBeginGreeting("missing"), Is.False);
            Assert.That(roster.TryEnd("missing", ServiceVisitEnd.SentHome), Is.False);
            Assert.That(roster.Entered, Is.EqualTo(1));
        }

        [Test]
        public void AVisitEndsOnlyOnce()
        {
            var roster = Open();
            var ended = new List<(string, ServiceVisitEnd)>();
            roster.VisitEnded += (id, end) => ended.Add((id, end));
            roster.Admit("v1", "g1");
            roster.TryBeginGreeting("v1");
            Assert.That(roster.TryEnd("v1", ServiceVisitEnd.SentHome), Is.True);
            Assert.That(roster.TryEnd("v1", ServiceVisitEnd.WaitedOut), Is.False);
            Assert.That(roster.Find("v1").End, Is.EqualTo(ServiceVisitEnd.SentHome));
            Assert.That(ended, Is.EqualTo(new[] { ("v1", ServiceVisitEnd.SentHome) }));
        }

        [Test]
        public void AServedVisitIsDispatched()
        {
            var roster = Open();
            roster.Admit("v1", "g1");
            SeatVisit(roster, "v1");
            Assert.That(roster.TryEnd("v1", ServiceVisitEnd.Dispatched), Is.False, "서빙 전 파견");
            Assert.That(roster.TryServe("v1"), Is.True);
            Assert.That(roster.TryEnd("v1", ServiceVisitEnd.Dispatched), Is.True);
            Assert.That(roster.Find("v1").End, Is.EqualTo(ServiceVisitEnd.Dispatched));
        }

        // ── 대기 시계 ──

        [Test]
        public void GuidingIsNotCountedAsWaiting()
        {
            var roster = Open();
            roster.Admit("v1", "g1");
            roster.Tick(5f);
            roster.TryBeginGreeting("v1");
            roster.Tick(3f);
            roster.TryGuide("v1");
            roster.Tick(8f);
            roster.TrySeat("v1");
            roster.Tick(4f);
            Assert.That(roster.Find("v1").WaitSeconds, Is.EqualTo(9f));
        }

        [Test]
        public void WaitingStopsAfterServingAndTheScoreDropsEveryStep()
        {
            var roster = Open(guests: 2, min: 100f, max: 100f);
            roster.Admit("v1", "g1");
            SeatVisit(roster, "v1");
            roster.Tick(19f);
            Assert.That(roster.TryServe("v1"), Is.True);
            roster.Tick(5f);
            Assert.That(roster.Find("v1").WaitSeconds, Is.EqualTo(19f), "서빙 후에는 멈춘다");
            Assert.That(roster.Find("v1").WaitScore, Is.EqualTo(1));

            roster.Tick(76f);
            roster.Admit("v2", "g2");
            SeatVisit(roster, "v2");
            roster.Tick(25f);
            roster.TryServe("v2");
            Assert.That(roster.Find("v2").WaitScore, Is.Zero, "0에서 멈춘다");
        }

        [Test]
        public void WaitingAtTheEntranceTooLongLeavesTheQueue()
        {
            var roster = Open();
            roster.Admit("v1", "g1");
            roster.Tick(29.5f);
            Assert.That(roster.Find("v1").Stage, Is.EqualTo(ServiceVisitStage.AtEntrance));
            roster.Tick(0.5f);
            Assert.That(roster.Find("v1").End, Is.EqualTo(ServiceVisitEnd.WaitedOut));
            Assert.That(roster.QueueCount, Is.Zero);
        }

        [Test]
        public void SeatedVisitsCanAlsoWaitOut()
        {
            var roster = Open();
            string left = null;
            roster.VisitEnded += (id, end) => left = id;
            roster.Admit("v1", "g1");
            SeatVisit(roster, "v1");
            roster.Tick(30f);
            Assert.That(left, Is.EqualTo("v1"));
            Assert.That(roster.TryServe("v1"), Is.False);
        }

        [Test]
        public void AVisitBeingGreetedNeverWaitsOut()
        {
            var roster = Open();
            roster.Admit("v1", "g1");
            roster.Tick(29f);
            roster.TryBeginGreeting("v1");
            roster.Tick(100f);
            Assert.That(roster.Find("v1").Stage, Is.EqualTo(ServiceVisitStage.Greeting));
        }

        // ── 마감·서빙 인원 ──

        [Test]
        public void AllEndedNeedsEveryPlannedVisitToArriveAndEnd()
        {
            var roster = Open(min: 5f, max: 5f, queue: 3);
            roster.Admit("v1", "g1");
            roster.Tick(5f);
            roster.Admit("v2", "g2");
            SeatVisit(roster, "v1");
            roster.TryServe("v1");
            roster.TryEnd("v1", ServiceVisitEnd.Dispatched);
            roster.TryBeginGreeting("v2");
            roster.TryEnd("v2", ServiceVisitEnd.SentHome);
            Assert.That(roster.AllEnded, Is.False, "예정 3명 중 1명이 아직 입장 전");

            roster.Tick(5f);
            roster.Admit("v3", "g3");
            Assert.That(roster.AllEnded, Is.False);
            roster.Tick(30f);
            Assert.That(roster.Find("v3").End, Is.EqualTo(ServiceVisitEnd.WaitedOut));
            Assert.That(roster.AllEnded, Is.True);
        }

        [Test]
        public void ServedCountOnlyCountsDistinctDispatchedGuests()
        {
            var roster = Open(guests: 4, min: 1f, max: 1f, queue: 4);
            roster.Admit("v1", "same-guest");
            for (int i = 2; i <= 4; i++)
            {
                roster.Tick(1f);
                roster.Admit("v" + i, i == 2 ? "same-guest" : "g" + i);
            }
            foreach (var visit in new[] { "v1", "v2" })
            {
                SeatVisit(roster, visit);
                roster.TryServe(visit);
                roster.TryEnd(visit, ServiceVisitEnd.Dispatched);
            }
            roster.TryBeginGreeting("v3");
            roster.TryEnd("v3", ServiceVisitEnd.SentHome);
            roster.TryEnd("v4", ServiceVisitEnd.WaitedOut);
            Assert.That(roster.ServedGuestCount, Is.EqualTo(1), "같은 손님 ID는 한 번만 센다");
            Assert.That(Open().ServedGuestCount, Is.Zero, "새 일차는 0부터");
        }

        [Test]
        public void TwoDistinctDispatchedGuestsCountAsTwo()
        {
            var roster = Open(guests: 4, min: 1f, max: 1f, queue: 4);
            roster.Admit("v1", "g1");
            for (int i = 2; i <= 4; i++)
            {
                roster.Tick(1f);
                roster.Admit("v" + i, "g" + i);
            }
            foreach (var visit in new[] { "v1", "v2" })
            {
                SeatVisit(roster, visit);
                roster.TryServe(visit);
                roster.TryEnd(visit, ServiceVisitEnd.Dispatched);
            }
            roster.TryBeginGreeting("v3");
            roster.TryEnd("v3", ServiceVisitEnd.SentHome);
            roster.TryEnd("v4", ServiceVisitEnd.WaitedOut);
            Assert.That(roster.ServedGuestCount, Is.EqualTo(2));
            Assert.That(roster.AllEnded, Is.True);
        }
    }
}
