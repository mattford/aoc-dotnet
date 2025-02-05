namespace aoc_dotnet.Year2018.Day1;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + input.Select(int.Parse).Sum();
    }

    public string Part2(string[] input)
    {
        var ints = input.Select(int.Parse).ToArray();
        var seen = new HashSet<int>();
        var frequency = 0;
        var i = 0;
        while (true)
        {
            frequency += ints[i];
            if (!seen.Add(frequency)) return "" + frequency;
            i += 1;
            i %= ints.Length;
        }
    }
}