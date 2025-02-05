using System.Collections.Immutable;
using System.Numerics;

namespace aoc_dotnet.Year2018.Day11;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var summedAreas = GetSummedAreas(int.Parse(input[0]));
        var max = 0;
        var coord = "";
        for (var y = 0; y < 298; y++)
        {
            for (var x = 0; x < 298; x++)
            {
                var p = GetSizeOf(summedAreas, x, y, 3);
                if (p <= max) continue;
                max = p;
                coord = $"{x},{y}";
            }
        }

        return coord;
    }

    public string Part2(string[] input)
    {
        var summedAreas = GetSummedAreas(int.Parse(input[0]));
        var max = 0;
        var coord = "";
        for (var y = 0; y < 298; y++)
        {
            for (var x = 0; x < 298; x++)
            {
                for (var s = 2; s <= 298 - Math.Max(y, x); s++)
                {
                    var p = GetSizeOf(summedAreas, x, y, s);
                    if (p <= max) continue;
                    max = p;
                    coord = $"{x},{y},{s}";
                }
            }
        }

        return coord;
    }

    private static int GetSizeOf(ImmutableDictionary<Complex, int> summedAreas, int x, int y, int size)
    {
        return summedAreas.GetValueOrDefault(new Complex(x - 1, y - 1), 0) +
               summedAreas.GetValueOrDefault(new Complex(x + (size - 1), y + (size - 1)), 0) -
               summedAreas.GetValueOrDefault(new Complex(x - 1, y + (size - 1)), 0) -
               summedAreas.GetValueOrDefault(new Complex(x + (size - 1), y - 1), 0);
    }

    private static ImmutableDictionary<Complex, int> GetSummedAreas(int serialNumber)
    {
        var summedAreas = new Dictionary<Complex, int>();
        foreach (var y in Enumerable.Range(0, 300))
        {
            var thisRow = 0;
            foreach (var x in Enumerable.Range(0, 300))
            {
                thisRow += GetPowerLevel(x, y, serialNumber);
                summedAreas.Add(Complex.ImaginaryOne * y + x, thisRow + summedAreas.GetValueOrDefault(new Complex(x, y - 1), 0));
            }
        }
        return summedAreas.ToImmutableDictionary();
    }

    private static int GetPowerLevel(int x, int y, int serialNumber)
    {
        var rackId = x + 10;
        var p = rackId * y + serialNumber;
        p *= rackId;
        if (p < 100) return -5;
        return int.Parse(""+p.ToString()[^3]) - 5;
    }
}