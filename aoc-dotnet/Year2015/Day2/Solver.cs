namespace aoc_dotnet.Year2015.Day2;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var result = input
            .Select(item => item.Split("x").Select(int.Parse).Order().ToArray())
            .Select(WrappingPaperSize)
            .Sum();

        return "" + result;
    }

    public string Part2(string[] input)
    {
        var result = input
            .Select(item => item.Split("x").Select(int.Parse).Order().ToArray())
            .Select(RibbonSize)
            .Sum();

        return "" + result;
    }

    private static int WrappingPaperSize(int[] dims)
    {
        var width = dims[0];
        var length = dims[1];
        var height = dims[2];

        var extra = dims[0] * dims[1];
        return extra + 2 * length * width + 2 * width * height + 2 * height * length;
    }

    private static int RibbonSize(int[] dims)
    {
        return dims[0] * 2 + dims[1] * 2 + dims[0] * dims[1] * dims[2];
    }
}