using System;
using System.IO;
using System.Linq;
using DOB.Define;
using DOB.Manager;
using DOB.UI;
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
    /// <summary>전환·식당 배경·개발 검증 씬의 골격을 만들고, 타이틀과 영업 씬(Basement·BasementUI) 구성을 검사한다.</summary>
    public static class SceneLayoutSetup
    {
        private const string SceneFolder = "Assets/_DOB/01.Scenes/";
        private const string CharacterFolder = "Assets/_DOB/03.Prefabs/Characters/";
        private const string FontPath = "Assets/_DOB/07.UI/Fonts/Gaegu-Regular.ttf";
        private const string LoadingPrefabPath = "Assets/_DOB/03.Prefabs/UI/PF_LoadingScreen.prefab";
        private const string LoadingContentPath = "Assets/_DOB/08.Data/LoadingScreenContent.asset";
        private static readonly string[] AllScenes = SceneComposition.RuntimeSceneNames.Concat(new[] { SceneComposition.CharacterTest }).ToArray();
        private static readonly string[] AuthoredScenes = {
            SceneComposition.Title, BasementComposition.BaseScene, BasementComposition.RestaurantScene, BasementComposition.UiScene
        };

        /// <summary>이미 작성한 씬을 덮어쓰지 않는 최초 생성 진입점.</summary>
        public static void Create()
        {
            foreach (var name in AllScenes.Except(AuthoredScenes))
                Require(!File.Exists(ScenePath(name)), "기존 씬 보존: " + name);
            Rebuild();
        }

        /// <summary>배경·전환용 골격을 다시 작성하고, 타이틀과 첫 영업 씬은 기존 에셋을 등록한다.</summary>
        public static void Rebuild()
        {
            Require(!EditorApplication.isPlaying, "플레이 중에는 씬을 재구성할 수 없습니다.");
            foreach (var name in AuthoredScenes)
                Require(File.Exists(ScenePath(name)), "직접 작성한 씬 누락: " + name);
            foreach (var kind in new[] { "Player", "Guest", "Enemy" })
                Require(AssetDatabase.LoadAssetAtPath<GameObject>(CharacterFolder + "PF_" + kind + ".prefab") != null, "공용 캐릭터 프리팹 누락: " + kind);
            Require(AssetDatabase.LoadAssetAtPath<Font>(FontPath) != null, "공용 한글 폰트 누락");

            // 같은 경로에 저장하여 기존 세 씬의 .meta/GUID를 유지한다. 프리팹 원본은 수정하지 않는다.
            PlayerSettings.companyName = "DOB Team";
            PlayerSettings.productName = "다녀왔어: 층계참 식당";
            EditorSettings.serializationMode = SerializationMode.ForceText;
            CreateLoading();
            CreateRestaurant();
            CreateCharacterTest();
            EditorBuildSettings.scenes = SceneComposition.RuntimeSceneNames.Select(n => new EditorBuildSettingsScene(ScenePath(n), true)).ToArray();
            AssetDatabase.SaveAssets();
            Validate();
            Debug.Log($"DOB_SCENE_LAYOUT_CREATED: {SceneComposition.RuntimeSceneNames.Count} runtime build scenes.");
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

        private static void CreateLoading()
        {
            var scene = NewScene();
            // 목표 씬의 오디오 리스너가 활성화돼도 전환 중 리스너가 겹치지 않게 한다.
            CreateCamera(Vector3.zero, Vector3.zero, false);
            PlaceLoadingScreen(scene);
            Save(scene, SceneComposition.Loading);
        }

        /// <summary>기존 Loading 씬의 예전 회색 진행 화면을 PF_LoadingScreen과 Presenter로 바꾼다. 카메라는 유지한다.</summary>
        [MenuItem("DOB/Scenes/Upgrade Loading Screen")]
        public static void UpgradeLoadingScene()
        {
            Require(!EditorApplication.isPlaying, "플레이 중에는 씬을 바꿀 수 없습니다.");
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;
            var scene = EditorSceneManager.OpenScene(ScenePath(SceneComposition.Loading), OpenSceneMode.Single);
            foreach (var root in scene.GetRootGameObjects())
                if (root.name == "LoadingCanvas")
                    UnityEngine.Object.DestroyImmediate(root);
            if (Components<LoadingScreenPresenter>(scene).Length == 0)
                PlaceLoadingScreen(scene);
            Save(scene, SceneComposition.Loading);
            Debug.Log("DOB_LOADING_SCREEN_UPGRADED");
        }

        private static void PlaceLoadingScreen(Scene scene)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(LoadingPrefabPath);
            var content = AssetDatabase.LoadAssetAtPath<LoadingScreenContent>(LoadingContentPath);
            Require(prefab != null && prefab.GetComponent<LoadingScreenView>() != null, "로딩 화면 프리팹 누락: " + LoadingPrefabPath);
            Require(content != null, "로딩 화면 내용 데이터 누락: " + LoadingContentPath);
            var screen = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);
            // 프리팹에 컴포넌트를 덧붙이지 않도록 Presenter는 별도 루트에 둔다.
            var presenterObject = new GameObject("LoadingScreenPresenter");
            SceneManager.MoveGameObjectToScene(presenterObject, scene);
            presenterObject.AddComponent<LoadingScreenPresenter>().Configure(screen.GetComponent<LoadingScreenView>(), content);
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
                OpenComposition();
        }

        [MenuItem("DOB/Scenes/Open Character Test")]
        public static void OpenCharacterTest()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(ScenePath(SceneComposition.CharacterTest));
        }

        // 영업 조합: 식당 공간 + 실행(Basement) + 화면(BasementUI). BasementComposition이 플레이 중 여는 조합과 같다.
        private static void OpenComposition()
        {
            var restaurant = EditorSceneManager.OpenScene(ScenePath(SceneComposition.Restaurant), OpenSceneMode.Single);
            EditorSceneManager.OpenScene(ScenePath(BasementComposition.BaseScene), OpenSceneMode.Additive);
            EditorSceneManager.OpenScene(ScenePath(BasementComposition.UiScene), OpenSceneMode.Additive);
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
                var needsCamera = name != SceneComposition.Restaurant && name != BasementComposition.UiScene;
                var needsInput = name == SceneComposition.Title || name == BasementComposition.UiScene;
                Require(cameras == (needsCamera ? 1 : 0), "카메라 역할 불일치: " + name);
                Require(inputs == (needsInput ? 1 : 0), "입력 역할 불일치: " + name);
                foreach (var module in Components<InputSystemUIInputModule>(scene))
                    Require(module.actionsAsset != null && module.point != null && module.leftClick != null && module.submit != null, "프로젝트 입력 연결 누락: " + name);
                if (name == SceneComposition.Title)
                {
                    var presenter = Components<TitlePresenter>(scene).SingleOrDefault();
                    Require(presenter != null, "타이틀 Presenter 누락");
                    var view = new SerializedObject(presenter).FindProperty("view").objectReferenceValue as TitleView;
                    Require(view != null, "타이틀 화면 연결 누락");
                    var menu = new SerializedObject(view);
                    foreach (var field in new[] { "newGameButton", "continueButton" })
                    {
                        var button = menu.FindProperty(field).objectReferenceValue as Button;
                        Require(button && button.onClick.GetPersistentEventCount() == 0, "타이틀 버튼 누락 또는 중복 전환: " + field);
                    }
                }
                else if (name == BasementComposition.UiScene)
                    Require(Components<BasementHud>(scene).SingleOrDefault()?.IsReady == true, "영업 UI 연결 누락");
                else
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
                if (name == BasementComposition.BaseScene)
                {
                    Require(scene.GetRootGameObjects().Any(o => o.name == "RuntimeCharacters") && scene.GetRootGameObjects().Any(o => o.name == "Systems"), "Basement 실행 영역 누락");
                    ValidateCharacter(scene, "Player");
                    ValidateCharacter(scene, "Guest");
                }
                if (name == SceneComposition.Loading)
                {
                    Require(Components<AudioListener>(scene).Length == 0, "Loading에 중복 가능 AudioListener 포함");
                    var presenter = Components<LoadingScreenPresenter>(scene).SingleOrDefault();
                    Require(presenter != null, "Loading 표시 연결 누락");
                    var serialized = new SerializedObject(presenter);
                    var screen = serialized.FindProperty("view").objectReferenceValue as LoadingScreenView;
                    Require(screen != null && screen.gameObject.scene == scene, "Loading 화면 연결 누락");
                    Require(serialized.FindProperty("content").objectReferenceValue != null, "Loading 내용 데이터 연결 누락");
                    Require(Components<LoadingScreenView>(scene).Length == 1, "Loading 화면 중복 또는 누락");
                }
                if (name == SceneComposition.CharacterTest)
                    foreach (var kind in new[] { "Player", "Guest", "Enemy" })
                        ValidateCharacter(scene, kind);
            }
            ValidateComposition();
            EditorSceneManager.OpenScene(ScenePath(SceneComposition.Title));
            Debug.Log($"DOB_SCENE_COMPOSITION_VALIDATED: {AllScenes.Length} scenes; {buildScenes.Length} runtime build entries.");
        }

        private static void ValidateComposition()
        {
            OpenComposition();
            Require(SceneManager.sceneCount == 3, "편집 조합 씬 수 불일치");
            Require(SceneManager.GetActiveScene().name == SceneComposition.Restaurant, "식당 활성 씬 불일치");
            var roots = Enumerable.Range(0, SceneManager.sceneCount).SelectMany(i => SceneManager.GetSceneAt(i).GetRootGameObjects()).ToArray();
            Require(roots.SelectMany(o => o.GetComponentsInChildren<Camera>(true)).Count() == 1, "조합에 카메라 중복 또는 누락");
            Require(roots.SelectMany(o => o.GetComponentsInChildren<AudioListener>(true)).Count() == 1, "조합에 AudioListener 중복 또는 누락");
            Require(roots.SelectMany(o => o.GetComponentsInChildren<EventSystem>(true)).Count() == 1, "조합에 EventSystem 중복 또는 누락");
            var basement = SceneManager.GetSceneByName(BasementComposition.BaseScene);
            ValidateCharacter(basement, "Player");
            ValidateCharacter(basement, "Guest");
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
