namespace aoc_dotnet.Year2024.Day23;

using Map = Dictionary<string, List<string>>;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var map = GetMap(input);
        var nodes = FindNetworks(map, map.Keys.ToArray());
        nodes = FindNetworks(map, nodes.ToArray());
        var total = nodes.Count(n => n.Split(',').Any(x => x.StartsWith('t')));
        return "" + total;
    }

    public string Part2(string[] input)
    {
        var map = GetMap(input);
        var nodes = map.Keys.ToHashSet();
        while (nodes.Count > 1)
        {
            nodes = FindNetworks(map, nodes.ToArray());
        }

        return nodes.First();
    }

    private HashSet<string> FindNetworks(Map map, string[] nodes)
    {
        var nextNodes = new List<string>();
        foreach (var node in nodes)
        {
            var members = node.Split(',');
            var neighbours = members
                .SelectMany(m => map[m])
                .Distinct()
                .Where(n => !members.Contains(n))
                .Where(n => members.All(m => map[n].Contains(m)));
            nextNodes.AddRange(neighbours.Select(neighbour => string.Join(',', members.Append(neighbour).Order())).ToList());
        }

        return nextNodes.ToHashSet();
    }

    private static Map GetMap(string[] input)
    {
        var dict = new Map();
        foreach (var line in input)
        {
            var parties = line.Split("-");
            dict.TryAdd(parties[0], []);
            dict.TryAdd(parties[1], []);
            dict[parties[0]].AddRange(parties);
            dict[parties[1]].AddRange(parties);
        }

        return dict.ToDictionary(x => x.Key, x => x.Value.Distinct().Order().ToList());
    }
    
}