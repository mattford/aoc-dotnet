using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2015.Day19;

internal record struct Replacement(string From, string To); 

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var (replacements, initial) = GetReplacements(input);
        var possibles = new HashSet<string>();
        foreach (var replacement in replacements)
        {
            var idx = 0;
            while (initial.IndexOf(replacement.From, idx, StringComparison.Ordinal) > -1)
            {
                var rIdx = initial.IndexOf(replacement.From, idx, StringComparison.Ordinal);
                var possible = initial[..rIdx] + replacement.To + initial[(rIdx + replacement.From.Length)..];
                possibles.Add(possible);
                idx = rIdx + 1;
            }
        }

        return "" + possibles.Count;
    }

    public string Part2(string[] input)
    {
        var (_, initial) = GetReplacements(input);
        // Logic here is based on /u/askalski's comment:
        // https://www.reddit.com/r/adventofcode/comments/3xflz8/comment/cy4etju/
        // I have taken the time to understand the comment and why it is correct,
        // however I won't pretend that I could have derived it from scratch.
        var elements = Regex.Matches(initial, "([A-Z][a-z]*)").Select(m => m.Groups[1].Value).ToArray();
        var a = elements.Count(c => c is "Ar" or "Rn");
        var b = elements.Count(c => c is "Y");
        var t = elements.Length;
        return "" + (t - a - 2 * b - 1);
    }

    private static (ImmutableList<Replacement>, string) GetReplacements(string[] input)
    {
        var parts = string.Join("\n", input).Split("\n\n");
        var replacements = (
            from line in parts[0].Split("\n")
            let split = line.Split(" => ")
            select new Replacement(split[0], split[1])
        ).ToImmutableList();

        return (replacements, parts[1]);
    }
}