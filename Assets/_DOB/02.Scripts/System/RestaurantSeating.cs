// RestaurantSeating — 한 번의 식당 영업에서 좌석별 방문 ID를 관리한다.
// 의사코드: 좌석 범위와 방문 ID를 검사한다. 빈 좌석을 한 방문에 한 번만 배정한다.
// 해제는 배정 당시와 같은 방문만 허용한다. 거절하면 배정 상태를 그대로 둔다.
using System;

namespace DOB.Systems
{
    /// <summary>식당 하나의 좌석 점유 상태. 재방문을 구분하기 위해 손님 ID 대신 방문 ID를 사용한다.</summary>
    public sealed class RestaurantSeating
    {
        private readonly string[] occupants;

        /// <summary>배치된 좌석 수로 새 영업의 빈 좌석표를 만든다.</summary>
        public RestaurantSeating(int seatCount)
        {
            occupants = new string[seatCount];
        }

        /// <summary>유효한 좌석에 방문이 배정되어 있는지 조회한다.</summary>
        public bool IsOccupied(int index) => IsValid(index) && occupants[index] != null;

        /// <summary>한 좌석과 한 방문의 중복 배정을 모두 거절한다.</summary>
        public bool TryAssign(int index, string visitId)
        {
            if (!IsValid(index) || string.IsNullOrEmpty(visitId)
                || IsOccupied(index) || Array.IndexOf(occupants, visitId) >= 0)
                return false;

            occupants[index] = visitId;
            return true;
        }

        /// <summary>일치하는 방문의 좌석만 해제한다. 늦게 도착한 이전 방문의 요청은 무시한다.</summary>
        public void Release(int index, string visitId)
        {
            if (IsValid(index) && occupants[index] == visitId)
                occupants[index] = null;
        }

        private bool IsValid(int index) => index >= 0 && index < occupants.Length;
    }
}
