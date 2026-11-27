// RecipeBook — 열린 레시피 장부. 처음부터 열린 레시피와 재료를 처음 손에 넣어 연 레시피를 기억하며, 늘기만 한다 (kitchen-recipes 명세).
// 호출자: KitchenManager(하루를 열 때 저장값으로 만들고, 하루 시작·아침 보고 확정 뒤 RefreshFrom), 저장(Unlocked 목록).
// 의사코드:
//   생성 — 시작 레시피를 연다. 저장된 목록이 있으면 그 안의 아는 id도 연다.
//   RefreshFrom — 아직 잠긴 레시피 중 여는 재료 하나라도 재고가 1 이상이면 연다(💡 하나 규칙). 새로 연 id를 목록 순서로 돌려준다.
//                 재료를 다 써도 닫지 않는다.
using System;
using System.Collections.Generic;
using DOB.Define;

namespace DOB.Systems
{
    public sealed class RecipeBook
    {
        private readonly KitchenRules rules;
        private readonly HashSet<string> unlocked = new HashSet<string>();

        /// <param name="saved">저장된 열린 레시피 id. null이면 시작 레시피만.</param>
        public RecipeBook(KitchenRules rules, IEnumerable<string> saved = null)
        {
            this.rules = rules ? rules : throw new ArgumentNullException(nameof(rules));
            foreach (var recipe in rules.Recipes)
                if (recipe.startUnlocked)
                    unlocked.Add(recipe.id);
            if (saved != null)
                foreach (string id in saved)
                    if (rules.TryGet(id, out _))
                        unlocked.Add(id);
        }

        public bool IsUnlocked(string id) => id != null && unlocked.Contains(id);

        /// <summary>열린 레시피 id(레시피 목록 순서).</summary>
        public IReadOnlyList<string> Unlocked
        {
            get
            {
                var list = new List<string>();
                foreach (var recipe in rules.Recipes)
                    if (unlocked.Contains(recipe.id))
                        list.Add(recipe.id);
                return list;
            }
        }

        /// <summary>재고에 여는 재료가 있는 레시피를 연다. 새로 연 id를 돌려준다.</summary>
        public IReadOnlyList<string> RefreshFrom(MaterialStock materials)
        {
            var opened = new List<string>();
            if (materials == null)
                return opened;
            foreach (var recipe in rules.Recipes)
            {
                if (unlocked.Contains(recipe.id) || recipe.unlockBy == null)
                    continue;
                foreach (string material in recipe.unlockBy)
                    if (materials.Count(material) > 0)
                    {
                        unlocked.Add(recipe.id);
                        opened.Add(recipe.id);
                        break;
                    }
            }
            return opened;
        }
    }
}
