using System.Collections.Immutable;
using System.Numerics;

namespace aoc_dotnet.Year2018.Day15;

using Map = Dictionary<Complex, char>;

internal record struct Unit(char Team, int Health, Complex Position);


public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        throw new NotImplementedException();
    }

    public string Part2(string[] input)
    {
        throw new NotImplementedException();
    }

    private (char, int) Play(Map map, List<Unit> units)
    {
        var rounds = 0;
        while (units.GroupBy(u => u.Team).Count() > 1)
        {
            units = units.OrderBy(u => u.Position.Imaginary).ThenBy(u => u.Position.Real).ToList();
            foreach (var unit in units)
            {
                var attackTarget = GetAttackTarget(units, unit);
                if (attackTarget == null)
                {
                    // do move
                    var moveTarget = GetMoveTarget(map, units, unit);
                    unit.Position = moveTarget;
                }
                attackTarget = GetAttackTarget(units, unit);
                
                // if not in range of any:
                // Find shortest paths to targets, break ties with reading order
                // Move 1 step towards target (if multiple shortest paths, use reading order)
                // if in range of any
                // select target using lowest hit points then reading order
                // do <attack power> damage to target
            }
            rounds++;
        }

        return (units.First().Team, rounds * units.Sum(u => u.Health));
    }

    private Complex? GetAttackTarget(List<Unit> units, Unit me)
    {
        var neighbours = new[] { -Complex.ImaginaryOne, -1, 1, Complex.ImaginaryOne };
        foreach (var n in neighbours)
        {
            var thisUnit = units.First(u => u.Position == me.Position + n);
            if (thisUnit.Team != me.Team)
            {
                return thisUnit.Position;
            }
        }

        return null;
    }

    private Complex GetMoveTarget(Map map, List<Unit> units, Unit me)
    {
        var neighbours = new[] { -Complex.ImaginaryOne, -1, 1, Complex.ImaginaryOne };
        var targets = new List<(Complex, ImmutableList<Complex>)>();
        var closest = int.MaxValue;
        var queue = new PriorityQueue<(Complex, ImmutableList<Complex>, int), int>();
        queue.Enqueue((me.Position, [], 0), 0);
        while (queue.Count > 0)
        {
            var (pos, route, cost) = queue.Dequeue();
            foreach (var v in neighbours)
            {
                if (map.GetValueOrDefault(pos + v, '#') == '#') continue;
                if (units.Any(u => u.Position == pos + v))
                {
                    var thisUnit = units.First(u => u.Position == pos + v);
                    if (thisUnit.Team != me.Team)
                    {
                        // need to break ties betweeen targets
                        targets.Add((pos + v, route.Add(pos + v)));
                        closest = Math.Min(closest, cost + 1);
                    }

                    continue;
                }

                if (cost + 1 > closest) continue;
                queue.Enqueue((pos + v, route.Add(pos + v), cost + 1), cost + 1);
            }
        }

        if (targets.Count == 0) return [];
        var minDist = targets.Min(v => v.Item2.Count);
        return targets
            .Where(v => v.Item2.Count == minDist)
            .Select(t => t.Item2[0])
            .OrderBy(t => t.Imaginary)
            .ThenBy(t => t.Real)
            .First();
    }
}