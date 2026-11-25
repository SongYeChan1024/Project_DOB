// =====================================================================
// PrepGuideView — 준비 단계 안내 (PreparationView의 실제 구현, 계약 초안 2-2)
// 역할   : 준비 단계에 들어오면 안내 띠(PrepGuideBannerView)에 "N일차 준비" 문구를 보이고, 끝나면 띠와 열린 준비 화면을 닫는다.
// 호출자 : DayFlowPresenter (Show · Hide). 단계 판단은 하지 않는다.
// 호출   : UIManager.OpenPrepScreen · ClosePrepScreen — 그래서 Presenter 폴더(DOB.Manager)에 둔다 (김언호 합의)
// 영업 시작: 입구 문패 [문 열기](OpenButton) → UIManager.StartBusinessRequested 한 경로만 쓴다. 이 화면에는 시작 버튼이 없다.
//
// 의사코드
// [Show] 일차를 넣은 안내 문구를 띠에 보인다 → (임시) 입구 문패 화면을 바로 연다
// [Hide] 띠를 끄고 열린 준비 화면을 닫는다. 이미 숨었거나 씬 종료 중(띠 파괴됨)이어도 오류 없이 지나간다
// =====================================================================
using DOB.UI;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>준비 단계 진입·종료를 안내 띠와 준비 화면에 옮긴다.</summary>
    public sealed class PrepGuideView : PreparationView
    {
        /// <summary>화면 위 안내 띠.</summary>
        [SerializeField] private PrepGuideBannerView banner;

        public override void Show(PreparationData data)
        {
            // 💡 문구 가안 (계약 초안 2-2)
            if (banner) banner.Show($"{data.Day}일차 준비 — 시장·레시피 책을 둘러보고 문패에서 문을 여세요");
            // TODO(계약) 식당 물건 상호작용(김언호)이 생기면 지운다. 그 전까지는 문패를 바로 열어 영업을 시작할 수 있게 한다
            UIManager.OpenPrepScreen(PrepScreen.DoorSign);
        }

        public override void Hide()
        {
            if (banner) banner.Hide();
            UIManager.ClosePrepScreen();
        }
    }
}
