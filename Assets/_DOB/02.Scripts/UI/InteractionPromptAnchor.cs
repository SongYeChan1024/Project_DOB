// =====================================================================
// InteractionPromptAnchor — 상호작용 표시를 띄울 자리 (오브젝트마다 선택으로 붙임)
// 역할   : 상호작용 오브젝트(손님 · 자리 · 주방 · 방명록 · 시장 판매대 …)에 붙여 [E] 표시가 뜰 높이와 이름을 정한다.
//          붙이지 않은 오브젝트는 기본 높이(InteractionPromptPresenter.height)에 뜬다.
// 사용   : 새 상호작용 오브젝트를 만들면 루트에 이 컴포넌트를 붙이고 높이만 맞춘다. 코드 수정 없음.
//          백엔드는 IInteractionPrompt.Target으로 그 오브젝트의 Transform만 넘기면 된다.
// 상태   : 없음 (인스펙터 값)
// =====================================================================
using UnityEngine;

namespace DOB.UI
{
    /// <summary>[E] 표시가 뜰 자리.</summary>
    [DisallowMultipleComponent]
    public sealed class InteractionPromptAnchor : MonoBehaviour
    {
        /// <summary>오브젝트 기준점에서 표시까지의 높이 (미터).</summary>
        [SerializeField] private float height = 2f;
        /// <summary>동작 이름을 이 오브젝트에서 정할 때 (비우면 IInteractionPrompt.Action을 쓴다). 예: 방명록 보기</summary>
        [SerializeField] private string actionOverride;

        /// <summary>표시가 뜰 월드 위치.</summary>
        public Vector3 Point => transform.position + Vector3.up * height;

        /// <summary>이 오브젝트가 정한 동작 이름. 없으면 null.</summary>
        public string ActionOverride => string.IsNullOrEmpty(actionOverride) ? null : actionOverride;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.95f, 0.77f, 0.43f);
            Gizmos.DrawWireSphere(Point, 0.15f);
        }
    }
}
