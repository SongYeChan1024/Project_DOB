// GuestAgent — 식당 안 손님 오브젝트 하나. 방문 ID와 생성된 방문 원고, 지금의 이동 목표와 배정 좌석만 가진다.
// 호출자: GuestManager(입장 시 생성·초기화), FirstGuestService(매 프레임 목표 결정과 이동).
// 상태  : 방문·손님 ID, 방문 원고, 이동 단계, 좌석 번호. 방문 단계·대기 시간은 ServiceRoster가 소유한다.
// 의사코드: 입장하면 입구에 놓고 Entering. WalkTo는 수평으로 목표를 바라보며 직선으로 걷고 도착 여부를 돌려준다.
using DOB.Systems;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>손님 한 명의 그레이박스 직선 이동. 길찾기·충돌 회피는 하지 않는다.</summary>
    public sealed class GuestAgent : MonoBehaviour
    {
        /// <summary>지금 걷고 있는 목표. 출구·계단에 도착하면 Gone이 되고 오브젝트를 끈다.</summary>
        public enum Movement { Entering, Queued, Following, ToSeat, Seated, ToExit, ToStairs, Gone }

        public string VisitId { get; private set; }
        public string GuestId { get; private set; }
        /// <summary>대사·단서·실제 위험을 가진 방문 원고.</summary>
        public GuestDialogueVisit Visit { get; private set; }
        public Movement Move { get; internal set; }
        /// <summary>배정받은 좌석 번호. 없으면 -1.</summary>
        public int SeatIndex { get; internal set; } = -1;
        /// <summary>응대 클로즈업이 바라보는 얼굴 기준점(캐릭터 프리팹의 Anchors/FacePoint).</summary>
        public Transform Face { get; private set; }

        internal void Begin(GuestDialogueVisit visit, Transform entrance)
        {
            Visit = visit;
            VisitId = visit.VisitId;
            GuestId = visit.GuestId;
            Face = transform.Find("Anchors/FacePoint");
            Move = Movement.Entering;
            SeatIndex = -1;
            transform.SetPositionAndRotation(entrance.position, entrance.rotation);
            gameObject.SetActive(true);
        }

        /// <summary>목표를 향해 수평으로 돌아 speed × dt만큼 걷는다. 도착하면 true.</summary>
        internal bool WalkTo(Vector3 target, float speed, float dt)
        {
            var direction = target - transform.position;
            direction.y = 0f;
            if (direction.sqrMagnitude > 0.0001f)
                transform.rotation = Quaternion.LookRotation(direction);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * dt);
            return transform.position == target;
        }
    }
}
