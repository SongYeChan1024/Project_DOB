// 본영업 한 방문의 진행 단계, 종료 방식, 영업 시계를 멈추는 정지 사유. ServiceRoster·ServicePauseReasons가 쓴다.
namespace DOB.Define
{
    /// <summary>방문의 진행 단계. 입구 대기 → 응대 → 안내 → 착석 → 서빙됨 순서이며, 종료는 어느 단계에서든 한 번이다.</summary>
    public enum ServiceVisitStage { AtEntrance, Greeting, Guided, Seated, Served, Ended }

    /// <summary>방문이 끝난 방식. 끝나기 전에는 None이다.</summary>
    public enum ServiceVisitEnd { None, Dispatched, SentHome, WaitedOut }

    /// <summary>영업 시계를 멈추는 이유. 둘 다 입장·대기·조리를 함께 멈춘다. 응대·수첩·재료 선택은 시간을 멈추지 않는다 (F2).</summary>
    public enum ServicePauseReason { PauseMenu, Transition }
}
