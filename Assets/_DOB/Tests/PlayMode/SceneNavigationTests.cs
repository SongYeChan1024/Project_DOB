using System.Collections;
using DOB.Manager;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace DOB.Tests
{
    /// <summary>실제 씬의 직렬화된 버튼 연결과 반복 로드 잠금, 재진입을 검증한다.</summary>
    public sealed class SceneNavigationTests
    {
        [UnityTest]
        public IEnumerator ButtonsCompleteSceneRoundTripWithoutDuplicateLoaders()
        {
            yield return SceneManager.LoadSceneAsync("Title");
            yield return InvokeButtonAndWait("Restaurant");
            yield return InvokeButtonAndWait("ReturnReport");
            yield return InvokeButtonAndWait("Restaurant");
            yield return InvokeButtonAndWait("Title");
        }

        private static IEnumerator InvokeButtonAndWait(string destination)
        {
            var button = GameObject.Find("Load_" + destination).GetComponent<Button>();
            Assert.That(button.onClick.GetPersistentEventCount(), Is.EqualTo(1));
            button.onClick.Invoke();
            // 같은 프레임의 반복 입력으로 두 개의 로더가 생기면 안 된다.
            button.onClick.Invoke();
            Assert.That(Object.FindObjectsByType<SceneLoadService>(FindObjectsSortMode.None).Length, Is.EqualTo(1));
            var deadline = Time.realtimeSinceStartup + 20;
            while (SceneManager.GetActiveScene().name != destination && Time.realtimeSinceStartup < deadline)
                yield return null;
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo(destination));
            yield return null;
            yield return null;
            Assert.That(Object.FindObjectsByType<SceneLoadService>(FindObjectsSortMode.None), Is.Empty);
        }
    }
}
