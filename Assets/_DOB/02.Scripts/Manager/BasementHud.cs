// UI 씬이 소유한 화면·버튼을 한곳에 묶고, 게임 베이스는 로드 완료 뒤 이 참조로 입력을 연결한다.
using DOB.UI;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.Manager
{
    /// <summary>첫 영업 화면에서 월드 서비스와 주방이 사용할 직렬화 참조.</summary>
    [DisallowMultipleComponent]
    public sealed class BasementHud : MonoBehaviour
    {
        [SerializeField] private Text guidance;
        [SerializeField] private FirstGuestDialogue dialogue;
        [SerializeField] private GameObject recipePanel;
        [SerializeField] private Text ingredients;
        [SerializeField] private Text receipt;
        [SerializeField] private Button confirm;
        [SerializeField] private Button cancel;
        [SerializeField] private Button settle;
        /// <summary>하루 진행 화면. 지금은 임시 화면(Temp*View)이며, 실제 화면은 같은 추상 View를 상속해 이 자리에 연결한다.</summary>
        [Header("하루 진행 화면"), SerializeField] private MorningReportView morningReport;
        [SerializeField] private PreparationView preparation;
        [SerializeField] private GuestbookView guestbook;

        public MorningReportView MorningReport => morningReport;
        public PreparationView Preparation => preparation;
        public GuestbookView Guestbook => guestbook;
        /// <summary>하루 진행 화면 세 개가 모두 연결됐는지. GameManager.Prepare가 확인한다.</summary>
        public bool HasDayFlowViews => morningReport && preparation && guestbook;

        public Text Guidance => guidance;
        public FirstGuestDialogue Dialogue => dialogue;
        public GameObject RecipePanel => recipePanel;
        public Text Ingredients => ingredients;
        public Text Receipt => receipt;
        public Button Confirm => confirm;
        public Button Cancel => cancel;
        public Button Settle => settle;

        public bool IsReady => guidance && dialogue && recipePanel && ingredients && receipt && confirm && cancel && settle;
    }
}
