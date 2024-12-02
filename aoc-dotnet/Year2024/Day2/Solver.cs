namespace aoc_dotnet.Year2024.Day2;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return ""+input.Count(IsSafe);
    }

    public string Part2(string[] input)
    {
        return ""+input.Count(v => IsSafe(v, true));
    }

    private static bool IsSafe(string input, bool dampener)
    {
        if (IsSafe(input))
        {
            return true;
        }
        // Remove each level in turn until we get safe or run out
        if (dampener)
        {
            var numbers = input.Split(" ").Select(int.Parse).ToArray();
            for (var i = 0; i < input.Length; i++)
            {
                var newInput = string.Join(" ", numbers.Where((_, j) => j != i).ToArray());
                if (IsSafe(newInput))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool IsSafe(string input)
    {
        var numbers = input
            .Split(" ")
            .Select(int.Parse)
            .ToArray();
        
        // all diffs same sign
        // diffs are >= 1 and <= 3
        var safe = true;
        var trend = numbers[0] - numbers[1] < 0;
        for (var i = 0; i < numbers.Length - 1; i++)
        {
            var diff = numbers[i] - numbers[i + 1];
            var followingTrend = diff < 0 == trend;
            if (!followingTrend || int.Abs(diff) < 1 || int.Abs(diff) > 3)
            {
                safe = false;
            }
        }
        return safe;
    }
}