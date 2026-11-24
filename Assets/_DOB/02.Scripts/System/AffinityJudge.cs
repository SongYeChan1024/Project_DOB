// =====================================================================
// AffinityJudge — 방문 호감·입소문 계산기 (순수 C#, 상태 없음)
// 호출자 : RelationshipManager (마감 정산 때 SettleDay, 아침 조회 때 Tally)
// 의사코드:
//   Score(기준, 손님·방문 ID, 종료 방식, 대기, 입맛, 등급)
//     — 파견: 대기 + 입맛 + 등급 점수. 등급이 없으면 예외.
//     — 돌려보냄·대기 이탈: 기준의 감점 하나. 대기·입맛·탐험 내역은 0.
//     — 끝나지 않은 방문(None)은 예외.
//   SettleDay(기준, 명단의 방문, 그날 방문 결과, 입맛 점수)
//     — 끝난 방문만 입장 순서대로 한 번씩 계산한다. 같은 방문 ID는 처음 것만 쓴다.
//     — 파견 방문은 방문 결과에서 같은 방문 ID의 등급을 찾는다. 없으면 기록을 만들지 않고 오류 설명에 남긴다.
//     — 입맛 점수는 호출자가 넘긴다(💡 입맛 채점 전까지 0).
//   Tally(기준, 기록) — 파견·대기 이탈은 항상, 돌려보냄은 기준의 포함 설정일 때만 N에 세고 합계를 S에 더한다.
//   ExtraGuests(기준, S, N) — N ≤ 0이면 0. 하루 최대 = 방문 최대 × N.
//     문턱마다 필요 점수 = ceil(하루 최대 × 퍼센트 / 100)을 정수 연산으로 구하고, S가 이상인 문턱 수를 돌려준다.
// =====================================================================
using System;
using System.Collections.Generic;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>끝난 방문의 호감 변화와 다음 영업의 입소문 추가 인원을 계산한다.</summary>
    public static class AffinityJudge
    {
        /// <summary>방문 하나의 호감 내역. 파견이면 grade가 필요하다.</summary>
        public static VisitAffinity Score(AffinityRules rules, string guestId, string visitId, ServiceVisitEnd end,
            int waitScore, int tasteScore, ExpeditionGrade? grade)
        {
            if (!rules) throw new ArgumentNullException(nameof(rules));
            switch (end)
            {
                case ServiceVisitEnd.Dispatched:
                    if (grade == null) throw new ArgumentNullException(nameof(grade));
                    int expedition = rules.GradePoints(grade.Value);
                    return new VisitAffinity(guestId, visitId, end, waitScore, tasteScore, expedition, waitScore + tasteScore + expedition);
                case ServiceVisitEnd.SentHome:
                    return new VisitAffinity(guestId, visitId, end, 0, 0, 0, rules.SentHomePoints);
                case ServiceVisitEnd.WaitedOut:
                    return new VisitAffinity(guestId, visitId, end, 0, 0, 0, rules.WaitedOutPoints);
                default:
                    throw new ArgumentOutOfRangeException(nameof(end));
            }
        }

        /// <summary>그날 끝난 방문마다 호감을 한 번 확정한다. 결과 기록이 없는 파견 방문은 빼고 error에 남긴다(없으면 null).</summary>
        public static List<VisitAffinity> SettleDay(AffinityRules rules, IEnumerable<ServiceVisit> visits,
            IEnumerable<VisitResult> results, int tasteScore, out string error)
        {
            if (visits == null) throw new ArgumentNullException(nameof(visits));
            var grades = new Dictionary<string, ExpeditionGrade>();
            if (results != null)
                foreach (var result in results)
                    if (result != null && !string.IsNullOrEmpty(result.VisitId) && !grades.ContainsKey(result.VisitId))
                        grades.Add(result.VisitId, result.Grade);

            error = null;
            var seen = new HashSet<string>();
            var records = new List<VisitAffinity>();
            foreach (var visit in visits)
            {
                if (visit == null || visit.End == ServiceVisitEnd.None || !seen.Add(visit.VisitId))
                    continue;
                ExpeditionGrade? grade = null;
                if (visit.End == ServiceVisitEnd.Dispatched)
                {
                    if (!grades.TryGetValue(visit.VisitId, out var found))
                    {
                        error = (error == null ? "" : error + " ") + $"탐험 결과가 없는 파견 방문({visit.VisitId})을 호감 정산에서 뺐습니다.";
                        continue;
                    }
                    grade = found;
                }
                records.Add(Score(rules, visit.GuestId, visit.VisitId, visit.End, visit.WaitScore ?? 0, tasteScore, grade));
            }
            return records;
        }

        /// <summary>입소문에 쓰는 합계 S와 손님 수 N.</summary>
        public static (int Sum, int Count) Tally(AffinityRules rules, IEnumerable<VisitAffinity> records)
        {
            if (!rules) throw new ArgumentNullException(nameof(rules));
            int sum = 0, count = 0;
            if (records != null)
                foreach (var record in records)
                {
                    if (record == null || (record.End == ServiceVisitEnd.SentHome && !rules.CountSentHome))
                        continue;
                    sum += record.Total;
                    count++;
                }
            return (sum, count);
        }

        /// <summary>S가 도달한 입소문 문턱의 개수. N이 0 이하이면 0.</summary>
        public static int ExtraGuests(AffinityRules rules, int sum, int count)
        {
            if (!rules) throw new ArgumentNullException(nameof(rules));
            if (count <= 0)
                return 0;
            long dayMax = (long)rules.VisitMaxPoints * count;
            int extra = 0;
            foreach (int percent in rules.ThresholdPercents)
                // 올림을 정수로: ceil(a/100) = (a + 99) / 100 (a ≥ 0).
                if (sum >= (dayMax * percent + 99) / 100)
                    extra++;
            return extra;
        }
    }
}
