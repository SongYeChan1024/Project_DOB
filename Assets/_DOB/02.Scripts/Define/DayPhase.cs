// 하루 진행 단계. DayCycle이 현재 값을 소유하고 GameManager·DayFlowPresenter가 읽는다.
namespace DOB.Define
{
    /// <summary>아침 귀환 보고 → 준비 → 영업 → 마감 순서로만 진행하는 하루 단계. 데모 엔딩은 마감 확인 뒤 서빙 인원이 기준 이상일 때만 들어간다.</summary>
    public enum DayPhase
    {
        MorningReport,
        Preparation,
        Service,
        Closing,
        DemoEnding
    }
}
