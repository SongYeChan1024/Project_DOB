// =====================================================================
// RiskSlotView — 수첩 '위험' 쪽의 줄 하나
// 역할   : 처음엔 빈 줄. 단서를 찾으면 위험 아이콘 · 이름이 적히고,
//          같은 위험의 단서를 찾을 때마다 오른쪽 작은 칸(최대 5칸)이 하나씩 찬다.
//          어떤 단서였는지는 단서 사전 쪽에서 본다.
// 호출자 : GuestDialogueView (Show, Clear)
// 상태   : 없음 (보여 주기만 한다)
// =====================================================================
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>위험 줄 하나. 빈 줄 또는 찾은 위험 + 칸 게이지를 보여 준다.</summary>
    public sealed class RiskSlotView : MonoBehaviour
    {
        [Header("빈 줄일 때"), SerializeField] private GameObject emptyGroup;

        [Header("채워졌을 때"), SerializeField] private GameObject filledGroup;
        /// <summary>줄 테두리. 위험 색으로 칠한다.</summary>
        [SerializeField] private Image outline;
        [SerializeField] private Image icon;
        /// <summary>위험 이름 (예: 독).</summary>
        [SerializeField] private TMP_Text nameText;

        /// <summary>작은 칸들 (왼쪽부터). 찾은 단서 수만큼 위험 색으로 칠한다.</summary>
        [Header("칸 게이지"), SerializeField] private Image[] pips;
        /// <summary>아직 안 찬 칸의 색.</summary>
        [SerializeField] private Color emptyPipColor = new Color32(215, 195, 160, 255);

        /// <summary>빈 줄로 만든다.</summary>
        public void Clear()
        {
            emptyGroup.SetActive(true);
            filledGroup.SetActive(false);
        }

        /// <summary>찾은 위험을 적고, 단서 수(count)만큼 칸을 채운다.</summary>
        public void Show(string riskName, Sprite iconSprite, Color color, int count)
        {
            emptyGroup.SetActive(false);
            filledGroup.SetActive(true);

            outline.color = color;
            icon.sprite = iconSprite;
            icon.enabled = iconSprite != null;
            nameText.text = riskName;

            for (int i = 0; i < pips.Length; i++)
                pips[i].color = i < count ? color : emptyPipColor;
        }
    }
}
