// 신규 손님의 이름·종족·직업을 각각 뽑는다. 뽑은 신원은 손님 ID에 고정되어 재방문 때 다시 뽑지 않는다(GuestLedger).
// 의사코드: DrawIdentity — 제외할 이름을 뺀 후보에서 이름을 뽑고, 남은 후보가 없으면 전체에서 뽑는다. 종족·직업은 따로 뽑는다.
//           개인 취약점은 신원에 고정이므로 여기서 한 번 추첨한다. DrawTired — 피로는 방문마다 다시 추첨한다.
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

        /// <summary>신규 손님의 고정 신원을 뽑는다. excludedNames에 든 이름은 후보가 남아 있는 동안 뽑지 않는다.</summary>
        public GuestRecord DrawIdentity(string guestId, int day, System.Random random, System.Collections.Generic.ICollection<string> excludedNames = null)
        {
            if (random == null) throw new ArgumentNullException(nameof(random));
            if (day < 1) throw new ArgumentOutOfRangeException(nameof(day));

            var free = excludedNames == null || names == null ? names
                : Array.FindAll(names, n => !string.IsNullOrWhiteSpace(n) && !excludedNames.Contains(n));
            // 후보를 다 썼으면 전체 이름을 다시 허용한다 (FeatureManagerMap F3).
            string name = Pick(free != null && free.Length > 0 ? free : names, random, "손님");
            string race = Pick(races, random, "인간");
            string job = Pick(jobs, random, string.Empty);
            string weakness = day > 1 && random.NextDouble() < weaknessChance ? Pick(weaknessAxes, random, null) : null;
            return new GuestRecord(guestId, name, race, job, weakness, 0, false, day);
        }

        /// <summary>이번 방문의 피로 여부. 첫날에는 항상 거짓이다.</summary>
        public bool DrawTired(int day, System.Random random)
        {
            if (random == null) throw new ArgumentNullException(nameof(random));
            return day > 1 && random.NextDouble() < tiredChance;
        }

        private static string Pick(string[] candidates, System.Random random, string fallback)
        {
            if (candidates == null || candidates.Length == 0) return fallback;
            string value = candidates[random.Next(candidates.Length)];
            return string.IsNullOrWhiteSpace(value) ? fallback : value;
        }
    }
}
