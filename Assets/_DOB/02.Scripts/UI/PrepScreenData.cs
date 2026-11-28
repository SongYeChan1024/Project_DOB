// =====================================================================
// PrepScreenData — 준비 화면(시장 · 레시피 · 수첩 · 가게 개량 · 입구 문패)에 보여 줄 표시 데이터
// 역할   : 화면이 그릴 값만 담는다. 금화 차감 · 재고 · 해금 판정은 담지 않는다 (백엔드 몫).
// 호출자 : PrepScreenPresenter가 채워서 PrepScreensView.Open / Refresh에 넘긴다.
// 상태   : 없음 (읽기 전용 값 묶음)
// 비고   : 계약 초안의 PrepView와 같은 내용. 김언호와 계약이 정해지면 DOB.Define 계약 타입으로 옮긴다.
// =====================================================================
using System;
using UnityEngine;

namespace DOB.UI
{
    /// <summary>준비 단계에서 여는 화면 종류.</summary>
    public enum PrepScreen
    {
        None,
        /// <summary>시장 (양념 구매)</summary>
        Market,
        /// <summary>레시피 책</summary>
        RecipeBook,
        /// <summary>수첩 (Tab)</summary>
        Notebook,
        /// <summary>가게 개량</summary>
        Upgrade,
        /// <summary>입구 문패 (영업 시작 확인)</summary>
        DoorSign,
    }

    /// <summary>준비 화면 전체 표시 값.</summary>
    [Serializable]
    public sealed class PrepViewData
    {
        public int gold;
        /// <summary>오늘의 소식 한 줄 (이변). 없으면 비움.</summary>
        public string news;

        public SpiceItemData[] spices = Array.Empty<SpiceItemData>();

        public RecipeItemData[] recipes = Array.Empty<RecipeItemData>();
        /// <summary>전체 레시피 수 (예: 22). "열린 수 / 전체"로 보여 준다.</summary>
        public int recipeTotal;

        public UpgradeItemData[] upgrades = Array.Empty<UpgradeItemData>();

        /// <summary>층 목록 (수첩 '층별 위험' 쪽). 아직 못 간 층은 opened = false.</summary>
        public FloorItemData[] floors = Array.Empty<FloorItemData>();

        /// <summary>수첩 '종족 · 직업' 쪽에 보일 이름 목록 (기획 10장 순서).</summary>
        public CodexItemData[] races = Array.Empty<CodexItemData>();
        public CodexItemData[] jobs = Array.Empty<CodexItemData>();

        /// <summary>입구 문패 위쪽 큰 칸 (예: 오늘 손님 | 6명).</summary>
        public DoorStatData[] doorStats = Array.Empty<DoorStatData>();
        /// <summary>입구 문패: 가진 양념 · 가진 재료 (개수 0인 것은 빼고 넘긴다).</summary>
        public StockItemData[] doorSpices = Array.Empty<StockItemData>();
        public StockItemData[] doorIngredients = Array.Empty<StockItemData>();

        /// <summary>수첩 기록. 비우면 이번 플레이 공용 기록(NotebookRecord.Session)을 쓴다.</summary>
        [NonSerialized] public NotebookRecord notebook;
    }

    /// <summary>입구 문패 위쪽 큰 칸 하나.</summary>
    [Serializable]
    public sealed class DoorStatData
    {
        public string label;
        public string value;
    }

    /// <summary>가진 물건 하나 (그림 + 개수).</summary>
    [Serializable]
    public sealed class StockItemData
    {
        public string id;
        public string name;
        /// <summary>마우스를 올리면 이름 아래에 보일 설명 (예: 해독 +1 · 향긋).</summary>
        public string description;
        public int count;
        public Sprite icon;
    }

    /// <summary>수첩 도감의 종족 · 직업 하나.</summary>
    [Serializable]
    public sealed class CodexItemData
    {
        public string name;
        /// <summary>약점이 없는 종족 (인간). '약점 없음'으로 보인다.</summary>
        public bool noWeakness;
    }

