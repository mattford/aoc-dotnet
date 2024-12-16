using System.Numerics;
using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2024.Day14;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var bounds = (101,103);
        var robots = ParseInput(input);
        return "" + SafetyFactor(Simulate(robots, bounds, 100), bounds);
    }

    public string Part2(string[] input)
    {
        var robots = ParseInput(input);
        var bounds = (101, 103);
        var steps = 0;
        while (!EasterEgg(robots))
        {
            robots = Simulate(robots, bounds, 1);
            steps++;
        }

        return "" + steps;
    }

    private bool EasterEgg((Complex, Complex)[] robots)
    {
        // Naive approach, check for a run of several diagonal robots
        return robots.Any(r => DiagonalDepth(robots, r.Item1) > 10);
    }

    private int DiagonalDepth((Complex, Complex)[] robots, Complex robot, int depth = 0)
    {
        var next = Complex.ImaginaryOne * (robot.Imaginary + 1) + robot.Real + 1;
        if (robots.Any(r => r.Item1 == next))
        {
            return DiagonalDepth(robots, next, depth + 1);
        }

        return depth;
    }

    private (Complex, Complex)[] Simulate((Complex, Complex)[] robots, (int, int) bounds, int steps)
    {
        var (maxX, maxY) = bounds;
        return robots.Select(r =>
        {
            var x = (r.Item1.Imaginary + steps * r.Item2.Imaginary) % (maxX);
            var y = (r.Item1.Real + steps * r.Item2.Real) % (maxY);
            if (x < 0)
            {
                x = maxX + x;
            }
            
            if (y < 0)
            {
                y = maxY + y;
            }
            return (Complex.ImaginaryOne * x + y, r.Item2);
        }).ToArray();
    }

    private int SafetyFactor((Complex, Complex)[] robots, (int, int) bounds)
    {
        var middleX = (int)Math.Floor(bounds.Item1 / 2d);
        var middleY = (int)Math.Floor(bounds.Item2 / 2d);
        return robots
            .Where(r => r.Item1.Imaginary != middleX && r.Item1.Real != middleY)
            .GroupBy(x => (x.Item1.Imaginary > middleX, x.Item1.Real > middleY))
            .Select(x => x.Count())
            .Aggregate((acc, item) => acc * item);
    }

    private static (Complex, Complex)[] ParseInput(string[] input)
    {
        return input.Select(
            x =>
            {
                var matches = Regex.Matches(x, "([0-9-]+)");
                return (
                    Complex.ImaginaryOne * int.Parse(matches[0].Value) + int.Parse(matches[1].Value),
                    Complex.ImaginaryOne * int.Parse(matches[2].Value) + int.Parse(matches[3].Value)
                );
            }).ToArray();
    }
}