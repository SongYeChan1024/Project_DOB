// 영업 입장 때 새 손님을 한 번 뽑고 방문 원고와 정답 위험을 함께 만든다.
using System;
using DOB.Define;
using DOB.Systems;
using UnityEngine;

namespace DOB.Manager
{
    public sealed class GuestManager : MonoBehaviour
    {
        private static readonly System.Random random = new System.Random();

        internal static GuestDialogueVisit CreateFirstVisit(int day)
        {
            var pool = Resources.Load<GuestArrivalPool>(GuestArrivalPool.ResourceName);
            var rules = Resources.Load<GuestDialogueRules>("SO_GuestDialogueRules");
            if (!pool || !rules)
            {
                Debug.LogError("[DOB] Resources에서 손님 후보 또는 대사 규칙을 찾을 수 없습니다.");
                return null;
            }

            var guest = pool.Draw(day, random);
            var input = new GuestVisitInput
            {
                GuestId = Guid.NewGuid().ToString("N"),
                VisitId = Guid.NewGuid().ToString("N"),
                GuestName = guest.GuestName,
                Race = guest.Race,
                Job = guest.Job,
                Day = day,
                Floor = 1,
                IsTired = guest.IsTired,
                PersonalWeakness = guest.PersonalWeakness
            };
            return GuestDialogueGenerator.Generate(rules, input, random);
        }
    }
}
