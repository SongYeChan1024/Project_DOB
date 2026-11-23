// =====================================================================
// RiskRowView — 수첩 위험 칸 한 줄 (축 하나)
// 역할   : 수첩에 기록된 값을 색 칸으로, 모르는 값은 '?'로 보여 주고,
//          메모 버튼을 누를 때마다 비움 → 있음 → 없음 순으로 바꿔 알린다.
// 호출자 : GuestDialogueView (값·메모 표시)
// 호출   : 없음 (MemoChanged 이벤트로만 알림)
// 상태   : 화면에 보이는 메모 값만 가진다. 판정에는 쓰지 않는다.
// =====================================================================
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.UI
{
    /// <summary>위험 축 한 줄의 표시와 메모 입력.</summary>
    public sealed class RiskRowView : MonoBehaviour
    {
        private static readonly string[] MemoLabels = { "메모 ?", "있음 !", "없음 X" };

        /// <summary>기록된 값만큼 켜는 색 칸들(왼쪽부터).</summary>
        [SerializeField] private GameObject[] cells;
        /// <summary>값을 모를 때 보이는 '?' 글자.</summary>
        [SerializeField] private GameObject unknownLabel;
        [SerializeField] private Button memoButton;
        [SerializeField] private TMP_Text memoLabel;

        private int memo;

        /// <summary>메모가 바뀌었다. 값은 0 비움 · 1 있음 · 2 없음.</summary>
        public event Action<int> MemoChanged;

        private void Awake()
        {
            memoButton.onClick.AddListener(CycleMemo);
        }

        /// <summary>기록 값(-1은 모름)과 메모를 표시한다. 기록된 축은 메모를 받지 않는다.</summary>
        public void Set(int knownValue, int memoState)
        {
            bool known = knownValue >= 0;
            for (int i = 0; i < cells.Length; i++)
                cells[i].SetActive(known && i < knownValue);
            unknownLabel.SetActive(!known);

            memoButton.gameObject.SetActive(!known);
            memo = Mathf.Clamp(memoState, 0, 2);
            memoLabel.text = MemoLabels[memo];
        }

        private void CycleMemo()
        {
            memo = (memo + 1) % 3;
            memoLabel.text = MemoLabels[memo];
            MemoChanged?.Invoke(memo);
        }
    }
}
