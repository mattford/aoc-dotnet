using System.Collections.Immutable;
using System.Net;
using System.Numerics;
using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2018.Day3;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var claims = input
            .Select(GetPoints)
            .SelectMany(c => c.Item2)
            .GroupBy(x => x)
            .Count(g => g.Count() > 1);
        return "" + claims;
    }

    public string Part2(string[] input)
    {
        var claims = input .Select(GetPoints).ToList();
        var grouped = claims
            .SelectMany(c => c.Item2)
            .GroupBy(x => x)
            .ToDictionary(i => i.Key, i => i.Count());
        return "" + claims.Single(c => c.Item2.All(p => grouped[p] == 1)).Item1;
    }

    private (int, List<Complex>) GetPoints(string line)
    {
        var ints = Regex.Matches(line, @"\d+").Select(m => int.Parse(m.Value)).ToList();
        var points = (
            from y in Enumerable.Range(ints[2], ints[4])
            from x in Enumerable.Range(ints[1], ints[3])
            select Complex.ImaginaryOne * y + x
        ).ToList();
        return (ints[0], points);
    }
}