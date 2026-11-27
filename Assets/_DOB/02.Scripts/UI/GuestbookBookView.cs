// =====================================================================
// GuestbookBookView — 방명록 화면 (GuestbookView의 실제 구현, 계약 초안 4장 · 시안 08)
// 역할   : 펼친 책 한 면에 손님 한 명씩, 이름과 그 식사의 맛 소감 한 줄을 적는다. 두 쪽씩 넘겨 본다.
// 호출자 : DayFlowPresenter (Show · Hide, Closed 구독). 계산 · 판정은 하지 않는다.
// 상태   : 마지막으로 받은 글 목록의 복사본과 지금 펼친 면 번호. 본 소감 목록(실행 동안), 써지는 글자 수. 게임 상태는 갖지 않는다.
//
// 의사코드
// [Show]  매번 전체 목록을 받는다 → 통째로 복사(추가분 계산 안 함) → 가장 최근 글이 보이는 면을 펼친다 → 창을 켠다
// [그리기] 펼친 면의 왼쪽 · 오른쪽 쪽에 글을 하나씩 채운다. 글이 없는 쪽은 비운다 → 앞 · 뒤 넘김은 넘길 쪽이 있을 때만
// [넘김]  앞 · 뒤로 한 면(두 쪽)씩 옮겨 다시 그린다
// [새 글]  (시안 18 ⑤) 이번 실행에서 아직 안 본 소감은 펼친 쪽에서 한 글자씩 써지듯 나타난다 (왼쪽 다음 오른쪽).
//         다 써지면 본 글로 기억해 다음부터는 바로 보인다
// [닫기]  [닫기] 버튼 · Esc → 닫기 입력을 알리고 스스로 숨는다 (DayFlowPresenter의 Hide가 다시 불려도 무해)
//         ESC 메뉴가 열려 있으면(timeScale 0) Esc는 메뉴 몫. UIManager.IsScreenOpen이 IsAnyOpen을 읽어 ESC 메뉴를 열지 않는다
// [Hide]  창을 끈다. 여러 번 불리거나 씬 종료 중(창 파괴됨)이어도 오류 없이 지나간다
// =====================================================================
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>책 모양 방명록. 한 쪽에 손님 한 명.</summary>
    public sealed class GuestbookBookView : GuestbookView
    {
        /// <summary>보이고 숨길 창 루트. 평소 비활성.</summary>
        [SerializeField] private GameObject panel;
        /// <summary>왼쪽 · 오른쪽 쪽의 손님 이름.</summary>
        [SerializeField] private TMP_Text leftName, rightName;
        /// <summary>왼쪽 · 오른쪽 쪽의 맛 소감.</summary>
        [SerializeField] private TMP_Text leftLine, rightLine;
        /// <summary>이름 밑줄. 빈 쪽에서는 숨긴다.</summary>
        [SerializeField] private GameObject leftRule, rightRule;
        [SerializeField] private Button prev, next, close;

        /// <summary>새 소감이 써지는 빠르기 (초당 글자 수).</summary>
        [SerializeField] private float charsPerSecond = 25f;

        private readonly List<GuestbookLineData> lines = new List<GuestbookLineData>();
        private int spread;

        // 이번 실행에서 다 써진 소감 (이름 + 소감). 다시 열면 바로 보인다
        private static readonly HashSet<string> seenLines = new HashSet<string>();
        private readonly List<(TMP_Text text, string key)> typing = new List<(TMP_Text, string)>();
        private float typed;

        private int LastSpread => Mathf.Max(0, (lines.Count - 1) / 2);

        // 마지막으로 연 방명록 (Esc 순서용). 화면은 하나뿐이라 정적 하나로 충분하다
        private static GuestbookBookView current;

        /// <summary>방명록 창이 열려 있는지.</summary>
        public bool IsOpen => panel && panel.activeInHierarchy;

        /// <summary>열려 있는 방명록이 있는지. 있으면 Esc는 방명록을 닫고 ESC 메뉴를 열지 않는다.</summary>
        public static bool IsAnyOpen => current && current.IsOpen;

        private void OnEnable()
        {
            if (close) close.onClick.AddListener(Close);
            if (prev) prev.onClick.AddListener(Prev);
            if (next) next.onClick.AddListener(Next);
        }

        private void OnDisable()
        {
            if (close) close.onClick.RemoveListener(Close);
            if (prev) prev.onClick.RemoveListener(Prev);
            if (next) next.onClick.RemoveListener(Next);
        }

        public override void Show(IReadOnlyList<GuestbookLineData> data)
        {
            lines.Clear();
            if (data != null) lines.AddRange(data);
            spread = LastSpread; // 새 글이 완성될 때마다 불리므로 가장 최근 글을 펼친다
            Draw();
            if (panel) panel.SetActive(true);
            current = this;
        }

        public override void Hide()
        {
            if (panel) panel.SetActive(false);
        }

        private void Update()
        {
            // ESC 메뉴가 먼저 받는다 (BasementPauseInput은 실행 순서 -150, 메뉴가 열리면 timeScale 0)
            if (IsOpen && Time.timeScale > 0f && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                Close();
            if (IsOpen) Type();
        }

        // 써지는 중인 소감을 왼쪽부터 차례로 한 글자씩 더 보인다. 다 써진 소감은 본 글로 기억한다
        private void Type()
        {
            if (typing.Count == 0) return;
            typed += Mathf.Min(Time.unscaledDeltaTime, 0.05f) * charsPerSecond;
            int left = Mathf.FloorToInt(typed);
            bool done = true;
            foreach (var (text, key) in typing)
            {
                int length = text.text.Length;
                text.maxVisibleCharacters = Mathf.Clamp(left, 0, length);
                if (left >= length) seenLines.Add(key);
                else done = false;
                left -= length;
            }
            if (done) typing.Clear();
        }

        private void Close()
        {
            if (!IsOpen) return;
            Hide();
            RaiseClosed();
        }

        private void Prev() { if (spread > 0) { spread--; Draw(); } }
        private void Next() { if (spread < LastSpread) { spread++; Draw(); } }

        private void Draw()
        {
            typing.Clear();
            typed = 0f;
            Page(spread * 2, leftName, leftLine, leftRule);
            Page(spread * 2 + 1, rightName, rightLine, rightRule);
            if (prev) prev.gameObject.SetActive(spread > 0);
            if (next) next.gameObject.SetActive(spread < LastSpread);
        }

        private void Page(int index, TMP_Text name, TMP_Text line, GameObject rule)
        {
            bool has = index < lines.Count;
            if (name) name.text = has ? lines[index].GuestName : "";
            if (line) line.text = has ? lines[index].Line : "";
            if (rule) rule.SetActive(has);
            if (!line) return;
            string key = has ? lines[index].GuestName + "\n" + lines[index].Line : null;
            bool fresh = has && !seenLines.Contains(key);
            line.maxVisibleCharacters = fresh ? 0 : 99999;
            if (fresh) typing.Add((line, key));
        }
    }
}
