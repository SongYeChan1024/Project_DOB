// PrepGuideBannerView — 준비 단계 안내 띠의 겉. 받은 문구만 보이고 계산하지 않는다.
// 호출자 : PrepGuideView (DOB.Manager)
using TMPro;
using UnityEngine;

namespace DOB.UI
{
    /// <summary>화면 위 안내 띠.</summary>
    public sealed class PrepGuideBannerView : MonoBehaviour
    {
        /// <summary>보이고 숨길 띠 루트. 평소 비활성.</summary>
        [SerializeField] private GameObject banner;
        /// <summary>안내 문구.</summary>
        [SerializeField] private TMP_Text label;

        public void Show(string text)
        {
            if (label) label.text = text;
            if (banner) banner.SetActive(true);
        }

        public void Hide()
        {
            if (banner) banner.SetActive(false);
        }
    }
}
