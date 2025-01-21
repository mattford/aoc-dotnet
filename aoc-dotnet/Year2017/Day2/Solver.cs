using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2017.Day2;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var t = input
            .Select(line => Regex.Split(line, @"\s+").Select(int.Parse).ToArray())
            .Select(ints => ints.Max() - ints.Min())
            .Sum();

        return "" + t;
    }

    public string Part2(string[] input)
    {
        var t = input
            .Select(line => Regex.Split(line, @"\s+").Select(int.Parse).ToArray())
            .Select(ints => (from f in ints from s in ints.Except([f]).ToArray() where f % s == 0 select f / s).FirstOrDefault())
            .Sum();

        return "" + t;
    }
}