using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2017.Day24;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var bridges = GetBridges(input);
        return "" + bridges.OrderByDescending(b => b.Item2).First().Item2;
    }

    public string Part2(string[] input)
    {
        var bridges = GetBridges(input);
        return "" + bridges.OrderByDescending(x => x.Item1).ThenByDescending(x => x.Item2).First().Item2;
    }

    private List<(int, int, ImmutableList<(int, int)>)> GetBridges(string[] input)
    {
        var bridges = new List<(int, int, ImmutableList<(int, int)>)>();
        var components = (
            from line in input
            let matches = Regex.Matches(line, @"(\d+)")
            select (int.Parse(matches[0].Value), int.Parse(matches[1].Value))
        ).ToImmutableList();
        foreach (var component in components)
        {
            if (component.Item1 != 0 && component.Item2 != 0) continue;
            var nextComponents = new List<(int, int)>(components.Except([component])).ToImmutableList();
            var thisBridge = new List<(int, int)> { component }.ToImmutableList();
            bridges.Add((thisBridge.Count, thisBridge.Sum(c => c.Item1 + c.Item2), thisBridge));
            bridges.AddRange(GetBridges(nextComponents, thisBridge, component.Item1 == 0 ? component.Item2 : component.Item1));
        }

        return bridges.Distinct().ToList();
    }
    
    private List<(int, int, ImmutableList<(int, int)>)> GetBridges(ImmutableList<(int, int)> components, ImmutableList<(int, int)> thisBridge, int end)
    {
        var bridges = new List<(int, int, ImmutableList<(int, int)>)>();
        foreach (var component in components)
        {
            if (component.Item1 != end && component.Item2 != end) continue;
            var nextComponents = components.Remove(component);
            var nextEnd = component.Item1 == end ? component.Item2 : component.Item1;
            var nextBridge = thisBridge.Add(component);
            bridges.Add((nextBridge.Count, nextBridge.Sum(c => c.Item1 + c.Item2), nextBridge));
            if (nextComponents.Count > 0)
            {
                bridges.AddRange(GetBridges(nextComponents, nextBridge, nextEnd));
            }
        }

        return bridges;
    }
}