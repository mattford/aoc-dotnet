using System.Collections.Immutable;
using System.Numerics;

namespace aoc_dotnet.Year2016.Day2;

public class Solver: SolverInterface
{
    private Dictionary<char, Complex> moveMap = new()
    {
        ['U'] = -Complex.ImaginaryOne,
        ['D'] = Complex.ImaginaryOne,
        ['L'] = -1,
        ['R'] = 1
    };
    
    public string Part1(string[] input)
    {
        return Solve("123\n456\n789", input);
    }

    public string Part2(string[] input)
    {
        return Solve("  1\n 234\n56789\n ABC\n  D", input);
    }

    private string Solve(string layout, string[] input)
    {
        var kb = GetMap(layout);
        var code = "";
        var pos = kb.First(x => x.Value == '5').Key;
        foreach (var line in input)
        {
            pos = line.Aggregate(pos, (current, c) =>
            {
                var n = current + moveMap[c];
                return kb.ContainsKey(n) ? n : current;
            });
            code += kb[pos];
        }

        return code;
    }

    private static ImmutableDictionary<Complex, char> GetMap(string input)
    {
        var lines = input.Split('\n');
        return (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, lines[y].Length)
            where lines[y][x] != ' '
            select new KeyValuePair<Complex, char>(Complex.ImaginaryOne * y + x, lines[y][x])
        ).ToImmutableDictionary();
    }
}