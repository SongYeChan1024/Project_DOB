// =====================================================================
// KitchenData — 주방(화구 하나) 요리 화면에 보여 줄 표시 데이터
// 역할   : 화면이 그릴 값만 담는다. 재고 소비 · 레시피 판정 · 맛 점수 · 조리 시간은 담지 않는다 (백엔드 몫).
// 호출자 : KitchenPresenter가 채워서 KitchenView.Show에 넘긴다.
// 상태   : 없음 (읽기 전용 값 묶음)
// =====================================================================
using System;
using UnityEngine;

namespace DOB.UI
{
    /// <summary>화구 상태.</summary>
    public enum BurnerState
    {
        /// <summary>비어 있음: 재료를 넣고 조리를 시작할 수 있다</summary>
        Empty,
        /// <summary>조리 중: 뚜껑이 닫히고 시계가 돈다</summary>
        Cooking,
        /// <summary>완성: 접시를 들 수 있다</summary>
        Done,
    }

    /// <summary>주문 하나의 진행 상태 (주문서 꼬리표에 표시).</summary>
    public enum OrderStatus { Waiting, Cooking, Done, Carrying, Served }

    /// <summary>요리 화면 전체 표시 값.</summary>
    [Serializable]
    public sealed class KitchenViewData
    {
        /// <summary>화구 이름 (예: 화구 1).</summary>
        public string burnerName;
        public BurnerState state;
        /// <summary>조리 전체 시간 · 남은 시간 (초).</summary>
        public float cookSeconds = 6f;
        public float remainingSeconds;

        public OrderTicketData[] orders = Array.Empty<OrderTicketData>();
        /// <summary>지금 만드는 주문. 없으면 비움.</summary>
        public string selectedOrderId;

        public PantryItemData[] ingredients = Array.Empty<PantryItemData>();
        public PantryItemData[] spices = Array.Empty<PantryItemData>();

        /// <summary>냄비에 넣은 재료 (접시 칸 순서). 빈 칸은 null.</summary>
        public PantryItemData[] potIngredients = Array.Empty<PantryItemData>();
        /// <summary>냄비에 넣은 양념 (양념 칸). 없으면 null.</summary>
        public PantryItemData potSpice;

        public RecipePreviewData preview = new RecipePreviewData();
        /// <summary>고른 주문 손님에게서 찾아 둔 위험 (카드에서 요리 효과와 나란히 비교). 모르면 비움.</summary>
        public GuestRiskData[] risks = Array.Empty<GuestRiskData>();
    }

    /// <summary>손님 위험 하나 (예: 열 2칸).</summary>
    [Serializable]
    public sealed class GuestRiskData
    {
        /// <summary>독 · 열 · 냉기 · 어둠 · 체력 · 함정 중 하나.</summary>
        public string risk;
        public int amount;
    }

    /// <summary>주문서 꼬리표 하나.</summary>
    [Serializable]
    public sealed class OrderTicketData
    {
        public string id;
        public string guestName;
        /// <summary>예: 1번 자리</summary>
        public string seatLabel;
        public OrderStatus status;
    }

    /// <summary>재료 · 양념 상자 하나.</summary>
    [Serializable]
    public sealed class PantryItemData
    {
        public string id;
        public string name;
        /// <summary>마우스를 올리면 이름 아래에 보일 설명.</summary>
        public string description;
        public Sprite icon;
        public int count;
        /// <summary>기본 재료 (∞ 표시, 개수 안 줄어듦).</summary>
        public bool infinite;
    }

    /// <summary>오른쪽 결과 카드: 지금 냄비 재료로 만들어질 요리.</summary>
    [Serializable]
    public sealed class RecipePreviewData
    {
        /// <summary>맞는 레시피를 찾았는지. false면 카드에 ??? 와 안내 문구.</summary>
        public bool found;
        public string title;
        public Sprite picture;
        /// <summary>재료 진행 (예: 밀가루 1 / 2 · 하나 더). 찾지 못했으면 안내 문구.</summary>
        public string needLine;
        public EffectAmount[] effects = Array.Empty<EffectAmount>();
        /// <summary>맛 점수 0~4. 모르면 -1 (별을 모두 흐리게).</summary>
        public int taste = -1;
        public string flavor;
        /// <summary>예: 금화 9개 예상</summary>
        public string priceText;
        /// <summary>조리를 시작할 수 있는지 (재료가 다 맞음).</summary>
        public bool canCook;
    }
}
