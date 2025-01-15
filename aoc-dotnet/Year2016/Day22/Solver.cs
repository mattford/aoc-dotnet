using System.Collections.Immutable;
using System.Numerics;
using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2016.Day22;

internal record struct Node(int Used, int Free, bool HasGoal);

public class Solver: SolverInterface
{
    private Complex[] Directions =
    [
        -Complex.ImaginaryOne,
        Complex.ImaginaryOne,
        -1,
        1
    ];
    public string Part1(string[] input)
    {
        var nodes = GetNodes(input);
        var pairs = 0;
        foreach (var node in nodes)
        {
            if (node.Value.Used == 0) continue;
            foreach (var other in nodes)
            {
                if (node.Key == other.Key) continue;
                if (node.Value.Used <= other.Value.Free) pairs++;
            }
        }

        return "" + pairs;
    }

    public string Part2(string[] input)
    {
        var initialNodes = GetNodes(input);
        var maxX = (int)initialNodes.Max(n => n.Key.Real);
        var empty = initialNodes.Single(n => n.Value.Used == 0);
        var grid = initialNodes.Select(kv =>
        {
            if (kv.Value.Used > empty.Value.Free) return new KeyValuePair<Complex, char>(kv.Key, '#');
            return new KeyValuePair<Complex, char>(kv.Key, '.');
        }).ToImmutableDictionary();
        var distToGoal = DistToGoal(grid, empty.Key, Complex.Zero + (maxX - 1));
        distToGoal += maxX;
        // for each move left, except the last, we must do 4 moves to get back to the front
        distToGoal += 4 * (maxX - 1);
        return ""+ distToGoal;
    }

    private int DistToGoal(ImmutableDictionary<Complex, char> grid, Complex start, Complex goal)
    {
        var queue = new PriorityQueue<(Complex, int), int>();
        queue.Enqueue((start, 0), 0);
        var visited = new HashSet<Complex>();
        while (queue.Count > 0)
        {
            var (pos, cost) = queue.Dequeue();
            if (!grid.TryGetValue(pos, out var node)) continue;
            if (!visited.Add(pos)) continue;
            foreach (var direction in Directions)
            {
                if (!grid.TryGetValue(pos + direction, out var next) || next == '#') continue;
                if (pos + direction == goal)
                {
                    return cost + 1;
                }
                queue.Enqueue((pos + direction, cost + 1), cost + 1);
            }
        }

        return int.MaxValue;
    }

    private ImmutableDictionary<Complex, Node> GetNodes(string[] input)
    {
        var dict = new Dictionary<Complex, Node>();
        foreach (var line in input.Skip(2).ToArray())
        {
            var m = Regex.Match(line, @"\/dev\/grid\/node-x([0-9]+)-y([0-9]+)\s+([0-9]+T)\s+([0-9]+)T\s+([0-9]+)T\s+([0-9]+)%");
            var coord = Complex.ImaginaryOne * int.Parse(m.Groups[2].Value) + int.Parse(m.Groups[1].Value);
            dict.Add(coord, new Node(int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value), false));
        }

        return dict.ToImmutableDictionary();
    }
}