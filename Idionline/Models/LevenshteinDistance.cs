using System;
using System.Collections.Generic;
using System.Linq;

namespace Idionline.Models
{
    public class LevenshteinDistance
    {
        public static int Compute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Verify arguments.
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Initialize arrays.
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Begin looping.
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    // Compute cost.
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
                }
            }
            // Return cost.
            return d[n, m];
        }
        public static Dictionary<string, int> Extract(string text, List<string> pool, int num)
        {
            if (pool.Count == 0)
                return new Dictionary<string, int>();
            Dictionary<string, int> result = new();
            foreach (string item in pool)
            {
                int res = Compute(text, item);
                if (result.Count < num && !result.ContainsKey(item))
                    result.Add(item, res);
                else
                {
                    List<string> keys = result.Keys.ToList();
                    List<int> values = result.Values.ToList();
                    int max = 0;
                    for (int i = 0; i < values.Count; i++)
                        if (max < values[i])
                            max = values[i];
                    if (res < max && !result.ContainsKey(item))
                    {
                        result.Remove(keys[values.IndexOf(max)]);
                        result.Add(item, res);
                    }
                }
            }
            var sortResult = from pair in result orderby pair.Value ascending select pair;
            return sortResult.ToDictionary(pair => pair.Key, d => d.Value);
        }
    }
}
