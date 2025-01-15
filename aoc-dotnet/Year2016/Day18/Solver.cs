namespace aoc_dotnet.Year2016.Day18;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + GetSafeTiles(input[0], 40);
    }

    public string Part2(string[] input)
    {
        return "" + GetSafeTiles(input[0], 400000);
    }

    private int GetSafeTiles(string state, int iterations)
    {
        var s = state.ToCharArray().ToList();
        var safe = s.Count(c => c == '.');
        var i = 1;
        while (i < iterations)
        {
            s = Enumerable.Range(0, s.Count).Select(idx => ValueOfPosition(s, idx - 1) != ValueOfPosition(s, idx + 1) ? '^' : '.').ToList();
            safe += s.Count(c => c == '.');
            i++;
        }
        return safe;
    }

    private char ValueOfPosition(List<char> l, int idx)
    {
        if (idx < 0 || idx > l.Count - 1) return '.';
        return l[idx];
    }
}