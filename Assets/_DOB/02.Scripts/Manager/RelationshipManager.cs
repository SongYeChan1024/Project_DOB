// RelationshipManager — 호감·단골·사연·방명록·수첩 기록을 총괄한다.
// 지금 대신하는 스크립트: 없음 (응대 중 수첩 메모는 FirstGuestDialogue)
// 상태: 오늘의 방명록(GuestbookLog), 호감·입소문 기준(AffinityRules), 일차 시작 스냅샷의 전날 방문 호감 기록과
//       오늘 적용하는 입소문 추가 인원, 오늘 마감 정산에서 확정한 방문 호감 기록과 다음 영업 추가 인원.
//       개인 누적 호감·단골·사연은 후속 (Docs/개발/FeatureManagerMap.md F10).
// 호출자: GameManager (SetupError, BeginDay, RecordMeal, Tick, SettleDay)
// 읽는 쪽: GameManager·DayFlowPresenter (Guestbook), 아침 보고 화면 연결(YesterdayAffinities·YesterdayTally·WordOfMouthBonus, 홍준우 인계)
// 의사코드:
//   SetupError — 기준이 없거나 Validate가 오류를 내면 그 설명. GameManager.Prepare가 실패한다.
//   BeginDay — 그날의 방명록을 새로 열고, 스냅샷의 전날 방문 호감 기록·입소문 추가 인원을 보관한다. 오늘 정산 결과를 비운다.
//   RecordMeal — 서빙이 끝난 방문의 글을 등록한다. 방명록이 열리지 않았거나 중복 방문이면 false.
//   Tick — 정지 중이 아닐 때 GameManager가 넘긴 시간만큼 글 작성을 진행한다.
//   SettleDay(명단의 방문, 오늘의 방문 결과) — 이미 정산했으면 보관한 결과를 그대로 돌려준다(저장 재시도에도 값이 같다).
//     처음이면 AffinityJudge로 방문 호감을 확정하고(💡 입맛 점수 0), S·N을 집계해 다음 영업의 추가 인원을 정해 보관한다.
//     결과 기록이 없는 파견 방문은 빠지고 오류 로그를 남긴다(정산은 막지 않는다).
//   조회 — 전날 기록·오늘 추가 인원·전날 S와 N. 상태를 바꾸지 않는다.
using System;
using System.Collections.Generic;
using DOB.Define;
using DOB.Systems;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>손님 관계 기록의 총괄. 계산은 System(GuestbookLog·AffinityJudge)이 한다.</summary>
    [DisallowMultipleComponent]
    public sealed class RelationshipManager : MonoBehaviour
    {
        /// <summary>방문 호감·입소문 기준값(08.Data/SO_AffinityRules).</summary>
        [SerializeField] private AffinityRules rules;

        // 통합 테스트가 방문 최대 점수 등을 바꾸려고 씬 기준값 대신 넣는다. 게임 코드는 쓰지 않는다.
        internal static AffinityRules RulesOverride;
        private IReadOnlyList<VisitAffinity> settledAffinities;

        private AffinityRules Rules => RulesOverride ? RulesOverride : rules;

        /// <summary>오늘의 방명록. BeginDay 전에는 null.</summary>
        public GuestbookLog Guestbook { get; private set; }
        /// <summary>기준값 연결·검사 오류. 문제가 없으면 null.</summary>
        public string SetupError => !Rules ? "호감·입소문 기준값이 없습니다." : Rules.Validate();
        /// <summary>전날 마감 정산에서 확정한 방문 호감 기록(정산 순서). 1일차·이전 스키마 저장이면 빈 목록.</summary>
        public IReadOnlyList<VisitAffinity> YesterdayAffinities { get; private set; } = Array.Empty<VisitAffinity>();
        /// <summary>오늘 영업에 적용하는 입소문 추가 인원(명).</summary>
        public int WordOfMouthBonus { get; private set; }
        /// <summary>오늘 추가 인원의 근거인 전날 방문 호감 합계 S와 손님 수 N.</summary>
        public (int Sum, int Count) YesterdayTally => AffinityJudge.Tally(Rules, YesterdayAffinities);

        /// <summary>그날의 방명록을 열고 일차 시작 스냅샷의 전날 방문 호감·입소문 추가 인원을 받는다. 작성 시간은 DayCycleSettings 기준값(초).</summary>
        public void BeginDay(float guestbookWriteSeconds, IReadOnlyList<VisitAffinity> yesterdayAffinities = null, int wordOfMouthBonus = 0)
        {
            Guestbook = new GuestbookLog(guestbookWriteSeconds);
            YesterdayAffinities = yesterdayAffinities ?? Array.Empty<VisitAffinity>();
            WordOfMouthBonus = wordOfMouthBonus;
            settledAffinities = null;
        }

        /// <summary>서빙이 끝난 방문의 방명록 글을 등록한다.</summary>
        public bool RecordMeal(string guestId, string visitId, string guestName, string line) =>
            Guestbook != null && Guestbook.TryBegin(guestId, visitId, guestName, line);

        /// <summary>방명록 작성 시간을 흘린다. 정지 중이면 0을 넘긴다.</summary>
        public void Tick(float deltaSeconds) => Guestbook?.Tick(deltaSeconds);

        /// <summary>오늘 끝난 방문의 호감과 다음 영업의 입소문 추가 인원을 한 번 확정한다. 다시 불리면 처음 값을 돌려준다.</summary>
        public (IReadOnlyList<VisitAffinity> Affinities, int NextBonus) SettleDay(IEnumerable<ServiceVisit> visits, IEnumerable<VisitResult> results)
        {
            if (settledAffinities == null)
            {
                // 💡 입맛 점수는 손님 입맛·요리 태그 데이터가 생길 때까지 0이다 (송예찬 확인 전).
                settledAffinities = AffinityJudge.SettleDay(Rules, visits, results, 0, out string error);
                if (error != null)
                    Debug.LogError("[DOB] " + error, this);
            }
            var (sum, count) = AffinityJudge.Tally(Rules, settledAffinities);
            return (settledAffinities, AffinityJudge.ExtraGuests(Rules, sum, count));
        }
    }
}
