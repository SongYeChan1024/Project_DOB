using System;
using System.IO;
using System.Linq;
using DOB.Manager;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DOB.Editor
{
    /// <summary>배경·실행·튜토리얼·개발 검증 역할을 구분해 승인된 씬 골격을 만든다.</summary>
    public static class SceneLayoutSetup
    {
        private const string SceneFolder = "Assets/_DOB/01.Scenes/";
        private const string CharacterFolder = "Assets/_DOB/03.Prefabs/Characters/";
        private const string FontPath = "Assets/_DOB/07.UI/Fonts/Gaegu-Regular.ttf";
        private static readonly string[] AllScenes = SceneComposition.RuntimeSceneNames.Concat(new[] { SceneComposition.CharacterTest }).ToArray();

        /// <summary>이미 작성한 씬을 덮어쓰지 않는 최초 생성 진입점.</summary>
        public static void Create()
        {
            foreach (var name in AllScenes)
                Require(!File.Exists(ScenePath(name)), "기존 씬 보존: " + name);
            Rebuild();
        }

        /// <summary>백업한 프로젝트에서 명시적으로 호출해 일곱 지정 씬만 다시 작성한다.</summary>
        public static void Rebuild()
        {
            Require(!EditorApplication.isPlaying, "플레이 중에는 씬을 재구성할 수 없습니다.");
            foreach (var kind in new[] { "Player", "Guest", "Enemy" })
                Require(AssetDatabase.LoadAssetAtPath<GameObject>(CharacterFolder + "PF_" + kind + ".prefab") != null, "공용 캐릭터 프리팹 누락: " + kind);
            Require(AssetDatabase.LoadAssetAtPath<Font>(FontPath) != null, "공용 한글 폰트 누락");

            // 같은 경로에 저장하여 기존 세 씬의 .meta/GUID를 유지한다. 프리팹 원본은 수정하지 않는다.
            PlayerSettings.companyName = "DOB Team";
            PlayerSettings.productName = "다녀왔어: 층계참 식당";
            EditorSettings.serializationMode = SerializationMode.ForceText;
            CreateScreen(SceneComposition.Title, "다녀왔어: 층계참 식당", new[] { ("식당으로", SceneComposition.Restaurant), ("튜토리얼 시작", SceneComposition.Tutorial) });
            CreateLoading();
            CreateRestaurant();
            CreateGameplay();
            CreateTutorial();
            CreateScreen(SceneComposition.ReturnReport, "아침 귀환 보고", new[] { ("식당으로", SceneComposition.Restaurant), ("타이틀로", SceneComposition.Title) });
            CreateCharacterTest();
            EditorBuildSettings.scenes = SceneComposition.RuntimeSceneNames.Select(n => new EditorBuildSettingsScene(ScenePath(n), true)).ToArray();
            AssetDatabase.SaveAssets();
            Validate();
            Debug.Log("DOB_SCENE_LAYOUT_CREATED: 7 scenes, 6 runtime build scenes; CharacterTest is editor-only.");
        }

        private static Scene NewScene()
        {
            return EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void Save(Scene scene, string name)
        {
            Require(EditorSceneManager.SaveScene(scene, ScenePath(name)), "씬 저장 실패: " + name);
        }

        private static string ScenePath(string name) => SceneFolder + name + ".unity";

        private static void CreateScreen(string name, string title, (string label, string route)[] destinations)
        {
            var scene = NewScene();
            CreateCamera(new Vector3(0, 8, -8), new Vector3(35, 0, 0));
            var canvas = CreateCanvas("NavigationCanvas");
            CreateText(canvas.transform, "SceneTitle", title, new Vector2(0, 220), new Vector2(1200, 90), 48);
            CreateText(canvas.transform, "DevelopmentNotice", "개발 중 · 씬 연결 확인용 화면", new Vector2(0, 150), new Vector2(1100, 70), 26);
            CreateButtons(canvas, destinations, new Vector2(0, 30));
            CreateEventSystem();
            Save(scene, name);
        }

        private static void CreateLoading()
        {
            var scene = NewScene();
            // 목표 씬의 오디오 리스너가 활성화돼도 전환 중 리스너가 겹치지 않게 한다.
            CreateCamera(Vector3.zero, Vector3.zero, false);
            var canvas = CreateCanvas("LoadingCanvas", 1000);
            var background = new GameObject("Background", typeof(RectTransform), typeof(Image));
            background.transform.SetParent(canvas.transform, false);
            var backgroundRect = background.GetComponent<RectTransform>();
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.anchorMax = Vector2.one;
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;
            background.GetComponent<Image>().color = new Color(0.035f, 0.045f, 0.06f, 1);
            CreateText(canvas.transform, "LoadingTitle", "불러오는 중", new Vector2(0, 85), new Vector2(900, 90), 42);
            var label = CreateText(canvas.transform, "ProgressText", "0%", Vector2.zero, new Vector2(900, 60), 30);
            var barObject = new GameObject("ProgressBar", typeof(RectTransform), typeof(Image), typeof(Slider));
            barObject.transform.SetParent(canvas.transform, false);
            var barRect = barObject.GetComponent<RectTransform>();
            barRect.sizeDelta = new Vector2(600, 26);
            barRect.anchoredPosition = new Vector2(0, -65);
            barObject.GetComponent<Image>().color = new Color(0.13f, 0.16f, 0.20f);
            var fill = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            fill.transform.SetParent(barObject.transform, false);
            var fillRect = fill.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            fill.GetComponent<Image>().color = new Color(0.50f, 0.72f, 0.64f);
            var bar = barObject.GetComponent<Slider>();
            bar.fillRect = fillRect;
            bar.targetGraphic = fill.GetComponent<Image>();
            bar.minValue = 0;
            bar.maxValue = 1;
            bar.value = 0;
            bar.interactable = false;
            bar.transition = Selectable.Transition.None;
            canvas.AddComponent<LoadingScreenPresenter>().Configure(label, bar);
            Save(scene, SceneComposition.Loading);
        }

        private static void CreateRestaurant()
        {
            var scene = NewScene();
            var environment = new GameObject("Environment");
            CreateFloor(environment.transform, new Vector3(1.2f, 1, 1.2f));
            CreateLight(environment.transform);
            new GameObject("ArtPlacement").transform.SetParent(environment.transform, false);
            var layout = new GameObject("LayoutAnchors");
            AddAnchor(layout.transform, "PlayerSpawn", new Vector3(0, 0, -1));
            AddAnchor(layout.transform, "GuestEntrance", new Vector3(-4, 0, -2));
            AddAnchor(layout.transform, "GreetingPoint", new Vector3(-2, 0, -1));
            AddAnchor(layout.transform, "Seat01", new Vector3(-2, 0, 2));
            AddAnchor(layout.transform, "Seat02", new Vector3(0, 0, 2));
            AddAnchor(layout.transform, "Seat03", new Vector3(2, 0, 2));
            AddAnchor(layout.transform, "Kitchen", new Vector3(4, 0, 0));
            AddAnchor(layout.transform, "StairsExit", new Vector3(4, 0, 4));
            Save(scene, SceneComposition.Restaurant);
        }

        private static void CreateGameplay()
        {
            var scene = NewScene();
            var characters = new GameObject("RuntimeCharacters");
            InstantiateCharacter("Player", characters.transform, new Vector3(0, 0, -1));
            InstantiateCharacter("Guest", characters.transform, new Vector3(-2, 0, -1));
            var systems = new GameObject("Systems");
            // 후속 기능의 연결 위치만 마련한다. 영업·조리·정산 컴포넌트는 아직 구현하지 않았다.
            AddAnchor(systems.transform, "SessionConnections", Vector3.zero);
            AddAnchor(systems.transform, "GuestFlowConnections", Vector3.zero);
            AddAnchor(systems.transform, "CookingConnections", Vector3.zero);
            CreateCamera(new Vector3(0, 8, -8), new Vector3(35, 0, 0));
            var canvas = CreateCanvas("GameplayCanvas");
            CreateText(canvas.transform, "SceneTitle", "층계참 식당", new Vector2(0, 470), new Vector2(1000, 70), 38);
            CreateText(canvas.transform, "DevelopmentNotice", "개발 중 · 캐릭터는 공용 프리팹 골격입니다", new Vector2(0, 415), new Vector2(1200, 65), 24);
            CreateButtons(canvas, new[] { ("귀환 보고로", SceneComposition.ReturnReport), ("타이틀로", SceneComposition.Title) }, new Vector2(690, -290));
            CreateEventSystem();
            Save(scene, SceneComposition.Gameplay);
        }

        private static void CreateTutorial()
        {
            var scene = NewScene();
            var flow = new GameObject("TutorialFlow");
            for (var index = 1; index <= 3; index++)
                AddAnchor(flow.transform, "TutorialGuest" + index.ToString("00"), Vector3.zero);
            var canvas = CreateCanvas("TutorialCanvas", 100);
            CreateText(canvas.transform, "TutorialTitle", "첫 영업 안내", new Vector2(0, 285), new Vector2(1100, 75), 36);
            CreateText(canvas.transform, "DevelopmentNotice", "개발 중 · 첫 손님 세 명의 단계·안내 연결 영역", new Vector2(0, 225), new Vector2(1300, 65), 24);
            CreateButtons(canvas, new[] { ("일반 영업으로", SceneComposition.Restaurant) }, new Vector2(-650, -400));
            Save(scene, SceneComposition.Tutorial);
        }

        private static void CreateCharacterTest()
        {
            var scene = NewScene();
            CreateFloor(null, Vector3.one);
            CreateLight(null);
            CreateCamera(new Vector3(0, 6, -7), new Vector3(32, 0, 0));
            var characters = new GameObject("CharacterSamples");
            InstantiateCharacter("Player", characters.transform, new Vector3(-2, 0, 0));
            InstantiateCharacter("Guest", characters.transform, Vector3.zero);
            InstantiateCharacter("Enemy", characters.transform, new Vector3(2, 0, 0));
            var canvas = CreateCanvas("CharacterTestCanvas");
            CreateText(canvas.transform, "DevelopmentNotice", "캐릭터 프리팹 검증용 · 이동·AI 미구현 · 플레이어 빌드 제외", new Vector2(0, 430), new Vector2(1500, 80), 28);
            Save(scene, SceneComposition.CharacterTest);
        }

        private static void CreateCamera(Vector3 position, Vector3 euler, bool audioListener = true)
        {
            var cameraObject = new GameObject("Main Camera", typeof(Camera));
            if (audioListener)
                cameraObject.AddComponent<AudioListener>();
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = position;
            cameraObject.transform.rotation = Quaternion.Euler(euler);
            var camera = cameraObject.GetComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.07f, 0.09f, 0.12f);
        }

        private static void CreateLight(Transform parent)
        {
            var lightObject = new GameObject("Directional Light", typeof(Light));
            lightObject.transform.SetParent(parent, false);
            lightObject.transform.rotation = Quaternion.Euler(45, -30, 0);
            var light = lightObject.GetComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.3f;
        }

        private static void CreateFloor(Transform parent, Vector3 scale)
        {
            var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Floor";
            floor.transform.SetParent(parent, false);
            floor.transform.localScale = scale;
        }

        private static void InstantiateCharacter(string kind, Transform parent, Vector3 position)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(CharacterFolder + "PF_" + kind + ".prefab");
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent.gameObject.scene);
            instance.transform.SetParent(parent, false);
            instance.transform.position = position;
        }

        private static GameObject CreateCanvas(string name, int sortingOrder = 0)
        {
            var canvasObject = new GameObject(name, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;
            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            return canvasObject;
        }

        private static Text CreateText(Transform parent, string name, string content, Vector2 position, Vector2 size, int fontSize)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var rect = go.GetComponent<RectTransform>();
            rect.sizeDelta = size;
            rect.anchoredPosition = position;
            var text = go.GetComponent<Text>();
            text.text = content;
            text.font = AssetDatabase.LoadAssetAtPath<Font>(FontPath);
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            text.raycastTarget = false;
            return text;
        }

        private static void CreateButtons(GameObject canvas, (string label, string route)[] destinations, Vector2 firstPosition)
        {
            var presenter = canvas.AddComponent<SceneNavigationPresenter>();
            for (var index = 0; index < destinations.Length; index++)
            {
                var target = destinations[index];
                var buttonObject = new GameObject("Load_" + target.route, typeof(RectTransform), typeof(Image), typeof(Button));
                buttonObject.transform.SetParent(canvas.transform, false);
                var rect = buttonObject.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(400, 80);
                rect.anchoredPosition = firstPosition + new Vector2(0, -index * 110);
                buttonObject.GetComponent<Image>().color = new Color(0.20f, 0.27f, 0.31f);
                CreateText(buttonObject.transform, "Label", target.label, Vector2.zero, new Vector2(380, 70), 30);
                UnityEventTools.AddStringPersistentListener(buttonObject.GetComponent<Button>().onClick, presenter.LoadScene, target.route);
            }
        }

        private static void CreateEventSystem()
        {
            // 일시 기본 액션 대신 프로젝트 입력 에셋을 직렬화해 씬 전환 뒤에도 참조를 유지한다.
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
        }

        private static void AddAnchor(Transform parent, string name, Vector3 position)
        {
            var anchor = new GameObject(name);
            anchor.transform.SetParent(parent, false);
            anchor.transform.localPosition = position;
        }

        [MenuItem("DOB/Scenes/Open Restaurant Composition")]
        public static void OpenRestaurantComposition()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                OpenComposition(false);
        }

        [MenuItem("DOB/Scenes/Open Tutorial Composition")]
        public static void OpenTutorialComposition()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                OpenComposition(true);
        }

        [MenuItem("DOB/Scenes/Open Character Test")]
        public static void OpenCharacterTest()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(ScenePath(SceneComposition.CharacterTest));
        }

        private static void OpenComposition(bool tutorial)
        {
            var restaurant = EditorSceneManager.OpenScene(ScenePath(SceneComposition.Restaurant), OpenSceneMode.Single);
            EditorSceneManager.OpenScene(ScenePath(SceneComposition.Gameplay), OpenSceneMode.Additive);
            if (tutorial)
                EditorSceneManager.OpenScene(ScenePath(SceneComposition.Tutorial), OpenSceneMode.Additive);
            SceneManager.SetActiveScene(restaurant);
        }

        /// <summary>생성 결과의 역할·공용 프리팹·입력·UI 연결과 실제 편집 조합을 검사한다.</summary>
        public static void Validate()
        {
            var buildScenes = EditorBuildSettings.scenes;
            Require(buildScenes.Length == SceneComposition.RuntimeSceneNames.Count, "런타임 빌드 씬 수 불일치");
            Require(buildScenes.All(s => s.enabled), "비활성 빌드 씬 발견");
            Require(buildScenes.Select(s => s.path).SequenceEqual(SceneComposition.RuntimeSceneNames.Select(ScenePath)), "빌드 씬 목록·순서 불일치 또는 CharacterTest 포함");
            foreach (var name in AllScenes)
            {
                Require(File.Exists(ScenePath(name)) && File.Exists(ScenePath(name) + ".meta"), "씬 또는 메타 누락: " + name);
                var scene = EditorSceneManager.OpenScene(ScenePath(name));
                var cameras = Components<Camera>(scene).Length;
                var inputs = Components<EventSystem>(scene).Length;
                var needsCamera = name != SceneComposition.Restaurant && name != SceneComposition.Tutorial;
                var needsInput = name == SceneComposition.Title || name == SceneComposition.Gameplay || name == SceneComposition.ReturnReport;
                Require(cameras == (needsCamera ? 1 : 0), "카메라 역할 불일치: " + name);
                Require(inputs == (needsInput ? 1 : 0), "입력 역할 불일치: " + name);
                foreach (var module in Components<InputSystemUIInputModule>(scene))
                    Require(module.actionsAsset != null && module.point != null && module.leftClick != null && module.submit != null, "프로젝트 입력 연결 누락: " + name);
                foreach (var button in Components<Button>(scene))
                    Require(button.onClick.GetPersistentEventCount() == 1 && button.onClick.GetPersistentTarget(0) != null, "버튼 연결 누락: " + name);

                if (name == SceneComposition.Restaurant)
                {
                    Require(Components<Canvas>(scene).Length == 0, "Restaurant에 실행 UI가 포함됨");
                    var anchors = scene.GetRootGameObjects().SingleOrDefault(o => o.name == "LayoutAnchors");
                    Require(anchors != null, "식당 기준점 루트 누락");
                    foreach (var anchor in new[] { "PlayerSpawn", "GuestEntrance", "GreetingPoint", "Seat01", "Seat02", "Seat03", "Kitchen", "StairsExit" })
                        Require(anchors.transform.Find(anchor) != null, "식당 기준점 누락: " + anchor);
                }
                if (name == SceneComposition.Gameplay)
                {
                    Require(scene.GetRootGameObjects().Any(o => o.name == "RuntimeCharacters") && scene.GetRootGameObjects().Any(o => o.name == "Systems"), "Gameplay 실행 영역 누락");
                    ValidateCharacter(scene, "Player");
                    ValidateCharacter(scene, "Guest");
                }
                if (name == SceneComposition.Tutorial)
                {
                    var flow = scene.GetRootGameObjects().SingleOrDefault(o => o.name == "TutorialFlow");
                    Require(flow != null && flow.transform.childCount == 3, "튜토리얼 첫 손님 세 명의 연결 영역 누락");
                    Require(Components<AudioListener>(scene).Length == 0, "튜토리얼에 중복 AudioListener 포함");
                }
                if (name == SceneComposition.Loading)
                {
                    Require(Components<AudioListener>(scene).Length == 0, "Loading에 중복 가능 AudioListener 포함");
                    var presenter = Components<LoadingScreenPresenter>(scene).SingleOrDefault();
                    Require(presenter != null, "Loading 표시 연결 누락");
                    var serialized = new SerializedObject(presenter);
                    Require(serialized.FindProperty("progressText").objectReferenceValue != null && serialized.FindProperty("progressBar").objectReferenceValue != null, "Loading 진행률 참조 누락");
                }
                if (name == SceneComposition.CharacterTest)
                    foreach (var kind in new[] { "Player", "Guest", "Enemy" })
                        ValidateCharacter(scene, kind);
            }
            ValidateComposition(false);
            ValidateComposition(true);
            EditorSceneManager.OpenScene(ScenePath(SceneComposition.Title));
            Debug.Log("DOB_SCENE_COMPOSITION_VALIDATED: 7 scenes; 6 runtime build entries; normal/tutorial compositions, shared prefabs and navigation bindings.");
        }

        private static void ValidateComposition(bool tutorial)
        {
            OpenComposition(tutorial);
            Require(SceneManager.sceneCount == (tutorial ? 3 : 2), "편집 조합 씬 수 불일치");
            Require(SceneManager.GetActiveScene().name == SceneComposition.Restaurant, "식당 활성 씬 불일치");
            var roots = Enumerable.Range(0, SceneManager.sceneCount).SelectMany(i => SceneManager.GetSceneAt(i).GetRootGameObjects()).ToArray();
            Require(roots.SelectMany(o => o.GetComponentsInChildren<Camera>(true)).Count() == 1, "조합에 카메라 중복 또는 누락");
            Require(roots.SelectMany(o => o.GetComponentsInChildren<AudioListener>(true)).Count() == 1, "조합에 AudioListener 중복 또는 누락");
            Require(roots.SelectMany(o => o.GetComponentsInChildren<EventSystem>(true)).Count() == 1, "조합에 EventSystem 중복 또는 누락");
            var gameplay = SceneManager.GetSceneByName(SceneComposition.Gameplay);
            ValidateCharacter(gameplay, "Player");
            ValidateCharacter(gameplay, "Guest");
        }

        private static T[] Components<T>(Scene scene) where T : Component
        {
            return scene.GetRootGameObjects().SelectMany(o => o.GetComponentsInChildren<T>(true)).ToArray();
        }

        private static void ValidateCharacter(Scene scene, string kind)
        {
            var instances = Components<Transform>(scene).Where(t => t.name == "PF_" + kind).Select(t => t.gameObject).ToArray();
            Require(instances.Length == 1, "캐릭터 인스턴스 중복 또는 누락: " + scene.name + "/" + kind);
            var instance = instances[0];
            Require(PrefabUtility.GetCorrespondingObjectFromSource(instance) == AssetDatabase.LoadAssetAtPath<GameObject>(CharacterFolder + "PF_" + kind + ".prefab"), "공용 프리팹 연결 누락: " + kind);
            Require(instance.transform.Find("VisualRoot/ModelSlot") != null && instance.transform.Find("Anchors") != null, "캐릭터 모델 슬롯·기준점 누락: " + kind);
        }

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
            Require(report.summary.result == BuildResult.Succeeded, "Windows 빌드 실패: " + report.summary.result);
            Debug.Log("DOB_WINDOWS_BUILD_SUCCEEDED");
        }

        private static void Require(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message);
        }
    }
}
