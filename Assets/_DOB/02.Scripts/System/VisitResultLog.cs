// =====================================================================
// VisitResultLog — 오늘 서빙한 방문의 탐험 결과 기록 (순수 C#)
// 호출자 : SettlementManager (RecordVisit → TryRecord, 정산 때 WithGifts의 결과를 저장에 넘김)
// 상태   : 오늘 확정된 방문 결과 목록(확정 순서)
// 의사코드:
//   TryRecord — 방문·손님 ID가 비었거나 축별 부족이 여섯 칸이 아니거나 음수가 있으면 거절한다.
//               같은 방문 ID가 이미 있으면 거절한다 (중복 서빙 입력 방지).
//               부족 합으로 등급을 정해 기록한다. 전리품은 비워 둔다(자격은 그날 정산 뒤 누적 호감으로 정해지므로).
//   WithGifts — 마감 정산이 명부를 갱신한 뒤 부른다. 결과마다 손님 기록을 찾아
//               영구 거절이 아니고 누적 호감이 기준 이상이면 그 층의 희귀 재료를 정한 개수만큼 담은 사본을 만든다.
//               그 밖(명부에 없음·거절·기준 미만·층에 희귀 재료 없음)은 전리품이 빈 사본이다. 기록 자체는 바꾸지 않는다.
//   등급과 무관하다: 부상(대실패)으로 돌아와도 자격이 있으면 받는다. 거절된 방문의 전리품은 폐기한다(받지 않은 상태를 두지 않음).
//   난수가 없어 같은 명부에서 다시 불러도 같은 결과다. 정산 결과는 GameManager가 한 번 저장해 재사용한다.
// =====================================================================
using System;
using System.Collections.Generic;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>서빙 시점에 방문 결과를 한 번 확정해 보관한다. 금화·재고는 바꾸지 않는다.</summary>
    public sealed class VisitResultLog
    {
        private readonly List<VisitResult> results = new List<VisitResult>();
        private readonly HashSet<string> visitIds = new HashSet<string>();

        /// <summary>오늘 확정된 방문 결과. 확정한 순서. 전리품은 비어 있다.</summary>
        public IReadOnlyList<VisitResult> Results => results;

        /// <summary>서빙이 성공한 방문의 결과를 확정한다. 잘못된 입력과 중복 방문은 아무것도 바꾸지 않고 false.</summary>
        public bool TryRecord(string guestId, string visitId, string guestName, int floor, string dish, bool usedSeasoning,
            IReadOnlyList<int> shortfalls, out VisitResult result) =>
            TryRecord(guestId, visitId, guestName, floor, dish, usedSeasoning, shortfalls, null, null, out result);

        /// <summary>축별 실제 위험과 요리가 막은 칸을 함께 남긴다 (보고 화면의 "막아냈다" 줄에 쓴다). 둘은 함께 주거나 함께 생략한다.
        /// 부족과 맞지 않는 위험·효과(부족 ≠ 위험 − 효과)는 거절한다.</summary>
        public bool TryRecord(string guestId, string visitId, string guestName, int floor, string dish, bool usedSeasoning,
            IReadOnlyList<int> shortfalls, IReadOnlyList<int> risks, IReadOnlyList<int> protections, out VisitResult result)
        {
            result = null;
            if (string.IsNullOrEmpty(guestId) || string.IsNullOrEmpty(visitId) || shortfalls == null
                || shortfalls.Count != RiskAxes.All.Count || visitIds.Contains(visitId))
                return false;
            if ((risks == null) != (protections == null)
                || (risks != null && (risks.Count != shortfalls.Count || protections.Count != shortfalls.Count)))
                return false;
            var copy = new int[shortfalls.Count];
            for (int i = 0; i < copy.Length; i++)
            {
                if (shortfalls[i] < 0)
                    return false;
                if (risks != null && (risks[i] < 0 || protections[i] < 0 || shortfalls[i] != Math.Max(0, risks[i] - protections[i])))
                    return false;
                copy[i] = shortfalls[i];
            }

            var grade = ExpeditionJudge.Grade(ExpeditionJudge.Total(copy));
            result = new VisitResult(guestId, visitId, guestName, floor, dish, usedSeasoning, copy, grade, Array.Empty<string>(),
                risks == null ? null : new List<int>(risks).ToArray(), protections == null ? null : new List<int>(protections).ToArray());
            visitIds.Add(visitId);
            results.Add(result);
            return true;
        }

        /// <summary>저장 복원용. 저장된 오늘의 결과를 그대로 다시 넣는다. 이미 있는 방문은 건너뛴다.</summary>
        public void Restore(IEnumerable<VisitResult> saved)
        {
            if (saved == null)
                return;
            foreach (var result in saved)
                if (result != null && !string.IsNullOrEmpty(result.VisitId) && visitIds.Add(result.VisitId))
                    results.Add(result);
        }

        /// <summary>정산 뒤 명부로 전리품·선물을 정한 사본 목록. 기록은 바꾸지 않는다.</summary>
        /// <param name="findGuest">손님 ID로 정산 뒤 명부 기록을 찾는다. 없으면 null.</param>
        /// <param name="minAffinity">전리품 자격이 생기는 누적 호감(점, 이상).</param>
        /// <param name="floorGift">층의 희귀 재료 이름(기획 데이터). 없으면 null.</param>
        /// <param name="giftCount">자격이 있을 때 받는 개수. 0 이상.</param>
        public IReadOnlyList<VisitResult> WithGifts(Func<string, GuestRecord> findGuest, int minAffinity, Func<int, string> floorGift, int giftCount)
        {
            if (findGuest == null) throw new ArgumentNullException(nameof(findGuest));
            if (floorGift == null) throw new ArgumentNullException(nameof(floorGift));
            if (giftCount < 0) throw new ArgumentOutOfRangeException(nameof(giftCount));
            var gifted = new List<VisitResult>(results.Count);
            foreach (var result in results)
            {
                var guest = findGuest(result.GuestId);
                string gift = guest != null && !guest.Rejected && guest.Affinity >= minAffinity ? floorGift(result.Floor) : null;
                var loot = new string[string.IsNullOrEmpty(gift) ? 0 : giftCount];
                for (int i = 0; i < loot.Length; i++)
                    loot[i] = gift;
                gifted.Add(result.WithLoot(loot));
            }
            return gifted;
        }
    }
}
