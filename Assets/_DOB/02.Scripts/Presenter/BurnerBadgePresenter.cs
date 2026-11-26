// =====================================================================
// BurnerBadgePresenter — 조리 중 · 완성된 화구 위에 요리 그림과 남은 초를 띄운다 (시안 09)
// 역할   : IKitchenBurners(백엔드 주방)가 알려 주는 화구마다 표시 하나를 맞추고, 화구 위치를 화면에 옮겨 그린다.
//          조리 시간은 세지 않고 남은 시간만 읽는다.
// 호출자 : 없음 (매 프레임 스스로 갱신)
// 호출   : UIManager.KitchenBurners · IsScreenOpen, BurnerBadgeView
// 상태   : 화구 수만큼 복제한 표시 목록
//
// 의사코드
// [매 프레임 끝] 연결이 없거나 다른 화면(준비 · 요리 · 방명록)이 열려 있으면 모두 숨긴다
//               → 화구 수만큼 표시를 견본에서 복제해 맞춘다
//               → 화구마다: 비어 있거나 위치가 없으면 숨김 / 화구 위 높이만큼 위를 화면 좌표로 → 카메라 뒤면 숨김
//                 → 요리 그림(T_UI_Dish{id}), 남은 비율, 남은 초(올림) 또는 완성을 넘긴다
// 비고   : 카메라는 씬이 달라 직렬화할 수 없어 Camera.main을 쓴다 (InteractionPromptPresenter와 같음).
//          화구에 InteractionPromptAnchor가 있어도 이 표시는 화구 높이(height)를 따로 쓴다 (상호작용 [E] 표시와 겹치지 않게).
// =====================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using DOB.UI;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>화구 위 조리 표시들을 맞춘다.</summary>
    public sealed class BurnerBadgePresenter : MonoBehaviour
    {
        /// <summary>표시 견본. 평소 비활성.</summary>
        [SerializeField] private BurnerBadgeView template;
        /// <summary>요리 그림 (07.UI/Sprites/Items의 T_UI_Dish○○). 이름으로 찾는다.</summary>
        [SerializeField] private Sprite[] dishIcons = Array.Empty<Sprite>();
        /// <summary>화구 기준점에서 표시까지의 높이 (미터). 연출 값.</summary>
        [SerializeField] private float height = 0.8f;

        private readonly List<BurnerBadgeView> badges = new List<BurnerBadgeView>();
        private RectTransform area;

        private void Awake()
        {
            area = (RectTransform)transform;
            if (template) template.gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            var burners = UIManager.KitchenBurners;
            var camera = Camera.main;
            if (burners == null || !camera || !template || UIManager.IsScreenOpen)
            {
                HideFrom(0);
                return;
            }

            int count = Mathf.Max(0, burners.Count);
            while (badges.Count < count)
                badges.Add(Instantiate(template, template.transform.parent));
            HideFrom(count);

            for (int i = 0; i < count; i++)
            {
                var badge = badges[i];
                var burner = burners.BurnerAt(i);
                var state = burners.StateAt(i);
                if (!burner || state == BurnerState.Empty) { badge.Hide(); continue; }

                var screen = camera.WorldToScreenPoint(burner.position + Vector3.up * height);
                if (screen.z <= 0f) { badge.Hide(); continue; } // 카메라 뒤
                RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screen, null, out var local);

                float remaining = Mathf.Max(0f, burners.RemainingSecondsAt(i));
                float total = burners.CookSecondsAt(i);
                badge.Show(local, Icon(burners.DishIdAt(i)), state == BurnerState.Done,
                    total > 0f ? remaining / total : 0f, Mathf.CeilToInt(remaining) + "초");
            }
        }

        private void HideFrom(int start)
        {
            for (int i = start; i < badges.Count; i++)
                if (badges[i]) badges[i].Hide();
        }

        private Sprite Icon(string dishId) =>
            string.IsNullOrEmpty(dishId) ? null : dishIcons.FirstOrDefault(sp => sp && sp.name == "T_UI_Dish" + dishId);
    }
}
