using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2017.Day15;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var gennies = input.Select(line => long.Parse(Regex.Match(line, @"\d+").Value)).ToArray();
        var pairs = 40_000_000;
        var factors = new[] { 16807, 48271 };
        var divisor = 2147483647;
        var points = 0;
        for (var i = 1; i <= pairs; i++)
        {
            gennies = gennies.Select((g, idx) => (g * factors[idx]) % divisor).ToArray();
            if (gennies.DistinctBy(x => x & 65535).Count() == 1) points++;
        }
        
        return ""+points;
    }

    public string Part2(string[] input)
    {
        var gennies = input.Select(line => long.Parse(Regex.Match(line, @"\d+").Value)).ToArray();
        var pairs = 5_000_000;
        var factors = new[] { 16807, 48271 };
        var multiples = new[] { 4, 8 };
        var divisor = 2147483647;
        var points = 0;
        for (var i = 1; i <= pairs; i++)
        {
            gennies = gennies.Select((g, idx) =>
            {
                do
                {
                    g = (g * factors[idx]) % divisor;
                } while (g % multiples[idx] != 0);
                return g;
            }).ToArray();
            if (gennies.DistinctBy(x => x & 65535).Count() == 1) points++;
        }
        
        return ""+points;
    }
}