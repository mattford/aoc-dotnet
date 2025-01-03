namespace aoc_dotnet.Year2015.Day9;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var map = ParseInput(input);
        return "" + FindRoutes(map).Min();
    }

    public string Part2(string[] input)
    {
        var map = ParseInput(input);
        return "" + FindRoutes(map).Max();
    }

    private List<int> FindRoutes(Dictionary<string, Dictionary<string, int>> map)
    {
        return map.Keys.SelectMany(x => FindRoutes(map, x, 0, [x])).ToList();
    }
    
    private List<int> FindRoutes(Dictionary<string, Dictionary<string, int>> map, string current, int cost, HashSet<string> visited)
    {
        var routes = new List<int>();
        foreach (var other in map[current])
        {
            if (visited.Contains(other.Key)) continue;
            var myVisited = visited.ToHashSet();
            myVisited.Add(other.Key);
            var myCost = cost + other.Value;
            if (myVisited.Count == map.Count)
            {
                routes.Add(myCost);
                continue;
            }
            routes.AddRange(FindRoutes(map, other.Key, myCost, myVisited));
        }

        return routes;
    }

    private Dictionary<string, Dictionary<string, int>> ParseInput(string[] input)
    {
        var dict = new Dictionary<string, Dictionary<string, int>>();
        foreach (var line in input)
        {
            var parts = line.Replace(" to ", " ").Replace(" = ", " ").Split(" ");
            dict.TryAdd(parts[0], []);
            dict[parts[0]].TryAdd(parts[1], int.Parse(parts[2]));
            dict.TryAdd(parts[1], []);
            dict[parts[1]].TryAdd(parts[0], int.Parse(parts[2]));
        }

        return dict;
    }
}