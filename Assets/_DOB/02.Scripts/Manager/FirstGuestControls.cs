using UnityEngine;

namespace DOB.Manager
{
    /// <summary>Gameplay가 읽는 이동·상호작용 입력. 배치한 입력 컴포넌트가 장치별 값을 제공한다.</summary>
    public abstract class FirstGuestControls : MonoBehaviour
    {
        /// <summary>현재 프레임의 좌우·전후 이동.</summary>
        public abstract Vector2 Move { get; }
        /// <summary>현재 프레임에 상호작용 버튼을 눌렀는지 확인한다.</summary>
        public abstract bool InteractPressed { get; }
    }
}
