// 귀환 보상 기준값. SettlementManager가 읽어 VisitResultLog에 등급별 전리품 개수와 층별 재료 목록을 넘긴다.
// 💡 값은 시험값이다 (FeatureManagerMap F9 기본 전리품 3·2·1·0, 1층 일반 재료 4종). 🔴 전리품 종류 규칙은 기획 확인 대기.
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
