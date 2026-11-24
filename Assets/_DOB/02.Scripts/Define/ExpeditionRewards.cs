// 귀환 보상 기준값. SettlementManager가 읽어 VisitResultLog에 등급별 전리품 개수, 층별 재료 목록, 희귀 재료와 확률을 넘긴다.
// 💡 값은 시험값이다 (FeatureManagerMap F9 기본 전리품 3·2·1·0, 1층 일반 재료 4종, 희귀 재료 30%).
// 전리품은 손님이 다녀온 층의 일반 재료에서 나온다 (2026-11-24 송예찬 확인, PR #29). 그중 무엇을 줄지는 💡 균등 추첨이다.
// 대성공일 때만 정해진 확률로 그 층의 희귀 재료 1개를 더 준다 (MainPlanning 5장, FirstPlayEconomy).
// 💡 희귀 재료는 기본 전리품에 더한다. 대신하는지 더하는지는 FeatureManagerMap에 미정으로 남아 있다.
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DOB.Define
{
    /// <summary>한 층에서 전리품으로 나오는 재료 목록.</summary>
    [Serializable]
    public struct FloorLoot
    {
        /// <summary>층 번호. 1부터.</summary>
        public int floor;
        /// <summary>그 층의 일반 재료 이름. 희귀 재료는 넣지 않는다.</summary>
        public string[] materials;
        /// <summary>그 층의 희귀 재료 이름. 없으면 비워 둔다.</summary>
        public string rareMaterial;
    }

    /// <summary>탐험 등급별 전리품 개수, 층별 재료 목록, 희귀 재료 확률.</summary>
    [CreateAssetMenu(menuName = "DOB/귀환 보상 기준", fileName = "SO_ExpeditionRewards")]
    public sealed class ExpeditionRewards : ScriptableObject
    {
        /// <summary>ExpeditionGrade 순서(대성공·성공·실패·대실패)의 전리품 개수.</summary>
        [SerializeField] private int[] lootByGrade = new int[4];
        /// <summary>대성공일 때 그 층의 희귀 재료 1개를 더 받을 확률. 0부터 1.</summary>
        [SerializeField, Range(0f, 1f)] private float rareLootChance;
        [SerializeField] private FloorLoot[] floors = Array.Empty<FloorLoot>();

        /// <summary>대성공일 때 희귀 재료를 받을 확률. 0부터 1.</summary>
        public float RareLootChance => rareLootChance;

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

        /// <summary>층의 희귀 재료 이름. 등록되지 않았으면 null.</summary>
        public string RareMaterial(int floor)
        {
            foreach (var entry in floors)
                if (entry.floor == floor)
                    return entry.rareMaterial;
            return null;
        }
    }
}
