// 게임 베이스 진입 → 공용 로딩 화면 → 식당과 UI 추가 로드 → GameManager에 하루 준비 요청 → 영업 시작.
// 베이스가 중간에 종료되면 직접 연 씬과 진행 중이던 로드를 정리한다. 편집자가 함께 연 씬은 빌려 쓴다.
// 로딩 화면 표시(열기·진행률·최소 표시·페이드·실패 안내)는 UIManager에 요청한다. 타이틀 복귀는 SceneLoadService가 맡는다.
// 일차·저장·정산 같은 하루 진행은 GameManager가 맡고, 이 클래스는 씬 조립만 한다.
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DOB.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace DOB.Manager
{
    /// <summary>게임 카메라와 캐릭터를 소유한 베이스에 식당 레벨과 영업 화면을 조합한다.</summary>
    [DisallowMultipleComponent]
    public sealed class BasementComposition : MonoBehaviour
    {
        public const string BaseScene = "Basement";
        public const string TitleScene = SceneComposition.Title;
        // 아트가 배치한 식당 씬을 레벨로 쓴다. 배치 계약(RestaurantLayout)은 DOB > Basement > Restaurant 씬 연결 추가로 붙인다.
        public const string RestaurantScene = SceneComposition.Restaurant;
        public const string UiScene = "BasementUI";
        private const string SceneFolder = "Assets/_DOB/01.Scenes/";

        /// <summary>씬 조립이 끝난 뒤 하루를 진행할 같은 베이스의 GameManager.</summary>
        [SerializeField] private GameManager game;

        private static BasementComposition active;
        private static bool entering;
        private static FirstServiceSaveData pendingStart;
        private static bool pendingNewTimeline;
        // 게임 베이스 진입 때 보일 로딩 내용. LoadingScreenContent의 "층계참 식당" 항목을 쓴다.
        private const string LoadingDestination = SceneComposition.Restaurant;

        private readonly List<Scene> ownedScenes = new List<Scene>(3);
        private bool quitting;

        public bool IsReady { get; private set; }
        public GameManager Game => game;
        public static BasementComposition Current => active;
        /// <summary>다음 일차 또는 타이틀로 넘어가는 전환이 진행 중인지.</summary>
        public static bool IsEntering => entering;
        public static FirstServiceSaveStore OpenSaves()
        {
            var directory = Environment.GetEnvironmentVariable("DOB_BASEMENT_SAVE_DIR");
            return new FirstServiceSaveStore(string.IsNullOrWhiteSpace(directory) ? Path.Combine(Application.persistentDataPath, "Basement") : directory);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetEntry()
        {
            active = null;
            entering = false;
            pendingStart = null;
            pendingNewTimeline = false;
        }

        private IEnumerator Start()
        {
            if (active && active != this)
            {
                if (active.gameObject.scene != gameObject.scene)
                    SceneManager.UnloadSceneAsync(gameObject.scene);
                yield break;
            }

            active = this;
            entering = false;
            var start = pendingStart;
            pendingStart = null;
            bool newTimeline = pendingNewTimeline;
            pendingNewTimeline = false;
            if (!game)
            {
                Fail("게임 베이스에 GameManager가 연결되지 않았습니다.");
                yield break;
            }

            // 공용 로더가 같은 Loading 씬을 사용하는 동안 소유권을 먼저 가져오지 않는다.
            while (SceneLoadService.IsLoading)
                yield return null;

            // 각 미등록 프리뷰 씬은 에디터에서 에셋 경로로 연다. 플레이어는 해당 빌드의 씬 목록을 사용한다.
            foreach (string sceneName in new[] { SceneComposition.Loading, RestaurantScene, UiScene })
            {
                if (GetScene(sceneName).isLoaded || CanLoad(sceneName))
                    continue;
                Fail("읽을 수 없는 씬입니다. " + sceneName);
                yield break;
            }

            yield return LoadIfNeeded(SceneComposition.Loading, 0);
            var loading = GetScene(SceneComposition.Loading);
            if (!loading.isLoaded)
            {
                Fail("로딩 화면을 열 수 없습니다.");
                yield break;
            }

            // 미리 열어 둔 경우에도 로딩 화면은 이번 입장 동안만 사용한다.
            if (!ownedScenes.Contains(loading))
                ownedScenes.Add(loading);
            // 에디터에서 베이스 씬을 직접 실행하면 타이틀의 진입 요청을 거치지 않으므로 여기서 화면을 연다.
            if (!UIManager.IsLoadingOpen)
                UIManager.OpenLoading(LoadingDestination);
            yield return LoadIfNeeded(RestaurantScene, 0);
            var level = GetScene(RestaurantScene);
            var layout = FindInScene<RestaurantLayout>(level);
            if (!layout || !layout.IsReady)
            {
                Fail("식당 레벨의 입구·좌석·주방 배치를 확인하세요.");
                yield break;
            }
            yield return LoadIfNeeded(UiScene, 1);
            var hud = FindInScene<BasementHud>(GetScene(UiScene));
            if (!hud || !hud.IsReady || !SceneManager.SetActiveScene(level))
            {
                Fail("식당 또는 영업 UI 연결을 확인하세요.");
                yield break;
            }

            if (!game.Prepare(OpenSaves(), start, newTimeline, hud))
            {
                Fail("GameManager의 응대·주방·관계·정산·경제·하루 진행 설정(정산의 귀환 보상 기준 포함) 또는 영업 UI의 일시 정지 메뉴·하루 진행 화면 연결을 확인하세요.");
                yield break;
            }
            ShowProgress(1f);
            // 최소 표시 시간·진행 바 채움·검은 페이드가 끝난 뒤에 로딩 화면을 닫아 식당이 갑자기 튀어나오지 않게 한다.
            yield return UIManager.CloseLoading();
            if (ownedScenes.Remove(loading))
                yield return SceneManager.UnloadSceneAsync(loading);

            game.StartService();
            IsReady = game.IsReady;
        }

        private static Scene GetScene(string name) => SceneManager.GetSceneByPath(SceneFolder + name + ".unity");

        /// <summary>타이틀 또는 명시적인 불러오기에서 공용 로딩 화면을 먼저 연 뒤 게임 베이스를 추가한다.</summary>
        public static bool EnterDay(FirstServiceSaveData start) =>
            // 스냅샷을 그대로 넘겨 던전 재료와 미처리 귀환 보고까지 GameManager가 받게 한다.
            BeginEntry(start, start == null || start.Slot == FirstServiceSaveSlot.Manual);

        private static bool BeginEntry(FirstServiceSaveData start, bool newTimeline)
        {
            if (entering || !CanLoad(RestaurantScene) || !CanLoad(UiScene))
                return false;
            pendingStart = start;
            pendingNewTimeline = newTimeline;
            return ThroughLoading(BaseScene, LoadSceneMode.Additive);
        }

        /// <summary>공용 전환으로 타이틀에 돌아간다. 로딩 화면의 최소 표시·페이드·실패 안내를 같이 쓴다.</summary>
        public static bool GoToTitle() => !entering && SceneLoadService.RequestLoad(TitleScene);

        private static bool ThroughLoading(string destination, LoadSceneMode mode)
        {
            if (SceneLoadService.IsLoading || !CanLoad(SceneComposition.Loading) || !CanLoad(destination))
            {
                pendingStart = null;
                pendingNewTimeline = false;
                return false;
            }
            entering = true;
            var loading = LoadScene(SceneComposition.Loading, LoadSceneMode.Single);
            if (loading == null)
            {
                entering = false;
                pendingStart = null;
                pendingNewTimeline = false;
                return false;
            }
            loading.completed += _ =>
            {
                UIManager.OpenLoading(LoadingDestination);
                var next = LoadScene(destination, mode);
                if (next != null)
                    return;
                entering = false;
                pendingStart = null;
                pendingNewTimeline = false;
                Debug.LogError("[DOB] 게임 베이스 씬 로드를 시작하지 못했습니다: " + destination);
                // 안내를 띄울 수 없으면 이전처럼 타이틀을 바로 연다.
                if (!UIManager.ShowLoadingFailure(true))
                    LoadScene(TitleScene, LoadSceneMode.Single);
            };
            return true;
        }

        private static bool CanLoad(string name)
        {
            var path = SceneFolder + name + ".unity";
            if (SceneUtility.GetBuildIndexByScenePath(path) >= 0)
                return true;
#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
#else
            return false;
#endif
        }

        private IEnumerator LoadIfNeeded(string name, int finished)
        {
            if (GetScene(name).isLoaded)
                yield break;

            var operation = LoadScene(name, LoadSceneMode.Additive);
            if (operation == null)
                yield break;

            // Unity의 씬 로드는 취소할 수 없다. 베이스가 먼저 닫힌 경우 완료 콜백이 해당 씬을 회수한다.
            operation.completed += _ =>
            {
                var scene = GetScene(name);
                if (!scene.isLoaded)
                    return;
                if (this && isActiveAndEnabled && active == this)
                {
                    if (!ownedScenes.Contains(scene)) ownedScenes.Add(scene);
                }
                else if (!quitting && !active && !entering && !SceneLoadService.IsLoading)
                    Unload(scene);
            };
            while (!operation.isDone)
            {
                ShowProgress((finished + operation.progress) / 2f);
                yield return null;
            }
        }

        private static AsyncOperation LoadScene(string name, LoadSceneMode mode)
        {
            string path = SceneFolder + name + ".unity";
            try
            {
#if UNITY_EDITOR
                if (SceneUtility.GetBuildIndexByScenePath(path) < 0)
                    return EditorSceneManager.LoadSceneAsyncInPlayMode(path, new LoadSceneParameters(mode));
#endif
                return SceneManager.LoadSceneAsync(path, mode);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        private static T FindInScene<T>(Scene scene) where T : Component
        {
            if (scene.isLoaded)
                foreach (var root in scene.GetRootGameObjects())
                {
                    var match = root.GetComponentInChildren<T>(true);
                    if (match) return match;
                }
            return null;
        }

        private static void ShowProgress(float value) => UIManager.SetLoadingProgress(value);

        private void Fail(string reason)
        {
            Debug.LogError("[DOB] " + reason, this);
            // 로딩 화면이 떠 있으면 실패 안내로 타이틀 복귀 길을 준다.
            // 안내가 떠 있는 Loading 씬은 아래 OnDisable에서 언로드하지 않고, 타이틀 복귀 전환이 함께 닫는다.
            if (UIManager.IsLoadingOpen && UIManager.ShowLoadingFailure(true))
                ownedScenes.Remove(GetScene(SceneComposition.Loading));
            enabled = false;
        }

        private void OnApplicationQuit() => quitting = true;

        private static void Unload(Scene scene)
        {
            if (scene.IsValid() && scene.isLoaded && SceneManager.loadedSceneCount > 1)
                SceneManager.UnloadSceneAsync(scene);
        }

        private void OnDisable()
        {
            if (active != this)
                return;
            active = null;
            StopAllCoroutines();
            IsReady = false;
            // 정산 버튼 구독 해제와 응대·주방 정지는 같은 베이스의 GameManager.OnDisable이 맡는다.
            // Single 전환과 앱 종료에서는 Unity가 씬을 함께 정리한다.
            if (!quitting && !entering && !SceneLoadService.IsLoading)
                foreach (var scene in ownedScenes)
                    Unload(scene);
            ownedScenes.Clear();
        }
    }
}
