using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DOB.Manager
{
    /// <summary>
    /// Presenter의 요청으로 씬을 로드한다. 전환 중에만 유지되며 게임 상태는 소유하지 않는다.
    /// </summary>
    public sealed class SceneLoadService : MonoBehaviour
    {
        private static SceneLoadService activeLoader;

        /// <summary>등록된 씬으로 이동한다. 중복 전환 요청은 무시한다.</summary>
        public static void RequestLoad(string sceneName)
        {
            if (activeLoader != null)
                return;

            if (string.IsNullOrWhiteSpace(sceneName) || !Application.CanStreamedLevelBeLoaded(sceneName))
            {
                Debug.LogError($"[DOB] 등록되지 않은 씬: {sceneName}");
                return;
            }

            var loaderObject = new GameObject(nameof(SceneLoadService));
            activeLoader = loaderObject.AddComponent<SceneLoadService>();
            DontDestroyOnLoad(loaderObject);
            activeLoader.StartCoroutine(activeLoader.Load(sceneName));
        }

        private IEnumerator Load(string sceneName)
        {
            // 전환 호스트만 유지한다. 세션·정산 상태의 보존은 이후 별도 계약으로 구현한다.
            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            if (operation != null)
                yield return operation;

            activeLoader = null;
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (activeLoader == this)
                activeLoader = null;
        }
    }
}
