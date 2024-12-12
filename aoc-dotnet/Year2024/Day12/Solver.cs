using System.Collections.Immutable;
using System.Numerics;

namespace aoc_dotnet.Year2024.Day12;

using Map = ImmutableDictionary<Complex, char>;

record struct Region(char id, int area, int perimeter, int corners);

public class Solver: SolverInterface
{
    private readonly Complex Up = Complex.ImaginaryOne;
    private readonly Complex Down = -Complex.ImaginaryOne;
    private readonly Complex Right = 1;
    private readonly Complex Left = -1;
    public string Part1(string[] input)
    {
        return "" + CountRegions(input).Select(r => r.area * r.perimeter).Sum();
    }

    public string Part2(string[] input)
    {
        // 897844 too low
        return "" + CountRegions(input).Select(r => r.area * r.corners).Sum();
    }

    private List<Region> CountRegions(string[] input)
    {
        var map = GetMap(input);
        var visited = new HashSet<Complex>();
        var directions = new[] { Up, Down, Left, Right };
        var verticals = new[] { Up, Down };
        var horizontals = new[] { Left, Right };
        var queue = new Queue<KeyValuePair<Complex, char>>();
        var current = new Region();
        var regions = new List<Region>();
        while (visited.Count < map.Count)
        {
            if (queue.Count == 0)
            {
                regions.Add(current);
                current = new Region();
                if (map.Count == 0) break;
                queue.Enqueue(map.First(x => !visited.Contains(x.Key)));
            }
            var pos = queue.Dequeue();
            if (!visited.Add(pos.Key)) continue;
            current.area++;
            current.id = pos.Value;
            // Outside corners
            var vNeighbours = verticals.Select(x => map.GetValueOrDefault(pos.Key + x, '#')).ToArray();
            var hNeighbours = horizontals.Select(x => map.GetValueOrDefault(pos.Key + x, '#')).ToArray();
            current.corners += vNeighbours.Count(x => x != current.id) * hNeighbours.Count(x => x != current.id);
            var insideCorners = verticals.Where(x => map.GetValueOrDefault(pos.Key + x, '#') == current.id).Sum(x => horizontals.Count(h =>
                                                        map.GetValueOrDefault(pos.Key + h, '#') == current.id &&
                                                        map.GetValueOrDefault(pos.Key + x + h, '#') != current.id));
            current.corners += insideCorners;

            foreach (var d in directions)
            {
                var n = pos.Key + d;
                var v = map.GetValueOrDefault(n, '#');
                if (v == pos.Value)
                {
                    queue.Enqueue(new KeyValuePair<Complex, char>(n, pos.Value));
                }
                else
                {
                    current.perimeter++;
                }
            }
            
        }
        regions.Add(current);

        return regions;
    }

    private Map GetMap(string[] input)
    {
        return (
            from y in Enumerable.Range(0, input.Length)
            from x in Enumerable.Range(0, input[y].Length)
            select new KeyValuePair<Complex, char>(Down * y + x, input[y][x])
        ).ToImmutableDictionary();
    }
}