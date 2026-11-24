// =====================================================================
// RecipeBookView — 레시피 책
// 역할   : 왼쪽 쪽에 레시피 목록(한 쪽에 7개), 오른쪽 쪽에 고른 레시피의 재료 · 효과 · 맛 · 기본가를 보여 준다.
//          아래 모서리를 누르면 목록의 앞 · 뒤 쪽으로 넘긴다.
// 호출자 : PrepScreensView (Show)
// 호출   : RecipeRowView, EffectChipView
// 상태   : 지금 목록 쪽 번호, 고른 레시피 번호.
//
// 의사코드
// [보이기]  첫 쪽, 첫 번째 열린 레시피를 고른 상태로 시작
// [목록]    열린 레시피는 누를 수 있고, 안 열린 레시피는 ??? (누를 수 없음)
// [자세히]  재료(기본 재료만이면 '∞ 무제한'), 효과 칩, 맛, 기본가
// [쪽]      첫 쪽이면 앞 모서리, 마지막 쪽이면 뒤 모서리를 숨긴다
// =====================================================================
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>레시피 책 화면.</summary>
    public sealed class RecipeBookView : MonoBehaviour
    {
        private const int RowsPerPage = 7;

        [SerializeField] private TMP_Text goldText;

        [Header("왼쪽 쪽: 목록"), SerializeField] private RectTransform rowList;
        [SerializeField] private RecipeRowView rowTemplate;
        /// <summary>열린 수 / 전체 (예: 4 / 22).</summary>
        [SerializeField] private TMP_Text countText;
        [SerializeField] private Button prevPageButton;
        [SerializeField] private Button nextPageButton;

        [Header("오른쪽 쪽: 자세히"), SerializeField] private GameObject detailRoot;
        /// <summary>요리 그림 (오른쪽 쪽 위). 그림이 없으면 숨긴다.</summary>
        [SerializeField] private Image detailPicture;
        [SerializeField] private TMP_Text detailName;
        [SerializeField] private TMP_Text detailSub;
        [SerializeField] private TMP_Text ingredientsText;
        [SerializeField] private TMP_Text unlimitedText;
        [SerializeField] private RectTransform effectList;
        [SerializeField] private EffectChipView chipTemplate;
        [SerializeField] private TMP_Text flavorText;

        [SerializeField] private Button closeButton;

        private readonly List<RecipeRowView> rows = new List<RecipeRowView>();
        private readonly List<GameObject> chips = new List<GameObject>();
        private PrepViewData data;
        private int page;
        private int selected;

        public event Action CloseClicked;

        private void Awake()
        {
            rowTemplate.gameObject.SetActive(false);
            chipTemplate.gameObject.SetActive(false);
            prevPageButton.onClick.AddListener(() => ShowPage(page - 1));
            nextPageButton.onClick.AddListener(() => ShowPage(page + 1));
            closeButton.onClick.AddListener(() => CloseClicked?.Invoke());
        }

        public void Show(PrepViewData viewData)
        {
            data = viewData;
            goldText.text = data.gold.ToString();

            int opened = 0;
            foreach (var r in data.recipes) if (r.unlocked) opened++;
            countText.text = $"{opened} / {Mathf.Max(data.recipeTotal, data.recipes.Length)}";

            selected = Array.FindIndex(data.recipes, r => r.unlocked);
            ShowPage(0);
            ShowDetail(selected);
        }

        // 목록의 몇 번째 쪽(0부터)을 보여 준다.
        private void ShowPage(int index)
        {
            int pageCount = Mathf.Max(1, Mathf.CeilToInt(data.recipes.Length / (float)RowsPerPage));
            page = Mathf.Clamp(index, 0, pageCount - 1);
            prevPageButton.gameObject.SetActive(page > 0);
            nextPageButton.gameObject.SetActive(page < pageCount - 1);

            foreach (var row in rows) { row.gameObject.SetActive(false); Destroy(row.gameObject); }
            rows.Clear();

            int start = page * RowsPerPage;
            for (int i = start; i < Mathf.Min(start + RowsPerPage, data.recipes.Length); i++)
            {
                int recipeIndex = i; // 람다가 반복 변수 대신 고정된 번호를 쓰도록 복사
                var row = Instantiate(rowTemplate, rowList);
                row.gameObject.SetActive(true);
                row.Bind(data.recipes[i]);
                row.SetSelected(i == selected);
                row.Clicked += () => ShowDetail(recipeIndex);
                rows.Add(row);
            }
        }

        // 오른쪽 쪽에 레시피 하나를 자세히 보여 준다.
        private void ShowDetail(int index)
        {
            selected = index;
            int start = page * RowsPerPage;
            for (int i = 0; i < rows.Count; i++)
                rows[i].SetSelected(start + i == selected);

            bool valid = index >= 0 && index < data.recipes.Length && data.recipes[index].unlocked;
            detailRoot.SetActive(valid);
            if (!valid) return;

            var recipe = data.recipes[index];
            detailName.text = recipe.name;
            if (detailPicture)
            {
                detailPicture.sprite = recipe.picture;
                detailPicture.gameObject.SetActive(recipe.picture);
            }
            detailSub.text = $"{(recipe.basicOnly ? "기본 레시피" : "레시피")} · 기본가 금화 {recipe.basePrice}개";
            ingredientsText.text = recipe.ingredients;
            unlimitedText.gameObject.SetActive(recipe.basicOnly);
            flavorText.text = recipe.flavor;

            foreach (var chip in chips) { chip.SetActive(false); Destroy(chip); }
            chips.Clear();
            foreach (var effect in recipe.effects)
            {
                var chip = Instantiate(chipTemplate, effectList);
                chip.gameObject.SetActive(true);
                chip.Set(effect.name, effect.value);
                chips.Add(chip.gameObject);
            }
        }
    }
}
