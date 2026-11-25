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
    /// <summary>타이틀 전환의 잠금·Loading 경유·로딩 화면 표시와 빌드 씬 목록을 검증한다. 영업 조합(Basement)은 BasementInput 테스트가 맡는다.</summary>
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
                new[] { "Title", "Loading", "Restaurant", "Basement", "BasementUI" },
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
        public IEnumerator TitleReloadGoesThroughLoadingAndIgnoresRequestsWhileLocked()
        {
            yield return SceneManager.LoadSceneAsync("Title");
            yield return TransitionAndWait(() => SceneLoadService.RequestLoad("Title"), "Title", "Title", "Title");
        }

        [UnityTest]
        public IEnumerator InternalAndDevelopmentScenesAreRejectedBeforeTheCurrentSceneChanges()
        {
            yield return SceneManager.LoadSceneAsync("Title");
            var previousDestination = SceneLoadService.CurrentDestination;

            foreach (var unsupportedRoute in new[] { "Loading", "Restaurant", "Basement", "CharacterTest", "UnknownScene" })
            {
                LogAssert.Expect(LogType.Error, new Regex(@"^\[DOB\]"));
                SceneLoadService.RequestLoad(unsupportedRoute);
                yield return null;

                Assert.That(SceneLoadService.IsLoading, Is.False);
                Assert.That(SceneLoadService.CurrentDestination, Is.EqualTo(previousDestination));
                AssertLoadedComposition("Title", "Title");
            }
        }

        [UnityTest]
        public IEnumerator LoadingScreenStaysForTheMinimumTimeAndFadesToBlackBeforeClosing()
        {
            yield return SceneManager.LoadSceneAsync("Title");

            var loadingSeenAt = -1f;
            var minimumSeconds = -1f;
            var lastFadeAlpha = -1f;
            var tipShown = false;
            void OnLoaded(Scene scene, LoadSceneMode mode)
            {
                if (scene.name == "Loading")
                    loadingSeenAt = Time.realtimeSinceStartup;
            }

            SceneManager.sceneLoaded += OnLoaded;
            try
            {
                Assert.That(SceneLoadService.RequestLoad("Title"), Is.True);
                var deadline = Time.realtimeSinceStartup + TransitionTimeoutSeconds;
                while (SceneLoadService.IsLoading && Time.realtimeSinceStartup < deadline)
                {
                    var loading = SceneManager.GetSceneByName("Loading");
                    if (loading.isLoaded)
                    {
                        var images = LoadingComponents<Image>(loading);
                        var fade = images.SingleOrDefault(image => image.name == "FadeOverlay");
                        if (fade) lastFadeAlpha = fade.color.a;
                        var presenter = LoadingComponents<LoadingScreenPresenter>(loading).SingleOrDefault();
                        if (presenter) minimumSeconds = presenter.MinimumDisplaySeconds;
                        var tip = LoadingComponents<TMPro.TMP_Text>(loading).SingleOrDefault(text => text.name == "TipText");
                        tipShown |= tip && !string.IsNullOrEmpty(tip.text);
                    }
                    yield return null;
                }
            }
            finally
            {
                SceneManager.sceneLoaded -= OnLoaded;
            }

            var shownSeconds = Time.realtimeSinceStartup - loadingSeenAt;
            Assert.That(SceneLoadService.IsLoading, Is.False, "전환이 제한 시간 안에 끝나야 합니다.");
            Assert.That(loadingSeenAt, Is.GreaterThan(0f), "전환은 Loading 씬을 거쳐야 합니다.");
            Assert.That(minimumSeconds, Is.GreaterThan(0f), "Loading 씬에 로딩 화면 Presenter와 내용 데이터가 연결되어야 합니다.");
            Assert.That(shownSeconds, Is.GreaterThanOrEqualTo(minimumSeconds), "로딩 화면은 최소 표시 시간 동안 머물러야 합니다.");
            Assert.That(lastFadeAlpha, Is.EqualTo(1f).Within(0.01f), "Loading을 닫기 전에 검은 페이드가 끝나야 합니다.");
            Assert.That(tipShown, Is.True, "목적지 팁이 표시되어야 합니다.");
            yield return null;
            AssertLoadedComposition("Title", "Title");
        }

        [UnityTest]
        public IEnumerator LoadingFailureNoticeIsClickableAndReturnsToTitle()
        {
            yield return SceneManager.LoadSceneAsync("Loading");
            var loading = SceneManager.GetSceneByName("Loading");
            Assert.That(LoadingComponents<EventSystem>(loading), Is.Empty, "Loading 씬은 평소 EventSystem을 두지 않습니다.");
            yield return UIManager.OpenLoading("Restaurant");

            Assert.That(UIManager.ShowLoadingFailure(true), Is.True);
            var panel = LoadingComponents<Transform>(loading).Single(item => item.name == "FailurePanel");
            Assert.That(panel.gameObject.activeInHierarchy, Is.True, "실패 안내가 보여야 합니다.");
            Assert.That(EventSystem.current, Is.Not.Null, "실패 안내 버튼을 누를 수 있게 EventSystem이 있어야 합니다.");
            Assert.That(EventSystem.current.gameObject.scene, Is.EqualTo(loading), "임시 EventSystem은 Loading과 함께 사라져야 합니다.");

            var button = LoadingComponents<Button>(loading).Single(item => item.name == "FailureButton");
            Assert.That(button.onClick.GetPersistentEventCount(), Is.EqualTo(1));
            Assert.That(button.onClick.GetPersistentTarget(0), Is.Not.Null);
            button.onClick.Invoke();
            button.onClick.Invoke();

            var deadline = Time.realtimeSinceStartup + TransitionTimeoutSeconds;
            while ((SceneLoadService.IsLoading || !SceneManager.GetSceneByName("Title").isLoaded) && Time.realtimeSinceStartup < deadline)
                yield return null;
            yield return null;
            AssertLoadedComposition("Title", "Title");
        }

        [UnityTest]
        public IEnumerator EachDestinationShowsItsOwnLoadingHeadingAndIllustration()
        {
            yield return SceneManager.LoadSceneAsync("Loading");
            var loading = SceneManager.GetSceneByName("Loading");
            var images = LoadingComponents<Image>(loading);
            var illustration = images.Single(image => image.name == "Illustration");
            var heading = images.Single(image => image.name == "HeadingImage");
            var tip = LoadingComponents<TMPro.TMP_Text>(loading).Single(text => text.name == "TipText");

            var destinations = new[] { "Title", "Restaurant" };
            var illustrations = new Dictionary<string, Sprite>();
            var headings = new Dictionary<string, Sprite>();
            foreach (var destination in destinations)
            {
                yield return UIManager.OpenLoading(destination);
                Assert.That(illustration.enabled && illustration.sprite, Is.True, destination + " 그림이 보여야 합니다.");
                Assert.That(heading.gameObject.activeInHierarchy && heading.sprite, Is.True, destination + " 제목 그림이 보여야 합니다.");
                Assert.That(tip.text, Is.Not.Empty, destination + " 팁이 보여야 합니다.");
                illustrations[destination] = illustration.sprite;
                headings[destination] = heading.sprite;
            }

            Assert.That(illustrations.Values.Distinct().Count(), Is.EqualTo(destinations.Length), "목적지마다 다른 그림이어야 합니다.");
            Assert.That(headings.Values.Distinct().Count(), Is.EqualTo(destinations.Length), "목적지마다 다른 제목 그림이어야 합니다.");

            // 데이터에 없는 목적지는 기본 그림과 공통 팁을 쓰고, 제목 그림은 숨긴다.
            yield return UIManager.OpenLoading("UnknownDestination");
            Assert.That(illustration.enabled && illustration.sprite, Is.True, "없는 목적지도 기본 그림을 보여야 합니다.");
            Assert.That(heading.gameObject.activeInHierarchy, Is.False, "없는 목적지는 제목 그림을 숨겨야 합니다.");
            Assert.That(tip.text, Is.Not.Empty, "없는 목적지도 공통 팁을 보여야 합니다.");

            yield return SceneManager.LoadSceneAsync("Title");
        }

        private static T[] LoadingComponents<T>(Scene scene) where T : Component
        {
            return scene.GetRootGameObjects().SelectMany(root => root.GetComponentsInChildren<T>(true)).ToArray();
        }

        private static IEnumerator TransitionAndWait(System.Action request, string destination, string activeScene, params string[] sceneNames)
        {
            var loadingObserved = false;
            var additiveTargetObserved = false;
            var lockedDuringLoading = false;
            var lockedDuringAdditive = false;
            var loaderCountsDuringTransition = new List<int>();

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
                SceneLoadService.RequestLoad("Title");
                loaderCountsDuringTransition.Add(Object.FindObjectsByType<SceneLoadService>(FindObjectsSortMode.None).Length);
            }

            SceneManager.sceneLoaded += TryCompetingRequest;
            try
            {
                request();
                request();
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
            foreach (var prefabName in new[] { "PF_Player", "PF_Guest", "PF_Enemy" })
                Assert.That(objects.Where(item => item.name == prefabName), Is.Empty, "타이틀 화면에 캐릭터가 남아 있으면 안 됩니다.");
        }
    }
}
