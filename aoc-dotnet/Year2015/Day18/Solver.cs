using System.Collections.Immutable;
using System.Numerics;
using System.Xml;

namespace aoc_dotnet.Year2015.Day18;

using Map = ImmutableDictionary<Complex, bool>;

public class Solver : SolverInterface
{
    public string Part1(string[] input)
    {
        var map = GetMap(input, []);
        for (var i = 0; i < 100; i++)
        {
            map = SimulateStep(map, []);
        }

        return "" + map.Count(x => x.Value);
    }

    public string Part2(string[] input)
    {
        var maxY = input.Length - 1;
        var maxX = input[maxY].Length - 1;
        Complex[] stuck = [Complex.Zero, Complex.ImaginaryOne * maxY, Complex.One * maxX, Complex.ImaginaryOne * maxY + maxX];
        var map = GetMap(input, stuck);
        for (var i = 0; i < 100; i++)
        {
            map = SimulateStep(map, stuck);
        }

        return "" + map.Count(x => x.Value);
    }

    private Map SimulateStep(Map map, Complex[] stuck)
    {
        return map.Select(x =>
        {
            var onNeighbours = GetNeighbours(x.Key).Count(n => map.TryGetValue(n, out var nv) && nv);
            return new KeyValuePair<Complex, bool>(x.Key, stuck.Contains(x.Key) || onNeighbours == 3 || (x.Value && onNeighbours == 2));
        }).ToImmutableDictionary();
    }

    private Complex[] GetNeighbours(Complex key)
    {
        return
            new[]
            {
                Complex.ImaginaryOne,
                Complex.ImaginaryOne - 1,
                Complex.ImaginaryOne + 1,
                -Complex.One,
                Complex.One,
                -Complex.ImaginaryOne,
                -Complex.ImaginaryOne - 1,
                -Complex.ImaginaryOne + 1
            }.Select(x => key + x).ToArray();
    }

    private static Map GetMap(string[] input, Complex[] stuck)
    {
        return (
            from y in Enumerable.Range(0, input.Length)
            from x in Enumerable.Range(0, input[y].Length)
            let coord = Complex.ImaginaryOne * y + x
            select new KeyValuePair<Complex, bool>(coord, input[y][x] == '#' || stuck.Contains(coord))
        ).ToImmutableDictionary();
    }
}