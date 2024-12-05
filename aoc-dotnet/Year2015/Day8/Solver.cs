using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2015.Day8;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + input.Select(x => x.Length - Regex.Unescape(x).Length + 2).Sum();
    }

    public string Part2(string[] input)
    {
        return "" + input.Select(x => Regex.Escape(x).Replace("\"", "\\\"").Length - x.Length + 2).Sum();
    }
}