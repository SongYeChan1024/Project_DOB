// =====================================================================
// MarketRules — 시장 양념 표와 판매가 기준값 (08.Data/Resources/SO_Market)
// 호출자 : EconomyManager(시작 금화·양념 재고, 구매), Market(장바구니 검증), PriceJudge(맛 점수·판매가),
//          FirstServiceSaveData(저장된 양념 id 검증), FirstGuestKitchen(양념 id → 입맛 기준 이름)
// 의사코드: 값은 인스펙터에서만 정한다.
//   TryGet(id)    — 양념 id로 한 줄을 찾는다. 없으면 false.
//   IsNotForSale  — 기본 재료·희귀 재료처럼 시장이 팔지 않는 품목인지.
//   Validate      — 양념 id·이름이 비거나 겹치지 않고, 가격·시작 재고·기준값이 범위 안인지 검사해 첫 오류를 돌려준다.
// 💡 값은 기획 05-데이터 「양념과 레시피」 표와 4-6 판매가 공식을 옮긴 것이며 송예찬 확인 전이다.
// =====================================================================
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DOB.Define
{
    /// <summary>시장 양념 한 종류.</summary>
    [Serializable]
    public struct SpiceEntry
    {
        /// <summary>저장·요청에 쓰는 영문 id.</summary>
        public string id;
        /// <summary>표시 이름. 입맛 기준(SO_TasteRules)의 양념 이름과 같다.</summary>
        public string name;
        /// <summary>효과 이름(+1).</summary>
        public string effect;
        /// <summary>더하는 맛 표현.</summary>
        public string flavor;
        /// <summary>개당 가격(금화).</summary>
        public int price;
        /// <summary>새 게임 시작 재고(개).</summary>
        public int startStock;
    }

    /// <summary>시작 금화, 시장 양념 표, 팔지 않는 품목, 맛 점수·판매가 기준.</summary>
    [CreateAssetMenu(menuName = "DOB/시장 기준", fileName = ResourceName)]
    public sealed class MarketRules : ScriptableObject
    {
        public const string ResourceName = "SO_Market";

        /// <summary>새 게임 시작 금화.</summary>
        [SerializeField, Min(0)] private int startGold;
        [SerializeField] private SpiceEntry[] spices = Array.Empty<SpiceEntry>();
        /// <summary>시장에서 팔지 않는 품목 이름·id(기본 재료·희귀 재료).</summary>
        [SerializeField] private string[] notForSale = Array.Empty<string>();
        /// <summary>입맛이 하나도 맞지 않은 접시의 맛 점수.</summary>
        [Header("맛 점수와 판매가"), SerializeField, Min(0)] private int baseTasteScore = 2;
        /// <summary>입맛 일치로 더할 수 있는 최대 점수.</summary>
        [SerializeField, Min(0)] private int maxTasteBonus = 2;
        /// <summary>판매가 배율 = priceBaseRate + priceTasteRate × 맛 점수.</summary>
        [SerializeField, Min(0f)] private float priceBaseRate = 0.6f;
        /// <summary>맛 점수 1당 더하는 판매가 배율.</summary>
        [SerializeField, Min(0f)] private float priceTasteRate = 0.2f;

        public int StartGold => startGold;
        public IReadOnlyList<SpiceEntry> Spices => spices;
        public int BaseTasteScore => baseTasteScore;
        public int MaxTasteBonus => maxTasteBonus;
        public float PriceBaseRate => priceBaseRate;
        public float PriceTasteRate => priceTasteRate;

        /// <summary>Resources의 시장 기준. 없으면 null.</summary>
        public static MarketRules Load() => Resources.Load<MarketRules>(ResourceName);

        /// <summary>양념 id의 한 줄. 없으면 false.</summary>
        public bool TryGet(string id, out SpiceEntry entry)
        {
            foreach (var spice in spices)
                if (spice.id == id)
                {
                    entry = spice;
                    return true;
                }
            entry = default;
            return false;
        }

        /// <summary>시장이 팔지 않는 품목인지.</summary>
        public bool IsNotForSale(string id) => id != null && Array.IndexOf(notForSale, id) >= 0;

        /// <summary>기준값 오류. 문제가 없으면 null.</summary>
        public string Validate()
        {
            if (spices == null || spices.Length == 0) return "양념이 하나 이상 있어야 합니다.";
            var ids = new HashSet<string>();
            var names = new HashSet<string>();
            foreach (var spice in spices)
            {
                if (string.IsNullOrWhiteSpace(spice.id) || !ids.Add(spice.id)) return "양념 id가 비었거나 겹칩니다.";
                if (string.IsNullOrWhiteSpace(spice.name) || !names.Add(spice.name)) return $"{spice.id}의 이름이 비었거나 겹칩니다.";
                if (spice.price < 1 || spice.startStock < 0) return $"{spice.id}의 가격은 1 이상, 시작 재고는 0 이상이어야 합니다.";
                if (IsNotForSale(spice.id)) return $"{spice.id}가 팔지 않는 품목에도 있습니다.";
            }
            if (startGold < 0 || !(priceBaseRate >= 0f) || !(priceTasteRate >= 0f)) return "시작 금화·판매가 배율이 음수입니다.";
            return null;
        }
    }
}
