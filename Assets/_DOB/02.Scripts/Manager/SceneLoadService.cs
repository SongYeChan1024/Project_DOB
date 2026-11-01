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

        /// <summary>목표 조합과 Loading 정리가 모두 끝날 때까지 참이다.</summary>
        public static bool IsLoading => activeLoader != null;

        /// <summary>현재 전환의 정규화된 진행률. 완료하면 1, 실패하거나 외부에서 중단하면 0이다.</summary>
        public static float Progress { get; private set; }

        /// <summary>현재 요청된 진입 경로 이름. 게임 진행 단계나 저장 데이터와 구분한다.</summary>
        public static string CurrentDestination { get; private set; } = string.Empty;

        /// <summary>지원하는 진입 경로를 요청한다. 전환 중 입력과 잘못된 요청은 현재 씬을 바꾸지 않는다.</summary>
        public static void RequestLoad(string destination)
        {
            if (IsLoading)
                return;

            if (!SceneComposition.TryGetScenes(destination, out var sceneNames))
            {
                Debug.LogError($"[DOB] 지원하지 않는 씬 진입 경로: {destination}");
                return;
            }

            // Loading을 연 뒤 누락을 발견하면 기존 화면을 잃으므로 전체 등록을 먼저 검사한다.
            if (!CanLoadScene(SceneComposition.Loading))
                return;
            foreach (var sceneName in sceneNames)
                if (!CanLoadScene(sceneName))
                    return;

            var loaderObject = new GameObject(nameof(SceneLoadService));
            activeLoader = loaderObject.AddComponent<SceneLoadService>();
            Progress = 0f;
            CurrentDestination = destination;
            DontDestroyOnLoad(loaderObject);
            activeLoader.StartCoroutine(activeLoader.Load(destination, sceneNames));
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
            var completedOperations = 0;
            // 진행률은 로딩 화면·목표 씬들·로딩 화면 정리의 실제 작업 수를 기준으로 표시한다.
            var operationCount = sceneNames.Length + 2;

            try
            {
                var loadingOperation = SceneManager.LoadSceneAsync(SceneComposition.Loading, LoadSceneMode.Single);
                if (loadingOperation == null)
                    throw new InvalidOperationException("Loading 씬 로드를 시작하지 못했습니다.");
                while (!loadingOperation.isDone)
                {
                    Progress = (completedOperations + loadingOperation.progress) / operationCount;
                    yield return null;
                }
                Progress = (float)++completedOperations / operationCount;

                foreach (var sceneName in sceneNames)
                {
                    var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                    if (operation == null)
                        throw new InvalidOperationException("씬 로드를 시작하지 못했습니다: " + sceneName);
                    while (!operation.isDone)
                    {
                        Progress = (completedOperations + operation.progress) / operationCount;
                        yield return null;
                    }
                    Progress = (float)++completedOperations / operationCount;
                }

                var activeScene = SceneManager.GetSceneByName(SceneComposition.GetActiveSceneName(destination));
                if (!SceneManager.SetActiveScene(activeScene))
                    throw new InvalidOperationException("활성 씬을 설정하지 못했습니다: " + destination);

                var unloadOperation = SceneManager.UnloadSceneAsync(SceneComposition.Loading);
                if (unloadOperation == null)
                    throw new InvalidOperationException("Loading 씬 정리를 시작하지 못했습니다.");
                while (!unloadOperation.isDone)
                {
                    Progress = (completedOperations + unloadOperation.progress) / operationCount;
                    yield return null;
                }

                Progress = 1f;
                completed = true;
            }
            finally
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
