using System;
using System.Collections.Generic;

namespace DOB.Manager
{
    /// <summary>화면 진입 요청을 함께 로드할 씬 목록으로 바꾼다. 게임 진행·저장 상태는 보관하지 않는다.</summary>
    public static class SceneComposition
    {
        public const string Title = "Title";
        public const string Loading = "Loading";
        public const string Restaurant = "Restaurant";
        public const string Gameplay = "Gameplay";
        public const string Tutorial = "Tutorial";
        public const string ReturnReport = "ReturnReport";
        public const string CharacterTest = "CharacterTest";

        /// <summary>플레이어 빌드에 포함할 씬 순서. CharacterTest는 에디터 검증용으로 제외한다.</summary>
        public static IReadOnlyList<string> RuntimeSceneNames { get; } = Array.AsReadOnly(new[]
        {
            Title, Loading, Restaurant, Gameplay, Tutorial, ReturnReport
        });

        /// <summary>지원하는 진입 경로의 씬 조합을 반환한다. 호출자는 반환 배열을 독립적으로 사용한다.</summary>
        public static bool TryGetScenes(string destination, out string[] sceneNames)
        {
            switch (destination)
            {
                case Title:
                    sceneNames = new[] { Title };
                    return true;
                case Restaurant:
                    sceneNames = new[] { Restaurant, Gameplay };
                    return true;
                case Tutorial:
                    sceneNames = new[] { Restaurant, Gameplay, Tutorial };
                    return true;
                case ReturnReport:
                    sceneNames = new[] { ReturnReport };
                    return true;
                default:
                    // Loading은 전환 내부 전용이고 Gameplay는 단독 진입 경로가 아니다.
                    sceneNames = Array.Empty<string>();
                    return false;
            }
        }

        /// <summary>유효한 진입 경로에서 새 오브젝트의 기본 소유 씬이 될 활성 씬 이름을 반환한다.</summary>
        public static string GetActiveSceneName(string destination)
        {
            return destination == Tutorial ? Restaurant : destination;
        }
    }
}
