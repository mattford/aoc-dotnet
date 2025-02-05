namespace aoc_dotnet.Year2018.Day6;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var coords = input.Select(l => l.Split(", ").Select(int.Parse).ToArray()).ToArray();
        var grid = new Dictionary<(int,int), int>();
        var (maxX, maxY) = (coords.Max(c => c[0]), coords.Max(c => c[1]));
        foreach (var y in Enumerable.Range(0, maxY))
        {
            foreach (var x in Enumerable.Range(0, maxX))
            {
                var closest = coords.Min(c => Math.Abs(c[0] - x) + Math.Abs(c[1] - y));
                var closePoints = coords.Index().Where(ic => Math.Abs(ic.Item[0] - x) + Math.Abs(ic.Item[1] - y) == closest).ToArray();
                if (closePoints.Length > 1)
                {
                    grid[(x, y)] = -1;
                }
                else
                {
                    grid[(x, y)] = closePoints[0].Index;
                }
            }
        }

        var toIgnore = new HashSet<int>{-1};
        foreach (var y in Enumerable.Range(0, maxY))
        {
            toIgnore.Add(grid[(0, y)]);
            toIgnore.Add(grid[(maxX-1, y)]);
        }

        foreach (var x in Enumerable.Range(0, maxX))
        {
            toIgnore.Add(grid[(x, 0)]);
            toIgnore.Add(grid[(x, maxY-1)]);
        }
        var max = grid.Where(x => !toIgnore.Contains(x.Value))
            .GroupBy(x => x.Value)
            .Max(x => x.Count());
        return "" + max;
    }

    public string Part2(string[] input)
    {
        var coords = input.Select(l => l.Split(", ").Select(int.Parse).ToArray()).ToArray();
        var (maxX, maxY) = (coords.Max(c => c[0]), coords.Max(c => c[1]));
        var t = Enumerable.Range(0, maxY).Sum(y => Enumerable.Range(0, maxX).Count(x => coords.Sum(c => Math.Abs(c[0] - x) + Math.Abs(c[1] - y)) < 10000));
        return "" + t;
    }
}