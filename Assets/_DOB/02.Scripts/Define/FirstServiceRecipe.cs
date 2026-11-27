// 첫 영업을 시작할 때 재고와 금화를 읽고, 첫 주문을 만들 때 지정 레시피와 조리 시간을 전달한다.
using System;
using UnityEngine;

namespace DOB.Define
{
    /// <summary>첫 서비스의 시작 재고와 구운 빵 레시피. 플레이 중 잔량은 각 영업 상태에서 관리한다.</summary>
    [CreateAssetMenu(menuName = "DOB/첫 영업 레시피", fileName = "SO_FirstService_")]
    public sealed class FirstServiceRecipe : ScriptableObject
    {
        /// <summary>화면 그림의 요리 id(T_UI_Dish{id}). 💡 화면 그림 이름과 맞춘 값.</summary>
        [SerializeField] private string dishId = "Bread";
        [SerializeField, Min(0f)] private float cookSeconds;
        [SerializeField, Min(0)] private int startGarlic;
        [SerializeField, Min(1)] private int flourRequired;
        [SerializeField, Min(0)] private int garlicRequired;
        [SerializeField, Min(0)] private int breadBasePrice;
        [SerializeField, Min(0)] private int startGold;
        [Header("구운 빵과 마늘의 효과"), SerializeField, Min(0)] private int breadStamina;
        [SerializeField, Min(0)] private int breadTrap;
        [SerializeField, Min(0)] private int garlicPoison;

        public string DishId => dishId;
        public float CookSeconds => cookSeconds;
        public int StartGarlic => startGarlic;
        public int FlourRequired => flourRequired;
        public int GarlicRequired => garlicRequired;
        public int BreadBasePrice => breadBasePrice;
        public int StartGold => startGold;

        /// <summary>이번 빵과 양념이 한 축을 덮는 칸 수. 효과가 없는 축은 0. 부족 계산은 ExpeditionJudge가 한다.</summary>
        public int Protection(string axis, bool hasGarlic)
        {
            switch (axis)
            {
                case "독": return hasGarlic ? garlicPoison : 0;
                case "체력": return breadStamina;
                case "함정": return breadTrap;
                default: return 0;
            }
        }
    }
}
