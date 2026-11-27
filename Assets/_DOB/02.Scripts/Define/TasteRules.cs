// 입맛 기준값. 손님 입맛 태그를 뽑는 TasteDraw와 접시 맛을 채점하는 TasteJudge가 읽는다(Resources/SO_TasteRules).
// 의사코드: 값은 인스펙터에서만 정한다.
//   Signature(종족) — 그 종족의 대표 맛. 등록되지 않았거나 비었으면 null(무작위 두 칸).
//   FlavorsOf(이름) — 레시피·양념 이름의 맛 목록. 등록되지 않았으면 빈 목록.
//   Validate — 후보가 2개 이상이고 겹치지 않으며, 대표 맛·요리 맛이 모두 후보 안에 있는지 검사해 첫 오류를 돌려준다.
// 💡 값은 기획 4-6·5장 표(맛 표현 13종, 종족 대표 맛, 레시피·양념 맛)를 옮긴 것이며 송예찬 확인 전이다.
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DOB.Define
{
    /// <summary>종족 하나의 대표 맛.</summary>
    [Serializable]
    public struct RaceTaste
    {
        /// <summary>종족 이름. 손님 후보·대사 규칙의 한글 이름과 같다.</summary>
        public string race;
        /// <summary>대표 맛. 없으면 비워 둔다(무작위 두 칸).</summary>
        public string signature;
    }

    /// <summary>레시피 또는 양념 하나의 맛.</summary>
    [Serializable]
    public struct DishTaste
    {
        /// <summary>레시피·양념 이름. 주방이 쓰는 한글 이름과 같다.</summary>
        public string name;
        public string[] flavors;
    }

    /// <summary>입맛 후보·종족 대표 맛·레시피와 양념의 맛.</summary>
    [CreateAssetMenu(menuName = "DOB/입맛 기준", fileName = ResourceName)]
    public sealed class TasteRules : ScriptableObject
    {
        public const string ResourceName = "SO_TasteRules";

        /// <summary>입맛 태그 후보(맛 표현). 무작위 칸은 여기서 뽑는다.</summary>
        [SerializeField] private string[] flavors = Array.Empty<string>();
        [SerializeField] private RaceTaste[] races = Array.Empty<RaceTaste>();
        [SerializeField] private DishTaste[] dishes = Array.Empty<DishTaste>();

        /// <summary>입맛 태그 후보.</summary>
        public IReadOnlyList<string> Flavors => flavors;

        /// <summary>종족의 대표 맛. 없으면 null.</summary>
        public string Signature(string race)
        {
            foreach (var entry in races)
                if (entry.race == race)
                    return string.IsNullOrWhiteSpace(entry.signature) ? null : entry.signature;
            return null;
        }

        /// <summary>레시피·양념 이름의 맛. 등록되지 않았으면 빈 목록.</summary>
        public IReadOnlyList<string> FlavorsOf(string name)
        {
            foreach (var entry in dishes)
                if (entry.name == name && entry.flavors != null)
                    return entry.flavors;
            return Array.Empty<string>();
        }

        /// <summary>기준값 오류. 문제가 없으면 null.</summary>
        public string Validate()
        {
            if (flavors == null || flavors.Length < 2) return "입맛 후보는 2개 이상이어야 합니다.";
            var known = new HashSet<string>();
            foreach (string flavor in flavors)
                if (string.IsNullOrWhiteSpace(flavor) || !known.Add(flavor))
                    return "입맛 후보가 비었거나 겹칩니다.";
            foreach (var entry in races)
                if (!string.IsNullOrWhiteSpace(entry.signature) && !known.Contains(entry.signature))
                    return $"{entry.race}의 대표 맛({entry.signature})이 후보에 없습니다.";
            foreach (var entry in dishes)
                if (entry.flavors != null)
                    foreach (string flavor in entry.flavors)
                        if (!known.Contains(flavor))
                            return $"{entry.name}의 맛({flavor})이 후보에 없습니다.";
            return null;
        }
    }
}
