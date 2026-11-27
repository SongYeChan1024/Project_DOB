// =====================================================================
// EconomyManager — 금화·양념 재고·던전 재료·시장의 총괄 (FeatureManagerMap F8)
// 역할   : 하루를 열 때 금화(Wallet)·양념 재고·던전 재료 재고(MaterialStock 두 개)를 만들고,
//          시장 구매·양념 소비·서빙 대금 요청을 아래 System에 전달한다. 같은 값을 다른 곳에 따로 두지 않는다.
// 호출자 : GameManager (BeginDay, MarketOpen 연결, 수동 저장 때 조회·Restore, 정산 저장 때 Gold·Spices·Materials)
//          FirstGuestKitchen (양념 조회·OrderBook에 양념 재고 전달, 서빙 대금 Earn)
//          준비 화면의 Presenter (Buy, 조회) — 화면 연결은 프론트엔드 인계 항목
//          SettlementManager가 받는 던전 재료 재고도 여기서 만든 것이다
// 호출   : Wallet, MaterialStock, Market, MarketRules(Resources/SO_Market)
// 상태   : 일차 시작 금화·양념 (수동 저장이 일차 시작 스냅샷을 다시 쓸 때 쓴다)
// 의사코드:
//   BeginDay(금화, 양념, 재료) — 저장값이 없으면(새 게임) 시장 기준의 시작 금화·양념 시작 재고로 연다.
//   Restore(금화, 양념)       — 수동 저장을 불러올 때 누른 시점의 금화·양념으로 바꾼다.
//   Buy(장바구니)              — 시장이 열려 있지 않으면(준비 단계가 아니면) 시장 닫힘. 아니면 Market에 맡긴다.
//   Earn                       — 지갑에 그대로 전달한다. 양념 소비는 OrderBook이 이 양념 재고(Spices)에 요청한다.
// 가게 개량·행상은 후속이다.
// =====================================================================
using System;
using System.Collections.Generic;
using DOB.Define;
using DOB.Systems;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>경제 상태의 총괄. 실제 금화·재고는 아래 System이 소유한다.</summary>
    [DisallowMultipleComponent]
    public sealed class EconomyManager : MonoBehaviour
    {
        private MarketRules rules;
        private Wallet wallet;

        /// <summary>시장 기준(Resources/SO_Market). 없으면 null.</summary>
        public MarketRules Rules => rules ? rules : rules = MarketRules.Load();
        /// <summary>기준이 빠졌거나 잘못됐으면 오류 문장, 아니면 null.</summary>
        public string SetupError => Rules ? Rules.Validate() : "Resources/SO_Market이 없습니다.";

        /// <summary>오늘의 던전 재료 재고. BeginDay 전에는 null.</summary>
        public MaterialStock Materials { get; private set; }
        /// <summary>오늘의 양념 재고(양념 id별). BeginDay 전에는 null.</summary>
        public MaterialStock Spices { get; private set; }
        /// <summary>지금 금화. BeginDay 전에는 0.</summary>
        public int Gold => wallet?.Gold ?? 0;
        /// <summary>이 일차를 연 금화.</summary>
        public int DayStartGold { get; private set; }
        /// <summary>이 일차를 연 양념 재고.</summary>
        public IReadOnlyList<MaterialCount> DayStartSpices { get; private set; } = Array.Empty<MaterialCount>();
        /// <summary>시장이 열려 있는지(준비 단계인지). GameManager가 연결한다.</summary>
        public Func<bool> MarketOpen { get; set; }

        /// <summary>일차 시작 값으로 하루를 연다. 금화·양념이 null이면 새 게임의 시작 값.</summary>
        public void BeginDay(int? gold, IEnumerable<MaterialCount> spices, IEnumerable<MaterialCount> startMaterials)
        {
            DayStartGold = gold ?? Rules.StartGold;
            DayStartSpices = new List<MaterialCount>(spices ?? StartSpices());
            wallet = new Wallet(DayStartGold);
            Spices = new MaterialStock(DayStartSpices);
            Materials = new MaterialStock(startMaterials);
        }

        /// <summary>수동 저장을 불러올 때 누른 시점의 금화·양념으로 바꾼다. 하루를 열기 전이거나 값이 잘못됐으면 false.</summary>
        public bool Restore(int gold, IEnumerable<MaterialCount> spices)
        {
            if (wallet == null || gold < 0)
                return false;
            try { Spices = new MaterialStock(spices); }
            catch (ArgumentException) { return false; }
            wallet = new Wallet(gold);
            return true;
        }

        /// <summary>그 양념의 보유 수.</summary>
        public int SpiceCount(string id) => Spices?.Count(id) ?? 0;

        /// <summary>시장 구매 요청. 준비 단계가 아니면 시장 닫힘이고 아무것도 바꾸지 않는다.</summary>
        public PurchaseOutcome Buy(IReadOnlyDictionary<string, int> cart)
        {
            if (wallet == null || MarketOpen?.Invoke() != true)
                return new PurchaseOutcome(PurchaseResult.MarketClosed, Gold, Spices?.All() ?? new List<MaterialCount>());
            return Market.TryBuy(Rules, wallet, Spices, cart);
        }

        /// <summary>서빙 대금을 더한다. 하루를 열기 전이거나 음수면 false.</summary>
        public bool Earn(int gold) => wallet != null && wallet.Earn(gold);

        private IEnumerable<MaterialCount> StartSpices()
        {
            foreach (var spice in Rules.Spices)
                if (spice.startStock > 0)
                    yield return new MaterialCount(spice.id, spice.startStock);
        }
    }
}
