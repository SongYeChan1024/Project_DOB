// UI 씬이 소유한 화면·버튼을 한곳에 묶고, 게임 베이스는 로드 완료 뒤 이 참조로 입력을 연결한다.
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
