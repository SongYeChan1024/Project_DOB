// 새 프리뷰 씬을 한 번 생성한다. EventSystem은 기존 Gameplay의 입력 바인딩을 복사해 사용한다.
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
            string[] scenes = { DOB.Manager.SceneComposition.Title, "Loading", "Basement", "BasementRestaurant", "BasementUI" };
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
            foreach (var name in new[] { "Basement", "BasementRestaurant", "BasementUI", "BasementTitle" })
                if (System.IO.File.Exists(Scenes + name + ".unity"))
                    throw new InvalidOperationException("이미 있는 프리뷰 씬을 보존합니다. " + name);
            EditorSceneManager.OpenScene(Scenes + "Title.unity", OpenSceneMode.Single);
            BasementGrayboxBuilder.CreateScene();
            CreateGame();
            CreateHud();
            CreateTitle();
            AssetDatabase.SaveAssets();
            Debug.Log("DOB_BASEMENT_SCENES_READY");
        }

        private static void NewScene(bool input)
        {
            if (!input) { EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single); return; }
            var source = EditorSceneManager.OpenScene(Scenes + "Gameplay.unity", OpenSceneMode.Single);
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
            var controls = ComponentOn(player, "BasementPlayerInput");
            var service = (Behaviour)ComponentOn(flow, "FirstGuestService");
            service.enabled = false;
            Wire(service, ("player",player.transform), ("guest",guest.transform), ("worldCamera",camera), ("controls",controls));
            var kitchen = (Behaviour)ComponentOn(Child("CookingConnections", systems.transform),"FirstGuestKitchen");
            kitchen.enabled = false;
            Wire(kitchen, ("service",service), ("recipe",Asset<ScriptableObject>("08.Data/SO_FirstService_Bread.asset")), ("guest", Asset<ScriptableObject>("08.Data/SO_Guest_Haru.asset")));
            var plate = ComponentOn(player,"FirstGuestPlateView");
            Wire(plate, ("kitchen", kitchen), ("carryAnchor",player.transform.Find("Anchors/PlatePoint")));
            var composer = ComponentOn(systems,"BasementComposition");
            Wire(composer,("service",service),("kitchen",kitchen));
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
            var settle=Action(canvas.transform,"SettleDay","시험 영업 정산",new Vector2(190,44),new Vector2(-18,20));
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
            var pause=ComponentOn(canvas,"BasementPauseMenu");
            Wire(pause,("font",Font));
            ComponentOn(canvas,"BasementPauseInput");
            Save("BasementUI");
        }

        private static void CreateTitle()
        {
            NewScene(true);
            CameraObject(new Vector3(0,0,-10),Vector3.zero);
            var canvas=Canvas("TitleCanvas");
            var card=Panel(canvas.transform,"Menu",new Vector2(575,500),Vector2.zero,Card);
            Label(card.transform,"Title","다녀왔어: 층계참 식당",44,new Vector2(550,78),new Vector2(0,166));
            Label(card.transform,"Mode","첫 손님 시험 영업",25,new Vector2(500,50),new Vector2(0,109));
            var begin=Action(card.transform,"NewGame","새 게임",new Vector2(340,55),new Vector2(0,32));
            var resume=Action(card.transform,"ContinueGame","이어하기",new Vector2(340,55),new Vector2(0,-40));
            var last=Label(card.transform,"LastSave","",22,new Vector2(530,50),new Vector2(0,-94));
            var exit=Action(card.transform,"Exit","끝내기",new Vector2(340,52),new Vector2(0,-163));
            var menu=ComponentOn(canvas,"BasementTitleMenu");
            Wire(menu,("continueButton",resume),("lastSave",last));
            Bind(begin,menu,"NewGame"); Bind(resume,menu,"ContinueGame"); Bind(exit,menu,"Exit");
            Save("BasementTitle");
        }

        private static void Bind(Button button, Component target, string method)
        {
            var action=(UnityAction)Delegate.CreateDelegate(typeof(UnityAction),target,method);
            UnityEventTools.AddPersistentListener(button.onClick,action);
        }
    }
}
