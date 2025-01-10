using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2015.Day22;

using State = (int, int, int, int, Dictionary<char, int>);

internal record struct Spell(string Name, int Cost, int Damage, int Healing, char? Status, int StatusDuration);

public class Solver: SolverInterface
{
    private readonly Spell[] spells =
    [
        new("Magic Missile", 53, 4, 0, null, 0),
        new("Drain", 73, 2, 2, null, 0),
        new("Shield", 113, 0, 0, 's', 6),
        new("Poison", 173, 0, 0, 'p', 6),
        new("Recharge", 229, 0, 0, 'r', 5)
    ];
    
    public string Part1(string[] input)
    {
        var boss = GetBoss(input);
        State state = (50, 500, boss.Item1, boss.Item2, []);
        var (min, path) = FindMinimumMana(state);
        return "" + min + " (" + string.Join(" -> ", path) + ")";
    }

    public string Part2(string[] input)
    {
        var boss = GetBoss(input);
        State state = (50, 500, boss.Item1, boss.Item2, new Dictionary<char, int>{ ['d'] = int.MaxValue });
        var (min, path) = FindMinimumMana(state);
        return "" + min + " (" + string.Join(" -> ", path) + ")";
    }

    private (int, string[]) FindMinimumMana(State initialState)
    {
        var queue = new PriorityQueue<(State, int, string[]), int>();
        queue.Enqueue((initialState, 0, []), 0);
        var min = int.MaxValue;
        string[] minPath = [];
        while (queue.Count > 0)
        {
            var ((playerHp, playerMana, bossHp, bossDamage, statuses), cost, path) = queue.Dequeue();
            if (statuses.ContainsKey('d')) playerHp -= 1;
            if (playerHp <= 0) continue;
            if (statuses.ContainsKey('p')) bossHp -= 3;
            if (bossHp <= 0)
            {
                if (cost < min) (min, minPath) = (cost, path);
                continue;
            }

            if (cost > min) continue;
            if (statuses.ContainsKey('r')) playerMana += 101;
            statuses = statuses.Where(kv => kv.Value > 1).Select(kv => new KeyValuePair<char, int>(kv.Key, kv.Value - 1)).ToDictionary();
        
            foreach (var spell in spells)
            {
                if (spell.Status != null && statuses.ContainsKey((char)spell.Status)) continue;
                if (spell.Cost > playerMana) continue;

                var nextPath = path.ToImmutableList().Add(spell.Name).ToArray();
                var nextUsed = cost + spell.Cost;
                var nextBossHp = bossHp - spell.Damage;
                if (nextBossHp <= 0)
                {
                    if (nextUsed < min) (min, minPath) = (nextUsed, nextPath);
                    continue;
                }
                var nextPlayerHp = playerHp + spell.Healing;
                var nextPlayerMana = playerMana - spell.Cost;
                var nextStatuses = statuses.ToDictionary();
                if (spell.Status != null) nextStatuses[(char)spell.Status] = spell.StatusDuration;
            
                // Now do the boss turn
                if (nextStatuses.ContainsKey('p')) nextBossHp -= 3;
                if (nextStatuses.ContainsKey('r')) nextPlayerMana += 101;
                nextStatuses = nextStatuses.Where(kv => kv.Value > 1).Select(kv => new KeyValuePair<char, int>(kv.Key, kv.Value - 1)).ToDictionary();
                if (nextBossHp <= 0)
                {
                    if (nextUsed < min) (min, minPath) = (nextUsed, nextPath);
                    continue;
                }

                nextPlayerHp -= Math.Max(1, bossDamage - (nextStatuses.ContainsKey('s') ? 7 : 0));
                if (nextPlayerHp <= 0) continue;
                queue.Enqueue(((nextPlayerHp, nextPlayerMana, nextBossHp, bossDamage, nextStatuses), nextUsed, nextPath), nextUsed);
            }
        }
        
        return (min, minPath);
    }

    private (int, int) GetBoss(string[] input)
    {
        var ints = Regex.Matches(string.Join(" ", input), @"([\d]+)").Select(m => int.Parse(m.Value)).ToArray();
        return (ints[0], ints[1]);
    }
}