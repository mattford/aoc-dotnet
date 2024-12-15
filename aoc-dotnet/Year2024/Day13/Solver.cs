using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2024.Day13;

public class Solver : SolverInterface
{
    public string Part1(string[] input)
    {
        var machines = ParseInput(input);
        return "" + machines.Sum(x => LowestPrizeCost(x));
    }

    public string Part2(string[] input)
    {
        var machines = ParseInput(input);
        return "" + machines.Sum(x => LowestPrizeCost(x, 10000000000000));
    }

    private static long LowestPrizeCost(long[] machine, long boostPrize = 0)
    {
        var (x1, y1, x2, y2, tx, ty) = (machine[0], machine[1], machine[2], machine[3], machine[4], machine[5]);
        tx += boostPrize;
        ty += boostPrize;
        if ((-y2 * tx + x2 * ty) % (-y2 * x1 + x2 * y1) != 0) return 0;

        var a = (-y2 * tx + x2 * ty) / (-y2 * x1 + x2 * y1);
        if ((tx - a * x1) % x2 != 0) return 0;
        var b = (tx - a * x1) / x2;
        return a * 3 + b;
    }

    private static long[][] ParseInput(string[] input)
    {
        var oneLine = string.Join("\n", input);
        var parts = oneLine.Split("\n\n");
        return parts.Select(part => Regex.Matches(part, "([0-9]+)").Select(match => long.Parse(match.Value)).ToArray())
            .ToArray();
    }
}