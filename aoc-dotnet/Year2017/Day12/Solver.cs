using System.Collections.Immutable;

namespace aoc_dotnet.Year2017.Day12;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + GetGroup(ParseInput(input), 0, []).Count;
    }

    public string Part2(string[] input)
    {
        var connections = ParseInput(input);
        var grouped = new List<int>();
        grouped.AddRange(GetGroup(ParseInput(input), 0, []));
        var i = 1;
        while (connections.Any(x => !grouped.Contains(x.Key)))
        {
            var next = connections.First(x => !grouped.Contains(x.Key));
            grouped.AddRange(GetGroup(connections, next.Key, []));
            i++;
        }

        return "" + i;
    }

    private List<int> GetGroup(ImmutableDictionary<int, List<int>> connections, int start, ImmutableHashSet<int> visited)
    {
        if (visited.Contains(start)) return [];
        var nextVisited = visited.Add(start);
        var group = new List<int>();
        group.AddRange(connections[start]);
        foreach (var connection in connections[start])
        {
            if (nextVisited.Contains(connection)) continue;
            group.AddRange(GetGroup(connections, connection, nextVisited));
        }

        return group.Distinct().ToList();
    }

    private ImmutableDictionary<int, List<int>> ParseInput(string[] input)
    {
        return (
            from line in input
            let parts = line.Split(" <-> ")
            select new KeyValuePair<int, List<int>>(int.Parse(parts[0]), parts[1].Split(", ").Select(int.Parse).ToList())
        ).ToImmutableDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}