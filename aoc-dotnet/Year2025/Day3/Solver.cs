namespace aoc_dotnet.Year2025.Day3;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        // Turn on two batteries in a row, find the biggest number before the end, then find the biggest number after that.
        var sum = input
            .Select(bank => bank.ToCharArray().Select(c => int.Parse("" + c)).ToList())
            .Select(joltages => GetBestJoltage(joltages, 2))
            .Sum();

        return "" + sum;
    }

    public string Part2(string[] input)
    {
        // Turn on 12 batteries in each bank.
        var sum = input
            .Select(bank => bank.ToCharArray().Select(c => int.Parse("" + c)).ToList())
            .Select(joltages => GetBestJoltage(joltages, 12))
            .Sum();

        return "" + sum; 
    }

    private static long GetBestJoltage(List<int> joltages, int batteries)
    {
        var v = 0L;
        var i = 0;
        for (var c = batteries - 1; c >= 0; c--)
        {
            v *= 10;
            var (t, ni) = GetBestJoltage(joltages[i..^c]);
            i += ni + 1;
            v += t;
        }

        return v;
    }

    private static (int, int) GetBestJoltage(List<int> joltages)
    {
        var max = joltages.Max();
        var pos = joltages.IndexOf(max);
        return (max, pos);
    }
}