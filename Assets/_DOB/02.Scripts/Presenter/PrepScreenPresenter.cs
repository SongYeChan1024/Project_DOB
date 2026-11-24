// =====================================================================
// PrepScreenPresenter — 준비 화면(시장 · 레시피 책 · 수첩 · 가게 개량 · 입구 문패)과 게임 쪽을 잇는다
// 역할   : UIManager에 자신을 등록해 두고, UIManager가 "준비 화면 열어 줘"를 전하면 표시 데이터를 만들어 화면을 연다.
//          화면의 결제 · 개량 · 문 열기 입력을 요청으로 옮긴다.
// 호출자 : UIManager (Open, Close, IsOpen) ← 식당 물건 상호작용(E) · Tab 입력 쪽에서 UIManager.OpenPrepScreen 호출
// 호출   : PrepScreensView, UIManager (등록 · 해제 · 영업 시작 알림)
// 상태   : 더미 단계에서는 금화 · 보유 양념 · 재료 · 개량 단계를 임시로 가진다. 계약 후 백엔드 결과로 바꾼다.
// 비고   : TODO(계약) 표시된 곳이 백엔드 호출로 바뀔 자리 (Docs/개발/FeatureManagerMap.md 💡 구성안 기준).
//          금화 · 재고 · 시장 · 개량 → EconomyManager (Wallet · Inventory · UpgradeState)
//          층 개방 · 기록된 위험 → SettlementManager (FloorProgress) / 수첩 기록 → RelationshipManager (NotebookRecords)
//          [문 열기] → GameManager (DayCycle)가 UIManager.StartBusinessRequested를 듣고 영업으로 넘긴다
//          문구 · 가격은 기획 10 · 11장 기준 임시 값.
//
// 의사코드
// [켜질 때]   화면 이벤트를 구독하고 UIManager에 등록한다
// [꺼질 때]   구독을 풀고 등록을 해제한다
// [열기]      표시 데이터를 만들어 화면을 연다
// [결제]      (양념 id → 개수)를 받아 구매 요청 → 성공이면 새 데이터로 다시 그림, 실패면 사유 안내
// [개량]      개량 id로 요청 → 다시 그림
// [문 열기]   화면을 닫고 UIManager에 영업 시작을 알린다
// =====================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using DOB.UI;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>준비 화면의 입력을 요청으로 옮기고 결과를 화면에 표시한다. 게임 규칙은 계산하지 않는다.</summary>
    public sealed class PrepScreenPresenter : MonoBehaviour
    {
        [SerializeField] private PrepScreensView view;

        /// <summary>양념 그림 (마늘 · 박하 · 생강 · 당근즙 · 소금 · 후추 순서).</summary>
        [Header("그림"), SerializeField] private Sprite[] spiceIcons = new Sprite[6];
        /// <summary>개량 그림 (좌석 · 화덕 · 양념 선반 순서).</summary>
        [SerializeField] private Sprite[] upgradePictures = new Sprite[3];
        /// <summary>재료 · 요리 그림 (07.UI/Sprites/Items). 파일 이름으로 찾는다 (예: T_UI_IngFlour, T_UI_DishBread).</summary>
        [SerializeField] private Sprite[] itemIcons = Array.Empty<Sprite>();

        // ---------------- 더미 상태 (계약 전 테스트용) ----------------
        private int gold = 59;
        private readonly Dictionary<string, int> owned = new Dictionary<string, int> { ["garlic"] = 1, ["salt"] = 2 };
        // 기본 재료는 무제한이라 빼고, 던전 재료만 (재료 그림 이름 → 개수)
        private readonly Dictionary<string, int> ownedIngredients = new Dictionary<string, int>
            { ["SlimeJelly"] = 2, ["ForestMushroom"] = 1, ["WaterMoss"] = 1, ["BatWing"] = 3 };
        private readonly Dictionary<string, int> upgradeLevels = new Dictionary<string, int>
            { ["seat"] = 0, ["hearth"] = 0, ["shelf"] = 0 };

        /// <summary>준비 화면이 열려 있는지.</summary>
        public bool IsOpen => view && view.IsOpen;
        public PrepScreen Current => view ? view.Current : PrepScreen.None;

        private void OnEnable()
        {
            if (view)
            {
                view.Market.PurchaseRequested += OnPurchase;
                view.Market.CloseClicked += Close;
                view.RecipeBook.CloseClicked += Close;
                view.Notebook.CloseClicked += Close;
                view.Upgrade.UpgradeRequested += OnUpgrade;
                view.Upgrade.CloseClicked += Close;
                view.DoorSign.OpenClicked += OnOpenBusiness;
                view.DoorSign.CancelClicked += Close;
            }
            UIManager.RegisterPrepScreen(this);
        }

        private void OnDisable()
        {
            if (view)
            {
                view.Market.PurchaseRequested -= OnPurchase;
                view.Market.CloseClicked -= Close;
                view.RecipeBook.CloseClicked -= Close;
                view.Notebook.CloseClicked -= Close;
                view.Upgrade.UpgradeRequested -= OnUpgrade;
                view.Upgrade.CloseClicked -= Close;
                view.DoorSign.OpenClicked -= OnOpenBusiness;
                view.DoorSign.CancelClicked -= Close;
            }
            UIManager.UnregisterPrepScreen(this);
        }

        /// <summary>화면 하나를 연다. 이미 다른 준비 화면이 열려 있으면 바꿔 끼운다.</summary>
        public bool Open(PrepScreen screen)
        {
            if (!view || screen == PrepScreen.None)
                return false;
            // TODO(계약): Economy · Settlement · Relationship Manager에서 받은 값으로 바꾼다.
            view.Open(screen, BuildData());
            return true;
        }

        /// <summary>열린 준비 화면을 닫는다.</summary>
        public void Close()
        {
            if (view) view.Close();
        }

        // ---------------- 화면 입력 → 요청 ----------------

        private void OnPurchase(IReadOnlyDictionary<string, int> cart)
        {
            // TODO(계약): EconomyManager 구매 요청(itemId, 수량). 지금은 더미로 금화를 빼고 보유를 늘린다.
            var spices = BuildSpices();
            int cost = cart.Sum(pair => pair.Value * Array.Find(spices, s => s.id == pair.Key).price);
            if (cost > gold)
            {
                view.Market.ShowNotice("금화가 모자라요.");
                return;
            }
            gold -= cost;
            foreach (var pair in cart)
                owned[pair.Key] = (owned.TryGetValue(pair.Key, out int n) ? n : 0) + pair.Value;
            view.Refresh(BuildData());
        }

        private void OnUpgrade(string upgradeId)
        {
            // TODO(계약): EconomyManager 개량 요청(upgradeId). 가게 개량은 첫 플레이 이후 범위(F8).
            var upgrade = Array.Find(BuildUpgrades(), u => u.id == upgradeId);
            if (upgrade == null || !upgrade.canBuy)
                return;
            gold -= upgrade.price;
            upgradeLevels[upgradeId]++;
            view.Refresh(BuildData());
        }

        private void OnOpenBusiness()
        {
            Close();
            // TODO(계약): GameManager(DayCycle)가 UIManager.StartBusinessRequested를 듣고 준비 → 영업으로 넘긴다.
            UIManager.HandleStartBusiness();
        }

        // ---------------- 더미 표시 데이터 ----------------

        private PrepViewData BuildData()
        {
            var spices = BuildSpices();
            foreach (var recipe in Recipes)
                recipe.picture = Icon("Dish" + recipe.id);
            return new PrepViewData
            {
                gold = gold,
                news = "2층 곰팡이 동굴에 박쥐 떼가 늘었대요",
                spices = spices,
                recipes = Recipes,
                recipeTotal = Recipes.Length,
                upgrades = BuildUpgrades(),
                floors = BuildFloors(),
                races = new[]
                {
                    new CodexItemData { name = "인간", noWeakness = true },
                    new CodexItemData { name = "엘프" }, new CodexItemData { name = "수인" },
                    new CodexItemData { name = "드워프" }, new CodexItemData { name = "하플링" },
                },
                jobs = new[] { "전사", "마법사", "도적", "사제", "광부", "궁수" }
                    .Select(n => new CodexItemData { name = n }).ToArray(),
                // TODO(계약): 손님 수 · 좌석 · 화구는 GameManager · GuestManager · KitchenManager, 가진 재료는 EconomyManager 값으로
                doorStats = new[]
                {
                    new DoorStatData { label = "오늘 손님", value = "6명" },
                    new DoorStatData { label = "좌석", value = $"{3 + upgradeLevels["seat"]}석" },
                    new DoorStatData { label = "화구", value = "2개" },
                },
                doorSpices = spices.Where(sp => sp.owned > 0)
                    .Select(sp => new StockItemData
                    {
                        id = sp.id, name = sp.name, count = sp.owned, icon = sp.icon,
                        description = $"{sp.effect} +1 · {sp.flavor}",
                    }).ToArray(),
                doorIngredients = ownedIngredients
                    .Select(kv => new StockItemData
                    {
                        id = kv.Key, name = IngredientName(kv.Key), count = kv.Value, icon = Icon("Ing" + kv.Key),
                        description = IngredientInfo(kv.Key),
                    }).ToArray(),
            };
        }

        // 기획 10장 층 표 (기본 위험 수 · 나오는 재료). 지금은 1 · 2층만 다녀온 상태로 가정
        private FloorItemData[] BuildFloors()
        {
            (string name, int risks, string[] items)[] table =
            {
                ("슬라임 숲", 2, new[] { "SlimeJelly", "ForestMushroom", "Herb", "ForestCarrot", "ForestTruffle" }),
                ("곰팡이 동굴", 3, new[] { "WaterMoss", "BatWing", "BlueMold", "FireflyMoss" }),
                ("용암 균열", 3, new[] { "LavaPepper", "SalamanderTail", "VolcanoSalt" }),
                ("서리 회랑", 3, new[] { "FrostBerry", "IceCrayfish", "MoonFrostFlower", "FrostCrystal" }),
                ("눈먼 심연", 4, new[] { "AbyssInk", "AbyssPearl" }),
            };
            var result = new FloorItemData[table.Length];
            for (int i = 0; i < table.Length; i++)
            {
                var t = table[i];
                result[i] = new FloorItemData
                {
                    number = i + 1, name = t.name, opened = i < 2, riskCount = t.risks,
                    ingredients = t.items.Select(id => new FloorIngredientData
                    {
                        id = id, icon = Icon("Ing" + id), seen = ownedIngredients.ContainsKey(id),
                        name = IngredientName(id), description = IngredientInfo(id),
                    }).ToArray(),
                };
            }
            return result;
        }

        // 기획 10장 던전 재료 (그림 이름 → 한글 이름 · 나오는 층 · 희귀)
        private static readonly Dictionary<string, (string name, int floor, bool rare)> Ingredients =
            new Dictionary<string, (string, int, bool)>
            {
                ["SlimeJelly"] = ("슬라임 젤리", 1, false), ["ForestMushroom"] = ("숲버섯", 1, false),
                ["Herb"] = ("들풀", 1, false), ["ForestCarrot"] = ("숲 당근", 1, false), ["ForestTruffle"] = ("숲 송로", 1, true),
                ["WaterMoss"] = ("물이끼", 2, false), ["BatWing"] = ("박쥐 날개", 2, false),
                ["BlueMold"] = ("푸른곰팡이", 2, false), ["FireflyMoss"] = ("반딧불 이끼", 2, true),
                ["LavaPepper"] = ("용암 고추", 3, false), ["SalamanderTail"] = ("불도마뱀 꼬리", 3, false), ["VolcanoSalt"] = ("화산 소금", 3, true),
                ["FrostBerry"] = ("서리 베리", 4, false), ["IceCrayfish"] = ("얼음 가재", 4, false),
                ["MoonFrostFlower"] = ("달빛 서리꽃", 4, false), ["FrostCrystal"] = ("서리 수정", 4, true),
                ["AbyssInk"] = ("심연 먹물", 5, false), ["AbyssPearl"] = ("심연 진주", 5, true),
            };
        private static readonly string[] FloorNames = { "", "슬라임 숲", "곰팡이 동굴", "용암 균열", "서리 회랑", "눈먼 심연" };

        private static string IngredientName(string id) => Ingredients.TryGetValue(id, out var v) ? v.name : id;

        // 마우스를 올리면 보일 설명: 어디서 나오는지 + 이걸로 만드는 요리와 그 효과
        // 예) "1층 슬라임 숲 재료\n슬라임 젤리 샐러드: 해독 2 · 감각 1"
        private static string IngredientInfo(string id)
        {
            if (!Ingredients.TryGetValue(id, out var v)) return string.Empty;
            string where = $"{v.floor}층 {FloorNames[v.floor]} {(v.rare ? "희귀 재료" : "재료")}";
            var uses = Recipes
                .Where(r => r.ingredients.Split('·').Any(part => part.Trim().StartsWith(v.name + " ")))
                .Select(r => $"{r.name}: {string.Join(" · ", r.effects.Select(e => $"{e.name} {e.value}"))}");
            return string.Join("\n", new[] { where }.Concat(uses));
        }

        // 그림 이름으로 찾기 (예: "IngFlour" → T_UI_IngFlour)
        private Sprite Icon(string key)
        {
            string file = "T_UI_" + key;
            return itemIcons.FirstOrDefault(sp => sp && sp.name == file);
        }

        // 기획 11장 양념 표 (효과 +1 · 맛 · 가격)
        private SpiceItemData[] BuildSpices()
        {
            (string id, string name, string effect, string flavor, int price)[] table =
            {
                ("garlic", "마늘", "해독", "향긋", 2),
                ("mint", "박하", "내열", "시원", 2),
                ("ginger", "생강", "방한", "얼큰", 2),
                ("carrot", "당근즙", "야광", "달콤", 3),
                ("salt", "소금", "활력", "짭짤", 2),
                ("pepper", "후추", "감각", "알싸", 2),
            };
            var result = new SpiceItemData[table.Length];
            for (int i = 0; i < table.Length; i++)
            {
                var t = table[i];
                result[i] = new SpiceItemData
                {
                    id = t.id, name = t.name, effect = t.effect, flavor = t.flavor, price = t.price,
                    owned = owned.TryGetValue(t.id, out int n) ? n : 0,
                    icon = i < spiceIcons.Length ? spiceIcons[i] : null,
                };
            }
            return result;
        }

        // 기획 9장 가게 개량 표
        private UpgradeItemData[] BuildUpgrades()
        {
            (string id, string name, string effect, int[] prices)[] table =
            {
                ("seat", "좌석 늘리기", "동시 좌석 +1", new[] { 30, 70, 100 }),
                ("hearth", "화덕 개량", "가장 강한 효과 +1", new[] { 80, 160 }),
                ("shelf", "양념 선반", "양념 두 칸", new[] { 150 }),
            };
            var result = new UpgradeItemData[table.Length];
            for (int i = 0; i < table.Length; i++)
            {
                var t = table[i];
                int level = upgradeLevels[t.id];
                int price = level < t.prices.Length ? t.prices[level] : 0;
                result[i] = new UpgradeItemData
                {
                    id = t.id, name = t.name, effect = t.effect,
                    detail = t.id == "seat" && level < t.prices.Length ? $"{3 + level}석 → {4 + level}석" : null,
                    level = level, maxLevel = t.prices.Length, price = price,
                    canBuy = level < t.prices.Length && price <= gold,
                    picture = i < upgradePictures.Length ? upgradePictures[i] : null,
                };
            }
            return result;
        }

        // 기획 11장 레시피 표 (처음엔 기본 레시피 3개만 열림). id는 요리 그림 이름과 같게 (T_UI_Dish + id)
        private static readonly RecipeItemData[] Recipes =
        {
            R("Bread", "구운 빵", true, "밀가루 × 2", true, "고소 · 담백", 8, ("활력", 1), ("감각", 1)),
            R("PotatoStew", "감자 스튜", true, "감자 × 1 · 고기 × 1", true, "진한", 10, ("활력", 2)),
            R("GrilledMeat", "고기 구이", true, "고기 × 2", true, "진한 · 바삭", 10, ("활력", 3)),
            R("SlimeSalad", "슬라임 젤리 샐러드", false, "슬라임 젤리 × 2 · 들풀 × 1", false, "상큼 · 시원", 21, ("해독", 2), ("감각", 1)),
            R("MushroomSoup", "숲버섯 수프", false, "숲버섯 × 2 · 감자 × 1", false, "담백", 16, ("활력", 2), ("해독", 1)),
            R("HerbPie", "들풀 파이", false, "들풀 × 1 · 밀가루 × 2", false, "고소 · 향긋", 11, ("감각", 2), ("활력", 1)),
            R("CarrotCake", "당근 케이크", false, "숲 당근 × 2 · 밀가루 × 1", false, "달콤", 16, ("야광", 2), ("활력", 1)),
            R("MossColdSoup", "물이끼 냉국", false, "물이끼 × 2", false, "시원 · 담백", 17, ("내열", 2), ("활력", 1)),
            R("MossSalad", "물이끼 무침", false, "물이끼 × 1 · 들풀 × 1", false, "시원 · 향긋", 17, ("내열", 2), ("감각", 1)),
            R("BatWingFry", "박쥐 날개 튀김", false, "박쥐 날개 × 2", false, "바삭 · 고소", 17, ("감각", 2), ("야광", 1)),
            R("MoldGratin", "푸른곰팡이 그라탱", false, "푸른곰팡이 × 1 · 감자 × 1 · 고기 × 1", false, "쿰쿰 · 진한", 12, ("해독", 2), ("활력", 1)),
            R("LavaPepperStirFry", "용암 고추 볶음", false, "용암 고추 × 2 · 고기 × 1", false, "얼큰 · 진한", 18, ("방한", 3)),
            R("SalamanderSkewer", "불도마뱀 꼬리구이", false, "불도마뱀 꼬리 × 1 · 고기 × 1", false, "진한 · 얼큰", 13, ("활력", 2), ("방한", 1)),
            R("VolcanoPotato", "화산 소금 감자", false, "화산 소금 × 1 · 감자 × 2", false, "짭짤 · 고소", 18, ("활력", 2), ("방한", 1)),
            R("FrostParfait", "서리 베리 파르페", false, "서리 베리 × 2", false, "달콤 · 시원", 19, ("내열", 2), ("감각", 1)),
            R("IceCrayfishStew", "얼음 가재 스튜", false, "얼음 가재 × 1 · 감자 × 1", false, "진한 · 시원", 14, ("활력", 2), ("내열", 1)),
            R("FrostSalad", "서리꽃 샐러드", false, "달빛 서리꽃 × 2", false, "향긋 · 쌉쌀", 19, ("야광", 3)),
            R("InkRisotto", "먹물 리조또", false, "심연 먹물 × 1 · 밀가루 × 1 · 고기 × 1", false, "진한 · 쌉쌀", 15, ("야광", 3), ("활력", 1)),
            R("AbyssCourse", "심연 코스", false, "심연 진주 × 1 · 심연 먹물 × 1 · 달빛 서리꽃 × 1", false, "쌉쌀 · 향긋", 30, ("야광", 3), ("해독", 1), ("방한", 1), ("활력", 1)),
            R("TruffleSoup", "숲 송로 수프", false, "숲 송로 × 1 · 숲버섯 × 2", false, "담백 · 향긋", 26, ("해독", 3), ("활력", 1)),
            R("FireflyBibim", "반딧불 이끼 비빔", false, "반딧불 이끼 × 1 · 물이끼 × 1 · 박쥐 날개 × 1", false, "시원 · 향긋", 27, ("내열", 3), ("야광", 1)),
            R("FrostIce", "서리 수정 빙과", false, "서리 수정 × 1 · 서리 베리 × 1 · 달빛 서리꽃 × 1", false, "달콤 · 시원", 29, ("야광", 3), ("해독", 1), ("방한", 1)),
        };

        private static RecipeItemData R(string id, string name, bool unlocked, string ingredients, bool basicOnly,
                                        string flavor, int price, params (string name, int value)[] effects)
        {
            return new RecipeItemData
            {
                id = id, name = name, unlocked = unlocked, ingredients = ingredients, basicOnly = basicOnly,
                flavor = flavor, basePrice = price,
                effects = effects.Select(e => new EffectAmount { name = e.name, value = e.value }).ToArray(),
            };
        }
    }
}
