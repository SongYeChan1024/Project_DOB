using UnityEngine;

namespace DOB.Manager
{
    /// <summary>버튼 입력을 씬 이동 서비스로 전달한다. 게임 규칙·세션 상태를 계산하지 않는다.</summary>
    public sealed class SceneNavigationPresenter : MonoBehaviour
    {
        /// <summary>Inspector에 연결된 대상 씬 이름으로 이동을 요청한다.</summary>
        public void LoadScene(string sceneName)
        {
            SceneLoadService.RequestLoad(sceneName);
        }
    }
}
