namespace aoc_dotnet.Year2017.Day5;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var jumps = input.Select(int.Parse).ToArray();
        var i = 0;
        var s = 0;
        while (i >= 0 && i < jumps.Length)
        {
            i += jumps[i]++;
            s++;
        }

        return "" + s;
    }

    public string Part2(string[] input)
    {
        var jumps = input.Select(int.Parse).ToArray();
        var i = 0;
        var s = 0;
        while (i >= 0 && i < jumps.Length)
        {
            var j = jumps[i];
            var m = j >= 3 ? -1 : 1;
            jumps[i] += m;
            i += j;
            s++;
        }

        return "" + s;
    }
}