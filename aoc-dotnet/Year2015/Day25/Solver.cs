using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2015.Day25;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var coord = Regex.Matches(input[0], @"\d+").Select(m => int.Parse(m.Value)).ToArray();
        var row = coord[0];
        var col = coord[1];
        var initial = Enumerable.Range(1, col).Sum();
        var iterations = Enumerable.Range(2, row-1).Aggregate(initial, (current, r) => current + col + (r - 2));
        var current = 20151125L;
        for (var i = 0; i < iterations - 1; i++) current = (current * 252533) % 33554393;  
        return "" + current;
    }

    public string Part2(string[] input)
    {
        return "Merry Christmas!";
    }
}