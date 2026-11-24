// 임시 방명록 화면: 완성된 글 목록과 [닫기] 버튼. 실제 화면이 나오면 이 컴포넌트 대신 연결한다.
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    public sealed class TempGuestbookView : GuestbookView
    {
        /// <summary>보이고 숨길 카드 루트. 평소 비활성.</summary>
        [SerializeField] private GameObject panel;
        [SerializeField] private Text body;
        [SerializeField] private Button close;

        private void OnEnable() { if (close) close.onClick.AddListener(RaiseClosed); }
        private void OnDisable() { if (close) close.onClick.RemoveListener(RaiseClosed); }

        public override void Show(IReadOnlyList<GuestbookLineData> lines)
        {
            var text = new StringBuilder("방명록\n");
            foreach (var line in lines)
                text.Append('\n').Append(line.GuestName).Append("  “").Append(line.Line).Append('”');
            body.text = text.ToString();
            panel.SetActive(true);
        }

        public override void Hide() { if (panel) panel.SetActive(false); }
    }
}
