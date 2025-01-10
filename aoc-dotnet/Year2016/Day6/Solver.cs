namespace aoc_dotnet.Year2016.Day6;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return new string(GetCols(input).Select(c => c.Last()).ToArray());
    }

    public string Part2(string[] input)
    {
        return new string(GetCols(input).Select(c => c.First()).ToArray());
    }

    private static char[][] GetCols(string[] input)
    {
        return Enumerable.Range(0, input[0].Length)
            .Select(x => Enumerable.Range(0, input.Length).Select(y => input[y][x]).ToArray().GroupBy(c => c).OrderBy(g => g.Count()).Select(g => g.Key).ToArray()
        ).ToArray();
    }
}