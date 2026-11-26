// =====================================================================
// InteractionPromptPresenter — 상호작용할 수 있는 대상 위에 [E] 동작 표시를 띄운다
// 역할   : IInteractionPrompt(백엔드 상호작용)가 알려 주는 대상의 머리 위 위치를 화면 좌표로 바꿔 View에 넘긴다.
//          가까운지 · 상호작용할 수 있는지는 판정하지 않는다 (대상이 null이면 숨김).
// 호출자 : 없음 (매 프레임 스스로 갱신)
// 호출   : UIManager.InteractionPrompt · IsScreenOpen, BasementPauseMenu.IsPaused, InteractionPromptView
// 상태   : 없음
//
// 의사코드
// [매 프레임 끝] 연결된 대상이 없거나 · 다른 화면(준비 · 요리 · 방명록)이나 ESC 메뉴가 열려 있으면 숨긴다
//               → 대상에 InteractionPromptAnchor가 있으면 그 자리, 없으면 대상 위치에서 기본 높이만큼 위를
//                 카메라로 화면 좌표로 바꾼다 → 카메라 뒤면 숨긴다 → 아니면 띄운다
// 새 오브젝트: InteractionPromptAnchor를 붙여 높이(와 필요하면 이름)만 정하면 된다. 이 코드는 고치지 않는다.
// 비고   : 카메라는 씬이 달라 직렬화할 수 없어 Camera.main을 쓴다 (PlayerMovementController와 같은 방식).
//          대상이 손님처럼 움직이므로 LateUpdate에서 매 프레임 읽는다 (UI 연결 계획 2-2: 계속 흐르는 값).
// =====================================================================
using DOB.UI;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>상호작용 대상 위에 [E] 표시를 옮긴다.</summary>
    public sealed class InteractionPromptPresenter : MonoBehaviour
    {
        [SerializeField] private InteractionPromptView view;
        /// <summary>InteractionPromptAnchor가 없는 대상의 기본 높이 (미터). 연출 값.</summary>
        [SerializeField] private float height = 2f;
        /// <summary>키 칸에 적을 글자.</summary>
        [SerializeField] private string key = "E";

        private void LateUpdate()
        {
            if (!view) return;
            var prompt = UIManager.InteractionPrompt;
            var target = prompt?.Target;
            var camera = Camera.main;
            if (!target || !camera || UIManager.IsScreenOpen || BasementPauseMenu.IsPaused)
            {
                view.Hide();
                return;
            }
            // 대상에 InteractionPromptAnchor가 있으면 그 자리 · 이름, 없으면 기본 높이 · 백엔드 이름
            target.TryGetComponent<InteractionPromptAnchor>(out var anchor);
            var point = anchor ? anchor.Point : target.position + Vector3.up * height;
            var screen = camera.WorldToScreenPoint(point);
            if (screen.z <= 0f) { view.Hide(); return; } // 카메라 뒤
            view.Show(screen, key, (anchor ? anchor.ActionOverride : null) ?? prompt.Action);
        }
    }
}
