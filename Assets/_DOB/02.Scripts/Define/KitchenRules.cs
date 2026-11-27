// =====================================================================
// KitchenRules — 주방 기준값: 조리 시간·시작/최대 화구 수·화구 가격·조리도구·기본 재료·레시피 목록 (08.Data/Resources/SO_Kitchen)
// 호출자 : KitchenManager(하루를 열 때 읽음), RecipeJudge(재료 조합 → 레시피), RecipeBook(해금),
//          OrderBook(조리 시간·화구 수·기본 재료), FirstServiceSaveData(저장된 레시피 id 검증)
// 의사코드: 값은 인스펙터에서만 정한다.
//   TryGet(id)      — 레시피 id로 한 줄을 찾는다.
//   IsBasic(id)     — 무료·무한인 기본 재료(밀가루·감자·고기)인지.
//   Validate(taste) — id·이름 중복, 빈 재료, 0 이하 개수, 모르는 효과·도구, 맛 기준(SO_TasteRules) 누락,
//                     여는 재료 없는 잠긴 레시피를 찾아 첫 오류를 돌려준다.
// 💡 값은 기획 05-데이터 「첫 영업 재료와 경제」의 첫 플레이 레시피 8개를 옮긴 것이며 송예찬 확인 전이다.
//    레시피별 조리도구와 해금 재료는 기획 미정(🔴 도구, 🟡 해금)이라 임의로 정한 값이다.
//    시작 화구 1칸·두 번째 화구 구매(금화 60)는 2026-11-27 김언호 결정의 임의값이다(기획 문서의 「💡 시작 화구 2개」와 다름, 송예찬 확인 전).
// =====================================================================
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DOB.Define
{
    /// <summary>재료 하나와 개수.</summary>
    [Serializable]
    public struct IngredientAmount
    {
        /// <summary>재료 id(한글 이름, 던전 재료 재고·전리품과 같다).</summary>
        public string id;
        public int count;
    }

    /// <summary>효과 이름과 막는 칸 수.</summary>
    [Serializable]
    public struct RecipeEffect
    {
        /// <summary>효과 이름(해독·내열·방한·야광·활력·감각). RiskAxes.AxisOfEffect로 위험 축을 찾는다.</summary>
        public string effect;
        public int amount;
    }

    /// <summary>레시피 한 줄.</summary>
    [Serializable]
    public struct RecipeEntry
    {
        /// <summary>저장·요청·그림(T_UI_Dish{id})에 쓰는 영문 id.</summary>
        public string id;
        /// <summary>표시 이름. 맛 기준(SO_TasteRules)의 요리 이름과 같다.</summary>
        public string name;
        /// <summary>조리도구 id(도구 목록 안). 💡 임의값.</summary>
        public string tool;
        public IngredientAmount[] ingredients;
        public RecipeEffect[] effects;
        /// <summary>판매가 계산의 기본가(금화).</summary>
        public int basePrice;
        /// <summary>새 게임부터 열려 있다.</summary>
        public bool startUnlocked;
        /// <summary>이 중 하나를 처음 손에 넣으면 열린다(💡 하나 규칙).</summary>
        public string[] unlockBy;
    }

    /// <summary>조리 시간·화구 수·조리도구·기본 재료·레시피 목록.</summary>
    [CreateAssetMenu(menuName = "DOB/주방 기준", fileName = ResourceName)]
    public sealed class KitchenRules : ScriptableObject
    {
        public const string ResourceName = "SO_Kitchen";

        /// <summary>모든 레시피가 같은 자동 조리 시간(초).</summary>
        [SerializeField, Min(0.1f)] private float cookSeconds = 6f;
        /// <summary>새 게임의 화구 수.</summary>
        [SerializeField, Min(1)] private int startBurners = 1;
        /// <summary>준비 단계에서 사서 늘릴 수 있는 최대 화구 수.</summary>
        [SerializeField, Min(1)] private int maxBurners = 2;
        /// <summary>화구 하나를 더 사는 값(금화).</summary>
        [SerializeField, Min(1)] private int burnerPrice = 60;
        [SerializeField] private string[] tools = Array.Empty<string>();
        /// <summary>무료·무한인 기본 재료 id.</summary>
        [SerializeField] private string[] basicIngredients = Array.Empty<string>();
        [SerializeField] private RecipeEntry[] recipes = Array.Empty<RecipeEntry>();

        public float CookSeconds => cookSeconds;
        public int StartBurners => startBurners;
        public int MaxBurners => maxBurners;
        public int BurnerPrice => burnerPrice;
        public IReadOnlyList<string> Tools => tools;
        public IReadOnlyList<RecipeEntry> Recipes => recipes;

        /// <summary>Resources의 주방 기준. 없으면 null.</summary>
        public static KitchenRules Load() => Resources.Load<KitchenRules>(ResourceName);

        /// <summary>레시피 id의 한 줄. 없으면 false.</summary>
        public bool TryGet(string id, out RecipeEntry entry)
        {
            foreach (var recipe in recipes)
                if (recipe.id == id)
                {
                    entry = recipe;
                    return true;
                }
            entry = default;
            return false;
        }

        /// <summary>무료·무한인 기본 재료인지.</summary>
        public bool IsBasic(string id) => id != null && Array.IndexOf(basicIngredients, id) >= 0;

        /// <summary>기준값 오류. 문제가 없으면 null. taste가 있으면 레시피마다 맛 기준이 있는지도 본다.</summary>
        public string Validate(TasteRules taste = null)
        {
            if (recipes == null || recipes.Length == 0) return "레시피가 하나 이상 있어야 합니다.";
            if (tools == null || tools.Length == 0) return "조리도구가 하나 이상 있어야 합니다.";
            if (startBurners < 1 || maxBurners < startBurners) return "화구 수는 1 ≤ 시작 ≤ 최대여야 합니다.";
            if (burnerPrice < 1) return "화구 가격은 1 이상이어야 합니다.";
            var ids = new HashSet<string>();
            var names = new HashSet<string>();
            foreach (var recipe in recipes)
            {
                if (string.IsNullOrWhiteSpace(recipe.id) || !ids.Add(recipe.id)) return "레시피 id가 비었거나 겹칩니다.";
                if (string.IsNullOrWhiteSpace(recipe.name) || !names.Add(recipe.name)) return $"{recipe.id}의 이름이 비었거나 겹칩니다.";
                if (Array.IndexOf(tools, recipe.tool) < 0) return $"{recipe.name}의 도구가 도구 목록에 없습니다.";
                if (recipe.ingredients == null || recipe.ingredients.Length == 0) return $"{recipe.name}에 재료가 없습니다.";
                var seen = new HashSet<string>();
                foreach (var item in recipe.ingredients)
                    if (string.IsNullOrWhiteSpace(item.id) || item.count < 1 || !seen.Add(item.id)) return $"{recipe.name}의 재료가 비었거나 겹치거나 개수가 0 이하입니다.";
                foreach (var effect in recipe.effects ?? Array.Empty<RecipeEffect>())
                    if (RiskAxes.AxisOfEffect(effect.effect) == null || effect.amount < 1) return $"{recipe.name}의 효과 {effect.effect}를 알 수 없거나 칸 수가 0 이하입니다.";
                if (recipe.basePrice < 1) return $"{recipe.name}의 기본가는 1 이상이어야 합니다.";
                if (!recipe.startUnlocked && (recipe.unlockBy == null || recipe.unlockBy.Length == 0)) return $"{recipe.name}은 잠겨 있는데 여는 재료가 없습니다.";
                if (taste && taste.FlavorsOf(recipe.name).Count == 0) return $"{recipe.name}의 맛이 맛 기준에 없습니다.";
            }
            return null;
        }
    }
}
