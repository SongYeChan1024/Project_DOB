// =====================================================================
// SaveManager — 마감 정산 저장의 총괄 (FeatureManagerMap F13)
// 역할   : 그날 정산 결과를 다음 일차 시작 스냅샷으로 자동 슬롯에 한 번 쓰고, 다시 불려도 처음 결과를 돌려준다.
// 호출자 : GameManager (하루를 열 때 Begin, 마감 때 Settle, 다음 일차·수동 저장 때 Settled)
// 호출   : FirstServiceSaveStore (SavePreparation·SettleDay)
// 상태   : 이 하루의 정산 결과(한 번 쓰면 바꾸지 않는다)
// 의사코드:
//   Begin(저장소)  — 하루를 열 때 정산 결과를 비운다.
//   Settle(값들)  — 이미 정산했으면 그 결과를 그대로 돌려준다. 새 흐름(새 게임·수동 슬롯으로 시작)이면 자동 슬롯에
//                   다음 일차를 바로 쓰고, 아니면 저장소의 멱등 SettleDay에 맡긴다(같은 날 두 번째 정산은 기존 저장본).
//                   파일 오류는 호출자에게 전달하고 결과를 남기지 않아 다음 마감 확인 때 다시 시도한다.
// 일차 시작·수동 저장·데모 완료 슬롯 쓰기는 아직 GameManager가 저장소에 직접 요청한다.
// =====================================================================
using System;
using System.Collections.Generic;
using DOB.Define;
using DOB.Systems;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>마감 정산 저장을 한 번만 쓰고 그 결과를 보관한다.</summary>
    [DisallowMultipleComponent]
    public sealed class SaveManager : MonoBehaviour
    {
        private FirstServiceSaveStore store;

        /// <summary>이 하루의 정산 결과(다음 일차 시작 스냅샷). 정산 전이면 null.</summary>
        public FirstServiceSaveData Settled { get; private set; }

        /// <summary>하루를 연다. 저장소가 없으면 예외.</summary>
        public void Begin(FirstServiceSaveStore saveStore)
        {
            store = saveStore ?? throw new ArgumentNullException(nameof(saveStore));
            Settled = null;
        }

        /// <summary>마감 정산을 자동 슬롯에 한 번 쓴다. 이미 썼으면 그 결과. 파일 오류는 그대로 던진다.</summary>
        public FirstServiceSaveData Settle(bool startsNewTimeline, int completedDay, int gold, IEnumerable<MaterialCount> spices,
            IEnumerable<MaterialCount> materials, IEnumerable<VisitResult> reports, IEnumerable<VisitAffinity> affinities,
            int wordOfMouthBonus, IEnumerable<GuestRecord> guests, int? nextDaySeed, FloorProgressState floors,
            IEnumerable<string> unlockedRecipes = null, int? burners = null)
        {
            if (Settled != null)
                return Settled;
            if (store == null)
                throw new InvalidOperationException("Begin 전에는 정산할 수 없습니다.");
            Settled = startsNewTimeline
                ? store.SavePreparation(FirstServiceSaveSlot.Auto, checked(completedDay + 1), gold, spices, materials, reports,
                    affinities, wordOfMouthBonus, guests, nextDaySeed, null, floors, unlockedRecipes, burners)
                : store.SettleDay(completedDay, gold, spices, materials, reports, affinities, wordOfMouthBonus, guests, nextDaySeed, floors, unlockedRecipes, burners);
            return Settled;
        }
    }
}
