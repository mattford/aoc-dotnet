using System.Collections.Immutable;
using System.Numerics;

namespace aoc_dotnet.Year2024.Day20;

using Map = ImmutableDictionary<Complex, char>;

public class Solver: SolverInterface
{
    private Complex North = -Complex.ImaginaryOne;
    private Complex South = Complex.ImaginaryOne;
    private Complex West = -1;
    private Complex East = 1;
    
    public string Part1(string[] input)
    {
        var (map, start, end) = ParseInput(input);
        var costs = GetCosts(map, start, end);
        var cheats = FindCheats(map, costs, 2);
        return "" + cheats;
    }

    public string Part2(string[] input)
    {
        var (map, start, end) = ParseInput(input);
        var costs = GetCosts(map, start, end);
        var cheats = FindCheats(map, costs, 20);
        return "" + cheats;
    }

    private Complex[] FindPossibles(Map map, Complex from, int steps)
    {
        return map.Keys.Where(other =>
        {
            var dist = (int)(Math.Abs(other.Imaginary - from.Imaginary) + Math.Abs(other.Real - from.Real));
            return dist > 0 && dist <= steps;
        }).ToArray();
    }

    private int FindCheats(Map map, ImmutableDictionary<Complex, int> costs, int cheatLength)
    {
        var cheats = new HashSet<(Complex, Complex, int)>();
        var points = map.Where(kp => kp.Value == '.').ToImmutableDictionary();
        foreach (var p in points)
        {
            var thisCost = costs[p.Key];
            var possibles = FindPossibles(points, p.Key, cheatLength);
            foreach (var other in possibles)
            {
                if (!costs.TryGetValue(other, out var otherCost) || otherCost <= thisCost) continue;
                var dist = (int)(Math.Abs(other.Imaginary - p.Key.Imaginary) + Math.Abs(other.Real - p.Key.Real));
                var saved = otherCost - thisCost - dist;
                if (saved < 1) continue;
                cheats.Add((p.Key, other, otherCost - thisCost - dist));
            }
        }
        return cheats.Count(x => x.Item3 >= 100);
    }

    private ImmutableDictionary<Complex, int> GetCosts(Map grid, Complex start, Complex end)
    {
        var directions = new[] { North, East, South, West };
        var current = start;
        var visited = new HashSet<Complex>();
        var costs = new Dictionary<Complex, int>();
        var cost = 0;
        while (current != end)
        {
            visited.Add(current);
            costs.Add(current, cost);
            foreach (var direction in directions)
            {
                if (grid.GetValueOrDefault(current + direction, '#') != '.' ||
                    visited.Contains(current + direction)) continue;
                cost++;
                current += direction;
                break;
            }
        }
        costs.Add(end, cost);

        return costs.ToImmutableDictionary();
    }

    private (Map, Complex, Complex) ParseInput(string[] input)
    {
        var map = new Dictionary<Complex, char>();
        Complex start = 0;
        Complex end = 0;
        foreach (var y in Enumerable.Range(0, input.Length))
        {
            foreach (var x in Enumerable.Range(0, input[y].Length))
            {
                var val = input[y][x];
                var coord = Complex.ImaginaryOne * y + x;
                switch (val)
                {
                    case 'E':
                        end = coord;
                        map.Add(coord, '.');
                        break;
                    case 'S':
                        start = coord;
                        map.Add(coord, '.');
                        break;
                    default:
                        map.Add(coord, val);
                        break;
                }
            }
        }

        return (map.ToImmutableDictionary(), start, end);
    }
}