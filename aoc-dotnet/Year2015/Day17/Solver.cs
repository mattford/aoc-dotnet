using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace aoc_dotnet.Year2015.Day17;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var containers = input.Select((v, i) => (i, int.Parse(v))).ToImmutableList();
        var cache = new ConcurrentDictionary<(int, string), HashSet<string>>();
        var combos = GetCombinations(containers, 150, 1, [], cache).Count;
        return "" + combos;
    }

    public string Part2(string[] input)
    {
        var containers = input.Select((v, i) => (i, int.Parse(v))).ToImmutableList();
        var cache = new ConcurrentDictionary<(int, string), HashSet<string>>();
        var combos = GetCombinations(containers, 150, 1, [], cache);
        var min = combos.Min(x => x.Split(",").Length);
        return "" + combos.Count(x => x.Split(",").Length == min);
    }

    private HashSet<string> GetCombinations(ImmutableList<(int, int)> containers, int target, int depth, ImmutableList<(int, int)> soFar, ConcurrentDictionary<(int, string), HashSet<string>> cache)
    {
        return cache.GetOrAdd((target, string.Join(",", containers.Order().ToList())), _ =>
        {
            var ways = new HashSet<string>();
            foreach (var (id, container) in containers)
            {
                if (container > target) continue;
                var nextSoFar = soFar.Add((id, container));
                if (container == target)
                {
                    ways.Add(string.Join(",", nextSoFar.OrderBy(x => x.Item1).Select(x => $"{x.Item1}:{x.Item2}").ToList()));
                    continue;
                }
                var nextContainers = containers.Remove((id, container));
                
                ways.UnionWith(GetCombinations(nextContainers, target - container, depth + 1, nextSoFar, cache));
            }

            return ways;
        });
    }
    
    
}