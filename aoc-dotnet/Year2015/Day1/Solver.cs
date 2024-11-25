namespace aoc_dotnet.Year2015.Day1;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var floor = 0;
        var line = input[0];
        foreach (var s in line)
        {
            if (s == '(') floor++;
            if (s == ')') floor--;
        }
        return ""+floor;
    }

    public string Part2(string[] input)
    {
        var floor = 0;
        var idx = 0;
        var line = input[0];
        foreach (var s in line)
        {
            if (s == '(') floor++;
            if (s == ')') floor--;
            idx++;
            if (floor < 0)
            {
                return ""+idx;
            }
        }
        return "0";
    }
}