namespace aoc_dotnet.Year2024.Day24;

using Values = Dictionary<string, bool>;
using Instruction = (string[], string, string);
using Instructions = List<(string[], string, string)>;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var (values, instructions) = ParseInput(input);
        return "" + GetValue("z", Operate(values, instructions));
    }

    public string Part2(string[] input)
    {
        var (_, instructions) = ParseInput(input);

        return string.Join(",", DoFix(instructions).Order());
    }

    // Used https://aoc.csokavar.hu/2024/24/ to understand the logic required here,
    // then wrote a version which works with my data layout.
    private List<string> DoFix(Instructions instructions)
    {
        var previousCarry = FindOutput(instructions, "x00", "y00", "AND");
        for (var i = 1; i < 45; i++)
        {
            var x = $"x{i:D2}";
            var y = $"y{i:D2}";
            var z = $"z{i:D2}";

            var xor1 = FindOutput(instructions, x, y, "XOR");
            var and1 = FindOutput(instructions, x, y, "AND");
            var xor2 = FindOutput(instructions, previousCarry, xor1, "XOR");
            var and2 = FindOutput(instructions, previousCarry, xor1, "AND");

            if (xor2 == null && and2 == null) {
                return DoFix(Swap(instructions, xor1, and1)).Concat([xor1, and1]).ToList();
            }

            var carry = FindOutput(instructions, and1, and2, "OR");
            if (xor2 != z)
            {
                return DoFix(Swap(instructions, z, xor2)).Concat([z, xor2]).ToList();
            }

            previousCarry = carry;
        }

        return [];
    }

    private string FindOutput(Instructions instructions, string a, string b, string op)
    {
        return instructions.FirstOrDefault(x => x.Item2 == op && x.Item1.Contains(a) && x.Item1.Contains(b)).Item3;
    }

    private static long GetValue(string key, Values values)
    {
        var bString = string.Join("", values
            .Where(kv => kv.Key.StartsWith(key))
            .OrderByDescending(kv => int.Parse(kv.Key[1..]))
            .Select(kv => kv.Value ? 1 : 0)
        );
        return Convert.ToInt64(bString, 2);
    }

    private static Values Operate(Values values, Instructions instructions)
    {
        while (instructions.Any(x => Doable(values, x)))
        {
            var idx = instructions.FindIndex(x => Doable(values, x));
            var (operands, op, output) = instructions[idx];
            values[output] = op switch
            {
                "AND" => operands.All(x => values[x]),
                "OR" => operands.Any(x => values[x]),
                "XOR" => operands.Select(x => values[x]).Distinct().ToArray().Length > 1,
                _ => throw new Exception("Unsupported operation: " + op)
            };
            instructions.RemoveAt(idx);
        }

        return values;
    }

    private Instructions Swap(Instructions instructions, string from, string to)
    {
        var thisSet = instructions.ToList();
        var fromIdx = instructions.FindIndex(x => x.Item3 == from);
        var toIdx = instructions.FindIndex(x => x.Item3 == to);
        var currentFrom = instructions[fromIdx];
        var currentFromDest = currentFrom.Item3;
        var currentTo = instructions[toIdx];
        thisSet[fromIdx] = (currentFrom.Item1, currentFrom.Item2, currentTo.Item3);
        thisSet[toIdx] = (currentTo.Item1, currentTo.Item2, currentFromDest);
        return thisSet;
    }

    private static bool Doable(Values values, Instruction instructions)
    {
        return instructions.Item1.All(values.ContainsKey);
    }

    private static (Values, Instructions) ParseInput(string[] input)
    {
        var values = new Values();
        var parts = string.Join('\n', input).Split("\n\n");
        foreach (var line in parts[0].Split("\n"))
        {
            var split = line.Split(": ");
            values[split.First()] = split[1] == "1";
        }

        var instructions = new Instructions();
        foreach (var line in parts[1].Split("\n"))
        {
            var split = line.Replace(" -> ", " ").Split(" ");
            instructions.Add(([split[0], split[2]], split[1], split[3]));
        }
        return (values, instructions);
    }
}