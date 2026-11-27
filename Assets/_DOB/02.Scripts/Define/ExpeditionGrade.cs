// 탐험 판정이 쓰는 등급과 위험 축의 기준 순서. System(판정·보고)과 UI(보고 표시)가 함께 읽는다.
using System.Collections.Generic;

namespace DOB.Define
{
    /// <summary>부족 합으로 정하는 탐험 등급. 부족이 적은 쪽부터 나열한다.</summary>
    public enum ExpeditionGrade
    {
        GreatSuccess,
        Success,
        Failure,
        GreatFailure
    }

    /// <summary>여섯 위험 축의 이름과 순서. 축별 부족 목록은 이 순서로 저장한다.</summary>
    public static class RiskAxes
    {
        // 대사 규칙 에셋(GuestDialogueRules)의 axis 문자열과 같은 이름을 쓴다.
        public static readonly IReadOnlyList<string> All = new[] { "독", "열", "냉기", "어둠", "체력", "함정" };

        // 요리·양념의 효과 이름(기획 11장 표). All과 같은 순서다.
        private static readonly string[] Effects = { "해독", "내열", "방한", "야광", "활력", "감각" };

        /// <summary>효과 이름이 막는 위험 축(해독 → 독). 모르는 이름이면 null.</summary>
        public static string AxisOfEffect(string effect)
        {
            int index = System.Array.IndexOf(Effects, effect);
            return index < 0 ? null : All[index];
        }
    }
}
