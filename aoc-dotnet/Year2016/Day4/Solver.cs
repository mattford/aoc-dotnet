using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2016.Day4;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return ""+input.Sum(GetScore);
    }

    public string Part2(string[] input)
    {
        return "" + input.Select(Decrypt).First(x => x.Item2 == "northpole object storage").Item1;
    }

    private (int, string) Decrypt(string line)
    {
        var match = Regex.Match(line, @"([a-z\-]+)-([0-9]+)\[([a-z]+)\]");
        var id = int.Parse(match.Groups[2].Value);
        var name = new string(match.Groups[1].Value.ToCharArray().Select(c =>
        {
            if (c == '-') return ' ';
            var moves = id % 26;
            var next = c + moves;
            if (next > 'z') next = 'a' + (next - 'z') - 1;
            return (char)next;
        }).ToArray());
        return (id, name);

    }

    private static int GetScore(string room)
    {
        var match = Regex.Match(room, @"([a-z\-]+)-([0-9]+)\[([a-z]+)\]");
        var id = new string(
            match.Groups[1].Value
                .Replace("-", "").ToCharArray()
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .ThenBy(x => x.Key)
                .Take(5)
                .Select(x => x.Key)
                .ToArray()
        );
        return id == match.Groups[3].Value ? int.Parse(match.Groups[2].Value) : 0;
    }
}