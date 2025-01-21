using System.Collections.Immutable;

namespace aoc_dotnet.Year2017.Day21;

public class Solver : SolverInterface
{
    public string Part1(string[] input)
    {
        var map = GetMap(input);
        var grid = ".#.\n..#\n###".Split("\n").Select(l => l.ToCharArray().ToList()).ToList();
        for (var t = 0; t < 5; t++)
        {
            grid = Enhance(grid, map);
        }
        return "" + grid.Sum(l => l.Count(x => x == '#'));
    }

    public string Part2(string[] input)
    {
        var map = GetMap(input);
        var grid = ".#.\n..#\n###".Split("\n").Select(l => l.ToCharArray().ToList()).ToList();
        for (var t = 0; t < 18; t++)
        {
            grid = Enhance(grid, map);
        }
        return "" + grid.Sum(l => l.Count(x => x == '#'));
    }

    private List<List<char>> Enhance(List<List<char>> grid, ImmutableDictionary<string, string> map)
    {
        var size = grid.Count % 2 == 0 ? 2 : 3;
        var nextGrid = new List<List<char>>();
        for (var y = 0; y < grid.Count; y += size)
        {
            var rows = grid.Skip(y).Take(size).ToArray();
            var nextRows = new List<List<char>>();
            for (var i = 0; i < size + 1; i++) nextRows.Add([]);
            for (var x = 0; x < grid[y].Count; x += size)
            {
                var chunk = string.Join('/', rows.Select(r => new string(r.Skip(x).Take(size).ToArray())));
                var nextChunk = map[chunk].Split('/');
                for (var xn = 0; xn < nextChunk.Length; xn++) nextRows[xn].AddRange(nextChunk[xn]);
            }
            nextGrid.AddRange(nextRows);
        }
        return nextGrid;
    }

    private ImmutableDictionary<string, string> GetMap(string[] input)
    {
        var map = new Dictionary<string, string>();
        foreach (var line in input)
        {
            var parts = line.Split(" => ");
            var versions = AllVersions(parts[0]);
            foreach (var from in versions) map.Add(from, parts[1]);
        }

        return map.ToImmutableDictionary();
    }

    private string[] AllVersions(string input)
    {
        var versions = new List<string> { input, FlipX(input), FlipY(input) };
        for (var i = 0; i < 3; i++)
        {
            input = Rotate(input);
            versions.AddRange([input, FlipX(input), FlipY(input)]);
        }

        return versions.Distinct().ToArray();
    }

    private string Rotate(string input)
    {
        // 123/456/789 => 741/852/963
        var lines = input.Split('/');
        var rotatedLines = new List<string>();
        for (var i = lines[0].Length - 1; i >= 0; i--)
        {
            var p = lines.Select(l => l[i]).Reverse().ToArray();
            rotatedLines.Add(new string(p));
        }

        rotatedLines.Reverse();
        return string.Join('/', rotatedLines);
    }

    private string FlipX(string input)
    {
        return string.Join('/', input.Split('/').Select(r => new string(r.Reverse().ToArray())));
    }

    private string FlipY(string input)
    {
        return string.Join('/', input.Split('/').Reverse());
    }
}