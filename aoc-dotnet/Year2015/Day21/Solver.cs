using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2015.Day21;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var boss = GetBoss(input);
        var minWin = (from loadout in GetLoadouts() where IsWin(loadout.Item2, boss) select loadout.Item1).Min();

        return "" + minWin;
    }

    public string Part2(string[] input)
    {
        var boss = GetBoss(input);
        var maxLoss = (from loadout in GetLoadouts() where !IsWin(loadout.Item2, boss) select loadout.Item1).Max();

        return "" + maxLoss;
    }

    private static bool IsWin((int, int, int) player, (int, int, int) boss)
    {
        var (php, pdm, par) = player;
        var (bhp, bdm, bar) = boss;
        var turnsBoss = Math.Ceiling((double)php / Math.Max(0, bdm - par));
        var turnsPlayer = Math.Ceiling((double)bhp / Math.Max(0, pdm - bar));
        return turnsPlayer <= turnsBoss;
    }

    private static (int, (int, int, int))[] GetLoadouts()
    {
        var weapons = new int[][]
        {
            [8, 4, 0],
            [10, 5, 0],
            [25, 6, 0],
            [40, 7, 0],
            [74, 8, 0]
        };

        var armour = new int[][]
        {
            [0, 0, 0],
            [13, 0, 1],
            [31, 0, 2],
            [53, 0, 3],
            [75, 0, 4],
            [102, 0, 5],
        };

        var rings = new int[][]
        {
            [25, 1, 0],
            [50, 2, 0],
            [100, 3, 0],
            [20, 0, 1],
            [40, 0, 2],
            [80, 0, 3]
        };

        var loadouts = new List<(int, (int, int, int))>();
        foreach (var weapon in weapons)
        {
            var thisCost = weapon[0];
            var thisLoadout = (100, weapon[1], weapon[2]);
            foreach (var piece in armour) // armour includes a dummy entry for no armour
            {
                var thisthisCost = thisCost + piece[0];
                var thisthisLoadout = (100, thisLoadout.Item2 + piece[1], thisLoadout.Item3 + piece[2]);
                loadouts.Add((thisthisCost, thisthisLoadout)); // 1 weapon, 0-1 armour, no rings
                // now we can have between 0-2 rings
                foreach (var ring in rings)
                {
                    var thisthisthisCost = thisthisCost + ring[0]; // variable names are getting fun
                    var thisthisthisLoadout = (100, thisthisLoadout.Item2 + ring[1], thisthisLoadout.Item3 + ring[2]);
                    loadouts.Add((thisthisthisCost, thisthisthisLoadout));
                    foreach (var ring2 in rings)
                    {
                        if (ring == ring2) continue;
                        var theLastCost = thisthisthisCost + ring2[0];
                        var theLastLoadout = (100, thisthisthisLoadout.Item2 + ring2[1], thisthisthisLoadout.Item3 + ring2[2]);
                        loadouts.Add((theLastCost, theLastLoadout));
                    }
                }
            }
        }

        return loadouts.ToArray();
    }

    private (int, int, int) GetBoss(string[] input)
    {
        var ints = Regex.Matches(string.Join(" ", input), @"([\d]+)").Select(m => int.Parse(m.Value)).ToArray();
        return (ints[0], ints[1], ints[2]);
    }
}