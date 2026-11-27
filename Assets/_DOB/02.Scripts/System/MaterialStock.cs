// =====================================================================
// MaterialStock — 이름·id별 재고 (순수 C#). 던전 재료와 양념이 서로 다른 인스턴스를 쓴다
// 호출자 : EconomyManager (일차 시작 때 던전 재료·양념 재고를 각각 생성), ReturnReportLog (보고 확정 때 재료 TryAdd),
//          Market (구매한 양념 TryAdd), OrderBook (조리 시작 때 양념 TryConsume), GameManager (저장 때 All)
// 상태   : 재료 이름별 보유 수 (0 이상)
// 의사코드:
//   생성   — 저장된 시작 재고를 받아 채운다. 이름이 비었거나 수량이 음수인 항목은 예외.
//   Count  — 가진 적 없는 재료는 0.
//   TryAdd — 이름이 비었거나 수량이 1보다 작으면 아무것도 바꾸지 않고 false.
//   TryConsume — 음수이거나 보유 수보다 많으면 아무것도 바꾸지 않고 false. 0개는 바꾸지 않고 true.
// 던전 재료를 쓰는 경로(던전 재료 요리)는 후속이다. 양념은 조리 시작 때 쓴다.
// =====================================================================
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DOB.Systems
{
    /// <summary>저장과 조회에 쓰는 재료 한 종류의 보유 수.</summary>
    [DataContract]
    public sealed class MaterialCount
    {
        [DataMember(Name = "id")] public string Id { get; private set; }
        [DataMember(Name = "count")] public int Count { get; private set; }

        public MaterialCount(string id, int count)
        {
            Id = id;
            Count = count;
        }
    }

    /// <summary>품목별 보유 수. 던전 재료와 양념은 서로 다른 인스턴스이며 금화는 다루지 않는다.</summary>
    public sealed class MaterialStock
    {
        private readonly Dictionary<string, int> counts = new Dictionary<string, int>();

        public MaterialStock(IEnumerable<MaterialCount> start = null)
        {
            if (start == null)
                return;
            foreach (var entry in start)
            {
                if (entry == null || string.IsNullOrEmpty(entry.Id) || entry.Count < 0)
                    throw new ArgumentException("재료 이름과 0 이상의 수량이 필요합니다.", nameof(start));
                counts[entry.Id] = Count(entry.Id) + entry.Count;
            }
        }

        /// <summary>재료의 보유 수. 가진 적 없으면 0.</summary>
        public int Count(string id) => id != null && counts.TryGetValue(id, out int n) ? n : 0;

        /// <summary>보유 중인 재료 전체. 저장할 때 쓴다.</summary>
        public List<MaterialCount> All()
        {
            var all = new List<MaterialCount>(counts.Count);
            foreach (var pair in counts)
                all.Add(new MaterialCount(pair.Key, pair.Value));
            return all;
        }

        /// <summary>재료를 더한다. 잘못된 요청은 재고를 바꾸지 않는다.</summary>
        public bool TryAdd(string id, int amount)
        {
            if (string.IsNullOrEmpty(id) || amount < 1)
                return false;
            counts[id] = checked(Count(id) + amount);
            return true;
        }

        /// <summary>보유 수에서 뺀다. 음수·부족이면 바꾸지 않고 false.</summary>
        public bool TryConsume(string id, int amount)
        {
            if (amount < 0 || amount > Count(id))
                return false;
            if (amount > 0)
                counts[id] -= amount;
            return true;
        }
    }
}
