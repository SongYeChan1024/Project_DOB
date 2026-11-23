// 손님 원고를 찾으면 응대 프리팹을 열고 수첩 메모를 복원한다. 버튼은 현재 방문 담당자에게 요청한다.
// 거절 사유는 대사창에 표시한다. 수락 또는 닫기 후 마지막 페이드가 끝나면 화면 인스턴스를 정리한다.
using System;
using System.Collections.Generic;
using DOB.Define;
using DOB.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DOB.Manager
{
    /// <summary>공용 응대 프리팹의 입력과 첫 영업의 손님 방문을 이어 준다.</summary>
    public sealed class FirstGuestDialogue : MonoBehaviour
    {
        [SerializeField] private GuestDialogueView viewPrefab;
        [SerializeField] private GuestDialogueProfile[] guests = Array.Empty<GuestDialogueProfile>();

        private readonly Dictionary<string, int[]> memosByGuest = new Dictionary<string, int[]>();
        private GuestDialogueView activeView;
        private Image fadeOverlay;
        private GuestDialogueProfile currentGuest;
        private Func<string, string> requestFollow;
        private Func<string, string> requestSendHome;

        public string FirstGuestId => guests.Length > 0 && guests[0] ? guests[0].GuestId : null;
        public bool BlocksWorldInput => activeView != null;

        /// <summary>요청이 수락되면 빈 문자열 또는 null, 거절되면 표시할 이유를 반환하는 함수를 연결한다.</summary>
        public void BindActions(Func<string, string> follow, Func<string, string> sendHome)
        {
            requestFollow = follow;
            requestSendHome = sendHome;
        }

        public bool TryOpenGuest(string guestId)
        {
            if (!isActiveAndEnabled || BlocksWorldInput || !viewPrefab || string.IsNullOrEmpty(guestId))
                return false;

            currentGuest = Array.Find(guests, guest => guest && guest.GuestId == guestId);
            if (!currentGuest)
                return false;

            activeView = Instantiate(viewPrefab);
            SceneManager.MoveGameObjectToScene(activeView.gameObject, gameObject.scene);
            // PF_GuestDialogue가 직접 소유하는 페이드 이미지의 입력 차단 상태로 전환 종료를 확인한다.
            var overlay = activeView.transform.Find("FadeOverlay");
            if (!activeView.TryGetComponent<Canvas>(out var canvas) || !overlay || !overlay.TryGetComponent(out fadeOverlay))
            {
                Debug.LogError("[DOB] 응대 프리팹의 Canvas 및 FadeOverlay/Image 연결을 확인하세요.", this);
                CancelDialogue();
                return false;
            }
            canvas.sortingOrder = 200;

            activeView.FollowClicked += OnFollow;
            activeView.SendHomeConfirmed += OnSendHome;
            activeView.CloseClicked += activeView.Close;
            activeView.ClueClicked += OnClue;
            activeView.MemoChanged += OnMemo;
            if (activeView.Open(BuildDisplayData(currentGuest)))
                return true;
            CancelDialogue();
            return false;
        }

        private void LateUpdate()
        {
            if (activeView && !activeView.IsOpen && !fadeOverlay.raycastTarget)
                CancelDialogue();
        }

        private void OnDisable() => CancelDialogue();

        public void CancelDialogue()
        {
            if (activeView)
            {
                activeView.FollowClicked -= OnFollow;
                activeView.SendHomeConfirmed -= OnSendHome;
                activeView.CloseClicked -= activeView.Close;
                activeView.ClueClicked -= OnClue;
                activeView.MemoChanged -= OnMemo;
                activeView.gameObject.SetActive(false);
                Destroy(activeView.gameObject);
            }
            activeView = null;
            currentGuest = null;
            fadeOverlay = null;
        }

        private void OnFollow() => ApplyRequest(requestFollow);

        private void OnSendHome()
        {
            if (currentGuest.CanSendHome)
                ApplyRequest(requestSendHome);
        }

        private void ApplyRequest(Func<string, string> request)
        {
            string reason = request == null ? "아직 요청을 처리할 수 없어요." : request(currentGuest.GuestId);
            if (!activeView) return; // 수락 처리 중 방문이나 씬이 끝날 수 있다.
            if (string.IsNullOrEmpty(reason))
                activeView.Close();
            else
                activeView.ShowNotice(reason);
        }

        private void OnClue(string phrase) => activeView.ShowDictionaryEntry(phrase, currentGuest.LookupClue(phrase));

        private void OnMemo(int axis, int memo)
        {
            var memos = memosByGuest[currentGuest.GuestId];
            if (axis >= 0 && axis < memos.Length)
                memos[axis] = memo;
        }

        private GuestDialogueData BuildDisplayData(GuestDialogueProfile guest)
        {
            var data = new GuestDialogueData
            {
                guestId = guest.GuestId,
                guestName = guest.GuestName,
                raceJob = guest.RaceJob,
                destinationLabel = guest.DestinationLabel,
                line = guest.OrderLine,
                canSendHome = guest.CanSendHome,
                clues = new ClueMarkerData[guest.ObservationCount]
            };
            for (int i = 0; i < data.clues.Length; i++)
            {
                var clue = guest.GetObservation(i);
                data.clues[i] = new ClueMarkerData { description = clue.description, position = clue.position };
            }
            // 사전에서 표현의 뜻을 찾아도 미확인 위험 수치 여섯 칸은 '?'를 유지한다.
            if (memosByGuest.TryGetValue(guest.GuestId, out var memos))
                data.memos = memos;
            else
                memosByGuest.Add(guest.GuestId, data.memos);
            return data;
        }
    }
}
