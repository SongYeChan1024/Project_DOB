using UnityEngine;
using UnityEngine.UI;

namespace DOB.Manager
{
    /// <summary>씬 로더의 실제 진행률을 Loading 화면에 표시한다. 로드 요청이나 게임 상태를 변경하지 않는다.</summary>
    public sealed class LoadingScreenPresenter : MonoBehaviour
    {
        /// <summary>정규화된 진행률을 백분율로 표시할 텍스트.</summary>
        [SerializeField] private Text progressText;

        /// <summary>씬 로드 진행률 표시 바. 선택적으로 연결한다.</summary>
        [SerializeField] private Slider progressBar;

        private int displayedPercent = -1;

        /// <summary>에디터 씬 작성 과정에서 표시 컴포넌트를 연결한다.</summary>
        public void Configure(Text label, Slider bar)
        {
            progressText = label;
            progressBar = bar;
            displayedPercent = -1;
            Refresh();
        }

        private void OnEnable()
        {
            displayedPercent = -1;
            Refresh();
        }

        private void Update()
        {
            Refresh();
        }

        private void Refresh()
        {
            var progress = Mathf.Clamp01(SceneLoadService.Progress);
            if (progressBar != null)
                progressBar.normalizedValue = progress;

            var percent = Mathf.RoundToInt(progress * 100f);
            if (progressText != null && percent != displayedPercent)
                progressText.text = $"불러오는 중 {percent}%";
            displayedPercent = percent;
        }
    }
}
