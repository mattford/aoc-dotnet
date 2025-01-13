using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2016.Day11;

public class Solver : SolverInterface
{
    public string Part1(string[] input)
    {
        var initialState = GetInitialState(input);
        return "" + FindMinimumSteps(initialState);
    }

    public string Part2(string[] input)
    {
        var initialState = GetInitialState(input);
        initialState[0].AddRange(["elerium:m", "elerium:g", "dilithium:m", "dilithium:g"]);
        return "" + FindMinimumSteps(initialState);
    }

    private int FindMinimumSteps(List<List<string>> initialState)
    {
        var totalItems = initialState.Sum(x => x.Count);
        var queue = new PriorityQueue<(List<List<string>>, int, int, ImmutableList<string>), int>();
        queue.Enqueue((initialState, 0, 0, []), 0);
        var visited = new HashSet<string>();
        while (queue.Count > 0)
        {
            var (state, floor, cost, path) = queue.Dequeue();
            if (!visited.Add(HashState(state, floor))) continue;
            var nextStates = GetNextStates(state, floor);
            foreach (var (nextState, nextFloor, nextCost, desc) in nextStates)
            {
                if (visited.Contains(HashState(nextState, nextFloor))) continue;
                if (nextState[3].Count == totalItems)
                {
                    // foreach (var d in path)
                    // {
                    //     Console.WriteLine(d);
                    // }
                    return cost + nextCost;
                }

                queue.Enqueue((nextState, nextFloor, cost + nextCost, path.Add(desc)), cost + nextCost);
            }
        }

        return 0;
    }

    private List<(List<List<string>>, int, int, string)> GetNextStates(List<List<string>> state, int floor)
    {
        // Start by moving up
        var combinations = GetCombinations(state[floor]).ToList();
        var nextStates = new List<(List<List<string>>, int, int, string)>();

        foreach (var mod in new[] { 1, -1 })
        {
            var nf = floor + mod;
            if (nf is > 3 or < 0) continue;
            var min = int.MaxValue;
            var max = 0;
            foreach (var combination in combinations)
            {
                if ((nf > floor && combination.Count < max) || (nf < floor && combination.Count > min)) continue; 
                var nextState = state.Select(x => x.ToList()).ToList();
                nextState[floor] = nextState[floor].Except(combination).Order().ToList();
                nextState[nf] = nextState[nf].Union(combination).Order().ToList();
                if (new[] { floor, nf }.Any(f =>
                        nextState[f].Any(i => i.EndsWith(":g")) && nextState[f].Any(it =>
                            it.EndsWith(":m") && !nextState[f].Contains(it.Split(":")[0] + ":g"))))
                continue;

                min = Math.Min(min, combination.Count);
                max = Math.Max(max, combination.Count);
                
                nextStates.Add((nextState, nf, 1, string.Join(", ", combination) + " to " + nf));
            }
        }

        return nextStates;
    }

    private List<List<string>> GetCombinations(List<string> items)
    {
        var combos = new List<List<string>>();
        // Get groups of 1 or more items, which must contain a microchip
        var grouped = items.GroupBy(x => x.Split(":")[0]).ToList();
        // of the size 2 groups, take the first one
        var doubleGroup = grouped.FirstOrDefault(g => g.Count() == 2);
        var singles = grouped.Where(g => g.Count() == 1).SelectMany(g => g.ToList()).ToList();
        if (doubleGroup != null)
        {
            combos.Add(doubleGroup.ToList());
            combos.AddRange(doubleGroup.Select(x => new List<string>{x}).ToList());
            foreach (var single in singles)
            {
                combos.AddRange(doubleGroup.Select(d => new List<string>{d, single}).ToList());
            }
        }
        foreach (var single in singles)
        {
            combos.Add([single]);
            foreach (var other in singles)
            {
                if (single == other) continue;
                combos.Add([single, other]);
            }
        }
        
        return combos.DistinctBy(c => string.Join(",", c.Order())).ToList();
    }

    private string HashState(List<List<string>> state, int floor)
    {
        // 2 pairs are interchangeable, so make them so
        var anonomised = state.Select(fl =>
        {
            var groups = fl.GroupBy(x => x.Split(":")[0]).ToList();
            return "p:" + groups.Count(g => g.Count() == 2) + "," +
                   string.Join(",", groups.Where(g => g.Count() == 1).SelectMany(g => g));
        });
        return $"{floor}/{string.Join("/", anonomised)}";
    }

    private static List<List<string>> GetInitialState(string[] input)
    {
        var state = new List<List<string>>();
        foreach (var line in input)
        {
            var items = new List<string>();
            if (!line.Contains("contains nothing"))
            {
                items = Regex.Matches(line, "a ([a-z]+)(?:-compatible)? ([a-z])[a-z]+")
                    .Select(m => $"{m.Groups[1].Value}:{m.Groups[2].Value[0]}").ToList();
            }

            state.Add(items);
        }

        return state;
    }
}