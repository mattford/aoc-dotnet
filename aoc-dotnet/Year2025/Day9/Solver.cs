namespace aoc_dotnet.Year2025.Day9;

public class Solver : SolverInterface
{
    public string Part1(string[] input)
    {
        var tiles = input.Select(l => l.Split(",").Select(long.Parse).ToList()).ToList();
        var best = 0L;
        while (tiles.Count > 1)
        {
            var f = tiles.First();
            tiles = tiles.Skip(1).ToList();
            best = Math.Max(best, tiles.Max(o => (1 + Math.Abs(f[0] - o[0])) * (1 + Math.Abs(f[1] - o[1]))));
        }

        return "" + best;
    }

    public string Part2(string[] input)
    {
        var tiles = input.Select(l => l.Split(",").Select(long.Parse).ToList()).ToList();
        var edges = new List<(long, long, long, long)>();
        for (var i = 0; i < tiles.Count; i++)
        {
            var f = tiles[i];
            var o = i == tiles.Count - 1 ? tiles[0] : tiles[i + 1];
            var dimsX = new List<long> { f[0], o[0] };
            var dimsY = new List<long> { f[1], o[1] };
            edges.Add((dimsX[0], dimsY[0], dimsX[1], dimsY[1]));
        }
        var best = 0L;
        while (tiles.Count > 1)
        {
            var f = tiles.First();
            tiles = tiles.Skip(1).ToList();
            var validTiles = tiles.Where(o => IsValidSquare(edges, f, o)).ToList();
            if (!validTiles.Any()) continue;
            best = Math.Max(best,
                validTiles
                    .Max(o => (1 + Math.Abs(f[0] - o[0])) * (1 + Math.Abs(f[1] - o[1]))));
        }

        return "" + best;
    }

    private static bool IsValidSquare(List<(long, long, long, long)> edges, List<long> a, List<long> b)
    {
        var x1 = Math.Min(a[0], b[0]);
        var y1 = Math.Min(a[1], b[1]);
        var x2 = Math.Max(a[0], b[0]);
        var y2 = Math.Max(a[1], b[1]);

        foreach (var edge in edges)
        {
            var (ex1, ey1, ex2, ey2) = edge;

            if (ey1 < y2 && ey2 > y1 && ex1 < x2 && ex2 > x1) return false;
        }

        return true;
    }
}