using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2016.Day10;

internal record struct Bin(string Name, List<int> Values, string DestHigh, string DestLow, bool Resolved);

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var bins = Simulate(input);
        return bins.Single(kv => string.Join(",", kv.Value.Values.Order().ToArray()) == "17,61").Value.Name.Replace("bot ", "");
    }

    public string Part2(string[] input)
    {
        var bins = Simulate(input);
        var targets = new[] { "output 0", "output 1", "output 2" };
        return "" + targets.Aggregate(1, (a, c) => a * bins[c].Values[0]);
    }

    private static Dictionary<string, Bin> Simulate(string[] input)
    {
        var intermediate = new Dictionary<string, Bin>();
        foreach (var line in input)
        {
            var m = Regex.Match(line, @"(bot [0-9]+) gives low to (\w* \d*) and high to (\w* \d*)");
            if (m.Success)
            {
                var bin = intermediate.GetValueOrDefault(m.Groups[1].Value, new Bin(m.Groups[1].Value, [], "", "", false));
                bin.DestLow = m.Groups[2].Value;
                bin.DestHigh = m.Groups[3].Value;
                intermediate[m.Groups[1].Value] = bin;
                continue;
            }
            var m2 = Regex.Match(line, @"value (\d+) goes to (\w* \d*)");
            if (m2.Success)
            {
                var bin = intermediate.GetValueOrDefault(m2.Groups[2].Value, new Bin(m2.Groups[2].Value, [], "", "", false));
                bin.Values.Add(int.Parse(m2.Groups[1].Value));
                intermediate[m2.Groups[2].Value] = bin;
            }
        }
        while (intermediate.Any(x => x.Value is { Resolved: false, Values.Count: >= 2 } && !x.Value.Name.StartsWith("output")))
        {
            foreach (var kvp in intermediate.Where(x => x.Value is { Resolved: false, Values.Count: >= 2 } && !x.Value.Name.StartsWith("output")))
            {
                var v = kvp.Value;
                var highBin = intermediate.GetValueOrDefault(v.DestHigh, new Bin(v.DestHigh, [], "", "", false));
                highBin.Values.Add(v.Values.Max());
                intermediate[v.DestHigh] = highBin;
                var lowBin = intermediate.GetValueOrDefault(v.DestLow, new Bin(v.DestLow, [], "", "", false));
                lowBin.Values.Add(v.Values.Min());
                intermediate[v.DestLow] = lowBin;
                v.Resolved = true;
                intermediate[kvp.Key] = v;
                break;
            }
        }
       
        return intermediate;
    }
}