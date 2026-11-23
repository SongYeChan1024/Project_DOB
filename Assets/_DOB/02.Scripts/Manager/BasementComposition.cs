// 게임 베이스 진입 → 공용 로딩 화면 → 식당과 UI 추가 로드 → 씬 사이 참조 연결 → 영업 시작.
// 베이스가 중간에 종료되면 직접 연 씬과 진행 중이던 로드를 정리한다. 편집자가 함께 연 씬은 빌려 쓴다.
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DOB.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        public const string TitleScene = "BasementTitle";
        public const string RestaurantScene = "BasementRestaurant";
        public const string UiScene = "BasementUI";
        private const string SceneFolder = "Assets/_DOB/01.Scenes/";

        [SerializeField] private FirstGuestService service;
        [SerializeField] private FirstGuestKitchen kitchen;

        private static BasementComposition active;
        private static bool entering;
        private static (int Day, int Gold, int GarlicStock)? pendingStart;
        private static bool pendingNewTimeline;
        private readonly List<Scene> ownedScenes = new List<Scene>(3);
        private Text loadingLabel;
        private Slider loadingBar;
        private Button settleButton;
        private bool quitting;
        private bool newTimeline;
        private FirstServiceSaveData settledCheckpoint;

        public bool IsReady { get; private set; }
        public static BasementComposition Current => active;
        public static FirstServiceSaveStore OpenSaves()
        {
            var directory = Environment.GetEnvironmentVariable("DOB_BASEMENT_SAVE_DIR");
            return new FirstServiceSaveStore(string.IsNullOrWhiteSpace(directory) ? Path.Combine(Application.persistentDataPath, "Basement") : directory);
        }
        public FirstServiceSaveStore Saves { get; private set; }
        public int Day { get; private set; } = 1;

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
            Saves = OpenSaves();
            var start = pendingStart;
            pendingStart = null;
            newTimeline = pendingNewTimeline;
            pendingNewTimeline = false;
            if (!service || !kitchen)
            {
                Fail("게임 베이스에 응대·주방 서비스가 연결되지 않았습니다.");
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
            var display = FindInScene<LoadingScreenPresenter>(loading);
            if (display)
            {
                display.enabled = false;
                loadingBar = display.GetComponentInChildren<Slider>(true);
                loadingLabel = display.transform.Find("ProgressText")?.GetComponent<Text>();
            }
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

            service.ConnectHud(hud.Guidance, hud.Dialogue);
            kitchen.ConnectHud(hud);
            settleButton = hud.Settle;
            settleButton.interactable = false;
            settleButton.onClick.AddListener(EndSampleDay);
            Day = start?.Day ?? 1;
            kitchen.StartDay(start?.Gold ?? kitchen.DayStartGold, start?.GarlicStock ?? kitchen.DayStartGarlic);
            try
            {
                if (!Saves.TryLoad(FirstServiceSaveSlot.Auto, out _))
                    Saves.SavePreparation(FirstServiceSaveSlot.Auto, Day, kitchen.DayStartGold, kitchen.DayStartGarlic);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Debug.LogWarning("[DOB] 첫 자동 저장을 만들 수 없습니다. " + e.Message, this);
            }
            ShowProgress(1f);
            if (ownedScenes.Remove(loading))
                yield return SceneManager.UnloadSceneAsync(loading);

            service.enabled = true;
            kitchen.enabled = true;
            IsReady = service.enabled && kitchen.enabled;
        }

        private static Scene GetScene(string name) => SceneManager.GetSceneByPath(SceneFolder + name + ".unity");

        /// <summary>현재 영업을 계속하면서 그 일차의 준비 상태를 수동 슬롯에 기록한다.</summary>
        public void SaveManual() => Saves.SavePreparation(FirstServiceSaveSlot.Manual, Day, kitchen.DayStartGold, kitchen.DayStartGarlic);

        /// <summary>현재 방문이 서빙된 뒤에만 다음 준비 일차를 자동 저장하고 그 시점으로 진입한다.</summary>
        public void EndSampleDay()
        {
            if (!IsReady || entering || !kitchen.HasServed)
                return;
            try
            {
                // 새 게임 또는 수동 슬롯 선택 시, 첫 정산에 한해 이 플레이의 결과가 자동 슬롯을 이어받는다.
                // 샘플 씬을 열지 못해 버튼을 다시 눌러도 이미 확정한 정산을 사용한다.
                if (settledCheckpoint == null)
                    settledCheckpoint = newTimeline
                        ? Saves.SavePreparation(FirstServiceSaveSlot.Auto, checked(Day + 1), kitchen.Gold, kitchen.GarlicStock)
                        : Saves.SettleDay(Day, kitchen.Gold, kitchen.GarlicStock);
                BeginEntry((settledCheckpoint.Day, settledCheckpoint.Gold, settledCheckpoint.GarlicStock), false);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Debug.LogWarning("[DOB] 영업 정산을 저장할 수 없습니다. " + e.Message, this);
            }
        }

        /// <summary>타이틀 또는 명시적인 불러오기에서 공용 로딩 화면을 먼저 연 뒤 게임 베이스를 추가한다.</summary>
        public static bool EnterDay(FirstServiceSaveData start) =>
            BeginEntry(start == null ? ((int, int, int)?)null : (start.Day, start.Gold, start.GarlicStock),
                start == null || start.Slot == FirstServiceSaveSlot.Manual);

        private static bool BeginEntry((int Day, int Gold, int GarlicStock)? start, bool newTimeline)
        {
            if (entering || !CanLoad(RestaurantScene) || !CanLoad(UiScene))
                return false;
            pendingStart = start;
            pendingNewTimeline = newTimeline;
            return ThroughLoading(BaseScene, LoadSceneMode.Additive);
        }

        public static bool GoToTitle() => !entering && ThroughLoading(TitleScene, LoadSceneMode.Single);

        private static bool ThroughLoading(string destination, LoadSceneMode mode)
        {
            if (!CanLoad(SceneComposition.Loading) || !CanLoad(destination))
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
                var next = LoadScene(destination, mode);
                if (next == null)
                {
                    entering = false;
                    pendingStart = null;
                    pendingNewTimeline = false;
                    if (destination != TitleScene) LoadScene(TitleScene, LoadSceneMode.Single);
                }
                else if (destination == TitleScene)
                    next.completed += _result => entering = false;
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

        private void ShowProgress(float value)
        {
            if (loadingBar) loadingBar.normalizedValue = value;
            if (loadingLabel) loadingLabel.text = $"불러오는 중 {Mathf.RoundToInt(value * 100f)}%";
        }

        private void Fail(string reason)
        {
            Debug.LogError("[DOB] " + reason, this);
            enabled = false;
        }

        private void OnApplicationQuit() => quitting = true;

        private void Update()
        {
            if (IsReady && settleButton)
                settleButton.interactable = kitchen.HasServed && !entering && !BasementPauseMenu.IsPaused;
        }

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
            if (settleButton) settleButton.onClick.RemoveListener(EndSampleDay);
            if (kitchen) kitchen.enabled = false;
            if (service) service.enabled = false;
            // Single 전환과 앱 종료에서는 Unity가 씬을 함께 정리한다.
            if (!quitting && !entering && !SceneLoadService.IsLoading)
                foreach (var scene in ownedScenes)
                    Unload(scene);
            ownedScenes.Clear();
        }
    }
}
