using System;
using System.IO;
using System.Linq;
using DOB.Manager;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DOB.Editor
{
    /// <summary>명시적으로 실행할 때만 초기 씬과 설정을 만든다. 기존 씬은 덮어쓰지 않는다.</summary>
    public static class UnityFoundationSetup
    {
        private const string SceneFolder = "Assets/_DOB/01.Scenes/";
        private static readonly string[] SceneNames = { "Title", "Restaurant", "ReturnReport" };

        [MenuItem("DOB/Setup/Create Initial Scenes")]
        public static void Create()
        {
            foreach (var name in SceneNames)
                if (File.Exists(SceneFolder + name + ".unity"))
                    throw new InvalidOperationException("기존 씬을 보존합니다: " + name);

            PlayerSettings.companyName = "DOB Team";
            PlayerSettings.productName = "다녀왔어: 층계참 식당";
            EditorSettings.serializationMode = SerializationMode.ForceText;

            CreateScene("Title", "다녀왔어: 층계참 식당", new[] { ("식당으로", "Restaurant") });
            CreateScene("Restaurant", "층계참 식당", new[] { ("귀환 보고로", "ReturnReport"), ("타이틀로", "Title") });
            CreateScene("ReturnReport", "아침 귀환 보고", new[] { ("식당으로", "Restaurant"), ("타이틀로", "Title") });

            EditorBuildSettings.scenes = SceneNames.Select(n => new EditorBuildSettingsScene(SceneFolder + n + ".unity", true)).ToArray();
            AssetDatabase.SaveAssets();
            EditorSceneManager.OpenScene(SceneFolder + "Title.unity");
            Validate();
        }

        private static void CreateScene(string name, string title, (string label, string scene)[] destinations)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            var camera = Camera.main;
            camera.backgroundColor = new Color(0.07f, 0.09f, 0.12f);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.transform.position = new Vector3(0, 8, -8);
            camera.transform.rotation = Quaternion.Euler(35, 0, 0);

            var canvasObject = new GameObject("NavigationCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster), typeof(SceneNavigationPresenter));
            canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            var presenter = canvasObject.GetComponent<SceneNavigationPresenter>();
            var font = AssetDatabase.LoadAssetAtPath<Font>("Assets/_DOB/07.UI/Fonts/Gaegu-Regular.ttf");
            CreateText(canvasObject.transform, "SceneTitle", title, font, 150, 48);
            for (var i = 0; i < destinations.Length; i++)
            {
                var target = destinations[i];
                var buttonObject = new GameObject("Load_" + target.scene, typeof(RectTransform), typeof(Image), typeof(Button));
                buttonObject.transform.SetParent(canvasObject.transform, false);
                var rect = buttonObject.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(400, 80);
                rect.anchoredPosition = new Vector2(0, -i * 110);
                buttonObject.GetComponent<Image>().color = new Color(0.20f, 0.27f, 0.31f);
                var text = CreateText(buttonObject.transform, "Label", target.label, font, 0, 32);
                text.GetComponent<RectTransform>().sizeDelta = new Vector2(380, 70);
                UnityEventTools.AddStringPersistentListener(buttonObject.GetComponent<Button>().onClick, presenter.LoadScene, target.scene);
            }

            // 씬 전환으로 임시 기본 액션이 파괴되지 않도록 프로젝트의 영속 입력 에셋을 연결한다.
            var eventSystem = new GameObject("EventSystem");
            eventSystem.SetActive(false);
            eventSystem.AddComponent<EventSystem>();
            var module = eventSystem.AddComponent<InputSystemUIInputModule>();
            var serializedModule = new SerializedObject(module);
            const string actionsPath = "Assets/InputSystem_Actions.inputactions";
            serializedModule.FindProperty("m_ActionsAsset").objectReferenceValue = AssetDatabase.LoadAssetAtPath<InputActionAsset>(actionsPath);
            var references = AssetDatabase.LoadAllAssetsAtPath(actionsPath).OfType<InputActionReference>().ToArray();
            var bindings = new[] { ("m_PointAction", "Point"), ("m_MoveAction", "Navigate"), ("m_SubmitAction", "Submit"), ("m_CancelAction", "Cancel"), ("m_LeftClickAction", "Click"), ("m_ScrollWheelAction", "ScrollWheel") };
            foreach (var binding in bindings)
                serializedModule.FindProperty(binding.Item1).objectReferenceValue = references.First(r => r.action.actionMap.name == "UI" && r.action.name == binding.Item2);
            serializedModule.ApplyModifiedPropertiesWithoutUndo();
            eventSystem.SetActive(true);

            if (name == "Restaurant")
            {
                var layout = new GameObject("LayoutAnchors");
                AddAnchor(layout.transform, "GuestEntrance", new Vector3(-4, 0, -2));
                AddAnchor(layout.transform, "GreetingPoint", new Vector3(-2, 0, -1));
                AddAnchor(layout.transform, "Seat01", new Vector3(-2, 0, 2));
                AddAnchor(layout.transform, "Seat02", new Vector3(0, 0, 2));
                AddAnchor(layout.transform, "Seat03", new Vector3(2, 0, 2));
                AddAnchor(layout.transform, "Kitchen", new Vector3(4, 0, 0));
                AddAnchor(layout.transform, "StairsExit", new Vector3(4, 0, 4));
            }

            EditorSceneManager.SaveScene(scene, SceneFolder + name + ".unity");
        }

        private static GameObject CreateText(Transform parent, string name, string content, Font font, float y, int size)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var rect = go.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(1100, 100);
            rect.anchoredPosition = new Vector2(0, y);
            var text = go.GetComponent<Text>();
            text.text = content;
            text.font = font;
            text.fontSize = size;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            text.raycastTarget = false;
            return go;
        }

        private static void AddAnchor(Transform parent, string name, Vector3 position)
        {
            var anchor = new GameObject(name);
            anchor.transform.SetParent(parent, false);
            anchor.transform.localPosition = position;
        }

        /// <summary>씬 등록·카메라·입력·버튼 연결을 검사한다. 게임 기능 완료를 판정하지 않는다.</summary>
        public static void Validate()
        {
            if (EditorBuildSettings.scenes.Length != SceneNames.Length)
                throw new InvalidOperationException("Build Settings 씬 수 불일치");
            foreach (var name in SceneNames)
            {
                var scene = EditorSceneManager.OpenScene(SceneFolder + name + ".unity");
                var objects = scene.GetRootGameObjects();
                if (!objects.SelectMany(o => o.GetComponentsInChildren<Camera>()).Any())
                    throw new InvalidOperationException("카메라 누락: " + name);
                if (!objects.SelectMany(o => o.GetComponentsInChildren<InputSystemUIInputModule>()).Any())
                    throw new InvalidOperationException("입력 모듈 누락: " + name);
                foreach (var button in objects.SelectMany(o => o.GetComponentsInChildren<Button>()))
                    if (button.onClick.GetPersistentEventCount() != 1 || button.onClick.GetPersistentTarget(0) == null)
                        throw new InvalidOperationException("버튼 연결 누락: " + name);
            }
            EditorSceneManager.OpenScene(SceneFolder + "Title.unity");
            Debug.Log("DOB_FOUNDATION_VALIDATED: 3 scenes, cameras, input modules and navigation bindings.");
        }

        /// <summary>등록된 세 씬으로 Windows 실행 빌드를 만들어 초기 프로젝트 구성을 검증한다.</summary>
        public static void BuildWindows()
        {
            Validate();
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
                locationPathName = "Build/Smoke/DOB.exe",
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.Development
            });
            if (report.summary.result != BuildResult.Succeeded)
                throw new InvalidOperationException("Windows 빌드 실패: " + report.summary.result);
            Debug.Log("DOB_WINDOWS_BUILD_SUCCEEDED");
        }

        /// <summary>모델 교체와 기능 연결 영역이 분리된 캐릭터 프리팹 골격을 생성한다.</summary>
        [MenuItem("DOB/Setup/Create Character Shells")]
        public static void CreateCharacterShells()
        {
            const string folder = "Assets/_DOB/03.Prefabs/Characters/";
            foreach (var kind in new[] { "Player", "Guest", "Enemy" })
                if (File.Exists(folder + "PF_" + kind + ".prefab"))
                    throw new InvalidOperationException("기존 프리팹 보존: " + kind);

            foreach (var kind in new[] { "Player", "Guest", "Enemy" })
            {
                var root = new GameObject("PF_" + kind);
                if (kind == "Player")
                {
                    var controller = root.AddComponent<CharacterController>();
                    controller.height = 1.8f;
                    controller.radius = 0.3f;
                    controller.center = new Vector3(0, 0.9f, 0);
                }
                else
                {
                    var collider = root.AddComponent<CapsuleCollider>();
                    collider.height = 1.8f;
                    collider.radius = 0.3f;
                    collider.center = new Vector3(0, 0.9f, 0);
                }
                var visual = new GameObject("VisualRoot");
                visual.transform.SetParent(root.transform, false);
                var slot = new GameObject("ModelSlot");
                slot.transform.SetParent(visual.transform, false);
                var placeholder = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                placeholder.name = "Placeholder";
                placeholder.transform.SetParent(slot.transform, false);
                placeholder.transform.localPosition = new Vector3(0, 0.9f, 0);
                placeholder.transform.localScale = new Vector3(0.6f, 0.9f, 0.6f);
                UnityEngine.Object.DestroyImmediate(placeholder.GetComponent<Collider>());
                var anchors = new GameObject("Anchors");
                anchors.transform.SetParent(root.transform, false);
                AddAnchor(anchors.transform, "InteractionOrigin", new Vector3(0, 1, 0.5f));
                AddAnchor(anchors.transform, "FacePoint", new Vector3(0, 1.55f, 0));
                AddAnchor(anchors.transform, "PlatePoint", new Vector3(0, 0.9f, 0.45f));
                AddAnchor(anchors.transform, "CameraTarget", new Vector3(0, 1.5f, 0));
                PrefabUtility.SaveAsPrefabAsset(root, folder + root.name + ".prefab");
                UnityEngine.Object.DestroyImmediate(root);
            }
            AssetDatabase.SaveAssets();
            Debug.Log("DOB_CHARACTER_SHELLS_CREATED: Player, Guest, Enemy (expansion shell only).");
        }
    }
}