    /// <summary>시장에서 파는 양념 하나.</summary>
    [Serializable]
    public sealed class SpiceItemData
    {
        public string id;
        public string name;
        /// <summary>효과 이름 (해독 · 내열 · 방한 · 야광 · 활력 · 감각).</summary>
        public string effect;
        public string flavor;
        public int price;
        public int owned;
        public Sprite icon;
    }

    /// <summary>레시피 하나.</summary>
    [Serializable]
    public sealed class RecipeItemData
    {
        public string id;
        public string name;
        /// <summary>false면 목록에 ???로 보인다.</summary>
        public bool unlocked;
        /// <summary>재료 표시 문구 (예: 밀가루 × 2).</summary>
        public string ingredients;
        /// <summary>기본 재료만 쓰는 레시피 (무제한 표시).</summary>
        public bool basicOnly;
        public EffectAmount[] effects = Array.Empty<EffectAmount>();
        public string flavor;
        public int basePrice;
        /// <summary>요리 그림 (레시피 책 오른쪽 쪽).</summary>
        public Sprite picture;
    }

    /// <summary>효과 이름과 칸 수 (예: 활력 +1).</summary>
    [Serializable]
    public sealed class EffectAmount
    {
        public string name;
        public int value;
    }

    /// <summary>가게 개량 하나.</summary>
    [Serializable]
    public sealed class UpgradeItemData
    {
        public string id;
        public string name;
        public string effect;
        /// <summary>지금 → 다음 (예: 3석 → 4석). 없으면 비움.</summary>
        public string detail;
        public int level;
        public int maxLevel;
        /// <summary>다음 단계 가격. 다 올렸으면 0.</summary>
        public int price;
        /// <summary>지금 살 수 있는지 (백엔드 판정 결과를 그대로 받는다).</summary>
        public bool canBuy;
        public Sprite picture;
    }

    /// <summary>던전 층 하나.</summary>
    [Serializable]
    public sealed class FloorItemData
    {
        /// <summary>층 번호 (1~5).</summary>
        public int number;
        /// <summary>층 이름 (예: 슬라임 숲). 수첩 기록 이름은 "1층 슬라임 숲" 꼴로 맞춘다.</summary>
        public string name;
        public bool opened;
        /// <summary>이 층의 기본 위험 개수. 수첩에 이만큼 칸을 그린다 (이름은 단서를 찾아야 보임).</summary>
        public int riskCount;
        /// <summary>귀환 보고로 공개된 이 층의 위험 이름 (예: 독 · 체력). 단서로 찾은 위험과 함께 칸에 보인다.</summary>
        public string[] revealedRisks = Array.Empty<string>();
        /// <summary>이 층에서 나오는 재료. seen = 손님이 가져와 본 적 있음.</summary>
        public FloorIngredientData[] ingredients = Array.Empty<FloorIngredientData>();
    }

    /// <summary>층별 재료 하나.</summary>
    [Serializable]
    public sealed class FloorIngredientData
    {
        public string id;
        /// <summary>마우스를 올리면 보일 이름 · 설명 (못 본 재료는 화면이 ???로 바꿔 보여 줌).</summary>
        public string name;
        public string description;
        public Sprite icon;
        public bool seen;
    }

    /// <summary>위험 · 효과 이름별 표시 색. 위험과 그 위험을 막는 효과는 같은 색.</summary>
    public static class PrepPalette
    {
        public static Color For(string riskOrEffect)
        {
            switch (riskOrEffect)
            {
                case "독": case "해독":   return new Color32(66, 120, 50, 255);
                case "열": case "내열":   return new Color32(175, 90, 30, 255);
                case "냉기": case "방한": return new Color32(50, 110, 165, 255);
                case "어둠": case "야광": return new Color32(95, 62, 140, 255);
                case "체력": case "활력": return new Color32(150, 112, 20, 255);
                case "함정": case "감각": return new Color32(160, 65, 105, 255);
                default: return new Color32(110, 85, 60, 255);
            }
        }
    }
}
