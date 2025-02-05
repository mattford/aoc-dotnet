using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2018.Day4;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var sleepiestGuard = ParseGuards(input).MaxBy(x => x.Value.Values.Sum());
        return ""+sleepiestGuard.Key * sleepiestGuard.Value.MaxBy(x => x.Value).Key;
    }

    public string Part2(string[] input)
    {
        var consistentGuard = ParseGuards(input).MaxBy(x => x.Value.Values.Count == 0 ? 0 : x.Value.Values.Max());
        return ""+consistentGuard.Key * consistentGuard.Value.MaxBy(x => x.Value).Key;
    }

    private Dictionary<int, Dictionary<int, int>> ParseGuards(string[] input)
    {
        var lines = input.Select(line => (line, Regex.Matches(line, @"\d+").Select(x => int.Parse(x.Value)).ToList())).ToList();
        var sorted = lines.OrderBy(x => string.Join("", x.Item2[..5]));
        var guards = new Dictionary<int, Dictionary<int, int>>();
        int id = 0;
        var asleepSince = -1;
        foreach (var (line, ints) in sorted)
        {
            if (line.Contains("Guard"))
            {
                id = ints[5];
                guards.TryAdd(id, new Dictionary<int, int>());
                asleepSince = -1;
            }

            if (line.Contains("falls")) asleepSince = ints[4];
            if (line.Contains("wakes") && asleepSince >= 0)
            {
                for (var i = asleepSince; i < ints[4]; i++)
                {
                    guards[id].TryAdd(i, 0);
                    guards[id][i]++;
                }
            }
        }
        return guards;
    }
}