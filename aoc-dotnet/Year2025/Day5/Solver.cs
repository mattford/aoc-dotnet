namespace aoc_dotnet.Year2025.Day5;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var fullInput = string.Join("\n", input);
        var split = fullInput.Split("\n\n");
        var ranges = split[0].Split("\n").Select(r => r.Split("-").Select(long.Parse).ToList()).ToList();
        var ingredients = split[1].Split("\n").Select(long.Parse).ToList();
        var freshCount = ingredients.Count(i => ranges.Any(r => r[0] <= i && r[1] >= i));
        return "" + freshCount;
    }

    public string Part2(string[] input)
    {
        var fullInput = string.Join("\n", input);
        var split = fullInput.Split("\n\n"); ;
        var ranges = MergeRanges(split[0].Split("\n").Select(r => r.Split("-").Select(long.Parse).ToList()).ToList());
        return "" + ranges.Sum(r => 1 + r[1] - r[0]);
    }

    private List<List<long>> MergeRanges(List<List<long>> ranges)
    {

        for (var i = 0; i < ranges.Count; i++)
        {
            var r1 = ranges[i];
            var thisRange = r1.ToList();
            for (var j = i + 1; j < ranges.Count; j++)
            {
                var r2 = ranges[j];
                if (!Overlaps(r1, r2)) continue;
                Console.WriteLine($"Merging ranges {i} -> {j}");
                ranges[i] = [Math.Min(r1[0], r2[0]), Math.Max(r1[1], r2[1])];
                ranges.RemoveAt(j);
                return MergeRanges(ranges);
            }
        }
        return ranges;
    }

    private bool Overlaps(List<long> r1, List<long> r2)
    {
        return (r1[0] <= r2[1] && r1[1] >= r2[0]) || (r2[0] <= r1[1] && r2[1] >= r1[0]);
    }
}