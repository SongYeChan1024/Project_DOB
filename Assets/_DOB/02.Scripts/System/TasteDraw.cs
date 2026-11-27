// =====================================================================
// TasteDraw — 손님 입맛 태그 두 개를 정한다 (순수 C#, 상태 없음)
// 호출자 : DailyRoster(신규 손님 신원을 뽑을 때), GuestLedger(입맛이 없는 이전 저장의 손님을 읽을 때)
// 의사코드:
//   Draw(기준, 손님 ID, 종족)
//     — 손님 ID 문자열의 FNV-1a 해시를 시드로 난수를 만든다. 같은 ID·종족이면 언제 뽑아도 같다.
//     — 종족 대표 맛이 있으면 첫 칸은 대표 맛, 없으면 후보에서 하나 뽑는다.
//     — 둘째 칸은 첫 칸을 뺀 후보에서 뽑는다(겹침 없음).
//   Fill(기준, 기록) — 입맛이 이미 있으면 그대로, 없으면 Draw로 채운 사본.
// string.GetHashCode는 실행마다 달라질 수 있어 쓰지 않는다. 그날 시드에서 순서대로 뽑으면 손님 순서가 바뀔 때 결과가 달라진다.
// =====================================================================
using System;
using System.Collections.Generic;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>손님 ID에 고정되는 입맛 태그 두 개를 정한다.</summary>
    public static class TasteDraw
    {
        /// <summary>그 손님의 입맛 태그 두 개. 같은 ID·종족이면 같은 결과다.</summary>
        public static string[] Draw(TasteRules rules, string guestId, string race)
        {
            if (!rules) throw new ArgumentNullException(nameof(rules));
            if (string.IsNullOrEmpty(guestId)) throw new ArgumentException("손님 ID가 비었습니다.", nameof(guestId));
            var random = new Random(Seed(guestId));
            var candidates = new List<string>(rules.Flavors);
            string first = rules.Signature(race) ?? candidates[random.Next(candidates.Count)];
            candidates.Remove(first);
            return new[] { first, candidates[random.Next(candidates.Count)] };
        }

        /// <summary>입맛이 없으면 채운 사본, 있으면 그대로.</summary>
        public static GuestRecord Fill(TasteRules rules, GuestRecord guest) =>
            guest == null || guest.Tastes.Count > 0 ? guest : guest.WithTastes(Draw(rules, guest.GuestId, guest.Race));

        // FNV-1a 32비트. 실행·플랫폼과 관계없이 같은 값이 나온다.
        private static int Seed(string text)
        {
            unchecked
            {
                uint hash = 2166136261;
                foreach (char c in text)
                {
                    hash ^= c;
                    hash *= 16777619;
                }
                return (int)hash;
            }
        }
    }
}
