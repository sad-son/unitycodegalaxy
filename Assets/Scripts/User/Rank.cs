using System;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.User
{
    public enum Rank
    {
        Junior = 0,
        Middle = 10,
        Senior = 20,
    }

    public static class RankExtensions
    {
        public static bool IsEqualOrGreaterThan(this Rank rank1, string rank2)
        {
            if (Enum.TryParse<Rank>(rank2, out var rank2Enum))
            {
                return (int) rank1 >= (int) rank2Enum;
            }

            return false;
        }
        
        public static bool IsEqualRankOrGreaterThan(this string rank1, string rank2)
        {
            if (Enum.TryParse<Rank>(rank2, true, out var rank2Enum))
            {
                if (Enum.TryParse<Rank>(rank1, true, out var rank1Enum))
                {
                    return (int) rank1Enum >= (int) rank2Enum;
                }
            }

            return false;
        }
        
        public static string NextRank(this string rank1)
        {
            var ranks = Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList();
            if (Enum.TryParse<Rank>(rank1, true, out var rank2Enum))
            {
                for (var i = 0; i < ranks.Count; i++)
                {
                    var currentRank = ranks[i];
                    Debug.LogError($"SAD {currentRank}");
                    if (currentRank.Equals(rank2Enum))
                    {
                        var index = Math.Clamp(i + 1, 0, ranks.Count - 1);
                        return ranks[index].ToString();
                    }
                }
            }

            Debug.LogError(rank1 + " is not a rank");
            return Rank.Senior.ToString();
        }
    }
}