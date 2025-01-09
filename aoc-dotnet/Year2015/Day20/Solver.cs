namespace aoc_dotnet.Year2015.Day20;

public class Solver : SolverInterface
{
    public string Part1(string[] input)
    {
        var target = int.Parse(input[0]);
        var houses = new int[target / 10];
        for (var e = 1; e < target / 10; e++)
        {
            for (var h = e; h < target / 10; h += e)
            {
                houses[h - 1] += 10 * e;
            }
        }

        return "" + (houses.ToList().FindIndex(h => h >= target) + 1);
    }

    public string Part2(string[] input)
    {
        var target = int.Parse(input[0]);
        var houses = new int[target / 10];
        for (var e = 1; e < target / 10; e++)
        {
            for (var h = e; h < target / 10; h += e)
            {
                houses[h - 1] += 11 * e;
                if (h >= e * 50) break;
            }
        }

        return "" + (houses.ToList().FindIndex(h => h >= target) + 1);
    }
}