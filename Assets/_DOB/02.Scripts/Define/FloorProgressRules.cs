// =====================================================================
// FloorProgressRules — 층 개방 기준과 층별 위험 축 (08.Data/Resources/SO_FloorProgress)
// 호출자 : FloorProgress(개방·공개 판정), SettlementManager(하루를 열 때 불러옴)
// 의사코드: 값은 인스펙터에서만 정한다.
//   AxesOf(층)  — 1부터 센 층의 위험 축 이름. 없는 층이면 빈 목록.
//   Validate    — 개방 승수·공개 수가 1 이상이고, 층이 하나 이상이며 축이 RiskAxes 이름 안에서 겹치지 않는지 검사한다.
// 💡 값은 기획 3-3 「던전 층과 재료」의 기본 위험 축과 판정·경제 상수 floorOpenWins를 옮긴 것이며 송예찬 확인 전이다.
// =====================================================================
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DOB.Define
{
    /// <summary>던전 층 하나의 이름과 위험 축.</summary>
    [Serializable]
    public struct FloorEntry
    {
        public string name;
        /// <summary>그 층의 위험 축 이름(RiskAxes.All의 이름).</summary>
        public string[] axes;
    }

    /// <summary>다음 층을 여는 성공 수, 보고 하나가 공개하는 축 수, 층 목록.</summary>
    [CreateAssetMenu(menuName = "DOB/층 진행 기준", fileName = ResourceName)]
    public sealed class FloorProgressRules : ScriptableObject
    {
        public const string ResourceName = "SO_FloorProgress";

        /// <summary>최심층 성공 이상 귀환이 이 수에 이르면 다음 층을 연다.</summary>
        [SerializeField, Min(1)] private int openWins = 2;
        /// <summary>보고 하나가 공개하는 미확인 위험 축의 최대 수.</summary>
        [SerializeField, Min(1)] private int revealPerReport = 2;
        /// <summary>1층부터 순서대로. 마지막 층에서는 더 열지 않는다.</summary>
        [SerializeField] private FloorEntry[] floors = Array.Empty<FloorEntry>();

        public int OpenWins => openWins;
        public int RevealPerReport => revealPerReport;
        public int FloorCount => floors.Length;

        /// <summary>Resources의 층 진행 기준. 없으면 null.</summary>
        public static FloorProgressRules Load() => Resources.Load<FloorProgressRules>(ResourceName);

        /// <summary>1부터 센 층의 위험 축. 없는 층이면 빈 목록.</summary>
        public IReadOnlyList<string> AxesOf(int floor) =>
            floor >= 1 && floor <= floors.Length && floors[floor - 1].axes != null ? floors[floor - 1].axes : (IReadOnlyList<string>)Array.Empty<string>();

        /// <summary>기준값 오류. 문제가 없으면 null.</summary>
        public string Validate()
        {
            if (openWins < 1 || revealPerReport < 1) return "개방 승수와 공개 수는 1 이상이어야 합니다.";
            if (floors == null || floors.Length == 0) return "층이 하나 이상 있어야 합니다.";
            for (int i = 0; i < floors.Length; i++)
            {
                var seen = new HashSet<string>();
                foreach (string axis in floors[i].axes ?? Array.Empty<string>())
                    if (!IsRiskAxis(axis) || !seen.Add(axis))
                        return $"{i + 1}층의 위험 축({axis})이 없는 이름이거나 겹칩니다.";
            }
            return null;
        }

        private static bool IsRiskAxis(string axis)
        {
            foreach (string known in RiskAxes.All)
                if (known == axis) return true;
            return false;
        }
    }
}
