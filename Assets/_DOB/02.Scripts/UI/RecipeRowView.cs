// =====================================================================
// RecipeRowView — 레시피 책 왼쪽 목록의 한 줄
// 역할   : 열린 레시피는 이름 · 재료 · 기본가를, 안 열린 레시피는 ??? 와 안내를 보여 준다.
//          누르면 알린다 (안 열린 줄은 누를 수 없음).
// 호출자 : RecipeBookView (Bind, SetSelected)
// 상태   : 없음
// =====================================================================
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>레시피 목록 한 줄.</summary>
    public sealed class RecipeRowView : MonoBehaviour
    {
        [SerializeField] private Button button;
        /// <summary>고른 줄 바탕.</summary>
        [SerializeField] private GameObject selectedMark;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text ingredientsText;
        [SerializeField] private TMP_Text priceText;
        /// <summary>요리 그림. 안 열린 레시피는 그림자로 (비워 두면 숨김).</summary>
        [SerializeField] private Image picture;

        private static readonly Color Ink = new Color32(70, 45, 25, 255);
        private static readonly Color Locked = new Color32(190, 170, 140, 255);
        private static readonly Color Shadow = new Color32(120, 95, 70, 90);

        public event Action Clicked;

        private void Awake() => button.onClick.AddListener(() => Clicked?.Invoke());

        public void Bind(RecipeItemData recipe)
        {
            button.interactable = recipe.unlocked;
            if (picture)
            {
                picture.sprite = recipe.picture;
                picture.color = recipe.unlocked ? Color.white : Shadow;
                picture.gameObject.SetActive(recipe.picture);
            }
            if (recipe.unlocked)
            {
                nameText.text = recipe.name;
                nameText.color = Ink;
                ingredientsText.text = recipe.ingredients;
                priceText.text = $"금화 {recipe.basePrice}개";
            }
            else
            {
                nameText.text = "???";
                nameText.color = Locked;
                ingredientsText.text = "재료를 처음 얻으면 열려요";
                priceText.text = string.Empty;
            }
        }

        public void SetSelected(bool selected) => selectedMark.SetActive(selected);
    }
}
