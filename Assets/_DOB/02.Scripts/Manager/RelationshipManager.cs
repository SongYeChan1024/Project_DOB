// RelationshipManager — 호감·단골·사연·방명록·수첩 기록을 총괄한다.
// 지금 대신하는 스크립트: 없음 (응대 중 수첩 메모는 FirstGuestDialogue)
// 상태: 첫 플레이 범위의 방명록(GuestbookLog)만 구현. 호감·단골·사연은 후속 (Docs/개발/FeatureManagerMap.md F10).
// 호출자: GameManager (BeginDay, RecordMeal, Tick) / 읽는 쪽: GameManager·DayFlowPresenter (Guestbook)
// 의사코드:
//   BeginDay — 그날의 방명록을 새로 연다.
//   RecordMeal — 서빙이 끝난 방문의 글을 등록한다. 방명록이 열리지 않았거나 중복 방문이면 false.
//   Tick — 정지 중이 아닐 때 GameManager가 넘긴 시간만큼 글 작성을 진행한다.
using DOB.Systems;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>손님 관계 기록의 총괄. 실제 상태는 아래 System(GuestbookLog)이 소유한다.</summary>
    [DisallowMultipleComponent]
    public sealed class RelationshipManager : MonoBehaviour
    {
        /// <summary>오늘의 방명록. BeginDay 전에는 null.</summary>
        public GuestbookLog Guestbook { get; private set; }

        /// <summary>그날의 방명록을 연다. 작성 시간은 DayCycleSettings 기준값(초).</summary>
        public void BeginDay(float guestbookWriteSeconds) => Guestbook = new GuestbookLog(guestbookWriteSeconds);

        /// <summary>서빙이 끝난 방문의 방명록 글을 등록한다.</summary>
        public bool RecordMeal(string guestId, string visitId, string guestName, string line) =>
            Guestbook != null && Guestbook.TryBegin(guestId, visitId, guestName, line);

        /// <summary>방명록 작성 시간을 흘린다. 정지 중이면 0을 넘긴다.</summary>
        public void Tick(float deltaSeconds) => Guestbook?.Tick(deltaSeconds);
    }
}
