using System.Text.RegularExpressions;
using static System.Int32;

namespace aoc_dotnet.Year2015.Day7;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var signals = new Dictionary<string, int>();
        return Solve(input, signals);
    }

    public string Part2(string[] input)
    {
        var signals = new Dictionary<string, int>
        {
            ["b"] = Parse(Part1(input))
        };
        return Solve(input, signals);
    }

    private string Solve(string[] input, Dictionary<string, int> signals)
    {
        var i = 0;
        while (!signals.ContainsKey("a"))
        {
            var line = input[i];
            var splitted = line.Split(" -> ");
            var inValues = splitted[0].Split(" ");
            var target = splitted[1];
            if (signals.ContainsKey(target))
            {
                i = (i + 1) % input.Length;
                continue;
            }
            var requiredInputs = inValues.Length switch
            {
                1 => new[]{inValues[0]},
                2 => new[]{inValues[1]},
                3 => new[]{inValues[0], inValues[2]},
                _ => throw new Exception($"Invalid input format: {line}"),
            };
            if (requiredInputs.Any(ri => !Regex.IsMatch(ri, "[0-9]+") && !signals.ContainsKey(ri)))
            {
                i = (i + 1) % input.Length;
                continue;
            }
            signals[target] = inValues.Length switch
            {
                1 => GetValueForWire(signals, inValues[0]) & 65535,
                2 => ~ GetValueForWire(signals, inValues[1]) & 65535,
                3 when inValues[1] == "AND" => GetValueForWire(signals, inValues[0]) &
                                               GetValueForWire(signals, inValues[2]) & 65535,
                3 when inValues[1] == "OR" => (GetValueForWire(signals, inValues[0]) | GetValueForWire(signals, inValues[2])) & 65535,
                3 when inValues[1] == "LSHIFT" => (GetValueForWire(signals, inValues[0]) << GetValueForWire(signals, inValues[2])) & 65535,
                3 when inValues[1] == "RSHIFT" => (GetValueForWire(signals, inValues[0]) >> GetValueForWire(signals, inValues[2])) & 65535,
                _ => throw new Exception($"Unknown signal: {line}")
            };
            i = (i + 1) % input.Length;
        }
        return "" + signals["a"];
    }

    private int GetValueForWire(Dictionary<string, int> signals, string x)
    {
        TryParse(x, out var value);
        return signals.GetValueOrDefault(x, value) & 65535;
    }
}