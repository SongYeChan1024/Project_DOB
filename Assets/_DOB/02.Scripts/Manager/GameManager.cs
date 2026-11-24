// =====================================================================
// GameManager — Gameplay 하루 진행의 진입점
// 역할   : 씬 조립이 끝난 뒤 하루를 연다. 응대·주방을 영업 UI에 연결하고, 시작 금화·재고로 주방을 열고,
//          정산과 수동 저장을 처리한다. 금화·재고 값은 주방(FirstGuestKitchen)이 소유하며 여기서 보관하지 않는다.
// 호출자 : BasementComposition (Prepare → 로딩 화면 닫힘 → StartService)
// 호출   : FirstGuestService, FirstGuestKitchen, FirstServiceSaveStore, BasementPauseMenu, BasementComposition.EnterDay
// 상태   : 현재 일차, 새 진행 여부, 이번 일차의 정산 결과, 준비 완료 여부
// 의사코드:
//   Prepare  — 연결이 하나라도 없으면 아무것도 바꾸지 않고 실패. 일차를 정하고 응대·주방·정산 버튼을 UI에 연결한 뒤
//              시작 스냅샷으로 주방을 연다. 자동 슬롯이 없으면 1일차 시작을 저장한다(파일 오류는 경고만).
//   StartService — 로딩 화면이 닫힌 뒤 일시 정지 메뉴에 저장소·일차를 넘기고 응대·주방을 켠다.
//   Settle   — 서빙 뒤에만. 정산 결과가 이미 있으면 재사용해 금화를 두 번 지급하지 않고 다음 일차로 진입한다.
//   OnDisable — 구독을 풀고 응대·주방을 끈다.
// =====================================================================
using System;
using System.IO;
using DOB.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace DOB.Manager
{
    /// <summary>게임 베이스에서 하루 한 판의 시작·저장·정산을 맡는다. 다른 Manager는 이 클래스를 참조하지 않는다.</summary>
    [DisallowMultipleComponent]
    public sealed class GameManager : MonoBehaviour
    {
        /// <summary>손님 입장·응대·착석을 진행하는 첫 손님 서비스.</summary>
        [SerializeField] private FirstGuestService service;
        /// <summary>주문·조리·서빙과 영업 중 금화·재고를 소유하는 첫 손님 주방.</summary>
        [SerializeField] private FirstGuestKitchen kitchen;

        private FirstServiceSaveStore saves;
        private BasementPauseMenu pauseMenu;
        private Button settleButton;
        private bool newTimeline;
        private FirstServiceSaveData settledCheckpoint;

        /// <summary>현재 영업 중인 일차. 1부터 시작한다.</summary>
        public int Day { get; private set; } = 1;
        /// <summary>로딩 화면이 닫히고 응대·주방이 켜진 상태.</summary>
        public bool IsReady { get; private set; }

        /// <summary>로딩 화면이 떠 있는 동안 하루를 준비한다. 연결이 빠졌으면 아무것도 바꾸지 않고 false.</summary>
        public bool Prepare(FirstServiceSaveStore store, (int Day, int Gold, int GarlicStock)? start, bool startsNewTimeline, BasementHud hud)
        {
            // 일시 정지 메뉴는 BasementUI의 같은 Canvas에 붙어 있다 (BasementSceneBuilder.CreateHud).
            var menu = hud ? hud.GetComponent<BasementPauseMenu>() : null;
            if (!service || !kitchen || store == null || !menu)
                return false;

            saves = store;
            pauseMenu = menu;
            newTimeline = startsNewTimeline;
            Day = start?.Day ?? 1;

            service.ConnectHud(hud.Guidance, hud.Dialogue);
            kitchen.ConnectHud(hud);
            settleButton = hud.Settle;
            settleButton.interactable = false;
            settleButton.onClick.AddListener(Settle);
            kitchen.StartDay(start?.Gold ?? kitchen.DayStartGold, start?.GarlicStock ?? kitchen.DayStartGarlic);
            try
            {
                if (!saves.TryLoad(FirstServiceSaveSlot.Auto, out _))
                    saves.SavePreparation(FirstServiceSaveSlot.Auto, Day, kitchen.DayStartGold, kitchen.DayStartGarlic);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Debug.LogWarning("[DOB] 첫 자동 저장을 만들 수 없습니다. " + e.Message, this);
            }
            return true;
        }

        /// <summary>로딩 화면이 닫힌 뒤 영업 입력을 연다.</summary>
        public void StartService()
        {
            pauseMenu.Initialize(saves, Day);
            pauseMenu.SaveRequested += SaveManual;
            service.enabled = true;
            kitchen.enabled = true;
            IsReady = service.enabled && kitchen.enabled;
        }

        /// <summary>현재 영업을 계속하면서 그 일차의 시작 스냅샷을 수동 슬롯에 기록한다. 파일 오류는 호출자에게 전달한다.</summary>
        public void SaveManual() => saves.SavePreparation(FirstServiceSaveSlot.Manual, Day, kitchen.DayStartGold, kitchen.DayStartGarlic);

        /// <summary>현재 방문이 서빙된 뒤에만 다음 준비 일차를 자동 저장하고 그 시점으로 진입한다.</summary>
        public void Settle()
        {
            if (!IsReady || BasementComposition.IsEntering || !kitchen.HasServed)
                return;
            try
            {
                // 새 게임 또는 수동 슬롯 선택 시, 첫 정산에 한해 이 플레이의 결과가 자동 슬롯을 이어받는다.
                // 다음 씬을 열지 못해 버튼을 다시 눌러도 이미 확정한 정산을 사용한다.
                if (settledCheckpoint == null)
                    settledCheckpoint = newTimeline
                        ? saves.SavePreparation(FirstServiceSaveSlot.Auto, checked(Day + 1), kitchen.Gold, kitchen.GarlicStock)
                        : saves.SettleDay(Day, kitchen.Gold, kitchen.GarlicStock);
                BasementComposition.EnterDay(settledCheckpoint);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Debug.LogWarning("[DOB] 영업 정산을 저장할 수 없습니다. " + e.Message, this);
            }
        }

        private void Update()
        {
            if (IsReady && settleButton)
                settleButton.interactable = kitchen.HasServed && !BasementComposition.IsEntering && !BasementPauseMenu.IsPaused;
        }

        private void OnDisable()
        {
            IsReady = false;
            if (settleButton) settleButton.onClick.RemoveListener(Settle);
            if (pauseMenu) pauseMenu.SaveRequested -= SaveManual;
            if (kitchen) kitchen.enabled = false;
            if (service) service.enabled = false;
        }
    }
}
