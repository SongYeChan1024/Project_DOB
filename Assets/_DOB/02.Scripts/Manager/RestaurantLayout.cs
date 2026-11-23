// RestaurantLayout — 식당 씬의 배치 기준점과 좌석 상태를 Gameplay에 전달한다.
// 의사코드: 인스펙터의 좌석 순서로 영업용 좌석표를 만든다. 배치되지 않은 자리는 제외한다.
// 플레이어 위치에서 상호작용 반경 안에 있는 가장 가까운 좌석을 반환한다. 점유 여부는 별도로 확인한다.
using System;
using DOB.Systems;
using UnityEngine;

namespace DOB.Manager
{
    /// <summary>Restaurant의 LayoutAnchors 루트에 배치한다. 인덱스는 인스펙터의 좌석 순서다.</summary>
    public sealed class RestaurantLayout : MonoBehaviour
    {
        [SerializeField] private Transform playerSpawn;
        [SerializeField] private Transform guestEntrance;
        [SerializeField] private Transform greetingPoint;
        [SerializeField] private Transform kitchen;
        [SerializeField] private Transform stairsExit;
        [SerializeField] private Transform[] seats = Array.Empty<Transform>();
        [SerializeField] private Collider floor;

        private RestaurantSeating seating;

        /// <summary>손님과 플레이어의 입장 위치 및 응대 대기 위치.</summary>
        public Transform PlayerSpawn => playerSpawn;
        /// <summary>손님이 도착하거나 식당에서 떠나는 문.</summary>
        public Transform GuestEntrance => guestEntrance;
        /// <summary>손님이 입장 후 기다리는 위치.</summary>
        public Transform GreetingPoint => greetingPoint;
        /// <summary>주문을 조리하고 완성한 접시를 받는 위치.</summary>
        public Transform Kitchen => kitchen;
        /// <summary>식사를 마친 손님이 던전으로 내려가는 계단.</summary>
        public Transform StairsExit => stairsExit;

        /// <summary>플레이 가능한 배치가 연결되어 있는지 검사한다. 좌석이 없으면 안내 요청만 거절한다.</summary>
        public bool IsReady => playerSpawn && guestEntrance && greetingPoint && kitchen && stairsExit && floor;

        private void Awake() => seating = new RestaurantSeating(seats.Length);

        /// <summary>평면 바닥의 경계에서 캐릭터 반지름만큼 여유를 두고 XZ 이동을 제한한다.</summary>
        public Vector3 ClampToFloor(Vector3 position, float radius)
        {
            var bounds = floor.bounds;
            position.x = Mathf.Clamp(position.x, bounds.min.x + radius, bounds.max.x - radius);
            position.z = Mathf.Clamp(position.z, bounds.min.z + radius, bounds.max.z - radius);
            return position;
        }

        /// <summary>배치에서 현재 사용할 수 있는 빈 좌석이 하나라도 있는지 확인한다.</summary>
        public bool HasFreeSeat()
        {
            for (int i = 0; i < seats.Length; i++)
                if (GetSeat(i) && !seating.IsOccupied(i))
                    return true;

            return false;
        }

        /// <summary>존재하고 활성인 좌석 기준점. 제거된 자리와 잘못된 인덱스는 null이다.</summary>
        public Transform GetSeat(int index)
        {
            if (index < 0 || index >= seats.Length || !seats[index])
                return null;
            return seats[index].gameObject.activeInHierarchy ? seats[index] : null;
        }

        /// <summary>범위 안 가장 가까운 자리. 점유석 앞에서 옆 자리로 잘못 배정되지 않도록 점유석도 포함한다.</summary>
        public int FindNearbySeat(Vector3 position, float range)
        {
            int nearest = -1;
            float nearestSquared = range * range;
            for (int i = 0; i < seats.Length; i++)
            {
                var seat = GetSeat(i);
                if (!seat)
                    continue;
                var offset = seat.position - position;
                float squared = offset.x * offset.x + offset.z * offset.z;
                if (squared > nearestSquared || (nearest >= 0 && squared == nearestSquared))
                    continue;
                nearestSquared = squared;
                nearest = i;
            }
            return nearest;
        }

        /// <summary>다른 방문의 점유 또는 배정 중인 자리를 조회한다.</summary>
        public bool IsOccupied(int index) => seating.IsOccupied(index);

        /// <summary>빈 좌석을 이 방문에 배정한다. 손님이 의자까지 걷는 동안에도 이 자리를 확보한다.</summary>
        public bool TryAssign(int index, string visitId) => GetSeat(index) && seating.TryAssign(index, visitId);

        /// <summary>떠나거나 중단된 방문이 사용하던 자리만 해제한다.</summary>
        public void Release(int index, string visitId) => seating.Release(index, visitId);
    }
}
