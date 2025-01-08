using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2015.Day13;

using Map = Dictionary<string, Dictionary<string, int>>;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var map = ParseInput(input);
        return "" + GetMaxHappiness(map);
    }

    public string Part2(string[] input)
    {
        var map = ParseInput(input);
        var meMap = map.ToDictionary(kv => kv.Key, _ => 0);
        foreach (var mapKey in map.Keys)
        {
            map[mapKey]["Matt"] = 0;
        }
        map["Matt"] = meMap;
        return "" + GetMaxHappiness(map);
    }

    private static int GetMaxHappiness(Map pairs)
    {
        var queue = new PriorityQueue<(string, string[], int), int>();
        queue.Enqueue((pairs.Keys.First(), [pairs.Keys.First()], 0), 0);
        var m = 0;
        while (queue.Count > 0)
        {
            var (person, sequence, cost) = queue.Dequeue();
            foreach (var kv in pairs[person])
            {
                if (kv.Key == person || sequence.Contains(kv.Key)) continue;
                var nextSequence = sequence.ToList();
                nextSequence.Add(kv.Key);
                if (nextSequence.Count == pairs.Keys.Count)
                {
                    m = Math.Max(m, cost + kv.Value + pairs[kv.Key][nextSequence.First()]);
                }
                else
                {
                    queue.Enqueue((kv.Key, nextSequence.ToArray(), cost + kv.Value), cost + kv.Value);
                }
            }
        }
        return m;
    }
    
    private static Map ParseInput(string[] input)
    {
        var map = new Map();
        foreach (var line in input)
        {
            var match = Regex.Match(line,
                @"([\w]+) would (gain|lose) ([\d]+) happiness units by sitting next to ([\w]+).");
            var people = new[] { match.Groups[1].Value, match.Groups[4].Value }.Order().ToArray();
            var units = int.Parse(match.Groups[3].Value);
            if (match.Groups[2].Value == "lose") units = 0 - units;
            map.TryAdd(people[0], new Dictionary<string, int>());
            map[people[0]].TryAdd(people[1], 0);
            map[people[0]][people[1]] += units;
            map.TryAdd(people[1], new Dictionary<string, int>());
            map[people[1]].TryAdd(people[0], 0);
            map[people[1]][people[0]] += units;
        }

        return map;
    }
}