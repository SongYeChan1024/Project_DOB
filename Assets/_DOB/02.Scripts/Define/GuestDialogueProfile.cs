// 응대를 열 때 손님 ID에 맞는 원고를 읽고, 플레이어가 밑줄을 누르면 표현의 사전 뜻을 찾는다.
using System;
using UnityEngine;

namespace DOB.Define
{
    /// <summary>손님 한 명의 응대 원고. 탐험 판정과 플레이 중 수첩 기록은 별도로 관리한다.</summary>
    [CreateAssetMenu(menuName = "DOB/손님 응대 원고", fileName = "SO_Guest_")]
    public sealed class GuestDialogueProfile : ScriptableObject
    {
        [Serializable]
        public struct Observation
        {
            public string description;
            public Vector2 position;
        }

        [Serializable]
        private struct DictionaryEntry
        {
            public string phrase;
            public string meaning;
        }

        [SerializeField] private string guestId;
        [SerializeField] private string guestName;
        [SerializeField] private string raceJob;
        [SerializeField] private string destinationLabel;
        [SerializeField] private bool canSendHome = true;
        [SerializeField, TextArea] private string orderLine;
        [SerializeField, TextArea] private string serveLine;
        [SerializeField, TextArea] private string plainBreadLine;
        [SerializeField] private Observation[] observations = Array.Empty<Observation>();
        [SerializeField] private DictionaryEntry[] dictionaryEntries = Array.Empty<DictionaryEntry>();

        public string GuestId => guestId;
        public string GuestName => guestName;
        public string RaceJob => raceJob;
        public string DestinationLabel => destinationLabel;
        public bool CanSendHome => canSendHome;
        public string OrderLine => orderLine;
        public string ServeLine => serveLine;
        public string PlainBreadLine => plainBreadLine;
        public int ObservationCount => observations.Length;
        public Observation GetObservation(int index) => observations[index];

        public string LookupClue(string phrase)
        {
            foreach (var entry in dictionaryEntries)
            {
                if (entry.phrase == phrase)
                    return entry.meaning;
            }
            return null;
        }
    }
}
