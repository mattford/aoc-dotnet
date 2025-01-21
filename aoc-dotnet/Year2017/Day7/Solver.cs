using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2017.Day7;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var held = new HashSet<string>();
        var holding = new HashSet<string>();
        foreach (var line in input)
        {
            if (!line.Contains("->")) continue;
            foreach (var heldProgram in line.Split(" -> ")[1].Split(", "))
            {
                held.Add(heldProgram);
            }

            holding.Add(line.Split(' ')[0]);
        }
        return holding.Single(x => !held.Contains(x));
    }

    public string Part2(string[] input)
    {
        var programs = (
            from line in input
            let match = Regex.Match(line, @"(\w+) \((\d+)\)(?: -> ([\w, ]+))?")
            select new KeyValuePair<string, (int, string[])>(match.Groups[1].Value, (int.Parse(match.Groups[2].Value), match.Groups[3].Value.Split(", ").Where(s => s != string.Empty).ToArray()))        
        ).ToImmutableDictionary();
        var bottom = Part1(input);
        return "" + FindCorrectWeight(programs, bottom);
    }

    private int FindCorrectWeight(ImmutableDictionary<string, (int, string[])> programs, string holder, int target = 0)
    {
        if (IsBalanced(programs, holder))
        {
            if (target > 0 && target != FindWeight(programs, holder))
            {
                // it must be the current program which is the wrong weight.
                return target - (FindWeight(programs, holder) - programs[holder].Item1);
            }

            return 0;
        }
        
        // one of our piles in the wrong weight
        if (programs[holder].Item2.Length == 2)
        {
            var results = new int[]
            {
                FindCorrectWeight(programs, programs[holder].Item2[0], programs[programs[holder].Item2[1]].Item1),
                FindCorrectWeight(programs, programs[holder].Item2[1], programs[programs[holder].Item2[0]].Item1)
            };
            if (results.Any(x => x > 0))
            {
                return results.Max();
            }
        }
        // we have >2 programs in our pile, so find the correct value
        var grouped = programs[holder].Item2.GroupBy(x => FindWeight(programs, x)).Single(x => x.Count() > 1).Key;
        foreach (var p in programs[holder].Item2)
        {
            var a = FindCorrectWeight(programs, p, grouped);
            if (a > 0) return a;
        }

        return 0;
    }

    private int FindWeight(ImmutableDictionary<string, (int, string[])> programs, string holder)
    {
        return programs[holder].Item1 + programs[holder].Item2.Sum(h => FindWeight(programs, h));
    }

    private bool IsBalanced(ImmutableDictionary<string, (int, string[])> programs, string holder)
    {
        return programs[holder].Item2.Length == 0 || programs[holder].Item2.Select(h => FindWeight(programs, h)).Distinct().Count() == 1;
    }
}