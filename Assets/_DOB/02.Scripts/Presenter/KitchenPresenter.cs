// =====================================================================
// KitchenPresenter — 주방 화구 요리 화면(KitchenView)과 게임 쪽을 잇는다
// 역할   : UIManager에 자신을 등록해 두고, "화구 N 열어 줘"를 받으면 그 화구의 요리 화면을 연다.
//          재료 넣기 · 빼기 · 조리 시작 · 접시 들기 입력을 요청으로 옮기고 결과를 다시 그린다.
// 호출자 : UIManager (Open, Close, IsOpen) ← 식당의 화구 상호작용(E) 쪽에서 UIManager.OpenKitchen(번호)
// 호출   : KitchenView, UIManager (등록 · 해제)
// 상태   : 더미 단계에서는 화구별 냄비 내용 · 남은 조리 시간, 주문서, 재료 · 양념 개수를 임시로 가진다.
// 비고   : TODO(계약) 표시된 곳이 백엔드 호출로 바뀔 자리 (Docs/개발/FeatureManagerMap.md 💡 구성안 기준).
//          주문 · 화구 · 조리 · 접시 → KitchenManager (OrderBook · CookingStation · PlateRegistry · TasteScore)
//          재료 · 양념 재고 → EconomyManager (Inventory) / 손님 위험 → GuestManager · RelationshipManager
//          레시피 · 재료 표는 PrepScreenPresenter의 더미 표를 같이 쓴다 (계약 후 08.Data로).
//          🔴 미정: 레시피마다 조리 도구(왼쪽 · 오른쪽 도구)가 다른지. 지금은 모든 요리를 냄비 하나로 만든다.
//
// 의사코드
// [켜질 때]   화면 이벤트를 구독하고 UIManager에 등록한다
// [열기]      화구 번호를 기억 → 표시 데이터로 화면을 연다 (주문서는 플레이어가 위 줄에서 직접 고른다)
// [주문 고르기] 대기 중인 주문서를 누르면 이 화구의 주문이 된다 (다른 빈 화구가 골라 둔 것이면 넘겨받음)
//             안 고르고 조리하면 다른 화구가 안 잡은 대기 주문 중 먼저 온 손님 것으로 만든다
// [재료]      비어 있는 화구면 빈 접시 칸에 넣고 개수를 하나 줄인다 (칸이 꽉 차면 무시)
// [빼기]      냄비 접시를 누르면 그 재료를 빼고 개수를 돌려준다
// [조리]      재료가 레시피와 딱 맞고 주문이 골라져 있으면 뚜껑을 닫고 6초를 센다 (화면을 닫아도 계속 흐름)
// [완성]      시간이 다 되면 완성 → [접시 들기]로 주문 손님 접시를 든다 → 화구를 비운다
// [Esc]       열려 있으면 닫는다
// =====================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using DOB.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DOB.Manager
{
    /// <summary>화구 요리 화면의 입력을 요청으로 옮기고 결과를 화면에 표시한다.</summary>
    public sealed class KitchenPresenter : MonoBehaviour
    {
        private const int PotSlotCount = 3; // 레시피 재료가 가장 많은 것이 3칸

        [SerializeField] private KitchenView view;
        [SerializeField, Min(1)] private int burnerCount = 2;
        [SerializeField, Min(0.1f)] private float cookSeconds = 6f;

        /// <summary>양념 그림 (마늘 · 박하 · 생강 · 당근즙 · 소금 · 후추 순서).</summary>
        [Header("그림"), SerializeField] private Sprite[] spiceIcons = new Sprite[6];
        /// <summary>재료 · 요리 그림 (07.UI/Sprites/Items). 파일 이름으로 찾는다.</summary>
        [SerializeField] private Sprite[] itemIcons = Array.Empty<Sprite>();

        /// <summary>완성된 접시를 들었다 (주문 id). TODO(계약) KitchenManager.PlateRegistry가 받는다.</summary>
        public event Action<string> PlateTaken;

        // ---------------- 더미 상태 (계약 전 테스트용) ----------------
        private sealed class Burner
        {
            public readonly string[] pot = new string[PotSlotCount];
            public string spice;
            public BurnerState state;
            public float remaining;
            public string orderId;
        }

        private sealed class Order
        {
            public string id, guest, seat;
            public OrderStatus status;
            public GuestRiskData[] risks;
        }

        // 기본 재료는 무제한 (그림 이름 → 한글 이름)
        private static readonly Dictionary<string, string> BasicIngredients = new Dictionary<string, string>
            { ["Flour"] = "밀가루", ["Potato"] = "감자", ["Meat"] = "고기" };

        private Burner[] burners = Array.Empty<Burner>();
        private readonly List<Order> orders = new List<Order>();
        private readonly Dictionary<string, int> ingredientStock = new Dictionary<string, int>
            { ["SlimeJelly"] = 2, ["Herb"] = 1, ["ForestMushroom"] = 2, ["WaterMoss"] = 1, ["BatWing"] = 3 };
        private readonly Dictionary<string, int> spiceStock = new Dictionary<string, int>
            { ["garlic"] = 1, ["salt"] = 2 };
        private int current = -1;

        /// <summary>요리 화면이 열려 있는지.</summary>
        public bool IsOpen => view && view.IsOpen;

        private void Awake()
        {
            burners = new Burner[burnerCount];
            for (int i = 0; i < burners.Length; i++) burners[i] = new Burner();
            // TODO(계약) KitchenManager.OrderBook에서 받기
            orders.Add(NewOrder("order_haru", "하루", "1번 자리", ("열", 2)));
            orders.Add(NewOrder("order_bori", "보리", "2번 자리", ("독", 1), ("어둠", 1)));
            orders.Add(NewOrder("order_sol", "솔", "3번 자리"));
        }

        private static Order NewOrder(string id, string guest, string seat, params (string risk, int amount)[] risks)
        {
            return new Order
            {
                id = id, guest = guest, seat = seat, status = OrderStatus.Waiting,
                risks = risks.Select(r => new GuestRiskData { risk = r.risk, amount = r.amount }).ToArray(),
            };
        }

        private void OnEnable()
        {
            if (view)
            {
                view.OrderClicked += OnOrder;
                view.IngredientClicked += OnIngredient;
                view.SpiceClicked += OnSpice;
                view.PotSlotClicked += OnPotSlot;
                view.SpiceSlotClicked += OnSpiceSlot;
                view.CookClicked += OnCook;
                view.CloseClicked += Close;
            }
            UIManager.RegisterKitchen(this);
        }

        private void OnDisable()
        {
            if (view)
            {
                view.OrderClicked -= OnOrder;
                view.IngredientClicked -= OnIngredient;
                view.SpiceClicked -= OnSpice;
                view.PotSlotClicked -= OnPotSlot;
                view.SpiceSlotClicked -= OnSpiceSlot;
                view.CookClicked -= OnCook;
                view.CloseClicked -= Close;
            }
            UIManager.UnregisterKitchen(this);
        }

        // =================================================================
        // 열기 · 닫기
        // =================================================================

        /// <summary>화구 하나(0부터)의 요리 화면을 연다. 번호가 없으면 경고하고 false.</summary>
        public bool Open(int burner)
        {
            if (!view || burner < 0 || burner >= burners.Length)
            {
                Debug.LogWarning("[DOB] 요리 화면을 열 수 없습니다: 화구 " + burner);
                return false;
            }
            current = burner;
            view.Open(BuildData());
            return true;
        }

        public void Close()
        {
            if (view) view.Close();
        }

        private void Update()
        {
            // 조리는 화면을 닫거나 대화 중이어도 계속 흐른다 (기획). 일시 정지(timeScale 0)에는 멈춘다.
            for (int i = 0; i < burners.Length; i++)
            {
                var b = burners[i];
                if (b.state != BurnerState.Cooking) continue;
                b.remaining = Mathf.Max(0f, b.remaining - Time.deltaTime);
                if (b.remaining > 0f)
                {
                    if (i == current && IsOpen) view.SetTimer(b.remaining, cookSeconds, b.state);
                    continue;
                }
                b.state = BurnerState.Done;
                SetOrderStatus(b.orderId, OrderStatus.Done);
                if (i == current && IsOpen) view.Show(BuildData());
            }

            // shortcut: Esc가 일시 정지 메뉴와 겹칠 수 있음, 입력 담당 정해지면 그쪽으로 옮긴다
            if (IsOpen && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                Close();
        }

        // =================================================================
        // 입력
        // =================================================================

        private Burner Editable => current >= 0 && burners[current].state == BurnerState.Empty ? burners[current] : null;

        private void OnOrder(string id)
        {
            var b = Editable;
            var order = orders.FirstOrDefault(o => o.id == id);
            if (b == null || order == null || order.status != OrderStatus.Waiting) return;
            // 아직 조리 전인 다른 화구가 골라 둔 주문이면 그쪽 선택을 풀고 가져온다
            foreach (var other in burners)
                if (other != b && other.orderId == id) other.orderId = null;
            b.orderId = id;
            Refresh();
        }

        private void OnIngredient(string id)
        {
            var b = Editable;
            if (b == null) return;
            int slot = Array.IndexOf(b.pot, null);
            if (slot < 0 || !Take(ingredientStock, id, BasicIngredients.ContainsKey(id))) return; // 칸이 꽉 찼거나 재고 없음
            b.pot[slot] = id;
            Refresh();
        }

        private void OnPotSlot(int slot)
        {
            var b = Editable;
            if (b == null || slot < 0 || slot >= b.pot.Length || b.pot[slot] == null) return;
            Give(ingredientStock, b.pot[slot], BasicIngredients.ContainsKey(b.pot[slot]));
            b.pot[slot] = null;
            Refresh();
        }

        private void OnSpice(string id)
        {
            var b = Editable;
            if (b == null || !Take(spiceStock, id, false)) return;
            if (b.spice != null) Give(spiceStock, b.spice, false); // 양념 칸은 하나라 바꿔 넣는다
            b.spice = id;
            Refresh();
        }

        private void OnSpiceSlot()
        {
            var b = Editable;
            if (b == null || b.spice == null) return;
            Give(spiceStock, b.spice, false);
            b.spice = null;
            Refresh();
        }

        private void OnCook()
        {
            if (current < 0) return;
            var b = burners[current];
            if (b.state == BurnerState.Done)
            {
                // TODO(계약) KitchenManager.PlateRegistry: 주문 손님 접시를 든다 (그 손님 자리에만 놓을 수 있음)
                SetOrderStatus(b.orderId, OrderStatus.Carrying);
                PlateTaken?.Invoke(b.orderId);
                // shortcut: 서빙이 아직 없어 든 접시를 바로 다 먹은 것으로 치고 새 손님 주문을 하나 붙인다. 손님 흐름이 붙으면 지운다
                SetOrderStatus(b.orderId, OrderStatus.Served);
                AddDummyOrder();
                burners[current] = new Burner();
                Close();
                return;
            }
            if (b.state != BurnerState.Empty || !Preview(b).canCook) return;
            b.orderId = OrderFor(b).id; // 주문서를 안 골랐으면 먼저 온 대기 손님 것으로
            // TODO(계약) KitchenManager.CookingStation.Start(orderId, 재료, 양념) → 결과로 바꾸기
            b.state = BurnerState.Cooking;
            b.remaining = cookSeconds;
            SetOrderStatus(b.orderId, OrderStatus.Cooking);
            Refresh();
        }

        private void Refresh() => view.Show(BuildData());

        // =================================================================
        // 표시 데이터
        // =================================================================

        private KitchenViewData BuildData()
        {
            var b = burners[current];
            var order = OrderFor(b);
            return new KitchenViewData
            {
                burnerName = "화구 " + (current + 1),
                state = b.state,
                cookSeconds = cookSeconds,
                remainingSeconds = b.remaining,
                orders = orders.Where(o => o.status != OrderStatus.Served).Select(o => new OrderTicketData
                    { id = o.id, guestName = o.guest, seatLabel = o.seat, status = o.status }).ToArray(),
                selectedOrderId = order?.id, // 안 골랐으면 이번에 만들 손님(먼저 온 대기 주문)이 빛난다
                ingredients = BasicIngredients.Keys.Select(id => Item(id, 0, true))
                    .Concat(ingredientStock.Select(kv => Item(kv.Key, kv.Value, false))).ToArray(),
                spices = PrepScreenPresenter.SpiceTable.Select(SpiceItem).ToArray(),
                potIngredients = b.pot.Select(id => id == null ? null : Item(id, 0, BasicIngredients.ContainsKey(id))).ToArray(),
                potSpice = b.spice == null ? null : SpiceItem(PrepScreenPresenter.SpiceTable.First(t => t.id == b.spice)),
                preview = Preview(b),
                risks = order?.risks ?? Array.Empty<GuestRiskData>(),
            };
        }

        private PantryItemData Item(string id, int count, bool infinite)
        {
            return new PantryItemData
            {
                id = id, name = Name(id), count = count, infinite = infinite, icon = Icon("Ing" + id),
                description = infinite ? "기본 재료 · 얼마든지 쓸 수 있어요" : PrepScreenPresenter.IngredientInfo(id),
            };
        }

        private PantryItemData SpiceItem((string id, string name, string effect, string flavor, int price) t)
        {
            int index = Array.FindIndex(PrepScreenPresenter.SpiceTable, s => s.id == t.id);
            return new PantryItemData
            {
                id = t.id, name = t.name, description = $"{t.effect} +1 · {t.flavor}",
                count = spiceStock.TryGetValue(t.id, out int n) ? n : 0,
                icon = index < spiceIcons.Length ? spiceIcons[index] : null,
            };
        }

        // 결과 카드: 지금 냄비 재료로 무엇이 되는지
        private RecipePreviewData Preview(Burner b)
        {
            var have = Count(b.pot.Where(id => id != null).Select(Name));
            if (have.Count == 0)
                return new RecipePreviewData { title = "???", needLine = "재료를 눌러 냄비에 넣어요" };

            var match = FindRecipe(have, out int missing);
            if (match == null)
                return new RecipePreviewData { title = "???", needLine = "맞는 레시피가 없어요 · 재료를 빼 보세요" };

            var need = Count(match.ingredients);
            string progress = string.Join(" · ", need.Select(kv => $"{kv.Key} {(have.TryGetValue(kv.Key, out int h) ? h : 0)} / {kv.Value}"));
            var order = OrderFor(b);
            if (missing > 0) progress += $" · {missing}개 더";
            else if (order == null) progress += " · 기다리는 주문이 없어요";

            // 양념은 그 효과 +1 · 맛 한 칸 (기획 11장)
            var spice = PrepScreenPresenter.SpiceTable.FirstOrDefault(t => t.id == b.spice);
            var effects = match.effects.Select(e => new EffectAmount { name = e.name, value = e.value }).ToList();
            if (b.spice != null)
            {
                var same = effects.FirstOrDefault(e => e.name == spice.effect);
                if (same != null) same.value++;
                else effects.Add(new EffectAmount { name = spice.effect, value = 1 });
            }

            // TODO(계약) KitchenManager.TasteScore: 맛 점수(0~4, 기본 2)와 가격은 백엔드가 정한다. 지금은 양념 하나당 +1
            int spices = b.spice != null ? 1 : 0;
            int taste = Mathf.Clamp(2 + spices, 0, 4);
            int price = Mathf.RoundToInt(match.basePrice * (0.6f + 0.2f * taste)) + spices;
            return new RecipePreviewData
            {
                found = true,
                title = order != null ? $"{match.name} · {order.guest}" : match.name,
                picture = Icon("Dish" + match.id),
                needLine = progress,
                effects = effects.ToArray(),
                taste = taste,
                flavor = b.spice != null ? $"{match.flavor} · {spice.flavor}" : match.flavor,
                priceText = $"금화 {price}개 예상",
                canCook = missing == 0 && order != null,
            };
        }

        // 냄비 재료를 모두 품는 레시피 중 모자란 재료가 가장 적은 것 (딱 맞으면 missing 0). 없으면 null.
        // TODO(계약) 아직 안 배운 레시피도 맞춰 준다. 레시피 해금이 정해지면 unlocked만 보기
        internal static RecipeItemData FindRecipe(Dictionary<string, int> have, out int missing)
        {
            RecipeItemData best = null;
            missing = int.MaxValue;
            foreach (var recipe in PrepScreenPresenter.Recipes)
            {
                var need = Count(recipe.ingredients);
                if (have.Any(kv => !need.TryGetValue(kv.Key, out int n) || kv.Value > n)) continue;
                int lack = need.Sum(kv => kv.Value - (have.TryGetValue(kv.Key, out int h) ? h : 0));
                if (lack < missing) { best = recipe; missing = lack; }
            }
            if (best == null) missing = 0;
            return best;
        }

        // "밀가루 × 2 · 감자 × 1" → {밀가루:2, 감자:1}
        private static Dictionary<string, int> Count(string ingredients)
        {
            var result = new Dictionary<string, int>();
            foreach (var part in ingredients.Split('·'))
            {
                var pair = part.Split('×');
                if (pair.Length == 2 && int.TryParse(pair[1].Trim(), out int n)) result[pair[0].Trim()] = n;
            }
            return result;
        }

        private static Dictionary<string, int> Count(IEnumerable<string> names)
        {
            var result = new Dictionary<string, int>();
            foreach (var name in names) result[name] = (result.TryGetValue(name, out int n) ? n : 0) + 1;
            return result;
        }

        // =================================================================
        // 도우미
        // =================================================================

        private static string Name(string id) =>
            BasicIngredients.TryGetValue(id, out var basic) ? basic : PrepScreenPresenter.IngredientName(id);


        // 이 화구가 만들 주문: 고른 주문서, 없으면 다른 화구가 안 잡은 대기 주문 중 먼저 온 것
        private Order OrderFor(Burner b)
        {
            if (b.orderId != null) return orders.FirstOrDefault(o => o.id == b.orderId);
            return orders.FirstOrDefault(o => o.status == OrderStatus.Waiting && burners.All(x => x.orderId != o.id));
        }

        // 테스트용 손님 이름을 돌려 쓰며 새 주문을 붙인다
        private static readonly string[] DummyGuests = { "하루", "보리", "솔", "도담", "미루", "가온" };
        private int dummyCount = 3;

        private void AddDummyOrder()
        {
            string guest = DummyGuests[dummyCount % DummyGuests.Length];
            dummyCount++;
            orders.Add(NewOrder("order_" + dummyCount, guest, $"{(dummyCount - 1) % 3 + 1}번 자리"));
        }

        private void SetOrderStatus(string id, OrderStatus status)
        {
            var order = orders.FirstOrDefault(o => o.id == id);
            if (order != null) order.status = status;
        }

        // 재고에서 하나 꺼낸다 (기본 재료는 무제한). TODO(계약) EconomyManager.Inventory 예약 · 소비
        private static bool Take(Dictionary<string, int> stock, string id, bool infinite)
        {
            if (infinite) return true;
            if (!stock.TryGetValue(id, out int n) || n <= 0) return false;
            stock[id] = n - 1;
            return true;
        }

        private static void Give(Dictionary<string, int> stock, string id, bool infinite)
        {
            if (!infinite) stock[id] = (stock.TryGetValue(id, out int n) ? n : 0) + 1;
        }

        private Sprite Icon(string key)
        {
            string file = "T_UI_" + key;
            return itemIcons.FirstOrDefault(sp => sp && sp.name == file);
        }
    }
}
