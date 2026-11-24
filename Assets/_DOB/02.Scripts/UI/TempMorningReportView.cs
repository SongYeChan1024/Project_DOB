// 임시 아침 귀환 보고 화면: 카드 하나에 글과 [보고 확인] 버튼. 실제 화면이 나오면 이 컴포넌트 대신 연결한다.
// 손님별 결과는 위험 판정(F7)·정산(F9) 구현 뒤 실제 화면에서 보인다.
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    public sealed class TempMorningReportView : MorningReportView
    {
        /// <summary>보이고 숨길 카드 루트. 평소 비활성.</summary>
        [SerializeField] private GameObject panel;
        [SerializeField] private Text body;
        [SerializeField] private Button confirm;

        private void OnEnable() { if (confirm) confirm.onClick.AddListener(RaiseConfirmed); }
        private void OnDisable() { if (confirm) confirm.onClick.RemoveListener(RaiseConfirmed); }

        public override void Show(MorningReportData data)
        {
            body.text = $"{data.Day}일차 아침\n\n어제 손님들의 귀환 보고는 준비 중입니다.\n보유 금화 {data.Gold}  ·  마늘 {data.GarlicStock}개";
            panel.SetActive(true);
        }

        public override void Hide() { if (panel) panel.SetActive(false); }
    }
}
