// 첫 주문을 만들 때 지정 레시피와 조리 시간, 기본가를 전달한다. 시작 금화·양념 재고는 시장 기준(SO_Market)에 있다.
using System;
using UnityEngine;

namespace DOB.Define
{
    /// <summary>첫 서비스의 구운 빵 레시피.</summary>
    [CreateAssetMenu(menuName = "DOB/첫 영업 레시피", fileName = "SO_FirstService_")]
    public sealed class FirstServiceRecipe : ScriptableObject
    {
        /// <summary>화면 그림의 요리 id(T_UI_Dish{id}). 💡 화면 그림 이름과 맞춘 값.</summary>
        [SerializeField] private string dishId = "Bread";
        [SerializeField, Min(0f)] private float cookSeconds;
        [SerializeField, Min(1)] private int flourRequired;
        [SerializeField, Min(0)] private int garlicRequired;
        [SerializeField, Min(0)] private int breadBasePrice;
        [Header("구운 빵과 마늘의 효과"), SerializeField, Min(0)] private int breadStamina;
        [SerializeField, Min(0)] private int breadTrap;
        [SerializeField, Min(0)] private int garlicPoison;

        public string DishId => dishId;
        public float CookSeconds => cookSeconds;
        public int FlourRequired => flourRequired;
        public int GarlicRequired => garlicRequired;
        public int BreadBasePrice => breadBasePrice;

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
