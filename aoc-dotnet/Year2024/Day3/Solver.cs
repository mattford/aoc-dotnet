using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2024.Day3;

public partial class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + ValidInstructionPattern().Matches(string.Join("", input))
            .Select(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value))
            .Sum();
    }

    public string Part2(string[] input)
    {
        return "" + ValidInstructionPattern().Matches(UnexecutedSectionPattern().Replace(string.Join("", input), string.Empty))
            .Select(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value))
            .Sum();
    }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex ValidInstructionPattern();
    
    [GeneratedRegex(@"don't\(\).*?do\(\)")]
    private static partial Regex UnexecutedSectionPattern();
}