// =====================================================================
// SceneLoadService — 진입 요청을 Loading 화면과 Additive 씬 조합으로 실행한다
// 역할   : 전환 호스트·진행률·실패 상태만 소유한다. 게임 세션·정산·튜토리얼 완료 상태는 보관하지 않는다.
//          로딩 화면 표시는 UIManager에 요청하고 화면을 직접 다루지 않는다.
// 호출자 : SceneNavigationPresenter, BasementComposition.GoToTitle, UIManager(실패 후 타이틀 복귀)
// 호출   : SceneComposition, UIManager (OpenLoading, SetLoadingProgress, CloseLoading, ShowLoadingFailure)
// 상태   : 진행 중인 호스트(전환 잠금), 진행률, 목적지, 실패 여부
//
// 의사코드
// [요청]    전환 중이면 무시 → 지원하지 않는 경로·빌드 미등록 씬이면 오류 기록 후 현재 씬 유지
//           → 씬을 넘어 유지되는 호스트를 만들고 전환 시작
// [전환]    1. Loading을 Single로 로드 (이전 화면 모두 닫힘)
//           2. 로딩 화면을 열고 페이드 인이 끝날 때까지 기다린다
//           3. 목적지 씬들을 Additive로 로드하며 진행률 갱신. 로드를 시작하지 못하거나 로드 후 씬이 없으면 실패
//           4. 활성 씬 지정. 실패하면 실패
//           5. 로딩 화면 닫기: 최소 표시 시간·진행 바 채움·페이드 아웃이 끝날 때까지 기다린다
//           6. Loading 언로드 → 진행률 1 → 호스트 정리
// [실패]    오류 기록 → 진행률 0 → 실패 안내 표시. 목적지가 타이틀이 아니고 타이틀을 열 수 있으면 [타이틀로],
//           아니면 [게임 종료]. 안내를 띄우면 잠금을 유지해 다른 전환을 막는다
//           안내를 띄울 화면이 없으면 잠금을 풀어 현재 화면을 계속 쓰게 한다
// [복귀]    실패 상태의 호스트를 정리한 뒤 타이틀 전환을 새로 요청한다
// =====================================================================
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DOB.Manager
{
    /// <summary>
    /// Presenter의 진입 요청을 Loading과 Additive 씬 조합으로 실행한다.
    /// 전환 호스트와 진행률만 소유하며 게임 세션·정산·튜토리얼 완료 상태는 보관하지 않는다.
    /// </summary>
    public sealed class SceneLoadService : MonoBehaviour
    {
        private static SceneLoadService activeLoader;

        private int completedOperations;
        private int operationCount;
        private bool failed;

        /// <summary>목표 조합과 Loading 정리가 모두 끝날 때까지 참이다. 실패 안내 중에도 참이다.</summary>
        public static bool IsLoading => activeLoader != null;

        /// <summary>마지막 전환이 실패해 안내를 띄운 상태인지.</summary>
        public static bool HasFailed => activeLoader != null && activeLoader.failed;

        /// <summary>현재 전환의 정규화된 진행률. 완료하면 1, 실패하거나 외부에서 중단하면 0이다.</summary>
        public static float Progress { get; private set; }

        /// <summary>현재 요청된 진입 경로 이름. 게임 진행 단계나 저장 데이터와 구분한다.</summary>
        public static string CurrentDestination { get; private set; } = string.Empty;

        /// <summary>지원하는 진입 경로를 요청한다. 전환 중 입력과 잘못된 요청은 현재 씬을 바꾸지 않고 false를 돌려준다.</summary>
        public static bool RequestLoad(string destination)
        {
            if (IsLoading)
                return false;

            if (!SceneComposition.TryGetScenes(destination, out var sceneNames))
            {
                Debug.LogError($"[DOB] 지원하지 않는 씬 진입 경로: {destination}");
                return false;
            }

            // Loading을 연 뒤 누락을 발견하면 기존 화면을 잃으므로 전체 등록을 먼저 검사한다.
            if (!CanLoadScene(SceneComposition.Loading))
                return false;
            foreach (var sceneName in sceneNames)
                if (!CanLoadScene(sceneName))
                    return false;

            var loaderObject = new GameObject(nameof(SceneLoadService));
            activeLoader = loaderObject.AddComponent<SceneLoadService>();
            Progress = 0f;
            CurrentDestination = destination;
            DontDestroyOnLoad(loaderObject);
            activeLoader.StartCoroutine(activeLoader.Load(destination, sceneNames));
            return true;
        }

        /// <summary>실패 안내 중이면 실패한 전환을 정리하고 타이틀로 새로 전환한다. 정상 전환 중에는 거절한다.</summary>
        public static bool RequestReturnToTitle()
        {
            if (activeLoader != null)
            {
                if (!activeLoader.failed)
                    return false;
                var failedLoader = activeLoader;
                activeLoader = null;
                Destroy(failedLoader.gameObject);
            }
            return RequestLoad(SceneComposition.Title);
        }

        private static bool CanLoadScene(string sceneName)
        {
            if (Application.CanStreamedLevelBeLoaded(sceneName))
                return true;

            Debug.LogError($"[DOB] 빌드에 등록되지 않은 씬: {sceneName}");
            return false;
        }

        private IEnumerator Load(string destination, string[] sceneNames)
        {
            var completed = false;
            completedOperations = 0;
            // 진행률은 로딩 화면·목표 씬들·로딩 화면 정리의 실제 작업 수를 기준으로 표시한다.
            operationCount = sceneNames.Length + 2;

            try
            {
                var operation = BeginLoad(SceneComposition.Loading, LoadSceneMode.Single);
                if (operation == null)
                {
                    Fail("Loading 씬 로드를 시작하지 못했습니다.");
                    yield break;
                }
                yield return Track(operation);

                yield return UIManager.OpenLoading(destination);

                foreach (var sceneName in sceneNames)
                {
                    operation = BeginLoad(sceneName, LoadSceneMode.Additive);
                    if (operation == null)
                    {
                        Fail("씬 로드를 시작하지 못했습니다: " + sceneName);
                        yield break;
                    }
                    yield return Track(operation);
                    if (!SceneManager.GetSceneByName(sceneName).isLoaded)
                    {
                        Fail("씬을 불러오지 못했습니다: " + sceneName);
                        yield break;
                    }
                }

                var activeScene = SceneManager.GetSceneByName(SceneComposition.GetActiveSceneName(destination));
                if (!activeScene.isLoaded || !SceneManager.SetActiveScene(activeScene))
                {
                    Fail("활성 씬을 설정하지 못했습니다: " + destination);
                    yield break;
                }

                yield return UIManager.CloseLoading();

                var unloadOperation = SceneManager.UnloadSceneAsync(SceneComposition.Loading);
                if (unloadOperation == null)
                {
                    Fail("Loading 씬 정리를 시작하지 못했습니다.");
                    yield break;
                }
                yield return Track(unloadOperation);

                Progress = 1f;
                completed = true;
            }
            finally
            {
                // 실패 안내 중에는 잠금을 유지한다. 정리는 안내의 버튼 응답(RequestReturnToTitle)이 맡는다.
                if (!failed)
                {
                    if (!completed)
                    {
                        Progress = 0f;
                        CurrentDestination = string.Empty;
                    }
                    if (activeLoader == this)
                        activeLoader = null;
                    Destroy(gameObject);
                }
            }
        }

        private static AsyncOperation BeginLoad(string sceneName, LoadSceneMode mode)
        {
            try
            {
                return SceneManager.LoadSceneAsync(sceneName, mode);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        private IEnumerator Track(AsyncOperation operation)
        {
            while (!operation.isDone)
            {
                ReportProgress((completedOperations + operation.progress) / operationCount);
                yield return null;
            }
            ReportProgress((float)++completedOperations / operationCount);
        }

        private static void ReportProgress(float value)
        {
            // 진행률은 줄어들지 않게 유지한다.
            Progress = Mathf.Max(Progress, Mathf.Clamp01(value));
            UIManager.SetLoadingProgress(Progress);
        }

        private void Fail(string reason)
        {
            Debug.LogError($"[DOB] 씬 전환 실패 ({CurrentDestination}): {reason}");
            Progress = 0f;
            var canReturnToTitle = CurrentDestination != SceneComposition.Title
                && Application.CanStreamedLevelBeLoaded(SceneComposition.Title);
            // 안내를 띄울 화면이 없으면(Loading 자체 실패 등) 잠금을 풀고 현재 화면을 그대로 쓴다.
            failed = UIManager.ShowLoadingFailure(canReturnToTitle);
        }

        private void OnDestroy()
        {
            if (activeLoader == this)
            {
                activeLoader = null;
                Progress = 0f;
                CurrentDestination = string.Empty;
            }
        }
    }
}
