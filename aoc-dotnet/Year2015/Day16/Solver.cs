namespace aoc_dotnet.Year2015.Day16;

public class Solver: SolverInterface
{
    private string sueInfo =
        "children: 3\ncats: 7\nsamoyeds: 2\npomeranians: 3\nakitas: 0\nvizslas: 0\ngoldfish: 5\ntrees: 3\ncars: 2\nperfumes: 1";
    
    public string Part1(string[] input)
    {
        var targetSue = ParseLine(sueInfo.Replace("\n", ", "));
        var possibleSues = GetPossibleSues(input);
        var idx = 0;
        foreach (var possibleSue in possibleSues)
        {
            idx++;
            if (targetSue.Any(x => possibleSue.TryGetValue(x.Key, out var v) && v != x.Value)) continue;
            return ""+ idx;
        }

        return "Sue not found!";
    }

    public string Part2(string[] input)
    {
        var targetSue = ParseLine(sueInfo.Replace("\n", ", "));
        var possibleSues = GetPossibleSues(input);
        var idx = 0;
        foreach (var possibleSue in possibleSues)
        {
            idx++;
            if (targetSue.Any(x => possibleSue.TryGetValue(x.Key, out var v) && x.Key switch {
                "cats" or "trees" => v <= x.Value,
                "pomeranians" or "goldfish" => v >= x.Value,
                _ => v != x.Value
            })) continue;
            return ""+ idx;
        }

        return "Sue not found!";
    }

    private static Dictionary<string, int>[] GetPossibleSues(string[] input)
    {
        return input.Select(x => x[(x.IndexOf(':') + 2)..]).Select(ParseLine).ToArray();
    }

    private static Dictionary<string, int> ParseLine(string line)
    {
        return line.Split(", ").Select(x => x.Split(": ")).Select(x => new KeyValuePair<string, int>(x[0], int.Parse(x[1])))
            .ToDictionary();
    }
}