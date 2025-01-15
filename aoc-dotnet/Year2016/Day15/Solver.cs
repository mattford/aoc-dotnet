using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2016.Day15;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var discs = GetDiscs(input);
        var i = 1;
        while (true)
        {
            if (discs.All(disc => i % disc[0] == disc[1])) return ""+i;
            i++;
        }
    }

    public string Part2(string[] input)
    {
        var discs = GetDiscs(input);
        discs.Add([11, 11 - (discs.Count + 1)]);
        var i = 1;
        while (true)
        {
            if (discs.All(disc => i % disc[0] == disc[1])) return ""+i;
            i++;
        }
    }

    private List<int[]> GetDiscs(string[] input)
    {
        return input.Select(line =>
            Regex.Match(line, "Disc #([0-9]+) has ([0-9]+) positions; at time=0, it is at position ([0-9]+)").Groups.Values.Skip(1)
                .Select(g => int.Parse(g.Value)).ToArray()).Select(disc =>
        {
            var rem = disc[1] - disc[2] - disc[0];
            if (rem < 0) rem = disc[1] + rem % disc[1];
            return new[] { disc[1], rem };
        }).ToList();
    }
}