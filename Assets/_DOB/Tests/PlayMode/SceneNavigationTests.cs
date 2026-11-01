using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DOB.Manager;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace DOB.Tests
{
    /// <summary>실제 버튼으로 조합 씬의 왕복·전환 잠금·캐릭터와 입력 소유권을 검증한다.</summary>
    public sealed class SceneNavigationTests
    {
        private const float TransitionTimeoutSeconds = 30f;

        [Test]
        public void RuntimeBuildScenesExcludeTheCharacterTestScene()
        {
            var sceneNames = Enumerable.Range(0, SceneManager.sceneCountInBuildSettings)
                .Select(SceneUtility.GetScenePathByBuildIndex)
                .Select(System.IO.Path.GetFileNameWithoutExtension)
                .Where(name => !IsUnityTestRunnerBootstrapScene(name))
                .ToArray();

            Assert.That(sceneNames.FirstOrDefault(), Is.EqualTo("Title"), "빌드는 타이틀에서 시작해야 합니다.");
            CollectionAssert.AreEquivalent(
                new[] { "Title", "Loading", "Restaurant", "Gameplay", "Tutorial", "ReturnReport" },
                sceneNames);
            Assert.That(sceneNames, Does.Not.Contain("CharacterTest"), "개발용 캐릭터 씬은 플레이어 빌드에서 제외합니다.");
        }

        private static bool IsUnityTestRunnerBootstrapScene(string sceneName)
        {
            const string prefix = "InitTestScene";
            // PlayMode runner가 임시 등록한 정확한 이름 형식만 제외한다. 다른 추가 씬은 여전히 검증한다.
            return sceneName != null
                && sceneName.Length == prefix.Length + 36
                && sceneName.StartsWith(prefix, System.StringComparison.Ordinal)
                && System.Guid.TryParseExact(sceneName.Substring(prefix.Length), "D", out _);
        }

        [UnityTest]
        public IEnumerator ButtonsCompleteRestaurantAndReportRoundTripWithoutResidualScenes()
        {
            yield return SceneManager.LoadSceneAsync("Title");
            yield return InvokeButtonAndWait("Restaurant", "Restaurant", "Restaurant", "Gameplay");
            yield return InvokeButtonAndWait("ReturnReport", "ReturnReport", "ReturnReport");
            yield return InvokeButtonAndWait("Restaurant", "Restaurant", "Restaurant", "Gameplay");
            yield return InvokeButtonAndWait("Title", "Title", "Title");
        }

        [UnityTest]
        public IEnumerator TutorialUsesTheSharedRestaurantAndGameplayThenUnloadsItsOwnScene()
        {
            yield return SceneManager.LoadSceneAsync("Title");
            yield return InvokeButtonAndWait("Tutorial", "Restaurant", "Restaurant", "Gameplay", "Tutorial");

            var tutorialScene = SceneManager.GetSceneByName("Tutorial");
            var tutorialObjects = tutorialScene.GetRootGameObjects()
                .SelectMany(root => root.GetComponentsInChildren<Transform>(true))
                .ToArray();
            foreach (var guestName in new[] { "TutorialGuest01", "TutorialGuest02", "TutorialGuest03" })
                Assert.That(tutorialObjects.Count(item => item.name == guestName), Is.EqualTo(1));

            Assert.That(tutorialScene.GetRootGameObjects().SelectMany(root => root.GetComponentsInChildren<Camera>(true)), Is.Empty);
            Assert.That(tutorialScene.GetRootGameObjects().SelectMany(root => root.GetComponentsInChildren<EventSystem>(true)), Is.Empty);

            yield return InvokeButtonAndWait("Restaurant", "Restaurant", "Restaurant", "Gameplay");
            Assert.That(SceneManager.GetSceneByName("Tutorial").isLoaded, Is.False);
            yield return InvokeButtonAndWait("Title", "Title", "Title");
        }

        [UnityTest]
        public IEnumerator InternalAndDevelopmentScenesAreRejectedBeforeTheCurrentSceneChanges()
        {
            yield return SceneManager.LoadSceneAsync("Title");
            var previousDestination = SceneLoadService.CurrentDestination;

            foreach (var unsupportedRoute in new[] { "Loading", "Gameplay", "CharacterTest", "UnknownScene" })
            {
                LogAssert.Expect(LogType.Error, new Regex(@"^\[DOB\]"));
                SceneLoadService.RequestLoad(unsupportedRoute);
                yield return null;

                Assert.That(SceneLoadService.IsLoading, Is.False);
                Assert.That(SceneLoadService.CurrentDestination, Is.EqualTo(previousDestination));
                AssertLoadedComposition("Title", "Title");
            }
        }

        private static IEnumerator InvokeButtonAndWait(string destination, string activeScene, params string[] sceneNames)
        {
            var buttons = Object.FindObjectsByType<Button>(FindObjectsSortMode.None)
                .Where(button => button.name == "Load_" + destination)
                .ToArray();
            Assert.That(buttons.Length, Is.EqualTo(1), "대상 경로의 전환 버튼은 현재 조합에 하나만 있어야 합니다.");
            var button = buttons[0];
            Assert.That(button.onClick.GetPersistentEventCount(), Is.EqualTo(1));
            Assert.That(button.onClick.GetPersistentTarget(0), Is.Not.Null);

            var loadingObserved = false;
            var additiveTargetObserved = false;
            var lockedDuringLoading = false;
            var lockedDuringAdditive = false;
            var loaderCountsDuringTransition = new List<int>();
            var competingDestination = destination == "Title" ? "Restaurant" : "Title";

            void TryCompetingRequest(Scene scene, LoadSceneMode mode)
            {
                if (scene.name == "Loading")
                {
                    loadingObserved = true;
                    lockedDuringLoading = SceneLoadService.IsLoading;
                }
                else if (mode == LoadSceneMode.Additive && scene.name == sceneNames[0])
                {
                    additiveTargetObserved = true;
                    lockedDuringAdditive = SceneLoadService.IsLoading;
                }
                else
                {
                    return;
                }

                // Loading 진입과 첫 추가 씬이 준비된 시점 모두에서 다른 경로 요청을 무시해야 합니다.
                SceneLoadService.RequestLoad(competingDestination);
                loaderCountsDuringTransition.Add(Object.FindObjectsByType<SceneLoadService>(FindObjectsSortMode.None).Length);
            }

            SceneManager.sceneLoaded += TryCompetingRequest;
            try
            {
                button.onClick.Invoke();
                button.onClick.Invoke();
                Assert.That(SceneLoadService.IsLoading, Is.True);
                Assert.That(Object.FindObjectsByType<SceneLoadService>(FindObjectsSortMode.None).Length, Is.EqualTo(1));

                var deadline = Time.realtimeSinceStartup + TransitionTimeoutSeconds;
                var previousProgress = SceneLoadService.Progress;
                while (SceneLoadService.IsLoading && Time.realtimeSinceStartup < deadline)
                {
                    Assert.That(SceneLoadService.Progress, Is.InRange(0f, 1f));
                    Assert.That(SceneLoadService.Progress + 0.001f, Is.GreaterThanOrEqualTo(previousProgress));
                    previousProgress = SceneLoadService.Progress;
                    yield return null;
                }

                Assert.That(SceneLoadService.IsLoading, Is.False, "씬 조합 전환이 제한 시간 안에 끝나야 합니다.");
                // Destroy는 프레임 끝에 반영되므로 전환 완료 뒤 호스트 정리를 기다립니다.
                yield return null;
                yield return null;
            }
            finally
            {
                SceneManager.sceneLoaded -= TryCompetingRequest;
            }

            Assert.That(loadingObserved, Is.True, "전환은 Loading 씬을 거쳐야 합니다.");
            Assert.That(additiveTargetObserved, Is.True, "목적지 구성은 Additive로 로드해야 합니다.");
            Assert.That(lockedDuringLoading, Is.True);
            Assert.That(lockedDuringAdditive, Is.True);
            Assert.That(loaderCountsDuringTransition, Has.All.EqualTo(1));
            Assert.That(SceneLoadService.CurrentDestination, Is.EqualTo(destination));
            Assert.That(SceneLoadService.Progress, Is.EqualTo(1f).Within(0.001f));
            AssertLoadedComposition(activeScene, sceneNames);
        }

        private static void AssertLoadedComposition(string activeScene, params string[] expectedScenes)
        {
            var loadedNames = Enumerable.Range(0, SceneManager.sceneCount)
                .Select(SceneManager.GetSceneAt)
                .Where(scene => scene.isLoaded)
                .Select(scene => scene.name)
                .ToArray();

            CollectionAssert.AreEquivalent(expectedScenes, loadedNames, "이전 조합이나 Loading 씬이 남아 있으면 안 됩니다.");
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo(activeScene));
            Assert.That(Object.FindObjectsByType<SceneLoadService>(FindObjectsSortMode.None), Is.Empty);
            Assert.That(Object.FindObjectsByType<Camera>(FindObjectsSortMode.None).Length, Is.EqualTo(1));
            Assert.That(Object.FindObjectsByType<AudioListener>(FindObjectsSortMode.None).Length, Is.EqualTo(1));
            Assert.That(Object.FindObjectsByType<EventSystem>(FindObjectsSortMode.None).Length, Is.EqualTo(1));

            var objects = Object.FindObjectsByType<Transform>(FindObjectsSortMode.None);
            foreach (var prefabName in new[] { "PF_Player", "PF_Guest" })
            {
                var characterInstances = objects.Where(item => item.name == prefabName).ToArray();
                if (expectedScenes.Contains("Gameplay"))
                {
                    Assert.That(characterInstances.Length, Is.EqualTo(1), prefabName + " 중복 또는 누락");
                    Assert.That(characterInstances[0].gameObject.scene.name, Is.EqualTo("Gameplay"));
                    Assert.That(characterInstances[0].root.name, Is.EqualTo("RuntimeCharacters"));
                }
                else
                {
                    Assert.That(characterInstances, Is.Empty, "일반 화면에 캐릭터가 남아 있으면 안 됩니다.");
                }
            }
            Assert.That(objects.Where(item => item.name == "PF_Enemy"), Is.Empty, "확장용 에너미 골격은 런타임 구성에서 제외합니다.");

            if (expectedScenes.Contains("Restaurant"))
            {
                var anchors = SceneManager.GetSceneByName("Restaurant").GetRootGameObjects()
                    .Single(root => root.name == "LayoutAnchors").transform;
                foreach (var anchorName in new[] { "PlayerSpawn", "GuestEntrance", "GreetingPoint", "Seat01", "Seat02", "Seat03", "Kitchen", "StairsExit" })
                    Assert.That(anchors.Find(anchorName), Is.Not.Null, "식당 배치 기준점 누락: " + anchorName);
            }
        }
    }
}
