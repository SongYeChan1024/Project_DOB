// 임시 준비 화면: 카드 하나에 글과 [영업 시작] 버튼. 실제 준비 화면(PF_PrepScreens)이 배치되면 이 컴포넌트를 뺀다.
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    public sealed class TempPreparationView : PreparationView
    {
        /// <summary>보이고 숨길 카드 루트. 평소 비활성.</summary>
        [SerializeField] private GameObject panel;
        [SerializeField] private Text body;
        [SerializeField] private Button start;

        private void OnEnable() { if (start) start.onClick.AddListener(RaiseStartRequested); }
        private void OnDisable() { if (start) start.onClick.RemoveListener(RaiseStartRequested); }

        public override void Show(PreparationData data)
        {
            body.text = $"{data.Day}일차 영업 준비\n\n보유 금화 {data.Gold}  ·  마늘 {data.GarlicStock}개";
            panel.SetActive(true);
        }

        public override void Hide() { if (panel) panel.SetActive(false); }
    }
}
