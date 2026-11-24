// 방명록 화면의 교체 지점. 실제 화면은 이 클래스를 상속해 BasementHud에 연결하면 된다.
// 호출자: DayFlowPresenter (Show, Hide, Closed 구독)
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DOB.UI
{
    /// <summary>방명록 화면. 완성된 글을 방문 순서대로 보이며, 닫기 입력만 알린다.</summary>
    public abstract class GuestbookView : MonoBehaviour
    {
        /// <summary>플레이어가 방명록을 닫았다.</summary>
        public event Action Closed;

        public abstract void Show(IReadOnlyList<GuestbookLineData> lines);
        public abstract void Hide();

        protected void RaiseClosed() => Closed?.Invoke();
    }
}
