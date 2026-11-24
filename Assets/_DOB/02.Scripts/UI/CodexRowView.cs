// =====================================================================
// CodexRowView — 도감 한 줄  예) 인간  [젖은 장화](냉기)
// 역할   : 이름 하나와 그 옆에 쌓이는 단서 태그 자리를 가진다.
// 호출자 : GuestDialogueView (종족 · 직업 · 층별 위험 쪽을 그릴 때)
// 상태   : 없음
// =====================================================================
using TMPro;
using UnityEngine;

namespace DOB.UI
{
    /// <summary>도감 한 줄. 이름 + 태그 목록.</summary>
    public sealed class CodexRowView : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        /// <summary>단서 태그를 넣을 자리 (세로로 쌓인다).</summary>
        [SerializeField] private RectTransform tagList;

        public RectTransform TagList => tagList;

        public void SetName(string rowName) => nameText.text = rowName;
    }
}
