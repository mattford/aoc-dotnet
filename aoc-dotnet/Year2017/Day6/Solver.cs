using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2017.Day6;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var banks = Regex.Matches(input[0], @"\d+").Select(m => int.Parse(m.Value)).ToList();
        var d = 0;
        var seen = new HashSet<string>();
        while (seen.Add(string.Join(",", banks)))
        {
            var m = banks.Max();
            var i = banks.IndexOf(m);
            banks[i] = 0;
            while (m > 0)
            {
                i = (i + 1) % banks.Count;
                banks[i]++;
                m--;
            }

            d++;
        }

        return "" + d;
    }

    public string Part2(string[] input)
    {
        var banks = Regex.Matches(input[0], @"\d+").Select(m => int.Parse(m.Value)).ToList();
        var d = 0;
        var seen = new Dictionary<string, int>();
        while (seen.TryAdd(string.Join(",", banks), d))
        {
            var m = banks.Max();
            var i = banks.IndexOf(m);
            banks[i] = 0;
            while (m > 0)
            {
                i = (i + 1) % banks.Count;
                banks[i]++;
                m--;
            }

            d++;
        }

        var last = seen[string.Join(",", banks)];
        return "" + (d - last);
    }
}