using UnityEngine;

namespace DOB.Manager
{
    /// <summary>화면의 진입 요청을 씬 조합 로더로 전달한다. 게임 규칙·세션 상태를 계산하지 않는다.</summary>
    public sealed class SceneNavigationPresenter : MonoBehaviour
    {
        /// <summary>Inspector의 Title·Restaurant·Tutorial·ReturnReport 진입 경로를 요청한다.</summary>
        public void LoadScene(string destination)
        {
            SceneLoadService.RequestLoad(destination);
        }
    }
}
