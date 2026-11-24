// 손님 응대를 열 때 FirstGuestService가 줌인 시간과 클로즈업 카메라의 크기·거리를 읽는다.
using UnityEngine;

namespace DOB.Define
{
    /// <summary>응대 진입 시 손님 얼굴 클로즈업 연출의 기준값. 실행 중 카메라 상태는 갖지 않는다.</summary>
    [CreateAssetMenu(menuName = "DOB/손님 클로즈업 연출", fileName = "SO_GuestCloseUp")]
    public sealed class GuestCloseUpSettings : ScriptableObject
    {
        [SerializeField, Min(0.05f)] private float zoomSeconds;
        [SerializeField, Min(0.05f)] private float orthographicSize;
        [SerializeField, Min(0.05f)] private float distance;

        /// <summary>전경과 얼굴 클로즈업 사이를 오가는 블렌드 시간(초).</summary>
        public float ZoomSeconds => zoomSeconds;
        /// <summary>클로즈업의 직교 크기(화면 세로 절반, 미터). 전경 카메라가 원근이면 쓰이지 않는다.</summary>
        public float OrthographicSize => orthographicSize;
        /// <summary>손님 얼굴에서 클로즈업 카메라까지의 거리(미터).</summary>
        public float Distance => distance;
    }
}
