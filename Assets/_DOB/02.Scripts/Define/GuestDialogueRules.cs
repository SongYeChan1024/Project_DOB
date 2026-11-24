using System;
using System.Collections.Generic;
using UnityEngine;

namespace DOB.Define
{
    public enum DialogueCondition { Floor, Event, Race, Job, Fatigue, PersonalWeakness }
    public enum DialogueClueChannel { Dialogue, Observation, News }

    /// <summary>첫 주문 구운 빵의 시험 원고. 맛 소감과 탐험 출발 각오는 별도로 표시한다.</summary>
    [Serializable]
    public struct BreadDialogueLines
    {
        public string garlicTaste;
        public string plainTaste;
        public string fullyPrepared;
        public string oneShort;
        public string severalShort;
    }

    /// <summary>한 위험 원인이 언제 적용되고 어떤 말이나 외형으로 드러나는지 정한다.</summary>
    [Serializable]
    public sealed class DialogueCauseRule
    {
        public string causeId;
        public DialogueCondition condition;
        public int firstDay = 1;
        public int floor;
        public string qualifier;
        public string axis;
        public int delta;
        [TextArea] public string line;
        public string[] alternateLines = Array.Empty<string>();
        public string observation;
        public string news;
    }

    /// <summary>방문 조건별 대사 원고와 단골 추가 단서의 시험 기준. 손님의 신원은 방문 생성자가 정한다.</summary>
    [CreateAssetMenu(menuName = "DOB/방문 대사 규칙", fileName = "SO_GuestDialogueRules")]
    public sealed class GuestDialogueRules : ScriptableObject
    {
        [SerializeField] private DialogueCauseRule[] causes = Array.Empty<DialogueCauseRule>();
        [SerializeField] private int secondClueAffinity;
        [SerializeField] private string calmLine;
        [SerializeField] private BreadDialogueLines breadLines;

        public IReadOnlyList<DialogueCauseRule> Causes => causes;
        public int SecondClueAffinity => secondClueAffinity;
        public string CalmLine => calmLine;
        public BreadDialogueLines BreadLines => breadLines;
    }
}
