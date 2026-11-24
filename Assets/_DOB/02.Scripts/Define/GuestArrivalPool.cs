// 방문을 새로 열 때 이름·종족·직업을 각각 뽑는다. 같은 방문을 다시 열 때에는 저장한 결과를 쓴다.
// 컨디션과 개인 취약점은 첫날에는 생략하고 이후 영업에서 각각 추첨한다.
using System;
using UnityEngine;

namespace DOB.Define
{
    [CreateAssetMenu(menuName = "DOB/방문 손님 후보", fileName = ResourceName)]
    public sealed class GuestArrivalPool : ScriptableObject
    {
        public const string ResourceName = "SO_GuestArrivalPool";

        [SerializeField] private string[] names = Array.Empty<string>();
        [SerializeField] private string[] races = Array.Empty<string>();
        [SerializeField] private string[] jobs = Array.Empty<string>();
        [SerializeField] private string[] weaknessAxes = Array.Empty<string>();
        [SerializeField, Range(0f, 1f)] private float tiredChance;
        [SerializeField, Range(0f, 1f)] private float weaknessChance;

        public readonly struct Arrival
        {
            public readonly string GuestName, Race, Job, PersonalWeakness;
            public readonly bool IsTired;

            internal Arrival(string guestName, string race, string job, bool isTired, string personalWeakness)
            {
                GuestName = guestName;
                Race = race;
                Job = job;
                IsTired = isTired;
                PersonalWeakness = personalWeakness;
            }
        }

        public Arrival Draw(int day, System.Random random)
        {
            if (random == null) throw new ArgumentNullException(nameof(random));
            if (day < 1) throw new ArgumentOutOfRangeException(nameof(day));

            string name = Pick(names, random, "손님");
            string race = Pick(races, random, "인간");
            string job = Pick(jobs, random, string.Empty);
            bool tired = day > 1 && random.NextDouble() < tiredChance;
            string weakness = day > 1 && random.NextDouble() < weaknessChance
                ? Pick(weaknessAxes, random, null) : null;
            return new Arrival(name, race, job, tired, weakness);
        }

        private static string Pick(string[] candidates, System.Random random, string fallback)
        {
            if (candidates == null || candidates.Length == 0) return fallback;
            string value = candidates[random.Next(candidates.Length)];
            return string.IsNullOrWhiteSpace(value) ? fallback : value;
        }
    }
}
