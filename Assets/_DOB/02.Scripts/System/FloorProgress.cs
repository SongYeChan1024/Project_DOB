// =====================================================================
// FloorProgress — 다음 층 개방과 층별 미확인 위험 공개 (순수 C#)
// 호출자 : SettlementManager(하루를 열 때 저장된 상태로 생성, 귀환 보고를 처음 확정할 때 Apply, 정산 저장 때 Snapshot)
// 호출   : FloorProgressRules(개방 승수·공개 수·층별 축)
// 상태   : 열린 층 수, 최심층 성공 수, 층별 공개 축, 오늘 반영한 방문 ID와 그 결과
// 의사코드:
//   생성     — 저장된 상태가 없으면 1층만 열림·성공 0·공개 없음.
//   Apply(보고)
//     — 이미 반영한 방문 ID면 처음 결과를 돌려주고 아무것도 바꾸지 않는다.
//     — 공개: 그 층의 축 중 미확인인 것을, 이 방문에서 부족했던 축부터 위험 축 순서로, 모자라면 나머지 미확인 축을 같은 순서로
//             최대 공개 수만큼 고른다(💡 채움 규칙 송예찬 확인 전).
//     — 개방: 목적지가 열린 가장 깊은 층이고 성공 이상이면 성공 수 +1. 기준에 이르면 다음 층을 열고 성공 수를 0으로.
//             마지막 층이면 더 열지 않는다(💡).
//   Snapshot — 저장할 상태(열린 층·성공 수·층별 공개 축).
// 같은 보고를 하루 안에 다시 확정해도 세지 않는다. 일차 시작 스냅샷의 상태는 그날 보고를 반영하기 전이라,
// 불러와 다시 확정하면 정확히 한 번 반영된다.
// =====================================================================
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>한 층의 공개된 위험 축(저장용).</summary>
    [DataContract]
    public sealed class FloorReveal
    {
        [DataMember(Name = "floor")] public int Floor { get; private set; }
        [DataMember(Name = "axes")] private string[] axes;

        public IReadOnlyList<string> Axes => axes ?? Array.Empty<string>();

        public FloorReveal(int floor, IEnumerable<string> axes)
        {
            Floor = floor;
            this.axes = axes == null ? null : new List<string>(axes).ToArray();
        }
    }

    /// <summary>저장하는 층 진행 상태.</summary>
    [DataContract]
    public sealed class FloorProgressState
    {
        [DataMember(Name = "openFloors")] public int OpenFloors { get; private set; }
        [DataMember(Name = "deepestWins")] public int DeepestWins { get; private set; }
        [DataMember(Name = "revealed")] private FloorReveal[] revealed;

        public IReadOnlyList<FloorReveal> Revealed => revealed ?? Array.Empty<FloorReveal>();

        public FloorProgressState(int openFloors, int deepestWins, IEnumerable<FloorReveal> revealed)
        {
            OpenFloors = openFloors;
            DeepestWins = deepestWins;
            this.revealed = revealed == null ? null : new List<FloorReveal>(revealed).ToArray();
        }

        /// <summary>열린 층 1 이상, 성공 수 0 이상, 공개 층은 1 이상에서 겹치지 않고 축은 위험 축 이름이다.</summary>
        public bool Validate()
        {
            if (OpenFloors < 1 || DeepestWins < 0)
                return false;
            var floors = new HashSet<int>();
            foreach (var reveal in Revealed)
            {
                if (reveal == null || reveal.Floor < 1 || !floors.Add(reveal.Floor))
                    return false;
                var axes = new HashSet<string>();
                foreach (string axis in reveal.Axes)
                    if (!IsRiskAxis(axis) || !axes.Add(axis))
                        return false;
            }
            return true;
        }

        private static bool IsRiskAxis(string axis)
        {
            foreach (string known in RiskAxes.All)
                if (known == axis) return true;
            return false;
        }
    }

    /// <summary>보고 하나를 반영한 결과.</summary>
    public sealed class FloorOutcome
    {
        /// <summary>이번에 공개한 위험 축(위험 축 순서가 아니라 고른 순서).</summary>
        public IReadOnlyList<string> Revealed { get; }
        /// <summary>이번에 새로 연 층. 열지 않았으면 null.</summary>
        public int? OpenedFloor { get; }

        public FloorOutcome(IReadOnlyList<string> revealed, int? openedFloor)
        {
            Revealed = revealed;
            OpenedFloor = openedFloor;
        }
    }

    /// <summary>열린 층과 층별 공개 위험을 소유한다.</summary>
    public sealed class FloorProgress
    {
        private readonly FloorProgressRules rules;
        private readonly Dictionary<int, HashSet<string>> revealed = new Dictionary<int, HashSet<string>>();
        private readonly Dictionary<string, FloorOutcome> applied = new Dictionary<string, FloorOutcome>();

        public FloorProgress(FloorProgressRules rules, FloorProgressState saved = null)
        {
            this.rules = rules ? rules : throw new ArgumentNullException(nameof(rules));
            if (saved != null && !saved.Validate())
                throw new ArgumentException("층 진행 상태가 올바르지 않습니다.", nameof(saved));
            OpenFloors = saved?.OpenFloors ?? 1;
            DeepestWins = saved?.DeepestWins ?? 0;
            if (saved != null)
                foreach (var reveal in saved.Revealed)
                    revealed[reveal.Floor] = new HashSet<string>(reveal.Axes);
        }

        /// <summary>열린 층 수. 1부터.</summary>
        public int OpenFloors { get; private set; }
        /// <summary>열린 가장 깊은 층의 성공 이상 귀환 수.</summary>
        public int DeepestWins { get; private set; }

        /// <summary>그 층에서 공개된 위험 축인지.</summary>
        public bool IsRevealed(int floor, string axis) => revealed.TryGetValue(floor, out var axes) && axes.Contains(axis);

        /// <summary>오늘 반영한 그 방문의 결과. 없으면 null.</summary>
        public FloorOutcome OutcomeOf(string visitId) => visitId != null && applied.TryGetValue(visitId, out var outcome) ? outcome : null;

        /// <summary>보고 하나를 반영한다. 같은 방문을 다시 넘기면 처음 결과를 돌려주고 상태를 바꾸지 않는다.</summary>
        public FloorOutcome Apply(VisitResult report)
        {
            if (report == null || string.IsNullOrEmpty(report.VisitId))
                throw new ArgumentException("방문 ID가 있는 보고가 필요합니다.", nameof(report));
            if (applied.TryGetValue(report.VisitId, out var earlier))
                return earlier;

            var picked = Reveal(report);
            int? opened = null;
            if (report.Floor == OpenFloors && (report.Grade == ExpeditionGrade.Success || report.Grade == ExpeditionGrade.GreatSuccess))
            {
                DeepestWins++;
                if (DeepestWins >= rules.OpenWins && OpenFloors < rules.FloorCount)
                {
                    OpenFloors++;
                    DeepestWins = 0;
                    opened = OpenFloors;
                }
            }
            var outcome = new FloorOutcome(picked, opened);
            applied[report.VisitId] = outcome;
            return outcome;
        }

        /// <summary>저장할 상태.</summary>
        public FloorProgressState Snapshot()
        {
            var reveals = new List<FloorReveal>();
            foreach (var pair in revealed)
            {
                var axes = new List<string>();
                // 위험 축 순서로 저장해 같은 상태면 같은 파일이 되게 한다.
                foreach (string axis in RiskAxes.All)
                    if (pair.Value.Contains(axis)) axes.Add(axis);
                reveals.Add(new FloorReveal(pair.Key, axes));
            }
            reveals.Sort((a, b) => a.Floor.CompareTo(b.Floor));
            return new FloorProgressState(OpenFloors, DeepestWins, reveals);
        }

        private List<string> Reveal(VisitResult report)
        {
            var floorAxes = rules.AxesOf(report.Floor);
            if (!revealed.TryGetValue(report.Floor, out var known))
                revealed[report.Floor] = known = new HashSet<string>();
            var picked = new List<string>();
            // 첫 바퀴는 부족했던 축만, 둘째 바퀴는 나머지 미확인 축. 둘 다 위험 축 순서다.
            for (int pass = 0; pass < 2; pass++)
                for (int i = 0; i < RiskAxes.All.Count && picked.Count < rules.RevealPerReport; i++)
                {
                    string axis = RiskAxes.All[i];
                    bool shortOnAxis = report.Shortfalls != null && i < report.Shortfalls.Count && report.Shortfalls[i] > 0;
                    if ((pass == 0) != shortOnAxis || known.Contains(axis) || !Contains(floorAxes, axis))
                        continue;
                    known.Add(axis);
                    picked.Add(axis);
                }
            return picked;
        }

        private static bool Contains(IReadOnlyList<string> list, string value)
        {
            foreach (string item in list)
                if (item == value) return true;
            return false;
        }
    }
}
