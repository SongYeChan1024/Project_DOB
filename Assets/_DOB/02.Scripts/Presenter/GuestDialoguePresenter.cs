// =====================================================================
// GuestDialoguePresenter — 응대 화면과 게임 쪽 요청을 잇는다 (더미 단계)
// 역할   : 손님 응대 열기 요청을 받아 표시 데이터를 만들어 화면에 넘기고,
//          화면의 버튼 입력을 요청으로 옮긴다.
// 호출자 : 상호작용(E) 쪽에서 OpenGuest 호출. 지금은 테스트용 호출만 있다.
// 호출   : GuestDialogueView (Open, Close, ShowNotice)
// 상태   : 수첩 기록(NotebookRecord) 하나만 가진다. 판정 · 좌석 · 저장은 하지 않는다.
// 비고   : 실제 게임 연결은 FirstGuestDialogue(송예찬)가 맡는다. 이 파일은 프론트 테스트용.
//          손님 데이터는 기획 02 · 03 첫 손님 원고 기준, 단서 정답(위험 · 도감 위치)은 임시 값.
// =====================================================================
using DOB.UI;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>응대 화면의 입력을 요청으로 옮기고 결과를 화면에 표시한다. 게임 규칙은 계산하지 않는다.</summary>
    public sealed class GuestDialoguePresenter : MonoBehaviour
    {
        [SerializeField] private GuestDialogueView view;

        /// <summary>테스트용: 켜면 "따라오세요"를 빈 자리 없음으로 거절한다.</summary>
        [Header("더미 응답 (계약 전 테스트)"), SerializeField] private bool simulateNoFreeSeat;

        /// <summary>테스트용: 켜면 대사를 길게 늘려 [다음] 쪽 넘김을 확인한다.</summary>
        [SerializeField] private bool simulateLongLine;

        /// <summary>찾은 단서 기록. 손님 응대를 닫았다 다시 열어도 이어진다.</summary>
        private readonly NotebookRecord notebook = new NotebookRecord();

        private void OnEnable()
        {
            view.FollowClicked += OnFollow;
            view.CloseClicked += OnClose;
            view.SendHomeConfirmed += OnSendHome;
            view.ClueFound += OnClueFound;
        }

        private void OnDisable()
        {
            view.FollowClicked -= OnFollow;
            view.CloseClicked -= OnClose;
            view.SendHomeConfirmed -= OnSendHome;
            view.ClueFound -= OnClueFound;
        }

        /// <summary>손님 응대를 연다. 이미 열려 있으면 무시한다.</summary>
        public void OpenGuest(string guestId)
        {
            if (view.IsOpen)
                return;
            view.Open(BuildDummyData(guestId));
        }

        /// <summary>테스트용 진입점: 첫 손님(하루) 응대를 연다. Play 중 컴포넌트 ⋮ 메뉴에서도 실행할 수 있다.</summary>
        [ContextMenu("테스트: 첫 손님 응대 열기")]
        public void OpenTutorialGuest() => OpenGuest("tutorial_guest_01");

        // ---------------- 버튼 ----------------

        private void OnFollow()
        {
            // TODO(계약): 안내 시작 요청 → 결과에 따라 닫기 또는 거절 안내
            if (simulateNoFreeSeat)
            {
                view.ShowNotice("지금은 빈 자리가 없어요.");
                return;
            }
            view.Close();
        }

        private void OnClose() => view.Close();

        private void OnSendHome()
        {
            // TODO(계약): 돌려보내기 요청. 받아들여지면 닫는다.
            view.Close();
        }

        // TODO(저장): 찾은 단서를 세이브에 남기려면 여기서 저장 담당 쪽에 알린다.
        private void OnClueFound(FoundClue clue) => Debug.Log($"[응대 테스트] 단서 찾음: [{clue.tag}]({clue.risk})");

        // ---------------- 더미 손님 ----------------

        // 기획 FirstGuestsData/Script.md 손님 1 원고. 계약 후 백엔드 표시 데이터로 교체한다.
        private GuestDialogueData BuildDummyData(string guestId)
        {
            return new GuestDialogueData
            {
                guestId = guestId,
                guestName = "하루",
                raceJob = "인간 · 마법사",
                race = "인간",
                job = "마법사",
                isRegular = false,
                destinationLabel = "1층 슬라임 숲",
                notebook = notebook,

                // [[ ]] 안이 숨은 키워드. 아래 keywords에 정답이 있다.
                line = "오늘은 1층 슬라임 숲. [[슬라임 점액]]이 튀면 [[따끔]]하더라. 돌아오는 [[길도 멀고]]."
                       + (simulateLongLine
                           ? " 지난번엔 숲 입구에서 길을 잃어서 한참 헤맸거든. 그때 먹은 게 영 안 맞았는지 계속 속이 쓰렸어. 이번엔 든든하게 먹고 가고 싶은데, 너무 무거운 건 곤란해. 마법 쓸 때 집중이 흐트러지거든."
                           : ""),
                keywords = new[]
                {
                    new ClueKeywordData { phrase = "슬라임 점액", risk = "독",   delta = 1, cause = "floor1_slime", recordTo = ClueRecordTarget.Floor },
                    new ClueKeywordData { phrase = "따끔",        risk = "독",   delta = 1, cause = "floor1_slime", recordTo = ClueRecordTarget.None },
                    new ClueKeywordData { phrase = "길도 멀고",   risk = "체력", recordTo = ClueRecordTarget.Floor },
                },

                // 초상화의 숨은 단서 자리 (초상 영역 기준 위치)
                clues = new[]
                {
                    new ClueMarkerData
                    {
                        description = "지팡이, 소매 끝이 살짝 그을린 로브", position = new Vector2(-170, -40),
                        tagName = "그을린 로브", risk = "열", recordTo = ClueRecordTarget.Job,
                    },
                    new ClueMarkerData
                    {
                        description = "허리를 펴고 서 있다, 땀방울 없음", position = new Vector2(150, -170),
                        // 인간은 약점이 없다(기획) → 종족 도감에는 안 적고 초상화 단서로만 남긴다
                        tagName = "땀방울 없음", risk = "체력", recordTo = ClueRecordTarget.None,
                    },
                },
                canSendHome = true,
            };
        }
    }
}
