// =====================================================================
// ExpeditionJudge — 탐험 판정 계산기 (순수 C#, 상태 없음)
// 호출자 : FirstGuestKitchen (서빙 때 출발 한마디와 방문 결과에 같은 계산을 쓴다)
// 의사코드:
//   Shortfalls(위험, 효과) — 여섯 축마다 위험 − 효과를 구하고 0 아래는 0으로 둔다. 남는 효과는 다른 축으로 옮기지 않는다.
//   Total(축별 부족)       — 여섯 축의 부족을 더한다.
//   Grade(부족 합)         — 0 대성공 · 1 성공 · 2 실패 · 3 이상 대실패.
// 자리 효과와 돌발·활력 보험은 아직 넣지 않는다 (FeatureManagerMap F5·F7 후속).
// =====================================================================
using System;
using System.Collections.Generic;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>방문의 실제 위험과 음식 효과를 비교해 축별 부족과 탐험 등급을 낸다.</summary>
    public static class ExpeditionJudge
    {
        /// <summary>RiskAxes.All 순서의 축별 부족. 위험과 효과는 축 이름으로 조회한다.</summary>
        public static int[] Shortfalls(Func<string, int> risk, Func<string, int> protection)
        {
            if (risk == null) throw new ArgumentNullException(nameof(risk));
            if (protection == null) throw new ArgumentNullException(nameof(protection));
            var result = new int[RiskAxes.All.Count];
            for (int i = 0; i < result.Length; i++)
                result[i] = Math.Max(0, risk(RiskAxes.All[i]) - protection(RiskAxes.All[i]));
            return result;
        }

        /// <summary>축 이름으로 조회한 값을 RiskAxes.All 순서로 늘어놓는다. 0 아래는 0. 보고 화면에 넘길 위험·효과 칸에 쓴다.</summary>
        public static int[] PerAxis(Func<string, int> valueOf)
        {
            if (valueOf == null) throw new ArgumentNullException(nameof(valueOf));
            var result = new int[RiskAxes.All.Count];
            for (int i = 0; i < result.Length; i++)
                result[i] = Math.Max(0, valueOf(RiskAxes.All[i]));
            return result;
        }

        /// <summary>축별 부족의 합.</summary>
        public static int Total(IReadOnlyList<int> shortfalls)
        {
            if (shortfalls == null) throw new ArgumentNullException(nameof(shortfalls));
            int total = 0;
            foreach (int value in shortfalls)
                total += value;
            return total;
        }

        /// <summary>부족 합(0 이상)에 해당하는 등급.</summary>
        public static ExpeditionGrade Grade(int totalShortfall)
        {
            if (totalShortfall < 0) throw new ArgumentOutOfRangeException(nameof(totalShortfall));
            // 등급 경계 0·1·2·3은 FeatureManagerMap F7의 판정 기준이다.
            return (ExpeditionGrade)Math.Min(totalShortfall, (int)ExpeditionGrade.GreatFailure);
        }
    }
}
