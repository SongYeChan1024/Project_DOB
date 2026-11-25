// =====================================================================
// VisitResultLog — 오늘 서빙한 방문의 탐험 결과 기록 (순수 C#)
// 호출자 : SettlementManager (RecordVisit → TryRecord, 정산 때 Results를 저장에 넘김)
// 상태   : 오늘 확정된 방문 결과 목록(확정 순서)
// 의사코드:
//   TryRecord — 방문·손님 ID가 비었거나 축별 부족이 여섯 칸이 아니거나 음수가 있으면 거절한다.
//               같은 방문 ID가 이미 있으면 거절한다 (중복 서빙 입력 방지).
//               부족 합으로 등급을 정하고, 등급별 개수만큼 그 층의 재료에서 전리품을 뽑아 함께 기록한다.
//               대성공이면 정해진 확률로 그 층의 희귀 재료 1개를 더한다.
//   전리품은 여기서 한 번만 뽑는다. 불러오기나 보고 재열기로 결과가 달라지지 않게 하기 위해서다.
// =====================================================================
using System;
using System.Collections.Generic;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>서빙 시점에 방문 결과를 한 번 확정해 보관한다. 금화·재고는 바꾸지 않는다.</summary>
    public sealed class VisitResultLog
    {
        private readonly Func<ExpeditionGrade, int> lootCount;
        private readonly Func<int, IReadOnlyList<string>> floorMaterials;
        private readonly Random random;
        private readonly Func<int, string> floorRareMaterial;
        private readonly double rareChance;
        private readonly List<VisitResult> results = new List<VisitResult>();
        private readonly HashSet<string> visitIds = new HashSet<string>();

        /// <summary>오늘 확정된 방문 결과. 확정한 순서.</summary>
        public IReadOnlyList<VisitResult> Results => results;

        /// <param name="lootCount">등급별 전리품 개수(기획 데이터).</param>
        /// <param name="floorMaterials">층별 일반 재료 목록(기획 데이터).</param>
        /// <param name="random">전리품 추첨용 난수. 테스트는 시드를 고정해 넘긴다.</param>
        /// <param name="floorRareMaterial">층별 희귀 재료 이름(기획 데이터). 생략하면 희귀 재료를 주지 않는다.</param>
        /// <param name="rareChance">대성공일 때 희귀 재료 1개를 더 줄 확률(기획 데이터). 0부터 1.</param>
        public VisitResultLog(Func<ExpeditionGrade, int> lootCount, Func<int, IReadOnlyList<string>> floorMaterials, Random random,
            Func<int, string> floorRareMaterial = null, double rareChance = 0)
        {
            if (!(rareChance >= 0 && rareChance <= 1)) throw new ArgumentOutOfRangeException(nameof(rareChance));
            this.lootCount = lootCount ?? throw new ArgumentNullException(nameof(lootCount));
            this.floorMaterials = floorMaterials ?? throw new ArgumentNullException(nameof(floorMaterials));
            this.random = random ?? throw new ArgumentNullException(nameof(random));
            this.floorRareMaterial = floorRareMaterial;
            this.rareChance = rareChance;
        }

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
            result = new VisitResult(guestId, visitId, guestName, floor, dish, usedSeasoning, copy, grade, DrawLoot(grade, floor),
                risks == null ? null : new List<int>(risks).ToArray(), protections == null ? null : new List<int>(protections).ToArray());
            visitIds.Add(visitId);
            results.Add(result);
            return true;
        }

        // shortcut: 종류를 그 층 일반 재료에서 균등 추첨한다(💡 가정), 기획이 전리품 종류 규칙을 정하면 이 메서드만 바꾼다.
        private string[] DrawLoot(ExpeditionGrade grade, int floor)
        {
            var materials = floorMaterials(floor);
            int count = materials == null || materials.Count == 0 ? 0 : Math.Max(0, lootCount(grade));
            var loot = new List<string>(count + 1);
            for (int i = 0; i < count; i++)
                loot.Add(materials[random.Next(materials.Count)]);

            // 대성공일 때만 희귀 재료를 한 번 추첨한다. 💡 기본 전리품을 대신하지 않고 맨 뒤에 하나 더한다
            // (대신하는지 더하는지는 FeatureManagerMap에 미정, 정해지면 이 블록만 바꾼다).
            string rare = grade == ExpeditionGrade.GreatSuccess ? floorRareMaterial?.Invoke(floor) : null;
            if (!string.IsNullOrEmpty(rare) && random.NextDouble() < rareChance)
                loot.Add(rare);
            return loot.ToArray();
        }
    }
}
