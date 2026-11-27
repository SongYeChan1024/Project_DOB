// KitchenManager — 주방 기준(레시피·조리도구·화구 수)과 열린 레시피 장부를 소유하고, 화구 위치·레시피 조회를 맡는다.
// 호출자: GameManager(하루를 열 때 BeginDay, 아침 보고 확정 뒤 Refresh, 저장 때 Unlocked), FirstGuestKitchen(레시피 판정·화구 위치 조회).
// 호출  : KitchenRules.Load, RecipeBook, RecipeJudge.
// 상태  : 주방 기준, 열린 레시피 장부. 주문·조리 진행은 아직 FirstGuestKitchen의 OrderBook에 있다(주방 전체 이전은 후속).
// 의사코드:
//   BeginDay(저장된 열린 목록, 던전 재료 재고) — 기준을 읽고 검증한다. 저장 목록이 있으면 그것과 시작 레시피를, 없으면(이전 스키마)
//     시작 레시피를 연 뒤, 지금 재료 재고로 열 수 있는 레시피를 더 연다.
//   Refresh(재고) — 아침 보고 전리품이 들어온 뒤 새로 열 레시피를 연다.
//   Identify — 재료 조합·도구를 열린 레시피 기준으로 판정한다(RecipeJudge).
//   화구 수 — 하루를 열 때 저장값(없으면 시작 수)을 시작~최대로 맞춘다. TryBuyBurner는 최대 미만일 때 경제에 대금을 요청하고
//             성공하면 하나 늘린다(준비 단계에서만, 시장 닫힘·금화 부족이면 그대로). 💡 시작 1칸·최대 2칸·금화 60.
//   BurnerAt(i, 대체) — 화구 기준점이 연결돼 있으면 그 위치. 없으면 대체 위치(주방 기준점)의 자식 「Burner{i}」,
//             그것도 없으면 0번은 주방 기준점, 나머지는 💡 옆으로 벌린 임시 자식을 만든다(배지가 겹치지 않게).
using System.Collections.Generic;
using DOB.Define;
using DOB.Systems;
using UnityEngine;

namespace DOB.Manager
{
    public sealed class KitchenManager : MonoBehaviour
    {
        /// <summary>화구별 월드 기준점. 💡 비어 있으면 모든 화구가 주방 기준점을 쓴다(두 번째 화구 기준점은 이기혁 인계).</summary>
        [SerializeField] private Transform[] burnerAnchors = new Transform[0];

        /// <summary>기준점이 없을 때 임시 화구를 주방 기준점 오른쪽으로 벌리는 간격(로컬 단위). 💡 이기혁 배치 전 임시값.</summary>
        [SerializeField, Min(0f)] private float fallbackSpacing = 0.8f;

        private RecipeBook book;
        /// <summary>지금 가진 화구 수(시작~최대).</summary>
        public int OwnedBurners { get; private set; } = 1;
        /// <summary>다음 화구 값. 다 샀거나 기준이 없으면 null.</summary>
        public int? NextBurnerPrice => Rules && OwnedBurners < Rules.MaxBurners ? Rules.BurnerPrice : (int?)null;
        public KitchenRules Rules { get; private set; }
        /// <summary>기준이 없거나 잘못됐으면 그 이유. 정상이면 null.</summary>
        public string SetupError { get; private set; }
        /// <summary>열린 레시피 id(레시피 목록 순서).</summary>
        public IReadOnlyList<string> Unlocked => book != null ? book.Unlocked : (IReadOnlyList<string>)new string[0];

        /// <summary>하루를 연다. saved가 null이면 이전 스키마로 보고 시작 레시피와 재료 재고로 다시 연다.</summary>
        public void BeginDay(IEnumerable<string> saved, MaterialStock materials, int? savedBurners = null)
        {
            Rules = KitchenRules.Load();
            SetupError = !Rules ? $"주방 기준({KitchenRules.ResourceName})이 Resources에 없습니다."
                : Rules.Validate(Resources.Load<TasteRules>(TasteRules.ResourceName));
            if (SetupError != null)
                return;
            OwnedBurners = ClampBurners(savedBurners ?? Rules.StartBurners);
            book = new RecipeBook(Rules, saved);
            book.RefreshFrom(materials);
        }

        /// <summary>재료가 새로 들어온 뒤 열 수 있는 레시피를 연다. 새로 연 id.</summary>
        public IReadOnlyList<string> Refresh(MaterialStock materials) => book != null ? book.RefreshFrom(materials) : new string[0];

        public bool IsUnlocked(string recipeId) => book != null && book.IsUnlocked(recipeId);

        /// <summary>열린 레시피 기준의 재료 조합 판정. tool이 null이면 도구를 보지 않는다.</summary>
        public RecipeMatch Identify(IReadOnlyDictionary<string, int> ingredients, string tool, out RecipeEntry recipe)
        {
            recipe = default;
            return Rules ? RecipeJudge.Identify(Rules, IsUnlocked, ingredients, tool, out recipe) : RecipeMatch.WrongIngredients;
        }

        /// <summary>수동 저장의 진행(누른 시점의 화구 수)으로 바꾼다. null이면 그대로.</summary>
        public void RestoreBurners(int? count)
        {
            if (Rules && count.HasValue)
                OwnedBurners = ClampBurners(count.Value);
        }

        /// <summary>화구 하나를 산다. 다 샀으면 NotForSale, 그 밖의 거절은 경제의 결과(시장 닫힘·금화 부족)이고 아무것도 바꾸지 않는다.</summary>
        public PurchaseResult TryBuyBurner(EconomyManager economy)
        {
            if (!economy)
                return PurchaseResult.MarketClosed;
            if (!(NextBurnerPrice is int price))
                return PurchaseResult.NotForSale;
            var result = economy.Spend(price);
            if (result == PurchaseResult.Purchased)
                OwnedBurners++;
            return result;
        }

        /// <summary>화구의 월드 위치. 기준점이 없으면 fallback의 자식 「Burner{i}」 또는 임시 위치.</summary>
        public Transform BurnerAt(int index, Transform fallback)
        {
            if (index >= 0 && index < burnerAnchors.Length && burnerAnchors[index])
                return burnerAnchors[index];
            if (!fallback || index < 0)
                return fallback;
            string name = "Burner" + index;
            var anchor = fallback.Find(name);
            if (anchor || index == 0)
                return anchor ? anchor : fallback;
            anchor = new GameObject(name).transform;
            anchor.SetParent(fallback, false);
            anchor.localPosition = Vector3.right * (fallbackSpacing * index);
            return anchor;
        }

        private int ClampBurners(int count) => Mathf.Clamp(count, Rules.StartBurners, Rules.MaxBurners);
    }
}
