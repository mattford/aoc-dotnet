using System.Numerics;

namespace aoc_dotnet.Year2016.Day13;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var s = int.Parse(input[0]);
        return "" + FindMinimumSteps(Complex.ImaginaryOne + 1, Complex.ImaginaryOne * 39 + 31, s).Item1;
    }

    public string Part2(string[] input)
    {
        var s = int.Parse(input[0]);
        return "" + FindMinimumSteps(Complex.ImaginaryOne + 1, Complex.ImaginaryOne * 39 + 31, s).Item2;
    }

    private (int, int) FindMinimumSteps(Complex start, Complex target, int seed)
    {
        var dirs = new[]
        {
            Complex.ImaginaryOne,
            -Complex.ImaginaryOne,
            -1,
            1
        };
        var queue = new PriorityQueue<(Complex, int), int>();
        var visited = new HashSet<Complex>();
        queue.Enqueue((start, 0), 0);
        var under50Points = new HashSet<Complex>();
        while (queue.Count > 0)
        {
            var (pos, cost) = queue.Dequeue();
            if (cost <= 50) under50Points.Add(pos);
            if (!visited.Add(pos)) continue;
            var next = dirs.Select(d => pos + d).Where(p => p is { Imaginary: >= 0, Real: >= 0 } && IsOpen(p, seed));
            foreach (var n in next)
            {
                if (visited.Contains(n)) continue;
                if (n == target)
                {
                    return (cost + 1, under50Points.Count);
                }
                queue.Enqueue((n, cost + 1), cost + 1);
            }
        }

        return (0, 0);
    }

    private bool IsOpen(Complex coord, int seed)
    {
        var y = coord.Imaginary;
        var x = coord.Real;
        var n = x * x + 3 * x + 2 * x * y + y + y * y;
        n += seed;
        var bin = Convert.ToString((int)n, 2);
        return bin.Count(c => c == '1') % 2 == 0;
    }
}