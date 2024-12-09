using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2024.Day8;

public class Solver : SolverInterface
{
    public string Part1(string[] input)
    {
        var points = ParseInput(input);
        var antipodes = new HashSet<(int, int)>();
        foreach (var p in points)
        {
            foreach (var p1 in p.Value)
            {
                foreach (var p2 in p.Value)
                {
                    if (p1 == p2) continue;
                    var distY = p1.Item1 - p2.Item1;
                    var distX = p1.Item2 - p2.Item2;
                    
                    var antipode = (p1.Item1 + distY, p1.Item2 + distX);
                    if (antipode != p1 && antipode != p2)
                    {
                        antipodes.Add(antipode);
                    } 
                }
            }
        }
        return "" + antipodes.Count(x => x.Item1 >= 0 && x.Item1 < input.Length && x.Item2 >= 0 && x.Item2 < input[x.Item1].Length);
    }

    public string Part2(string[] input)
    {
        var points = ParseInput(input);
        var antipodes = new HashSet<(int, int)>();
        foreach (var p in points)
        {
            foreach (var p1 in p.Value)
            {
                foreach (var p2 in p.Value)
                {
                    if (p1 == p2) continue;
                    var distY = p1.Item1 - p2.Item1;
                    var distX = p1.Item2 - p2.Item2;
                    var antipode = p1;
                    while (antipode.Item1 >= 0 && antipode.Item1 < input.Length && antipode.Item2 >= 0 && antipode.Item2 < input[antipode.Item1].Length)
                    {
                        antipodes.Add(antipode);

                        antipode.Item1 += distY;
                        antipode.Item2 += distX;
                    }
                    
                }
            }
        }
            
        return "" + antipodes.Count(x => x.Item1 >= 0 && x.Item1 < input.Length && x.Item2 >= 0 && x.Item2 < input[x.Item1].Length);
    }
    
    private ImmutableDictionary<char, (int, int)[]> ParseInput(string[] input)
    {
        return (
                from y in Enumerable.Range(0, input.Length)
                from x in Enumerable.Range(0, input[y].Length)
                where Regex.IsMatch("" + input[y][x], "[a-zA-Z0-9]")
                select (input[y][x], (y, x))
            ).GroupBy(x => x.Item1)
            .ToImmutableDictionary(x => x.Key, x => x.Select(y => y.Item2).ToArray());
    }
}