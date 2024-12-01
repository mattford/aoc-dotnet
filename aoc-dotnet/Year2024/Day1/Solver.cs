using System.Collections;

namespace aoc_dotnet.Year2024.Day1;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var listA = new int[input.Length];
        var listB = new int[input.Length];
        var i = 0;
        foreach (var line in input)
        {
            var numbers = line.Split().Where(s => s != "").Select(int.Parse).ToArray();
            listA[i] = numbers[0];
            listB[i] = numbers[1];
            i++;
        }

        var orderedA = listA.Order().ToArray();
        var orderedB = listB.Order().ToArray();
        var total = orderedA.Select((t, j) => int.Abs(t - orderedB[j])).Sum();

        return "" + total;
    }

    public string Part2(string[] input)
    {
        var listA = new int[input.Length];
        var listB = new int[input.Length];
        var i = 0;
        foreach (var line in input)
        {
            var numbers = line.Split().Where(s => s != "").Select(int.Parse).ToArray();
            listA[i] = numbers[0];
            listB[i] = numbers[1];
            i++;
        }
        var groupedB = listB.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
        var total = listA.Select(x => groupedB.GetValueOrDefault(x, 0) * x).Sum();

        return "" + total;
    }
}