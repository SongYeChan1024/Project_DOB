// ServiceRoster — 본영업 하루의 방문 명단. 예정 수, 입장 일정, 입구 줄, 방문별 단계·대기 시간·종료 방식을 소유한다.
// 호출자: GuestManager(입장·대기 Δ, 방문 등록), FirstGuestService(응대·안내·착석 요청), GameManager(서빙·파견, 마감 판정).
// 호출  : 없음. 방문이 끝나면 VisitEnded로 주방(주문 취소)·손님 이동(퇴장)에 알린다.
// 상태  : 예정 수, 입장한 수, 다음 입장까지 남은 시간, 미뤄 둔 입장 여부, 방문 목록, 파견한 손님 ID 집합.
// 의사코드:
//   생성 — 기준값을 검사하고(잘못되면 예외) 예정 수를 기본 인원 + 입소문 추가 인원으로 정한다(추가 인원이 음수면 예외).
//          첫 방문은 바로 입장할 수 있다(간격 0).
//   Tick(Δ) — 입구 대기·착석 상태 방문의 대기 시간을 Δ만큼 늘리고, 한도에 이른 방문을 대기 이탈로 끝낸다.
//             아직 입장할 방문이 남았고 미뤄 둔 입장이 없으면 간격을 Δ만큼 줄이고, 0 이하가 되면 입장을 하나 미뤄 둔다.
//   CanAdmit/Admit — 미뤄 둔 입장이 있고 입구 줄에 자리가 있을 때만 새 방문 ID를 등록한다. 등록하면 다음 간격을
//             주입받은 난수로 최소~최대 사이에서 뽑는다. 줄이 차 있으면 자리가 날 때까지 기다린다.
//   단계 요청 — 방문 ID로 입구 대기 → 응대 → 안내 → 착석 → 서빙됨 순서만 허용한다. 응대를 닫으면 입구 대기로 돌아간다.
//             서빙할 때 대기 점수 = max(0, 시작 − floor(대기/간격))를 기록한다(WaitScoreNow와 같은 계산).
//   조회 — WaitScoreNow: 지금 서빙하면 받을 대기 점수. PatienceLeft: 대기 이탈까지 남은 시간 max(0, 한도 − 대기). 상태를 바꾸지 않는다.
//   TryEnd — 파견은 서빙된 방문, 돌려보냄은 응대 중인 방문, 대기 이탈은 입구 대기·착석 방문만 한 번 끝낸다.
//             없는 ID·이미 끝난 방문은 거절하고 아무것도 바꾸지 않는다. 파견이면 손님 ID를 서빙 인원 집합에 넣는다.
//   AllEnded — 예정 방문이 모두 입장했고 모두 끝났을 때만 true.
using System;
using System.Collections.Generic;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>명단 안의 방문 하나. 단계·시간은 ServiceRoster만 바꾼다.</summary>
    public sealed class ServiceVisit
    {
        internal ServiceVisit(string visitId, string guestId)
        {
            VisitId = visitId;
            GuestId = guestId;
        }

        public string VisitId { get; }
        public string GuestId { get; }
        public ServiceVisitStage Stage { get; internal set; } = ServiceVisitStage.AtEntrance;
        public ServiceVisitEnd End { get; internal set; } = ServiceVisitEnd.None;
        /// <summary>입구 대기와 착석 후 서빙 전 시간을 합한 대기 시간(초).</summary>
        public float WaitSeconds { get; internal set; }
        /// <summary>서빙 시점에 확정한 대기 점수. 서빙 전에는 null.</summary>
        public int? WaitScore { get; internal set; }
    }

    /// <summary>하루 영업의 방문 명단. 일차마다 새로 만든다.</summary>
    public sealed class ServiceRoster
    {
        private readonly ServiceRosterSettings settings;
        private readonly Random random;
        private readonly List<ServiceVisit> visits = new List<ServiceVisit>();
        private readonly HashSet<string> servedGuests = new HashSet<string>();
        private bool arrivalDue;

        /// <summary>기준값과 주입받은 난수로 새 영업 명단을 연다. 기준값이 잘못되면 ArgumentException.</summary>
        /// <param name="extraGuests">전날 입소문으로 더 오는 인원(명). 0 이상.</param>
        public ServiceRoster(ServiceRosterSettings settings, Random random, int extraGuests = 0)
        {
            if (!settings) throw new ArgumentNullException(nameof(settings));
            string error = settings.Validate();
            if (error != null) throw new ArgumentException(error, nameof(settings));
            if (extraGuests < 0) throw new ArgumentOutOfRangeException(nameof(extraGuests));
            this.settings = settings;
            this.random = random ?? throw new ArgumentNullException(nameof(random));
            Planned = settings.BaseGuests + extraGuests;
        }

        /// <summary>방문 ID, 종료 방식 순서로 방문이 끝난 순간 한 번 알린다.</summary>
        public event Action<string, ServiceVisitEnd> VisitEnded;

        /// <summary>그날 예정 방문 수. 영업 중에는 바뀌지 않는다.</summary>
        public int Planned { get; }
        /// <summary>지금까지 입장한 방문 수.</summary>
        public int Entered => visits.Count;
        /// <summary>다음 입장까지 남은 입장 시계(초). 입장이 미뤄졌거나 남은 예정이 없으면 0.</summary>
        public float NextArrivalSeconds { get; private set; }
        /// <summary>입장한 순서의 방문 목록.</summary>
        public IReadOnlyList<ServiceVisit> Visits => visits;
        /// <summary>입구에서 맞이를 기다리는 방문 수.</summary>
        public int QueueCount => visits.FindAll(v => v.Stage == ServiceVisitStage.AtEntrance).Count;
        /// <summary>미뤄 둔 입장이 있고 입구 줄에 자리가 있다.</summary>
        public bool CanAdmit => arrivalDue && QueueCount < settings.QueueCapacity;
        /// <summary>예정 방문이 모두 입장했고 모두 끝났다.</summary>
        public bool AllEnded => Entered == Planned && visits.TrueForAll(v => v.Stage == ServiceVisitStage.Ended);
        /// <summary>그날 서빙 후 파견한 서로 다른 손님 수.</summary>
        public int ServedGuestCount => servedGuests.Count;

        /// <summary>방문 ID로 찾는다. 없으면 null.</summary>
        public ServiceVisit Find(string visitId) => string.IsNullOrEmpty(visitId) ? null : visits.Find(v => v.VisitId == visitId);

        /// <summary>입장·대기 시계가 허용한 경과 시간만 받는다. 정지 중이면 0을 넘긴다.</summary>
        public void Tick(float arrivalAndWaitDelta)
        {
            float delta = arrivalAndWaitDelta > 0f && !float.IsInfinity(arrivalAndWaitDelta) ? arrivalAndWaitDelta : 0f;
            for (int i = 0; i < visits.Count; i++)
            {
                var visit = visits[i];
                if (visit.Stage != ServiceVisitStage.AtEntrance && visit.Stage != ServiceVisitStage.Seated)
                    continue;
                visit.WaitSeconds += delta;
                if (visit.WaitSeconds >= settings.LeaveAfterSeconds)
                    Finish(visit, ServiceVisitEnd.WaitedOut);
            }

            if (arrivalDue || Entered >= Planned)
                return;
            NextArrivalSeconds = Math.Max(0f, NextArrivalSeconds - delta);
            if (NextArrivalSeconds == 0f)
                arrivalDue = true;
        }

        /// <summary>미뤄 둔 입장 하나를 새 방문으로 등록한다. 줄이 차 있거나 ID가 비었거나 중복이면 거절한다.</summary>
        public bool Admit(string visitId, string guestId)
        {
            if (!CanAdmit || string.IsNullOrEmpty(visitId) || string.IsNullOrEmpty(guestId) || Find(visitId) != null)
                return false;
            visits.Add(new ServiceVisit(visitId, guestId));
            arrivalDue = false;
            if (Entered < Planned)
                NextArrivalSeconds = (float)(settings.ArrivalMinSeconds
                    + random.NextDouble() * (settings.ArrivalMaxSeconds - settings.ArrivalMinSeconds));
            return true;
        }

        /// <summary>입구에서 기다리던 방문의 응대를 연다. 응대 중에는 대기 시간이 늘지 않는다.</summary>
        public bool TryBeginGreeting(string visitId) => Move(visitId, ServiceVisitStage.AtEntrance, ServiceVisitStage.Greeting);

        /// <summary>안내·돌려보냄 없이 응대를 닫으면 입구 대기로 돌아간다.</summary>
        public bool TryCancelGreeting(string visitId) => Move(visitId, ServiceVisitStage.Greeting, ServiceVisitStage.AtEntrance);

        /// <summary>응대에서 동행을 수락했다. 자리로 안내하는 동안 대기 시간이 늘지 않는다.</summary>
        public bool TryGuide(string visitId) => Move(visitId, ServiceVisitStage.Greeting, ServiceVisitStage.Guided);

        /// <summary>안내한 방문이 의자에 앉았다. 이때부터 서빙 전 대기 시간을 다시 센다.</summary>
        public bool TrySeat(string visitId) => Move(visitId, ServiceVisitStage.Guided, ServiceVisitStage.Seated);

        /// <summary>착석한 방문에 서빙했다. 대기 시간을 멈추고 그 시점의 대기 점수를 기록한다.</summary>
        public bool TryServe(string visitId)
        {
            if (!Move(visitId, ServiceVisitStage.Seated, ServiceVisitStage.Served))
                return false;
            var visit = Find(visitId);
            visit.WaitScore = WaitScoreNow(visit);
            return true;
        }

        /// <summary>지금까지의 대기 시간으로 서빙하면 받을 대기 점수(점). 0 이상.</summary>
        public int WaitScoreNow(ServiceVisit visit)
        {
            if (visit == null) throw new ArgumentNullException(nameof(visit));
            return Math.Max(0, settings.WaitStartScore - (int)Math.Floor(visit.WaitSeconds / settings.ScoreStepSeconds));
        }

        /// <summary>대기 이탈까지 남은 대기 시간(초). 0 이상. 응대·안내·서빙 뒤에는 대기 시간이 늘지 않는다.</summary>
        public float PatienceLeft(ServiceVisit visit)
        {
            if (visit == null) throw new ArgumentNullException(nameof(visit));
            return Math.Max(0f, settings.LeaveAfterSeconds - visit.WaitSeconds);
        }

        /// <summary>방문을 한 번 끝낸다. 종료 방식에 맞지 않는 단계, 없는 ID, 이미 끝난 방문은 거절한다.</summary>
        public bool TryEnd(string visitId, ServiceVisitEnd end)
        {
            var visit = Find(visitId);
            if (visit == null)
                return false;
            bool allowed;
            switch (end)
            {
                case ServiceVisitEnd.Dispatched: allowed = visit.Stage == ServiceVisitStage.Served; break;
                case ServiceVisitEnd.SentHome: allowed = visit.Stage == ServiceVisitStage.Greeting; break;
                case ServiceVisitEnd.WaitedOut:
                    allowed = visit.Stage == ServiceVisitStage.AtEntrance || visit.Stage == ServiceVisitStage.Seated;
                    break;
                default: allowed = false; break;
            }
            if (allowed)
                Finish(visit, end);
            return allowed;
        }

        private bool Move(string visitId, ServiceVisitStage from, ServiceVisitStage to)
        {
            var visit = Find(visitId);
            if (visit == null || visit.Stage != from)
                return false;
            visit.Stage = to;
            return true;
        }

        private void Finish(ServiceVisit visit, ServiceVisitEnd end)
        {
            visit.Stage = ServiceVisitStage.Ended;
            visit.End = end;
            if (end == ServiceVisitEnd.Dispatched)
                servedGuests.Add(visit.GuestId);
            VisitEnded?.Invoke(visit.VisitId, end);
        }
    }
}
