// 귀환 보상 기준값. SettlementManager가 읽어 마감 정산 때 VisitResultLog.WithGifts에 자격 호감·개수·층별 희귀 재료를 넘긴다.
// 전리품·선물은 누적 개인 호감이 기준 이상인 손님만, 다녀온 던전 층의 희귀 재료로 받는다 (2026-11-25 사용자 정정, FeatureManagerMap F9).
// 기준 9는 확정값이다. 💡 받는 개수(1)와 등급 무관(부상 귀환도 지급)은 김언호 가안이며 송예찬 확인 전이다.
// 호감과 무관한 등급별 일반 재료 3·2·1·0과 대성공 30% 희귀 재료는 정정 전 구안이라 지급하지 않는다.
using System;
using UnityEngine;

namespace DOB.Define
{
    /// <summary>한 층에서 나오는 던전 재료 목록.</summary>
    [Serializable]
    public struct FloorLoot
    {
        /// <summary>층 번호. 1부터.</summary>
        public int floor;
        /// <summary>그 층의 일반 재료 이름. 희귀 재료는 넣지 않는다. 지금은 전리품으로 주지 않는다(정정 전 구안).</summary>
        public string[] materials;
        /// <summary>그 층의 희귀 재료 이름. 없으면 비워 둔다.</summary>
        public string rareMaterial;
    }

    /// <summary>전리품·선물 자격 호감, 받는 개수, 층별 재료 목록.</summary>
    [CreateAssetMenu(menuName = "DOB/귀환 보상 기준", fileName = "SO_ExpeditionRewards")]
    public sealed class ExpeditionRewards : ScriptableObject
    {
        /// <summary>전리품·선물 자격이 생기는 누적 개인 호감(점, 이상).</summary>
        [SerializeField, Min(0)] private int giftAffinityMin = 9;
        /// <summary>자격이 있는 손님이 받는 희귀 재료 개수. 0 이상.</summary>
        [SerializeField, Min(0)] private int giftCount = 1;
        [SerializeField] private FloorLoot[] floors = Array.Empty<FloorLoot>();

        /// <summary>전리품·선물 자격이 생기는 누적 개인 호감(점, 이상).</summary>
        public int GiftAffinityMin => giftAffinityMin;
        /// <summary>자격이 있는 손님이 받는 희귀 재료 개수.</summary>
        public int GiftCount => Mathf.Max(0, giftCount);

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
