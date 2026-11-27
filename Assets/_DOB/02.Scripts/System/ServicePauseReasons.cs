// ServicePauseReasons — 겹쳐 켜진 정지 사유로 영업 시계(입장·대기·조리·방명록 작성)를 멈춘다.
// 호출자: GameManager가 매 프레임 일시 정지 메뉴·전환 상태로 사유를 켜고 끈 뒤 IsStopped를 읽는다.
// 상태: 켜진 사유의 비트 집합. 같은 사유를 두 번 켜도 한 번이고, 하나를 꺼도 다른 사유는 남는다.
// 의사코드: 사유가 하나라도 켜져 있으면 모든 영업 시계를 멈춘다.
// 응대·수첩·재료 선택은 정지 사유가 아니다. 시간은 ESC(일시 정지)를 눌렀을 때만 멈춘다 (노션 확정, FeatureManagerMap F2).
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

        /// <summary>입장 간격·대기·조리·방명록 작성 시간을 모두 멈춰야 하는지.</summary>
        public bool IsStopped => active != 0;
    }
}
