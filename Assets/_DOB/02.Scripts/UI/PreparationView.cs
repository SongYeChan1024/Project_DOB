// 준비 단계 화면의 교체 지점. 실제 화면은 이 클래스를 상속해 BasementHud에 연결하면 된다.
// 입구 문패의 [문 열기](UIManager.StartBusinessRequested)도 같은 '영업 시작'으로 처리된다 (DayFlowPresenter).
// 호출자: DayFlowPresenter (Show, Hide, StartRequested 구독)
using System;
using UnityEngine;

namespace DOB.UI
{
    /// <summary>준비 단계 화면. 계산하지 않고 받은 값을 보이며, 영업 시작 입력만 알린다.</summary>
    public abstract class PreparationView : MonoBehaviour
    {
        /// <summary>플레이어가 준비를 마치고 영업 시작을 눌렀다.</summary>
        public event Action StartRequested;

        public abstract void Show(PreparationData data);
        public abstract void Hide();

        protected void RaiseStartRequested() => StartRequested?.Invoke();
    }
}
