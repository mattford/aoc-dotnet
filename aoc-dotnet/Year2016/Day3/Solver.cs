using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2016.Day3;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + input.Count(l => IsPossible(Regex.Matches(l, @"\d+").Select(x => int.Parse(x.Value)).ToArray()));
    }

    public string Part2(string[] input)
    {
        var lines = input.Select(l => Regex.Matches(l, @"\d+").Select(x => int.Parse(x.Value)).ToArray()).ToArray();
        var t = 0;
        for (var y = 0; y < lines.Length; y += 3)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                var thisTriangle = Enumerable.Range(y, 3).Select(y1 => lines[y1][x]).ToArray();
                if (IsPossible(thisTriangle)) t++;
            }
        }

        return "" + t;
    }

    private static bool IsPossible(int[] sides)
    {
        return sides[0] + sides[1] > sides[2] && sides[0] + sides[2] > sides[1] && sides[1] + sides[2] > sides[0];
    }
}