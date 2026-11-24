// ServicePauseReasons — 겹쳐 켜진 정지 사유로 입장·대기 시계와 조리 시계를 따로 멈춘다.
// 호출자: GameManager가 매 프레임 각 화면·메뉴 상태로 사유를 켜고 끈 뒤 두 값을 읽어 Δ를 나눈다.
// 상태: 켜진 사유의 비트 집합. 같은 사유를 두 번 켜도 한 번이고, 하나를 꺼도 다른 사유는 남는다.
// 의사코드: 사유가 하나라도 켜져 있으면 입장·대기를 멈춘다. 일시 정지·전환이 켜져 있을 때만 조리도 멈춘다.
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>영업 중 겹친 정지 사유 집합.</summary>
    public sealed class ServicePauseReasons
    {
        private int active;

        /// <summary>사유를 켜거나 끈다. 이미 같은 상태면 아무 일도 없다.</summary>
        public void Set(ServicePauseReason reason, bool on)
        {
            int bit = 1 << (int)reason;
            active = on ? active | bit : active & ~bit;
        }

        /// <summary>그 사유가 켜져 있는지.</summary>
        public bool IsOn(ServicePauseReason reason) => (active & (1 << (int)reason)) != 0;

        /// <summary>손님 입장 간격과 손님별 대기 시간을 멈춰야 하는지.</summary>
        public bool StopsArrivalAndWait => active != 0;

        /// <summary>조리·방명록 작성 시간까지 멈춰야 하는지.</summary>
        public bool StopsCooking => IsOn(ServicePauseReason.PauseMenu) || IsOn(ServicePauseReason.Transition);
    }
}
