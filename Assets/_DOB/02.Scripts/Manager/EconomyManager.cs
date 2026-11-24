// =====================================================================
// EconomyManager — 금화·재고·시장·가게 개량의 총괄 (FeatureManagerMap F8)
// 역할   : 지금은 던전 재료 재고(MaterialStock)를 하루 단위로 열어 주는 것까지만 한다.
// 호출자 : GameManager (BeginDay, 정산 저장 때 Materials.All) / SettlementManager가 받는 재고도 여기서 만든 것이다
// 호출   : MaterialStock
// 상태   : 없음 (재고는 MaterialStock이 소유한다)
// 의사코드:
//   BeginDay — 일차 시작 스냅샷의 던전 재료로 재고를 새로 만든다.
// shortcut: 마늘 재고는 주문(FirstGuestOrder), 금화는 주방(FirstGuestKitchen)이 아직 소유한다,
//           주문이 여러 개가 되는 방문 일반화 때 이곳의 System으로 옮긴다.
// 시장·가게 개량은 후속이다.
// =====================================================================
using System.Collections.Generic;
using DOB.Systems;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>경제 상태의 총괄. 실제 재고는 아래 System(MaterialStock)이 소유한다.</summary>
    [DisallowMultipleComponent]
    public sealed class EconomyManager : MonoBehaviour
    {
        /// <summary>오늘의 던전 재료 재고. BeginDay 전에는 null.</summary>
        public MaterialStock Materials { get; private set; }

        /// <summary>일차 시작 재고로 하루를 연다. 시작 재고가 없으면 빈 재고.</summary>
        public void BeginDay(IEnumerable<MaterialCount> startMaterials) => Materials = new MaterialStock(startMaterials);
    }
}
