// =====================================================================
// DailyRoster — 그날 명단(인원·신원·입장 순서)을 영업을 열 때 한 번 정한다 (순수 C#, 상태 없음)
// 호출자 : GuestManager (BeginService에서 Compose, 입장 때 목록 순서대로 방문 생성)
// 의사코드:
//   Compose(기준값, 후보, 일차, 올 수 있는 손님, 이미 쓰는 이름, 입소문 신규 인원, 난수)
//     — 신규 수: 1일차는 기준값의 1일차 인원(입소문은 쓰지 않는다). 2일차부터는 입소문 신규 인원.
//     — 재방문 + 신규가 최소 인원보다 적으면 모자란 만큼 신규를 더한다. 최소 인원이 0이면 더하지 않는다.
//       결석한 자리는 따로 채우지 않는다 (2026-11-25 송예찬 결정).
//     — 신규 손님마다 손님 ID를 난수로 만들고, 이미 쓰는 이름(명부·그날 명단)을 뺀 후보에서 신원을 뽑는다.
//     — 재방문·신규를 한 목록에 넣고 한 번 섞어 입장 순서로 돌려준다 (Fisher–Yates).
//   NewId(난수) — 난수 16바이트를 16진수 32자로 적는다. 같은 시드면 같은 ID가 나온다.
// 같은 시드의 난수와 같은 입력이면 같은 명단이 나온다. 일차 시작 스냅샷을 불러와도 같은 하루가 되게 한다.
// 💡 최소 인원 보장과 무작위 입장 순서는 기획 🔴 미정 항목의 임시 규칙이다 (송예찬 확인 전).
// =====================================================================
using System;
using System.Collections.Generic;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>재방문 손님과 신규 손님으로 그날 명단을 만든다.</summary>
    public static class DailyRoster
    {
        /// <summary>입장 순서대로의 그날 손님 신원. 재방문 손님은 명부 기록 그대로, 신규 손님은 새로 뽑은 신원이다.</summary>
        /// <param name="eligible">그 일차에 올 수 있는 명부의 손님.</param>
        /// <param name="takenNames">신규 이름에서 뺄 이름(영구 거절되지 않은 명부 손님). 재방문 손님의 이름은 따로 넣지 않아도 된다.</param>
        /// <param name="wordOfMouthBonus">오늘의 입소문 신규 인원(명). 0 이상.</param>
        public static List<GuestRecord> Compose(ServiceRosterSettings settings, GuestArrivalPool pool, int day,
            IReadOnlyList<GuestRecord> eligible, IEnumerable<string> takenNames, int wordOfMouthBonus, Random random)
        {
            if (!settings) throw new ArgumentNullException(nameof(settings));
            if (!pool) throw new ArgumentNullException(nameof(pool));
            if (random == null) throw new ArgumentNullException(nameof(random));
            if (day < 1) throw new ArgumentOutOfRangeException(nameof(day));
            if (wordOfMouthBonus < 0) throw new ArgumentOutOfRangeException(nameof(wordOfMouthBonus));

            var roster = new List<GuestRecord>();
            var ids = new HashSet<string>();
            var names = new HashSet<string>();
            if (takenNames != null)
                foreach (string name in takenNames)
                    names.Add(name);
            if (eligible != null)
                foreach (var guest in eligible)
                    // 같은 손님은 한 번만 센다.
                    if (guest != null && ids.Add(guest.GuestId))
                    {
                        roster.Add(guest);
                        names.Add(guest.Name);
                    }

            int newGuests = day == 1 ? settings.BaseGuests : wordOfMouthBonus;
            newGuests = Math.Max(newGuests, settings.MinGuests - roster.Count);
            for (int i = 0; i < newGuests; i++)
            {
                string id = NewId(random);
                while (!ids.Add(id))
                    id = NewId(random);
                var guest = pool.DrawIdentity(id, day, random, names);
                roster.Add(guest);
                names.Add(guest.Name);
            }

            for (int i = roster.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (roster[i], roster[j]) = (roster[j], roster[i]);
            }
            return roster;
        }

        /// <summary>난수로 만든 32자 16진수 식별자. 손님 ID와 방문 ID에 쓴다.</summary>
        public static string NewId(Random random)
        {
            if (random == null) throw new ArgumentNullException(nameof(random));
            var bytes = new byte[16];
            random.NextBytes(bytes);
            return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
