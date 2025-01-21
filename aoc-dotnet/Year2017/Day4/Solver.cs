namespace aoc_dotnet.Year2017.Day4;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + input.Count(x => x.Split(' ').Length == x.Split(' ').Distinct().Count());
    }

    public string Part2(string[] input)
    {
        return "" + input.Count(x => x.Split(' ').Length == x.Split(' ').Select(s => new string(s.Order().ToArray())).Distinct().Count());
    }
}