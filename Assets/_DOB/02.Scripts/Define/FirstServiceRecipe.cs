// 첫 영업을 시작할 때 재고와 금화를 읽고, 첫 주문을 만들 때 지정 레시피와 조리 시간을 전달한다.
using System;
using UnityEngine;

namespace DOB.Define
{
    /// <summary>첫 서비스의 시작 재고와 구운 빵 레시피. 플레이 중 잔량은 각 영업 상태에서 관리한다.</summary>
    [CreateAssetMenu(menuName = "DOB/첫 영업 레시피", fileName = "SO_FirstService_")]
    public sealed class FirstServiceRecipe : ScriptableObject
    {
        [SerializeField, Min(0f)] private float cookSeconds;
        [SerializeField, Min(0)] private int startGarlic;
        [SerializeField, Min(1)] private int flourRequired;
        [SerializeField, Min(0)] private int garlicRequired;
        [SerializeField, Min(0)] private int breadBasePrice;
        [SerializeField, Min(0)] private int startGold;
        [Header("구운 빵과 마늘의 효과"), SerializeField, Min(0)] private int breadStamina;
        [SerializeField, Min(0)] private int breadTrap;
        [SerializeField, Min(0)] private int garlicPoison;

        public float CookSeconds => cookSeconds;
        public int StartGarlic => startGarlic;
        public int FlourRequired => flourRequired;
        public int GarlicRequired => garlicRequired;
        public int BreadBasePrice => breadBasePrice;
        public int StartGold => startGold;

        /// <summary>손님에게 실제로 필요한 여섯 축에서 이번 빵과 양념의 효과를 빼고 부족한 칸만 더한다.</summary>
        public int GetShortfall(Func<string, int> actualRisk, bool hasGarlic)
        {
            if (actualRisk == null) throw new ArgumentNullException(nameof(actualRisk));
            int Missing(string axis, int protection = 0) => Math.Max(0, actualRisk(axis) - protection);
            return Missing("독", hasGarlic ? garlicPoison : 0) + Missing("체력", breadStamina)
                + Missing("함정", breadTrap) + Missing("열") + Missing("냉기") + Missing("어둠");
        }
    }
}
