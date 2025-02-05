namespace aoc_dotnet.Year2018.Day2;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var parts = input.Aggregate((0, 0), (a, c) =>
        {
            var groups = c.ToCharArray().GroupBy(x => x).ToList();
            var g2 = Math.Min(1, groups.Count(g => g.Count() == 2));
            var g3 = Math.Min(1, groups.Count(g => g.Count() == 3));
            return (a.Item1 + g2, a.Item2 + g3);
        });
        return "" + parts.Item1 * parts.Item2;
    }

    public string Part2(string[] input)
    {
        for (var x = 0; x < input[0].Length; x++)
        {
            var thisInputs = input.Select(l => l.Substring(0, x) + l.Substring(x + 1)).GroupBy(x => x).ToList();
            var answer = thisInputs.FirstOrDefault(x => x.Count() > 1)?.Key;
            if (answer != null) return answer;
        }

        return "Not found!";
    }
}