using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2015.Day10;

public partial class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return ""+LookAndSay(input[0], 40).Length;
    }

    public string Part2(string[] input)
    {
        return ""+LookAndSay(input[0], 50).Length;
    }

    private static string LookAndSay(string input, int iterations)
    {
        while (iterations-- > 0)
        {    
            input = RepeatedDigitsPattern().Replace(input, m => $"{m.Groups[1].Value.Length}{m.Groups[2].Value}");
        }
        return input;
    }

    [GeneratedRegex(@"((\d)\2*)")]
    private static partial Regex RepeatedDigitsPattern();
}