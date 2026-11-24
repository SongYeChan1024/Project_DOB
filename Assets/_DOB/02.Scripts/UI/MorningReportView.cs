// 아침 귀환 보고 화면의 교체 지점. 실제 화면은 이 클래스를 상속해 BasementHud에 연결하면 된다.
// 호출자: DayFlowPresenter (Show, Hide, Confirmed 구독)
using System;
using UnityEngine;

namespace DOB.UI
{
    /// <summary>아침 귀환 보고 화면. 계산하지 않고 받은 값을 보이며, 확인 입력만 알린다.</summary>
    public abstract class MorningReportView : MonoBehaviour
    {
        /// <summary>플레이어가 보고를 다 읽고 확인했다.</summary>
        public event Action Confirmed;

        public abstract void Show(MorningReportData data);
        public abstract void Hide();

        protected void RaiseConfirmed() => Confirmed?.Invoke();
    }
}
