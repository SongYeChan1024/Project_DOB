// 귀환 보상 기준값. SettlementManager가 읽어 VisitResultLog에 등급별 전리품 개수와 층별 재료 목록을 넘긴다.
// 💡 값은 시험값이다 (FeatureManagerMap F9 기본 전리품 3·2·1·0, 1층 일반 재료 4종). 전리품은 손님이 다녀온 층의 일반 재료에서 나온다 (2026-11-24 송예찬 확인, PR #29). 그중 무엇을 줄지는 💡 균등 추첨이다.
// 🔴 대성공일 때 30% 확률로 주는 그 층의 희귀 재료 1개(MainPlanning 5장, FirstPlayEconomy)는 아직 넣지 않았다.
//    기본 전리품에 더하는지 대신하는지, 첫 완료 범위에 넣는지가 FeatureManagerMap에 미정으로 남아 있다.
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DOB.Define
{
    /// <summary>한 층에서 전리품으로 나오는 일반 재료 목록.</summary>
    [Serializable]
    public struct FloorLoot
    {
        /// <summary>층 번호. 1부터.</summary>
        public int floor;
        /// <summary>그 층의 일반 재료 이름. 희귀 재료는 넣지 않는다.</summary>
        public string[] materials;
    }

    /// <summary>탐험 등급별 전리품 개수와 층별 재료 목록.</summary>
    [CreateAssetMenu(menuName = "DOB/귀환 보상 기준", fileName = "SO_ExpeditionRewards")]
    public sealed class ExpeditionRewards : ScriptableObject
    {
        /// <summary>ExpeditionGrade 순서(대성공·성공·실패·대실패)의 전리품 개수.</summary>
        [SerializeField] private int[] lootByGrade = new int[4];
        [SerializeField] private FloorLoot[] floors = Array.Empty<FloorLoot>();

        /// <summary>등급에 해당하는 전리품 개수. 값이 비어 있으면 0.</summary>
        public int LootCount(ExpeditionGrade grade) =>
            (int)grade < lootByGrade.Length ? Mathf.Max(0, lootByGrade[(int)grade]) : 0;

        /// <summary>층의 일반 재료 목록. 등록되지 않은 층은 빈 목록.</summary>
        public IReadOnlyList<string> Materials(int floor)
        {
            foreach (var entry in floors)
                if (entry.floor == floor && entry.materials != null)
                    return entry.materials;
            return Array.Empty<string>();
        }
    }
}
