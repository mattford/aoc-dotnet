using System.Collections.Immutable;
using System.Numerics;

namespace aoc_dotnet.Year2025.Day4;

public class Solver : SolverInterface
{
    List<Complex> Neighbours = new()
    {
        Complex.ImaginaryOne, // down
        -Complex.ImaginaryOne, // up
        1, // right
        -1, // left
        Complex.ImaginaryOne + 1, // se
        Complex.ImaginaryOne - 1, // sw
        -Complex.ImaginaryOne + 1, // ne
        -Complex.ImaginaryOne - 1, // nw
    };
    
    public string Part1(string[] input)
    {
        var grid = (
            from y in Enumerable.Range(0, input.Length)
            from x in Enumerable.Range(0, input[0].Length)
            where input[y][x] == '@'
            select Complex.ImaginaryOne * y + x
        ).ToImmutableDictionary(v => v, _ => 1);
        
        var sum = grid.Keys.Count(k => Neighbours.Count(n => grid.ContainsKey(k + n)) < 4);
        return "" + sum;
    }

    public string Part2(string[] input)
    {
        var grid = (
            from y in Enumerable.Range(0, input.Length)
            from x in Enumerable.Range(0, input[0].Length)
            where input[y][x] == '@'
            select Complex.ImaginaryOne * y + x
        ).ToImmutableDictionary(v => v, _ => 1);

        return "" + RemoveMaximumRolls(grid); 
    }

    private int RemoveMaximumRolls(ImmutableDictionary<Complex, int> grid)
    {
        var removed = grid.Keys.Count(k => Neighbours.Count(n => grid.ContainsKey(k + n)) < 4);
        if (removed == 0) return removed;
        return removed + RemoveMaximumRolls(grid.Where(k => Neighbours.Count(n => grid.ContainsKey(k.Key + n)) >= 4)
            .ToImmutableDictionary());
    }
}