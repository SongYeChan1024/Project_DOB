// 새 프리뷰 씬을 한 번 생성한다. EventSystem은 기존 Title의 입력 바인딩을 복사해 사용한다.
// 이미 있는 씬에는 'DOB/Basement/하루 진행 연결 추가' 메뉴로 하루 진행(F1) 연결과 임시 화면만 추가한다.
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace DOB.Editor
{
    public static class BasementSceneBuilder
    {
        private const string Root = "Assets/_DOB/";
        private const string Scenes = Root + "01.Scenes/";
        private static Font Font => AssetDatabase.LoadAssetAtPath<Font>(Root + "07.UI/Fonts/Gaegu-Regular.ttf");
        private static readonly Color Ink = new Color(.95f,.91f,.82f);
        private static readonly Color Card = new Color(.12f,.17f,.17f,.98f);
        private static readonly Color ButtonColor = new Color(.18f,.31f,.3f);

        [MenuItem("DOB/Basement/Windows 시험 빌드")]
        public static void BuildPreview()
        {
            string[] scenes = { DOB.Manager.SceneComposition.Title, "Loading", "Basement", DOB.Manager.BasementComposition.RestaurantScene, "BasementUI" };
            const string output = "Builds/Basement/Basement.exe";
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(output));
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = scenes.Select(name => Scenes + name + ".unity").ToArray(),
                locationPathName = output,
                target = BuildTarget.StandaloneWindows64
            });
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
                throw new InvalidOperationException("Basement 프리뷰 빌드 실패: " + report.summary.result);
            Debug.Log("DOB_BASEMENT_WINDOWS_BUILD_READY " + output);
        }

        public static void Generate()
        {
            foreach (var name in new[] { "Basement", "BasementUI" })
                if (System.IO.File.Exists(Scenes + name + ".unity"))
                    throw new InvalidOperationException("이미 있는 프리뷰 씬을 보존합니다. " + name);
            EditorSceneManager.OpenScene(Scenes + "Title.unity", OpenSceneMode.Single);
            CreateGame();
            CreateHud();
            AssetDatabase.SaveAssets();
            Debug.Log("DOB_BASEMENT_SCENES_READY");
        }

        private static void NewScene(bool input)
        {
            if (!input) { EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single); return; }
            var source = EditorSceneManager.OpenScene(Scenes + "Title.unity", OpenSceneMode.Single);
            var original = source.GetRootGameObjects().Single(o => o.name == "EventSystem");
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            SceneManager.SetActiveScene(scene);
            var copy = Object.Instantiate(original);
            copy.name = "EventSystem";
            SceneManager.MoveGameObjectToScene(copy, scene);
            EditorSceneManager.CloseScene(source, true);
        }

        private static void Save(string name)
        {
            if (!EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), Scenes + name + ".unity"))
                throw new InvalidOperationException(name);
        }

        private static GameObject Child(string name, Transform parent)
        {
            var o = new GameObject(name);
            o.transform.SetParent(parent, false);
            return o;
        }

        private static Component ComponentOn(GameObject obj, string className)
        {
            var type = TypeCache.GetTypesDerivedFrom<MonoBehaviour>().Single(t => t.Name == className && t.Namespace.StartsWith("DOB."));
            return obj.AddComponent(type);
        }

        private static void Wire(Object obj, params (string, Object)[] fields)
        {
            var serialized = new SerializedObject(obj);
            foreach (var field in fields)
            {
                var property = serialized.FindProperty(field.Item1);
                if (property == null || !field.Item2) throw new InvalidOperationException(obj.name + " / " + field.Item1);
                if (property.isArray) { property.arraySize = 1; property = property.GetArrayElementAtIndex(0); }
                property.objectReferenceValue = field.Item2;
            }
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static T Asset<T>(string path) where T : Object => AssetDatabase.LoadAssetAtPath<T>(Root + path);

        private static GameObject Character(Transform parent, string kind)
        {
            string name = "PF_" + kind;
            var obj = (GameObject)PrefabUtility.InstantiatePrefab(Asset<GameObject>("03.Prefabs/Characters/" + name + ".prefab"), parent);
            obj.name = name;
            return obj;
        }

        private static void CreateGame()
        {
            NewScene(false);
            var actors = new GameObject("RuntimeCharacters").transform;
            var player = Character(actors, "Player");
            var guest = Character(actors, "Guest");
            player.GetComponentInChildren<Renderer>().sharedMaterial = Asset<Material>("04.Art/Materials/Basement/GlazedTeal.mat");
            guest.GetComponentInChildren<Renderer>().sharedMaterial = Asset<Material>("04.Art/Materials/Basement/Cream.mat");
            player.transform.position = new Vector3(0,0,-1);
            guest.transform.position = new Vector3(-4,0,-2);
            var camera = CameraObject(new Vector3(0,10,-10), new Vector3(0,0,.25f));
            var systems = new GameObject("Systems");
            var flow = Child("GuestFlowConnections",systems.transform);
            player.GetComponent<DOB.Manager.PlayerMovementController>().enabled = false;
            var service = (Behaviour)ComponentOn(flow, "FirstGuestService");
            service.enabled = false;
            var guests = ComponentOn(systems,"GuestManager");
            Wire(guests, ("settings",ServiceRosterSettings()), ("template",guest.transform));
            Wire(service, ("player",player.transform), ("guests",guests), ("worldCamera",camera),
                ("zoom", Asset<ScriptableObject>("08.Data/SO_GuestCloseUp.asset")));
            var kitchen = (Behaviour)ComponentOn(Child("CookingConnections", systems.transform),"FirstGuestKitchen");
            kitchen.enabled = false;
            Wire(kitchen, ("service",service), ("recipe",Asset<ScriptableObject>("08.Data/SO_FirstService_Bread.asset")));
            var plate = ComponentOn(player,"FirstGuestPlateView");
            Wire(plate, ("kitchen", kitchen), ("carryAnchor",player.transform.Find("Anchors/PlatePoint")));
            var game = ComponentOn(systems,"GameManager");
            var relationship = ComponentOn(systems,"RelationshipManager");
            Wire(relationship,("rules",AffinityRules()));
            Wire(game,("service",service),("kitchen",kitchen),("guests",guests),("relationship",relationship),("settings",DayCycleSettings()));
            var settlement = ComponentOn(systems,"SettlementManager");
            Wire(settlement,("rewards",ExpeditionRewards()));
            Wire(game,("settlement",settlement),("economy",ComponentOn(systems,"EconomyManager")));
            var composer = ComponentOn(systems,"BasementComposition");
            Wire(composer,("game",game));
            Save("Basement");
        }

        private static Camera CameraObject(Vector3 pos, Vector3 target)
        {
            var obj = new GameObject("Main Camera",typeof(Camera),typeof(AudioListener));
            obj.tag="MainCamera";
            obj.transform.position=pos;
            obj.transform.LookAt(target);
            var cam=obj.GetComponent<Camera>();
            cam.orthographic=true;
            cam.orthographicSize=5.6f;
            cam.clearFlags=CameraClearFlags.SolidColor;
            cam.backgroundColor= new Color(.065f,.09f,.1f);
            return cam;
        }

        private static GameObject Canvas(string name)
        {
            var obj=new GameObject(name,typeof(RectTransform),typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));
            obj.GetComponent<Canvas>().renderMode=RenderMode.ScreenSpaceOverlay;
            var scaler=obj.GetComponent<CanvasScaler>();
            scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution=new Vector2(1280,720);
            scaler.matchWidthOrHeight=.5f;
            return obj;
        }

        private static GameObject Box(Transform parent,string name,Vector2 size,Vector2 pos)
        {
            var obj=new GameObject(name,typeof(RectTransform));
            var rect=(RectTransform)obj.transform;
            rect.SetParent(parent,false);
            rect.sizeDelta=size;
            rect.anchoredPosition=pos;
            return obj;
        }

        private static GameObject Panel(Transform parent,string name,Vector2 size,Vector2 pos,Color color)
        {
            var obj=Box(parent,name,size,pos);
            obj.AddComponent<Image>().color=color;
            return obj;
        }

        private static Text Label(Transform parent,string name,string text,int size,Vector2 extent,Vector2 pos)
        {
            var label=Box(parent,name,extent,pos).AddComponent<Text>();
            label.font=Font;
            label.fontSize=size;
            label.color=Ink;
            label.text=text;
            label.alignment=TextAnchor.MiddleCenter;
            label.raycastTarget=false;
            return label;
        }

        private static Button Action(Transform parent,string name,string caption,Vector2 size,Vector2 pos)
        {
            var obj=Panel(parent,name,size,pos,ButtonColor);
            var button=obj.AddComponent<Button>();
            button.targetGraphic=obj.GetComponent<Image>();
            Label(obj.transform,"Label",caption,27,size,Vector2.zero);
            return button;
        }

        private static void Anchor(RectTransform rect, Vector2 anchor)
        {
            rect.anchorMin=rect.anchorMax=rect.pivot=anchor;
        }

        private static void CreateHud()
        {
            NewScene(true);
            var canvas=Canvas("GameplayCanvas");
            var bar=Panel(canvas.transform,"GuidanceBar",new Vector2(850,58),new Vector2(0,-16),Card);
            Anchor((RectTransform)bar.transform,new Vector2(.5f,1));
            var guidance=Label(bar.transform,"ServiceGuidance","",28,new Vector2(830,54),Vector2.zero);
            var receipt=Label(canvas.transform,"Receipt","",25,new Vector2(820,78),new Vector2(0,25));
            Anchor(receipt.rectTransform,new Vector2(.5f,0));
            var settle=Action(canvas.transform,"SettleDay",SettleCaption,new Vector2(190,44),new Vector2(-18,20));
            Anchor((RectTransform)settle.transform,Vector2.right);
            var panel=Panel(canvas.transform,"KitchenRecipePanel",Vector2.zero,Vector2.zero,new Color(.04f,.07f,.07f,.82f));
            var cover=(RectTransform)panel.transform; cover.anchorMin=Vector2.zero; cover.anchorMax=Vector2.one; cover.offsetMin=cover.offsetMax=Vector2.zero;
            var card=Panel(panel.transform,"Recipe",new Vector2(510,305),Vector2.zero,Card);
            var recipe=Label(card.transform,"Ingredients","",34,new Vector2(480,177),new Vector2(0,45));
            var confirm=Action(card.transform,"ConfirmCook","조리 시작",new Vector2(196,52),new Vector2(-105,-88));
            var cancel=Action(card.transform,"CancelCook","돌아가기",new Vector2(196,52),new Vector2(105,-88));
            panel.SetActive(false);
            var dialogue=ComponentOn(canvas,"FirstGuestDialogue");
            var view=Asset<GameObject>("03.Prefabs/UI/PF_GuestDialogue.prefab").GetComponent("GuestDialogueView");
            Wire(dialogue,("viewPrefab",view),("guests", Asset<ScriptableObject>("08.Data/SO_Guest_Haru.asset")));
            var hud=ComponentOn(canvas,"BasementHud");
            Wire(hud,("guidance",guidance),("dialogue",dialogue),("recipePanel",panel),("ingredients",recipe),("receipt",receipt),("confirm",confirm),("cancel",cancel),("settle",settle));
            AddDayFlowScreens(canvas.transform,hud);
            var pause=ComponentOn(canvas,"BasementPauseMenu");
            Wire(pause,("font",Font));
            ComponentOn(canvas,"BasementPauseInput");
            Save("BasementUI");
        }

        private const string SettleCaption = "마감 확인";
        private const string DayCyclePath = "08.Data/SO_DayCycle.asset";

        // 이미 있는 Basement·BasementUI 씬에 하루 진행(F1) 연결만 추가한다. 이미 연결돼 있으면 건드리지 않는다.
        [MenuItem("DOB/Basement/하루 진행 연결 추가")]
        public static void AddDayFlow()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;
            var game = EditorSceneManager.OpenScene(Scenes + "Basement.unity", OpenSceneMode.Single).GetRootGameObjects()
                .Select(root => root.GetComponentInChildren<DOB.Manager.GameManager>(true)).Single(found => found);
            // Unity 오브젝트는 ?? 로 없음을 판정할 수 없어 명시적으로 확인한다.
            var relationship = game.GetComponent<DOB.Manager.RelationshipManager>();
            if (!relationship) relationship = game.gameObject.AddComponent<DOB.Manager.RelationshipManager>();
            Wire(game,("relationship",relationship),("settings",DayCycleSettings()));
            Save("Basement");

            var hud = EditorSceneManager.OpenScene(Scenes + "BasementUI.unity", OpenSceneMode.Single).GetRootGameObjects()
                .Select(root => root.GetComponentInChildren<DOB.Manager.BasementHud>(true)).Single(found => found);
            if (!hud.HasDayFlowViews)
                AddDayFlowScreens(hud.transform,hud);
            hud.Settle.GetComponentInChildren<Text>().text = SettleCaption;
            EditorUtility.SetDirty(hud.Settle.GetComponentInChildren<Text>());
            Save("BasementUI");
            Debug.Log("DOB_BASEMENT_DAY_FLOW_READY");
        }

        // 아트가 배치한 Restaurant 씬을 새 게임 경로의 식당 레벨로 쓰도록 배치 계약(RestaurantLayout)과 이동 경계만 붙인다.
        // 기준점 위치와 가구·조명은 건드리지 않는다. 아트 메뉴(Build Restaurant Greybox)는 LayoutAnchors를 지우지 않으므로 다시 빌드해도 남는다.
        [MenuItem("DOB/Basement/Restaurant 씬 연결 추가")]
        public static void AddRestaurantLayout()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;
            var anchors = EditorSceneManager.OpenScene(Scenes + "Restaurant.unity", OpenSceneMode.Single).GetRootGameObjects()
                .Single(root => root.name == "LayoutAnchors").transform;
            var layout = anchors.GetComponent<DOB.Manager.RestaurantLayout>();
            if (!layout) layout = anchors.gameObject.AddComponent<DOB.Manager.RestaurantLayout>();

            // 아트의 블록 바닥은 빌드할 때마다 새로 만들어지고 충돌체가 없다. 이동 경계와 발판은 기준점 쪽에 따로 둔다.
            var bounds = anchors.Find("FloorBounds");
            if (!bounds)
            {
                bounds = new GameObject("FloorBounds").transform;
                bounds.SetParent(anchors, false);
            }
            var floor = bounds.GetComponent<BoxCollider>();
            if (!floor) floor = bounds.gameObject.AddComponent<BoxCollider>();
            // 건물 격자(RestaurantGreyboxSetup: x -6~6, z -4~6)를 덮고 윗면이 바닥 높이 0에 오게 한다.
            floor.center = new Vector3(0f, -0.5f, 1f);
            floor.size = new Vector3(12f, 1f, 10f);

            var data = new SerializedObject(layout);
            foreach (var (field, anchor) in new[] { ("playerSpawn", "PlayerSpawn"), ("guestEntrance", "GuestEntrance"),
                         ("greetingPoint", "GreetingPoint"), ("kitchen", "Kitchen"), ("stairsExit", "StairsExit") })
                data.FindProperty(field).objectReferenceValue = RequiredAnchor(anchors, anchor);
            // 💡 시작 3석만 영업 좌석으로 잇는다 (아트 배치의 start 좌석). Seat04~06은 '좌석 늘리기' 해금을 연결할 때 추가한다.
            string[] startSeats = { "Seat01", "Seat02", "Seat03" };
            var seats = data.FindProperty("seats");
            seats.arraySize = startSeats.Length;
            for (int i = 0; i < startSeats.Length; i++)
                seats.GetArrayElementAtIndex(i).objectReferenceValue = RequiredAnchor(anchors, startSeats[i]);
            data.FindProperty("floor").objectReferenceValue = floor;
            data.ApplyModifiedPropertiesWithoutUndo();
            Save("Restaurant");
            Debug.Log("DOB_RESTAURANT_LAYOUT_READY");
        }

        private static Transform RequiredAnchor(Transform anchors, string name)
        {
            var anchor = anchors.Find(name);
            if (!anchor) throw new InvalidOperationException("Restaurant 기준점 누락: " + name);
            return anchor;
        }

        // 프론트엔드가 만든 귀환 보고 화면(PF_ReturnReport)을 BasementUI의 아침 보고 칸에 연결하고 임시 보고 카드를 지운다.
        // 화면 프리팹은 고치지 않는다. 단추 이름 ConfirmMorningReport는 프리팹이 유지한다 (통합 테스트가 이름으로 누른다).
        [MenuItem("DOB/Basement/귀환 보고 화면 연결 추가")]
        public static void AddReturnReport()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;
            var scene = EditorSceneManager.OpenScene(Scenes + "BasementUI.unity", OpenSceneMode.Single);
            var hud = scene.GetRootGameObjects().Select(root => root.GetComponentInChildren<DOB.Manager.BasementHud>(true)).Single(found => found);
            var view = scene.GetRootGameObjects().Select(root => root.GetComponentInChildren<DOB.UI.ReturnReportView>(true)).FirstOrDefault(found => found);
            if (!view)
            {
                var prefab = Asset<GameObject>("03.Prefabs/UI/PF_ReturnReport.prefab");
                if (!prefab) throw new InvalidOperationException("PF_ReturnReport 프리팹 누락");
                // 프리팹이 자기 Canvas를 가지므로 씬 루트에 둔다.
                view = ((GameObject)PrefabUtility.InstantiatePrefab(prefab, scene)).GetComponentInChildren<DOB.UI.ReturnReportView>(true);
                if (!view) throw new InvalidOperationException("PF_ReturnReport에 ReturnReportView가 없습니다.");
            }

            if (hud.MorningReport is DOB.UI.TempMorningReportView temp)
            {
                var card = new SerializedObject(temp).FindProperty("panel").objectReferenceValue as GameObject;
                if (card) UnityEngine.Object.DestroyImmediate(card);
                UnityEngine.Object.DestroyImmediate(temp);
            }
            Wire(hud,("morningReport",view));
            Save("BasementUI");
            Debug.Log("DOB_BASEMENT_RETURN_REPORT_READY");
        }

        private const string ExpeditionRewardsPath = "08.Data/SO_ExpeditionRewards.asset";

        // 이미 있는 Basement 씬에 정산·경제 총괄(F7·F9 방문 결과와 귀환 보고, 던전 재료 재고) 연결만 추가한다.
        [MenuItem("DOB/Basement/정산·경제 연결 추가")]
        public static void AddSettlement()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;
            var game = EditorSceneManager.OpenScene(Scenes + "Basement.unity", OpenSceneMode.Single).GetRootGameObjects()
                .Select(root => root.GetComponentInChildren<DOB.Manager.GameManager>(true)).Single(found => found);
            var settlement = game.GetComponent<DOB.Manager.SettlementManager>();
            if (!settlement) settlement = game.gameObject.AddComponent<DOB.Manager.SettlementManager>();
            var economy = game.GetComponent<DOB.Manager.EconomyManager>();
            if (!economy) economy = game.gameObject.AddComponent<DOB.Manager.EconomyManager>();
            Wire(settlement,("rewards",ExpeditionRewards()));
            Wire(game,("settlement",settlement),("economy",economy));
            Save("Basement");
            Debug.Log("DOB_BASEMENT_SETTLEMENT_READY");
        }

        // 전리품 자격 누적 호감 9(확정)·💡 희귀 재료 1개는 ExpeditionRewards의 기본값이다. 1층 일반 재료 4종과 희귀 재료(숲 송로)를 넣는다
        // (FeatureManagerMap F9). 에셋이 이미 있으면 값을 건드리지 않는다.
        private static ScriptableObject ExpeditionRewards()
        {
            var rewards = Asset<ScriptableObject>(ExpeditionRewardsPath);
            if (rewards)
                return rewards;
            rewards = ScriptableObject.CreateInstance<DOB.Define.ExpeditionRewards>();
            var data = new SerializedObject(rewards);
            var floors = data.FindProperty("floors");
            floors.arraySize = 1;
            var first = floors.GetArrayElementAtIndex(0);
            first.FindPropertyRelative("floor").intValue = 1;
            var materials = first.FindPropertyRelative("materials");
            string[] forest = { "슬라임 젤리", "숲버섯", "들풀", "숲 당근" };
            materials.arraySize = forest.Length;
            for (int i = 0; i < forest.Length; i++)
                materials.GetArrayElementAtIndex(i).stringValue = forest[i];
            first.FindPropertyRelative("rareMaterial").stringValue = "숲 송로";
            data.ApplyModifiedPropertiesWithoutUndo();
            AssetDatabase.CreateAsset(rewards, Root + ExpeditionRewardsPath);
            return rewards;
        }

        private const string ServiceRosterPath = "08.Data/SO_ServiceRoster.asset";

        // 이미 있는 Basement 씬에 여러 손님 명단(GuestManager)과 영업 손님 기준값만 연결한다. 손님 템플릿은 첫 손님 서비스의 기존 PF_Guest다.
        [MenuItem("DOB/Basement/여러 손님 연결 추가")]
        public static void AddGuestRoster()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;
            var roots = EditorSceneManager.OpenScene(Scenes + "Basement.unity", OpenSceneMode.Single).GetRootGameObjects();
            var game = roots.Select(root => root.GetComponentInChildren<DOB.Manager.GameManager>(true)).Single(found => found);
            var service = new SerializedObject(game).FindProperty("service").objectReferenceValue as DOB.Manager.FirstGuestService;
            var guest = roots.SingleOrDefault(root => root.name == "RuntimeCharacters")?.transform.Find("PF_Guest");
            if (!service || !guest) throw new InvalidOperationException("Basement의 첫 손님 서비스와 RuntimeCharacters/PF_Guest를 확인하세요.");
            var guests = game.GetComponent<DOB.Manager.GuestManager>();
            if (!guests) guests = game.gameObject.AddComponent<DOB.Manager.GuestManager>();
            Wire(guests,("settings",ServiceRosterSettings()),("template",guest));
            Wire(service,("guests",guests));
            Wire(game,("guests",guests));
            Save("Basement");
            Debug.Log("DOB_BASEMENT_GUEST_ROSTER_READY");
        }

        private const string AffinityRulesPath = "08.Data/SO_AffinityRules.asset";

        // 이미 있는 Basement 씬의 관계 총괄(RelationshipManager)에 호감·입소문 기준값만 연결한다.
        [MenuItem("DOB/Basement/호감·입소문 기준 연결 추가")]
        public static void AddAffinityRules()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;
            var relationship = EditorSceneManager.OpenScene(Scenes + "Basement.unity", OpenSceneMode.Single).GetRootGameObjects()
                .Select(root => root.GetComponentInChildren<DOB.Manager.RelationshipManager>(true)).Single(found => found);
            Wire(relationship,("rules",AffinityRules()));
            Save("Basement");
            Debug.Log("DOB_BASEMENT_AFFINITY_RULES_READY");
        }

        // 💡 시험값: 대성공 +2·성공 +1·실패 −1·대실패 −2, 돌려보냄·대기 이탈 −4, 방문 최대 6, 문턱 60·75·85·100%, 돌려보낸 방문 입소문 포함(2026-11-25 확정)
        // (FeatureManagerMap F10, 송예찬 확인 전). 값은 AffinityRules의 기본값이다. 에셋이 이미 있으면 값을 건드리지 않는다.
        private static ScriptableObject AffinityRules()
        {
            var rules = Asset<ScriptableObject>(AffinityRulesPath);
            if (rules)
                return rules;
            rules = ScriptableObject.CreateInstance<DOB.Define.AffinityRules>();
            AssetDatabase.CreateAsset(rules, Root + AffinityRulesPath);
            return rules;
        }

        // 💡 시험값: 기본 3명, 입장 간격 8~20초, 줄 2명, 대기 2점·10초마다 −1·30초 이탈 (FeatureManagerMap F1·F2·F10 시험안, 송예찬 확인 전).
        // 값은 ServiceRosterSettings의 기본값이다. 에셋이 이미 있으면 값을 건드리지 않는다.
        private static ScriptableObject ServiceRosterSettings()
        {
            var settings = Asset<ScriptableObject>(ServiceRosterPath);
            if (settings)
                return settings;
            settings = ScriptableObject.CreateInstance<DOB.Define.ServiceRosterSettings>();
            AssetDatabase.CreateAsset(settings, Root + ServiceRosterPath);
            return settings;
        }

        private static ScriptableObject DayCycleSettings()
        {
            var settings = Asset<ScriptableObject>(DayCyclePath);
            if (settings)
                return settings;
            settings = ScriptableObject.CreateInstance<DOB.Define.DayCycleSettings>();
            AssetDatabase.CreateAsset(settings, Root + DayCyclePath);
            return settings;
        }

        // 임시 아침보고·준비·방명록 카드. 실제 화면은 같은 추상 View를 상속해 BasementHud의 같은 칸에 연결하면 된다.
        private static void AddDayFlowScreens(Transform canvas, Component hud)
        {
            var report=TempCard(canvas,"MorningReport","ConfirmMorningReport","보고 확인","TempMorningReportView","confirm",Vector2.zero);
            var prep=TempCard(canvas,"Preparation","StartService","영업 시작","TempPreparationView","start",Vector2.zero);
            // 방명록은 영업 화면 위쪽에 띄워 마감 확인 버튼과 주방 안내를 가리지 않는다.
            var book=TempCard(canvas,"Guestbook","CloseGuestbook","닫기","TempGuestbookView","close",new Vector2(0,120));
            Wire(hud,("morningReport",report),("preparation",prep),("guestbook",book));
        }

        private static Component TempCard(Transform canvas, string name, string buttonName, string caption, string viewClass, string buttonField, Vector2 pos)
        {
            var card=Panel(canvas,name,new Vector2(560,300),pos,Card);
            var body=Label(card.transform,"Body","",28,new Vector2(520,190),new Vector2(0,40));
            var button=Action(card.transform,buttonName,caption,new Vector2(196,52),new Vector2(0,-105));
            card.SetActive(false);
            var view=ComponentOn(canvas.gameObject,viewClass);
            Wire(view,("panel",card),("body",body),(buttonField,button));
            return view;
        }
    }
}
