// =====================================================================
// GuestLedger — 손님 명부의 유일한 소유자 (순수 C#)
// 호출자 : RelationshipManager (일차 시작 때 저장된 명부로 생성, 마감 정산 때 SettleDay, 조회 전달)
// 상태   : 등록 순서의 손님 기록(신원·누적 호감·영구 거절·복귀 일차), 마지막으로 정산한 일차
// 의사코드:
//   생성       — 저장된 기록을 받는다. 손님 ID가 비었거나 겹치면 예외.
//   Find       — 손님 ID로 기록을 찾는다. 없으면 null. 상태를 바꾸지 않는다.
//   EligibleOn — 그 일차에 올 수 있는 손님(영구 거절 아님 + 복귀 일차 ≤ 일차)을 등록 순서로 돌려준다. 상태를 바꾸지 않는다.
//   SettleDay  — 그 일차를 이미 정산했으면 아무것도 바꾸지 않고 false.
//                그날 끝난 방문의 호감 기록마다(같은 손님은 한 번만):
//                ① 명부에 없으면 그날 신원으로 등록한다. 신원을 모르면 건너뛴다.
//                ② 누적 호감에 방문 호감을 더한다.
//                ③ 더한 값이 0 미만이면 영구 거절한다(0으로 맞추기 전의 값으로 판정, 한 번 거절되면 유지).
//                ④ 누적을 0 이상 상한 이하로 맞춘다.
//                ⑤ 복귀 일차 = 일차 + 1. 그 방문의 탐험이 대실패면 휴식 일수를 더한다.
//                그날 오지 않은 손님과 호감 기록이 없는 방문(결과 없는 파견)은 그대로 둔다.
// 누적 0인 손님도 다음 날 다시 온다 (0 미만만 거절). 2026-11-25 사용자 검토 답변으로 확정 (FeatureManagerMap 「다음 영업에 오는 손님」).
// 영구 거절된 손님도 명부에 남긴다. 방문 호감 기록·귀환 보고의 손님 ID가 가리키는 대상을 유지한다.
// =====================================================================
using System;
using System.Collections.Generic;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>손님을 날을 넘겨 같은 사람으로 기억한다. 화면·저장은 모른다.</summary>
    public sealed class GuestLedger
    {
        private readonly List<GuestRecord> records = new List<GuestRecord>();
        private int settledDay;

        /// <summary>저장된 명부로 연다. 손님 ID가 비었거나 겹치면 ArgumentException.</summary>
        public GuestLedger(IEnumerable<GuestRecord> saved = null)
        {
            if (saved == null)
                return;
            foreach (var record in saved)
            {
                if (record == null || string.IsNullOrEmpty(record.GuestId) || Find(record.GuestId) != null)
                    throw new ArgumentException("명부의 손님 ID가 비었거나 겹칩니다.", nameof(saved));
                records.Add(record);
            }
        }

        /// <summary>등록 순서의 손님 기록.</summary>
        public IReadOnlyList<GuestRecord> Records => records;

        /// <summary>손님 ID로 찾는다. 없으면 null.</summary>
        public GuestRecord Find(string guestId) =>
            string.IsNullOrEmpty(guestId) ? null : records.Find(r => r.GuestId == guestId);

        /// <summary>그 일차에 올 수 있는 손님을 등록 순서로 돌려준다.</summary>
        public List<GuestRecord> EligibleOn(int day) => records.FindAll(r => r.CanVisitOn(day));

        /// <summary>그날 끝난 방문을 명부에 한 번 반영한다. 같은 일차를 다시 요청하면 바꾸지 않고 false.</summary>
        /// <param name="todaysGuests">그날 명단의 신원. 처음 온 손님을 등록할 때 쓴다.</param>
        public bool SettleDay(AffinityRules rules, int day, IEnumerable<GuestRecord> todaysGuests,
            IEnumerable<VisitAffinity> affinities, IEnumerable<VisitResult> results)
        {
            if (!rules) throw new ArgumentNullException(nameof(rules));
            if (day < 1) throw new ArgumentOutOfRangeException(nameof(day));
            if (day <= settledDay)
                return false;
            settledDay = day;

            var identities = new Dictionary<string, GuestRecord>();
            if (todaysGuests != null)
                foreach (var guest in todaysGuests)
                    if (guest != null && !string.IsNullOrEmpty(guest.GuestId))
                        identities[guest.GuestId] = guest;
            var injuredVisits = new HashSet<string>();
            if (results != null)
                foreach (var result in results)
                    if (result != null && result.Grade == ExpeditionGrade.GreatFailure)
                        injuredVisits.Add(result.VisitId);

            var settledGuests = new HashSet<string>();
            if (affinities != null)
                foreach (var visit in affinities)
                {
                    if (visit == null || string.IsNullOrEmpty(visit.GuestId) || !settledGuests.Add(visit.GuestId))
                        continue;
                    int index = records.FindIndex(r => r.GuestId == visit.GuestId);
                    var known = index >= 0 ? records[index] : null;
                    if (known == null && !identities.TryGetValue(visit.GuestId, out known))
                        continue;

                    int sum = known.Affinity + visit.Total;
                    // 영구 거절은 0으로 맞추기 전의 값으로 판정한다 (기획 4-8).
                    bool rejected = known.Rejected || sum < 0;
                    int affinity = Math.Min(Math.Max(sum, 0), rules.AffinityCap);
                    int returnDay = day + 1 + (injuredVisits.Contains(visit.VisitId) ? rules.InjuryRestDays : 0);
                    var updated = new GuestRecord(known.GuestId, known.Name, known.Race, known.Job, known.Weakness,
                        affinity, rejected, returnDay);
                    if (index >= 0) records[index] = updated;
                    else records.Add(updated);
                }
            return true;
        }
    }
}
