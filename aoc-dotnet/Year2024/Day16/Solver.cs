using System.Collections.Immutable;
using System.Numerics;

namespace aoc_dotnet.Year2024.Day16;

using Map = ImmutableDictionary<Complex, char>;

public class Solver: SolverInterface
{
    private Complex North = -Complex.ImaginaryOne;
    private Complex South = Complex.ImaginaryOne;
    private Complex West = -1;
    private Complex East = 1;

    private Complex TurnLeft = -Complex.ImaginaryOne;
    private Complex TurnRight = Complex.ImaginaryOne;
    public string Part1(string[] input)
    {
        var (map, start, end) = GetMap(input);
        var (best, _) = FindShortestPath(map, start, end);
        return "" + best;
    }

    public string Part2(string[] input)
    {
        var (map, start, end) = GetMap(input);
        var (_, bestSpaces) = FindShortestPath(map, start, end);
        // 9535 too high
        return "" + bestSpaces;
    }

    private (int, int) FindShortestPath(Map map, Complex start, Complex end)
    {
        var queue = new PriorityQueue<(Complex, Complex, int, List<Complex>), int>();
        var visited = new HashSet<(Complex, Complex)>();
        var bestSpaces = new HashSet<Complex>();
        var best = 0;
        queue.Enqueue((start, East, 0, []), int.MaxValue - 0);
        while (queue.Count > 0)
        {
            var (pos, dir, cost, thisSpaces) = queue.Dequeue();
            visited.Add((pos, dir));
            thisSpaces.Add(pos);
            if (pos == end)
            {
                if (best > 0 && cost > best)
                {
                    return (best, bestSpaces.Count);
                }
                bestSpaces.UnionWith(thisSpaces);
                best = cost;
                continue;
            }
            if (map.ContainsKey(pos + dir) && !visited.Contains((pos + dir, dir)))
            {
                queue.Enqueue((pos + dir, dir, cost + 1, thisSpaces.ToList()), cost + 1);
            }

            if (!visited.Contains((pos, dir * TurnLeft)))
            {
                queue.Enqueue((pos, dir * TurnLeft, cost + 1000, thisSpaces.ToList()), cost + 1000);
            }
            
            if (!visited.Contains((pos, dir * TurnRight)))
            {
                queue.Enqueue((pos, dir * TurnRight, cost + 1000, thisSpaces.ToList()), cost + 1000);
            }
        }

        return (0, 0);
    }

    private (Map, Complex, Complex) GetMap(string[] input)
    {
        var map = (
            from y in Enumerable.Range(0, input.Length)
            from x in Enumerable.Range(0, input[y].Length)
            where input[y][x] != '#'
            select new KeyValuePair<Complex, char>(South * y + x, input[y][x] == 'E' || input[y][x] == 'S' ? '.' : input[y][x])
        ).ToImmutableDictionary();
        var endY = input.ToList().FindIndex(x => x.Contains('E'));
        var endPoint = South * endY + input[endY].IndexOf('E');
        var startY = input.ToList().FindIndex(x => x.Contains('S'));
        var startPoint = South * startY + input[startY].IndexOf('S');
        return (map, startPoint, endPoint);
    }
}