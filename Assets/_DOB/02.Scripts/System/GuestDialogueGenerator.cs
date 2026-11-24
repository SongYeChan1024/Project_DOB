using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DOB.Define;

namespace DOB.Systems
{
    /// <summary>방문의 실제 위험을 먼저 계산하고, 가장 큰 양수 축을 가리키는 원고를 한 번 고른다.</summary>
    public static class GuestDialogueGenerator
    {
        private static readonly Regex Phrase = new Regex(@"\[\[([^\[\]]+)\]\]");

        public static GuestDialogueVisit Generate(GuestDialogueRules rules, GuestVisitInput input, Random random)
        {
            if (rules == null) throw new ArgumentNullException(nameof(rules));
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (random == null) throw new ArgumentNullException(nameof(random));
            if (string.IsNullOrWhiteSpace(input.GuestId) || string.IsNullOrWhiteSpace(input.VisitId))
                throw new ArgumentException("손님 ID와 방문 ID가 필요합니다.", nameof(input));
            if (input.Day < 1 || !rules.Causes.Any(r => r.condition == DialogueCondition.Floor && r.floor == input.Floor))
                throw new ArgumentException("일차와 목적지 층을 확인하세요.", nameof(input));

            var active = rules.Causes.Where(rule => input.Day >= rule.firstDay && Applies(rule, input)).ToList();
            var risk = active.GroupBy(rule => rule.axis).ToDictionary(g => g.Key, g => g.Sum(rule => rule.delta));
            var clues = new List<GuestVisitClue>();
            foreach (var cause in active)
            {
                if (!string.IsNullOrEmpty(cause.observation))
                    clues.Add(new GuestVisitClue(cause, DialogueClueChannel.Observation, cause.observation));
                if (!string.IsNullOrEmpty(cause.news))
                    clues.Add(new GuestVisitClue(cause, DialogueClueChannel.News, cause.news));
            }

            var candidates = active.Where(r => r.delta > 0 && risk[r.axis] > 0).ToList();
            var lines = new List<string>();
            string firstAxis = null;
            int limit = input.Affinity >= rules.SecondClueAffinity ? 2 : 1;
            while (lines.Count < limit && candidates.Count > 0)
            {
                // 두 번째 단서는 다른 양수 축이 있으면 그중 큰 축을 먼저 고른다.
                var axes = candidates.Select(r => r.axis).Distinct().ToList();
                if (axes.Count > 1 && firstAxis != null) axes.Remove(firstAxis);
                int highest = axes.Max(a => risk[a]);
                var tiedAxes = axes.Where(a => risk[a] == highest).ToList();
                string axis = tiedAxes[random.Next(tiedAxes.Count)];
                var onAxis = candidates.Where(r => r.axis == axis).ToList();
                var chosen = onAxis[random.Next(onAxis.Count)];
                candidates.Remove(chosen);
                firstAxis = axis;
                int variant = random.Next(1 + (chosen.alternateLines?.Length ?? 0));
                string lineVariant = variant == 0 ? chosen.line : chosen.alternateLines[variant - 1];
                if (string.IsNullOrWhiteSpace(lineVariant) || !Phrase.IsMatch(lineVariant))
                    throw new InvalidOperationException(chosen.causeId + "에 클릭할 표현을 넣은 대사가 필요합니다.");
                lines.Add(lineVariant);
                foreach (string word in Phrase.Matches(lineVariant).Cast<Match>().Select(m => m.Groups[1].Value).Distinct())
                    clues.Add(new GuestVisitClue(chosen, DialogueClueChannel.Dialogue, word));
            }

            string line = lines.Count == 0 ? rules.CalmLine : string.Join(" ", lines);
            return new GuestDialogueVisit(input, line, clues, risk, rules.BreadLines);
        }

        private static bool Applies(DialogueCauseRule rule, GuestVisitInput input)
        {
            switch (rule.condition)
            {
                case DialogueCondition.Floor: return input.Floor == rule.floor;
                case DialogueCondition.Event:
                    return input.EventFloor == input.Floor && input.EventAxis == rule.axis;
                case DialogueCondition.Race: return input.Race == rule.qualifier;
                case DialogueCondition.Job: return input.Job == rule.qualifier;
                case DialogueCondition.Fatigue: return input.IsTired;
                case DialogueCondition.PersonalWeakness: return input.PersonalWeakness == rule.axis;
                default: throw new ArgumentOutOfRangeException(nameof(rule.condition));
            }
        }
    }
}
