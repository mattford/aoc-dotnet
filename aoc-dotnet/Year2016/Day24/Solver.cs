using System.Collections.Immutable;
using System.Numerics;

namespace aoc_dotnet.Year2016.Day24;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var (grid, points) = GetGrid(input);
        var routes = GetFastestRoutes(grid, points);
        return "" + TravelingSalesmanSimulator(routes, false);
    }

    public string Part2(string[] input)
    {
        var (grid, points) = GetGrid(input);
        var routes = GetFastestRoutes(grid, points);
        return "" + TravelingSalesmanSimulator(routes, true);
    }

    private static int TravelingSalesmanSimulator(ImmutableDictionary<char, ImmutableDictionary<char, int>> routes, bool backHome)
    {
        return TravelingSalesmanSimulator(routes, '0', ['0'], ['0'], 0, backHome);
    }

    private static int TravelingSalesmanSimulator(ImmutableDictionary<char, ImmutableDictionary<char, int>> routes, char pos, ImmutableHashSet<int> visited, ImmutableList<int>route,  int cost, bool backHome = false)
    {
        if (visited.Count == routes.Keys.Count())
        {
            if (backHome) cost += routes[pos]['0'];
            return cost;
        }

        var min = int.MaxValue;
        foreach (var next in routes[pos])
        {
            if (visited.Contains(next.Key)) continue;
            var nextCost = cost + next.Value;
            min = Math.Min(min,
                TravelingSalesmanSimulator(routes, next.Key, visited.Add(next.Key), route.Add(next.Key), nextCost, backHome));
        }

        return min;
    }

    private static ImmutableDictionary<char, ImmutableDictionary<char, int>> GetFastestRoutes(List<Complex> grid, Dictionary<Complex, char> points)
    {
        var routes = new Dictionary<char, ImmutableDictionary<char, int>>();
        foreach (var (startPos, start) in points)
        {
            var fastestRoutes = new Dictionary<char, int>();
            var queue = new PriorityQueue<(Complex, int), int>();
            queue.Enqueue((startPos, 0), 0);
            var visited = new HashSet<Complex>();
            while (fastestRoutes.Count < points.Count && queue.Count > 0)
            {
                var (pos, cost) = queue.Dequeue();
                if (!visited.Add(pos)) continue;
                if (points.TryGetValue(pos, out var id)) fastestRoutes.TryAdd(id, cost);
                var d = new[] { Complex.ImaginaryOne, -Complex.ImaginaryOne, -1, 1 };
                foreach (var n in d)
                {
                    if (!grid.Contains(pos + n)) continue;
                    queue.Enqueue((pos + n, cost + 1), cost + 1);
                }
            }

            routes[start] = fastestRoutes.ToImmutableDictionary();
        }

        return routes.ToImmutableDictionary();
    }

    private (List<Complex>, Dictionary<Complex, char>) GetGrid(string[] input)
    {
        var points = new Dictionary<Complex, char>();
        var grid = new List<Complex>();
        foreach (var y in Enumerable.Range(0, input.Length))
        {
            foreach (var x in Enumerable.Range(0, input[y].Length))
            {
                if (input[y][x] == '#') continue;
                var coord = Complex.ImaginaryOne * y + x;
                if (input[y][x] != '.') points.Add(coord, input[y][x]);
                grid.Add(coord);
            }
        }

        return (grid, points);
    }
}