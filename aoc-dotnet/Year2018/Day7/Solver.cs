using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2018.Day7;

public class Solver : SolverInterface
{
    public string Part1(string[] input)
    {
        var map = GetMap(input);
        var done = new List<char>();
        while (done.Count < map.Keys.Count)
        {
            var available = map
                .Where(s => !done.Contains(s.Key) && s.Value.All(o => done.Contains(o)))
                .OrderBy(s => s.Key)
                .Take(1);
            done.AddRange(available.Select(a => a.Key));
        }

        return new string(done.ToArray());
    }

    public string Part2(string[] input)
    {
        var map = GetMap(input);
        var done = new List<char>();
        var t = 0;
        var workers = new List<(int, char)>();
        while (done.Count < map.Keys.Count)
        {
            var available = map
                .Where(s => workers.All(w => w.Item2 != s.Key) && !done.Contains(s.Key) && s.Value.All(o => done.Contains(o)))
                .OrderBy(s => s.Key)
                .ToArray();
            workers.AddRange(available.Take(5 - workers.Count).Select(a => (61 + (a.Key - 'A'), a.Key)));
            // advance time until the first worker is done
            var min = workers.Min(w => w.Item1);
            t += min;
            workers = workers.Select(w => (w.Item1 - min, w.Item2)).ToList();
            done.AddRange(workers.Where(w => w.Item1 == 0).Select(a => a.Item2));
            workers.RemoveAll(w => w.Item1 == 0);
        }

        return ""+t;
    }

    private Dictionary<char, List<char>> GetMap(string[] input)
    {
        var map = (
            from c in Enumerable.Range('A', 26)
            select new KeyValuePair<char, List<char>>((char)c, [])
        ).ToDictionary();
        foreach (var line in input)
        {
            var m = Regex.Matches(line, "step ([A-Z])", RegexOptions.IgnoreCase);
            map[m[1].Groups[1].Value[0]].Add(m[0].Groups[1].Value[0]);
        }

        return map;
    }
}