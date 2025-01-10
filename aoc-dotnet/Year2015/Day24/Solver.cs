using System.Collections.Immutable;

namespace aoc_dotnet.Year2015.Day24;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var packages = input.Select(int.Parse).ToImmutableList();
        var combos = FindCombinationsOfSize(packages, packages.Sum() / 3, []);
        var ordered = combos.OrderBy(x => x.Count).ThenBy(x => x.Aggregate(1L, (a, i) => a * i));
        var firstGroup = ordered.First();
        return "" + firstGroup.Aggregate(1L, (a, i) => a * i);
    }

    public string Part2(string[] input)
    {
        var packages = input.Select(int.Parse).ToImmutableList();
        var combos = FindCombinationsOfSize(packages, packages.Sum() / 4, []);
        var ordered = combos.OrderBy(x => x.Count).ThenBy(x => x.Aggregate(1L, (a, i) => a * i));
        var firstGroup = ordered.First();
        return "" + firstGroup.Aggregate(1L, (a, i) => a * i);
    }

    private List<ImmutableList<int>> FindCombinationsOfSize(ImmutableList<int> packages, int size, ImmutableList<int> current)
    {
        var combos = new List<ImmutableList<int>>();
        var nextPackages = packages.ToList();
        foreach (var package in packages)
        {
            nextPackages.Remove(package);
            var next = current.Add(package);
            if (next.Sum() == size)
            {
                combos.Add(next);
                return combos;
            }

            if (next.Sum() > size) continue;
            if (nextPackages.Count > 0) combos = combos.Union(FindCombinationsOfSize(nextPackages.ToImmutableList(), size, next)).ToList();
        }

        return combos;
    }
}