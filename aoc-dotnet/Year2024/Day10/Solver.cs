using System.Collections.Immutable;
using System.Numerics;

namespace aoc_dotnet.Year2024.Day10;

public class Solver: SolverInterface
{
    private readonly Complex Up = Complex.ImaginaryOne;
    private readonly Complex Down = -Complex.ImaginaryOne;
    private readonly Complex Left = -1;
    private readonly Complex Right = 1;

    public string Part1(string[] input)
    {
        var map = ParseInput(input);

        var total = map.Where(kv => kv.Value == 0)
            .Select(kv => FindTrails(map, kv.Key, kv.Value, []).Item1.Count).Sum();
        return "" + total;
    }

    public string Part2(string[] input)
    {
        var map = ParseInput(input);

        var total = map.Where(kv => kv.Value == 0)
            .Select(kv => FindTrails(map, kv.Key, kv.Value, []).Item2).Sum();
        return "" + total;
    }

    private (HashSet<Complex>, int total) FindTrails(ImmutableDictionary<Complex, int> map, Complex position, int lastValue, HashSet<Complex> seen)
    {
        var total = 0;
        var directions = new[]{Up, Down, Left, Right};
        foreach (var direction in directions)
        {
            var nextPos = position + direction;
            var nextValue = map.GetValueOrDefault(nextPos, 0);
            if (nextValue != lastValue + 1) continue;
            if (nextValue == 9)
            {
                seen.Add(nextPos);
                total++;
                continue;
            }
            (seen, var newTotal) = FindTrails(map, nextPos, nextValue, seen);
            total += newTotal;
        }

        return (seen, total);
    }

    private ImmutableDictionary<Complex, int> ParseInput(string[] input)
    {
        return (
            from y in Enumerable.Range(0, input.Length) 
            from x in Enumerable.Range(0, input[y].Length)
            where input[y][x] != '.'
            select new KeyValuePair<Complex, int>(y * -Up + x, int.Parse(""+input[y][x]))
        ).ToImmutableDictionary();
    }
}