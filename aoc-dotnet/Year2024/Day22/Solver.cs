namespace aoc_dotnet.Year2024.Day22;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + input.Select(x => Next(long.Parse(x), 2000).Last()).Sum();
    }

    public string Part2(string[] input)
    {
        var groups = new Dictionary<string, long>();
        foreach (var line in input)
        {
            var visited = new HashSet<string>();
            var n = long.Parse(line);
            var diffs = GetDiffs(Next(n, 2000).Select(x => x % 10).ToArray());
            for (var i = 3; i < diffs.Length; i++)
            {
                var last4 = string.Join(",", diffs[(i - 3)..(i+1)].Select(x => x.Item2).ToArray());
                if (visited.Contains(last4)) continue;
                visited.Add(last4);
                groups.TryAdd(last4, 0);
                groups[last4] += diffs[i].Item1;
            }

        }

        return "" + groups.Max(x => x.Value);
    }

    private (long, long)[] GetDiffs(long[] ns)
    {
        var diffs = new (long, long)[ns.Length - 1];
        var idx = 0;
        for (var i = 1; i < ns.Length - 1; i++)
        {
            diffs[idx] = (ns[i], ns[i] - ns[i - 1]);
            idx++;
        }

        return diffs;
    }

    private long[] Next(long n, int c)
    {
        var ns = new long[c];
        for (var i = 0; i < c; i++)
        {
            n = Next(n);
            ns[i] = n;
        }

        return ns;
    }
    
    private long Next(long n)
    {
        var n2 = Prune(Mix(n, n * 64));
        var n3 = Prune(Mix(n2, n2 / 32));
        return Prune(Mix(n3, n3 * 2048));
    }
    
    private long Mix(long n, long m)
    {
        return n ^ m;
    }
    
    private long Prune(long n)
    {
        return n % 16777216;
    }
}