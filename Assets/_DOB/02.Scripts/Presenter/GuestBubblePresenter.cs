// =====================================================================
// GuestBubblePresenter — 손님 머리 위 말풍선을 맞춘다 (시안 10)
// 역할   : IGuestMoods(백엔드 손님)가 알려 주는 손님마다 말풍선 하나를 맞추고, 머리 위치를 화면에 옮겨 그린다.
//          대기 시간 · 점수 · 떠남은 판정하지 않고 받은 값만 넘긴다.
// 호출자 : 없음 (매 프레임 스스로 갱신)
// 호출   : UIManager.GuestMoods · InteractionPrompt · IsScreenOpen, GuestBubbleView
// 상태   : 손님 수만큼 복제한 말풍선 목록
//
// 의사코드
// [매 프레임 끝] 연결이 없거나 다른 화면(준비 · 요리 · 방명록)이 열려 있으면 모두 숨긴다
//               → 손님 수만큼 말풍선을 견본에서 복제해 맞춘다
//               → 손님마다: 상태 없음 · 위치 없음 · 카메라 뒤면 숨김
//                 → 머리 위 높이를 화면 좌표로 바꾼다. 그 손님 위에 [E] 표시가 떠 있으면 그만큼 위로 올린다
//                 → 기다림(남은 비율 · 점수 구간) / 서빙 직후 / 퇴장(표정)으로 그린다
// 비고   : 카메라는 씬이 달라 직렬화할 수 없어 Camera.main을 쓴다 (다른 월드 표시와 같음).
// =====================================================================
using System.Collections.Generic;
using DOB.UI;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>손님 머리 위 말풍선들을 맞춘다.</summary>
    public sealed class GuestBubblePresenter : MonoBehaviour
    {
        /// <summary>말풍선 견본. 평소 비활성.</summary>
        [SerializeField] private GuestBubbleView template;
        /// <summary>손님 기준점에서 말풍선까지의 높이 (미터). 연출 값.</summary>
        [SerializeField] private float height = 2f;
        /// <summary>같은 손님 위에 [E] 표시가 떠 있을 때 말풍선을 올리는 거리 (캔버스 단위).</summary>
        [SerializeField] private float liftOverPrompt = 64f;

        private readonly List<GuestBubbleView> bubbles = new List<GuestBubbleView>();
        private RectTransform area;

        private void Awake()
        {
            area = (RectTransform)transform;
            if (template) template.gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            var moods = UIManager.GuestMoods;
            var camera = Camera.main;
            if (moods == null || !camera || !template || UIManager.IsScreenOpen)
            {
                HideFrom(0);
                return;
            }

            int count = Mathf.Max(0, moods.Count);
            while (bubbles.Count < count)
                bubbles.Add(Instantiate(template, template.transform.parent));
            HideFrom(count);

            var promptTarget = UIManager.InteractionPrompt?.Target;
            for (int i = 0; i < count; i++)
            {
                var bubble = bubbles[i];
                var guest = moods.GuestAt(i);
                var state = moods.StateAt(i);
                if (!guest || state == GuestMoodState.None) { bubble.Hide(); continue; }

                var screen = camera.WorldToScreenPoint(guest.position + Vector3.up * height);
                if (screen.z <= 0f) { bubble.Hide(); continue; } // 카메라 뒤
                RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screen, null, out var local);
                if (promptTarget == guest) local.y += liftOverPrompt; // [E] 표시와 겹치지 않게

                switch (state)
                {
                    case GuestMoodState.Waiting: bubble.ShowWaiting(local, moods.PatienceLeftAt(i), moods.WaitScoreAt(i)); break;
                    case GuestMoodState.Served: bubble.ShowServed(local); break;
                    case GuestMoodState.Leaving: bubble.ShowLeaving(local, (int)moods.ReactionAt(i)); break;
                }
            }
        }

        private void HideFrom(int start)
        {
            for (int i = start; i < bubbles.Count; i++)
                if (bubbles[i]) bubbles[i].Hide();
        }
    }
}
