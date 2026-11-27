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
//          실제 게임 연결 (홍준우 2026-11-25): 주방 쪽(FirstGuestKitchen)이 UIManager.ConnectKitchenOrders로 IKitchenOrders를 붙이면
//            주문 · 조리 상태 · 마늘 재고 · 효과 · 가격을 그쪽에서 읽고, 조리 시작 · 접시 들기를 그쪽에 요청한다 (판정은 다시 하지 않음).
//            지금 실제 레시피는 구운 빵 하나라 재료 상자에는 밀가루, 양념 상자에는 마늘만 보인다.
//            연결이 없으면(단독 테스트 씬) 아래 더미로 그린다.
//
// 의사코드
// [켜질 때]   화면 이벤트를 구독하고 UIManager에 등록한다
// [열기]      화구 번호를 기억 → 표시 데이터로 화면을 연다 (주문서는 플레이어가 위 줄에서 직접 고른다)
// [주문 고르기] 대기 중인 주문서를 누르면 이 화구의 주문이 된다 (다른 빈 화구가 골라 둔 것이면 넘겨받음)
//             안 고르고 조리하면 다른 화구가 안 잡은 대기 주문 중 먼저 온 손님 것으로 만든다
// [재료]      비어 있는 화구면 빈 접시 칸에 넣고 개수를 하나 줄인다 (칸이 꽉 차면 무시)
// [빼기]      냄비 접시를 누르면 그 재료를 빼고 개수를 돌려준다
// [조리]      재료가 레시피와 딱 맞고 주문이 골라져 있으면 조리를 시작하고 바로 창을 닫는다 (시간은 화면을 닫아도 계속 흐르고 화구 배지가 보여 줌)
// [완성]      시간이 다 되면 완성 → [접시 들기]로 주문 손님 접시를 든다 → 화구를 비운다
// [Esc]       열려 있으면 닫는다
// [실제 연결] 매 프레임 주문 상태를 읽어 화구 상태(비어 있음 · 조리 중 · 완성)를 맞추고, 바뀌면 다시 그린다
// [실제 요청] 조리 시작 · 접시 들기에 orderId를 넣어 보낸다 → 시작되면 바로 닫고, 거절되면 사유에 맞는 안내를 결과 카드에 보이고, 재료를 바꾸면 지운다
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

        // 실제 주문 (없으면 더미). 마지막으로 그린 주문 상태와 비교해 바뀌면 다시 그린다
        private static IKitchenOrders Real => UIManager.KitchenOrders;
        private Systems.FirstGuestOrderState? lastRealState;
        // 마지막 요청이 거절된 사유의 안내. 결과 카드의 필요 재료 줄 대신 보인다
        private string rejectLine;

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
            if (Real != null) SyncReal();
            // 조리는 화면을 닫거나 대화 중이어도 계속 흐른다 (기획). 일시 정지(timeScale 0)에는 멈춘다.
            for (int i = 0; i < burners.Length && Real == null; i++)
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

            // Esc 순서: ESC 메뉴가 열려 있으면 메뉴가 받고, 이 화면이 열려 있으면 이 화면만 닫는다 (BasementPauseInput은 UIManager.IsScreenOpen이면 메뉴를 열지 않음)
            if (IsOpen && !BasementPauseMenu.IsPaused && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
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
            if (Real != null || b == null || order == null || order.status != OrderStatus.Waiting) return; // 실제 게임은 주문이 하나
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
            if (b != null && Real != null)
            {
                // 실제 게임: 마늘 재고가 있을 때만 (양념 칸 하나)
                if (id != Garlic || b.spice == Garlic || Real.GarlicStock <= 0) return;
                b.spice = Garlic;
                Refresh();
                return;
            }
            if (b == null || !Take(spiceStock, id, false)) return;
            if (b.spice != null) Give(spiceStock, b.spice, false); // 양념 칸은 하나라 바꿔 넣는다
            b.spice = id;
            Refresh();
        }

        private void OnSpiceSlot()
        {
            var b = Editable;
            if (b == null || b.spice == null) return;
            if (Real == null) Give(spiceStock, b.spice, false); // 실제 게임은 조리를 확정할 때 재고가 줄어 돌려줄 것이 없다
            b.spice = null;
            Refresh();
        }

        private void OnCook()
        {
            if (current < 0) return;
            var b = burners[current];
            if (Real != null)
            {
                // 실제 게임: 조리 시작 · 접시 들기를 주방 쪽에 요청한다. 판정은 주방 쪽, 화면은 결과에 맞는 안내만 고른다
                string id = Real.OrderId;
                if (b.state == BurnerState.Done)
                {
                    var picked = Real.TryPickUp(id);
                    if (picked == PickUpResult.PickedUp)
                    {
                        burners[current] = new Burner();
                        Close();
                    }
                    else rejectLine = PickUpLine(picked);
                }
                else if (b.state == BurnerState.Empty)
                {
                    var started = Real.TryStartCooking(id, b.pot.Count(p => p == Flour), b.spice == Garlic ? 1 : 0);
                    if (started == CookStartResult.Started)
                    {
                        // 조리가 시작되면 기다릴 일만 남으므로 바로 창을 닫는다 (남은 시간은 화구 배지가 보여 준다)
                        SyncReal();
                        Close();
                        return;
                    }
                    rejectLine = CookLine(started);
                }
                SyncReal();
                if (rejectLine != null && IsOpen) view.Show(BuildData());
                return;
            }
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
            Close(); // 조리를 시작하면 바로 닫는다 (실제 게임과 같게)
        }

        private void Refresh()
        {
            rejectLine = null; // 재료를 바꾸면 지난 거절 안내는 지운다
            view.Show(BuildData());
        }

        // =================================================================
        // 표시 데이터
        // =================================================================

        private KitchenViewData BuildData()
        {
            if (Real != null) return BuildReal(Real);
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
        // 실제 게임 (IKitchenOrders) — 지금은 구운 빵 한 가지
        // =================================================================

        private const string Flour = "Flour";
        private const string Garlic = "garlic";
        private static readonly string[] Axes = { "독", "열", "냉기", "어둠", "체력", "함정" };

        // 주문 상태 → 화구 상태. 접시를 들었거나 주문이 없으면 화구를 비운다
        private void SyncReal()
        {
            if (current < 0) current = 0;
            var real = Real;
            var b = burners[current];
            var st = real.State;
            var next = st == Systems.FirstGuestOrderState.Cooking ? BurnerState.Cooking
                     : st == Systems.FirstGuestOrderState.Ready ? BurnerState.Done
                     : BurnerState.Empty;
            if (next == BurnerState.Empty && st != Systems.FirstGuestOrderState.Waiting && b.state != BurnerState.Empty)
                burners[current] = b = new Burner(); // 들고 갔거나 다 먹음 → 냄비 비우기
            b.state = next;
            b.remaining = real.RemainingSeconds;
            if (!IsOpen) { lastRealState = st; return; }
            if (st != lastRealState) view.Show(BuildData());
            else if (next == BurnerState.Cooking) view.SetTimer(real.RemainingSeconds, real.CookSeconds, next);
            lastRealState = st;
        }

        private KitchenViewData BuildReal(IKitchenOrders real)
        {
            var b = burners[current];
            bool hasOrder = real.GuestName != null;
            int reserved = b.state == BurnerState.Empty && b.spice == Garlic ? 1 : 0;
            var garlicRow = PrepScreenPresenter.SpiceTable.First(t => t.id == Garlic);
            var garlic = SpiceItem(garlicRow);
            garlic.count = Mathf.Max(0, real.GarlicStock - reserved);
            return new KitchenViewData
            {
                burnerName = "화구 " + (current + 1),
                state = b.state,
                cookSeconds = real.CookSeconds,
                remainingSeconds = real.RemainingSeconds,
                orders = hasOrder
                    ? new[] { new OrderTicketData { id = real.OrderId, guestName = real.GuestName, seatLabel = "주문", status = Status(real.State) } }
                    : Array.Empty<OrderTicketData>(),
                selectedOrderId = hasOrder ? real.OrderId : null,
                ingredients = new[] { Item(Flour, 0, true) },
                spices = new[] { garlic },
                potIngredients = b.pot.Select(id => id == null ? null : Item(id, 0, BasicIngredients.ContainsKey(id))).ToArray(),
                potSpice = b.spice == null ? null : SpiceItem(garlicRow),
                preview = PreviewReal(real, b),
                risks = Axes.Where(axis => (real.KnownRisk(axis) ?? 0) > 0)
                    .Select(axis => new GuestRiskData { risk = axis, amount = real.KnownRisk(axis).Value }).ToArray(),
            };
        }

        // 결과 카드: 구운 빵 재료 진행 · 효과 · 가격 (값은 주방 쪽 레시피에서 읽는다)
        private RecipePreviewData PreviewReal(IKitchenOrders real, Burner b)
        {
            if (real.GuestName == null)
                return new RecipePreviewData { title = "???", needLine = "아직 주문이 없어요" };
            int flour = b.pot.Count(id => id == Flour);
            bool garlic = b.spice == Garlic;
            string need = $"밀가루 {flour} / {real.FlourRequired}"
                        + (real.GarlicRequired > 0 ? $" · 마늘 {(garlic ? 1 : 0)} / {real.GarlicRequired}" : " · 마늘이 없어 양념 없이 굽기");
            if (flour > real.FlourRequired) need += " · 밀가루가 너무 많아요";
            if (rejectLine != null) need = rejectLine;
            var effects = RiskEffects.Select(e => new EffectAmount { name = e.effect, value = real.Protection(e.axis, garlic) })
                .Where(e => e.value > 0).ToArray();
            return new RecipePreviewData
            {
                found = true,
                title = $"구운 빵 · {real.GuestName}",
                picture = Icon("DishBread"),
                needLine = need,
                effects = effects,
                taste = 2, // 첫 영업은 맛 점수 2 고정 (FirstGuestOrder.TryServe)
                flavor = garlic ? "고소 · 담백 · 향긋" : "고소 · 담백",
                priceText = $"금화 {real.Price(garlic)}개 예상",
                canCook = b.state == BurnerState.Empty && flour == real.FlourRequired && (garlic ? 1 : 0) == real.GarlicRequired,
            };
        }

        // 위험 축 → 그 축을 막는 요리 효과 이름
        private static readonly (string axis, string effect)[] RiskEffects =
            { ("독", "해독"), ("열", "내열"), ("냉기", "방한"), ("어둠", "야광"), ("체력", "활력"), ("함정", "감각") };

        // 거절 사유 → 안내 문구 (💡 문구 가안)
        private static string CookLine(CookStartResult r)
        {
            switch (r)
            {
                case CookStartResult.NoOrder: return "아직 주문이 없어요";
                case CookStartResult.NotSeated: return "손님이 자리에 앉은 뒤에 만들 수 있어요";
                case CookStartResult.NotNearKitchen: return "주방 가까이에서 만들어 주세요";
                case CookStartResult.Paused: return "잠시 멈춘 동안에는 만들 수 없어요";
                case CookStartResult.BurnerBusy: return "이 화구는 이미 쓰는 중이에요";
                case CookStartResult.WrongIngredients: return "재료가 레시피와 맞지 않아요";
                default: return null;
            }
        }

        private static string PickUpLine(PickUpResult r)
        {
            switch (r)
            {
                case PickUpResult.NotCooked: return "아직 다 익지 않았어요";
                case PickUpResult.NotNearKitchen: return "주방 가까이에서 접시를 들어 주세요";
                case PickUpResult.AlreadyHolding: return "이미 접시를 들고 있어요";
                default: return null;
            }
        }

        private static OrderStatus Status(Systems.FirstGuestOrderState? state)
        {
            switch (state)
            {
                case Systems.FirstGuestOrderState.Cooking: return OrderStatus.Cooking;
                case Systems.FirstGuestOrderState.Ready: return OrderStatus.Done;
                case Systems.FirstGuestOrderState.Carrying: return OrderStatus.Carrying;
                case Systems.FirstGuestOrderState.Served: return OrderStatus.Served;
                default: return OrderStatus.Waiting;
            }
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
