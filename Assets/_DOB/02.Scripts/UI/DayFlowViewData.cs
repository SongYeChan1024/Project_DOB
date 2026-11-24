// 하루 진행 화면(아침보고·준비·방명록)에 넘기는 표시 데이터. DayFlowPresenter가 만들고 각 View가 읽는다.
namespace DOB.UI
{
    /// <summary>아침 귀환 보고 화면에 보일 값.</summary>
    public readonly struct MorningReportData
    {
        public readonly int Day;
        public readonly int Gold;
        public readonly int GarlicStock;

        public MorningReportData(int day, int gold, int garlicStock)
        {
            Day = day;
            Gold = gold;
            GarlicStock = garlicStock;
        }
    }

    /// <summary>준비 화면에 보일 값.</summary>
    public readonly struct PreparationData
    {
        public readonly int Day;
        public readonly int Gold;
        public readonly int GarlicStock;

        public PreparationData(int day, int gold, int garlicStock)
        {
            Day = day;
            Gold = gold;
            GarlicStock = garlicStock;
        }
    }

    /// <summary>방명록 한 줄.</summary>
    public readonly struct GuestbookLineData
    {
        public readonly string GuestName;
        public readonly string Line;

        public GuestbookLineData(string guestName, string line)
        {
            GuestName = guestName;
            Line = line;
        }
    }
}
