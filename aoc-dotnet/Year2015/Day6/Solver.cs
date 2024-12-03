using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2015.Day6;

public partial class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var lit = new Dictionary<string, bool>();
        foreach (var line in input)
        {
            var match = ValidInstructionsPattern().Match(line);
            for (var x = int.Parse(match.Groups[2].Value); x <= int.Parse(match.Groups[4].Value); x++)
            {
                for (var y = int.Parse(match.Groups[3].Value); y <= int.Parse(match.Groups[5].Value); y++)
                {
                    var key = $"{x},{y}";
                    lit[key] = match.Groups[1].Value switch
                    {
                        "turn on" => true,
                        "turn off" => false,
                        "toggle" => !lit.GetValueOrDefault(key, false),
                        _ => lit[key]
                    };
                }
            }
        }

        return "" + lit.Count(x => x.Value);
    }

    public string Part2(string[] input)
    {
        var lit = new Dictionary<string, int>();
        foreach (var line in input)
        {
            var match = ValidInstructionsPattern().Match(line);
            for (var x = int.Parse(match.Groups[2].Value); x <= int.Parse(match.Groups[4].Value); x++)
            {
                for (var y = int.Parse(match.Groups[3].Value); y <= int.Parse(match.Groups[5].Value); y++)
                {
                    var key = $"{x},{y}";
                    var current = lit.GetValueOrDefault(key, 0);
                    lit[key] = match.Groups[1].Value switch
                    {
                        "turn on" => current + 1,
                        "turn off" => int.Max(0, current - 1),
                        "toggle" => current + 2,
                        _ => lit[key]
                    };
                }
            }
        }

        return "" + lit.Select(x => x.Value).Sum();
    }

    [GeneratedRegex(@"(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)")]
    private static partial Regex ValidInstructionsPattern();
}