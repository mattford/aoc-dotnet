using System.Collections.Immutable;
using System.Numerics;

namespace aoc_dotnet.Year2024.Day18;

public class Solver : SolverInterface
{
    private Complex North = -Complex.ImaginaryOne;
    private Complex South = Complex.ImaginaryOne;
    private Complex West = -1;
    private Complex East = 1;

    public string Part1(string[] input)
    {
        var blocks = ParseInput(input.Take(1024).ToArray(), 70, 70);

        var (l, _) = ShortestPath(blocks, Complex.ImaginaryOne + 1, Complex.ImaginaryOne * 71 + 71);
        return "" + l;
    }

    public string Part2(string[] input)
    {
        var blocks = ParseInput(input.Take(1024).ToArray(), 70, 70).ToList();
        var (l, needed) = ShortestPath(blocks.ToImmutableList(), Complex.ImaginaryOne + 1, Complex.ImaginaryOne * 71 + 71);

        var remaining = input.Skip(1024).ToList();
        while (l > 0 && remaining.Count > 0)
        {
            var next = remaining.First().Split(",").Select(int.Parse).ToArray();
            remaining.RemoveAt(0);
            var nextCoord = Complex.ImaginaryOne * (next[1] + 1) + next[0] + 1;
            blocks.Add(nextCoord);
            if (!needed.Contains(nextCoord)) continue;
            (l, needed) = ShortestPath(blocks.ToImmutableList(), Complex.ImaginaryOne + 1, Complex.ImaginaryOne * 71 + 71);
            if (l == 0)
            {
                return string.Join(",", next);
            }
        }
        return "Not found";
    }

    private (int, Complex[]) ShortestPath(ImmutableList<Complex> blocks, Complex start, Complex target)
    {
        var queue = new PriorityQueue<(Complex, int, Complex[]), int>();
        var costs = new Dictionary<Complex, int>
        {
            [start] = 0,
        };
        var visited = new HashSet<Complex>();
        queue.Enqueue((start, 0, []), 0);
        while (queue.Count > 0)
        {
            var (current, cost, steps) = queue.Dequeue();
            if (visited.Contains(current)) continue;
            visited.Add(current);
            var directions = new[] { North, East, South, West };
            foreach (var direction in directions)
            {
                if (blocks.Contains(current + direction)) continue;
                var existingCost = costs.GetValueOrDefault(current + direction, 0);
                if (existingCost > 0 && existingCost < cost) continue;
                var thisSteps = steps.ToList();
                thisSteps.Add(current + direction);
                if (current + direction == target)
                {
                    return (cost + 1, thisSteps.ToArray());
                }
                queue.Enqueue((current + direction, cost + 1, thisSteps.ToArray()), cost + 1);
            }
        }

        return (0, []);
    }

    private ImmutableList<Complex> ParseInput(string[] input, int maxY, int maxX)
    {
        var blocks = input.Select(line =>
        {
            var s = line.Split(",").Select(int.Parse).ToArray();
            return Complex.ImaginaryOne * (s[1] + 1) + s[0] + 1;
        }).ToList();
        for (var y = 0; y <= maxY + 1; y++)
        {
            blocks.Add(y * Complex.ImaginaryOne);
            blocks.Add(y * Complex.ImaginaryOne + maxX + 2);
        }

        for (var x = 0; x <= maxX + 1; x++)
        {
            blocks.Add(x);
            blocks.Add(Complex.ImaginaryOne * (maxY + 2) + x);
        }

        return blocks.ToImmutableList();
    }
}