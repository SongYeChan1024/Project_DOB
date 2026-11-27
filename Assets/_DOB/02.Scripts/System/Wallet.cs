// =====================================================================
// Wallet — 가게 금화의 유일한 소유자 (순수 C#)
// 호출자 : EconomyManager(하루 시작·복원 때 생성, 서빙 대금 Earn), Market(구매 TrySpend)
// 상태   : 금화(0 이상)
// 의사코드:
//   생성     — 0 이상의 금화로 시작한다. 음수면 예외.
//   TrySpend — 음수이거나 가진 금화보다 많으면 아무것도 바꾸지 않고 false. 아니면 뺀다.
//   Earn     — 음수면 false. 아니면 더한다(넘치면 예외로 막는다).
// =====================================================================
using System;

namespace DOB.Systems
{
    /// <summary>가게 금화. 금화를 바꾸는 곳은 이 클래스 하나다.</summary>
    public sealed class Wallet
    {
        public Wallet(int gold)
        {
            if (gold < 0) throw new ArgumentOutOfRangeException(nameof(gold));
            Gold = gold;
        }

        /// <summary>지금 금화.</summary>
        public int Gold { get; private set; }

        /// <summary>금화를 쓴다. 음수·잔액 초과면 바꾸지 않고 false.</summary>
        public bool TrySpend(int amount)
        {
            if (amount < 0 || amount > Gold)
                return false;
            Gold -= amount;
            return true;
        }

        /// <summary>금화를 더한다. 음수면 false.</summary>
        public bool Earn(int amount)
        {
            if (amount < 0)
                return false;
            Gold = checked(Gold + amount);
            return true;
        }
    }
}
