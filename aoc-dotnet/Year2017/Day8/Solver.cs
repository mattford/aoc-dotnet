using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2017.Day8;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + Run(ParseInput(input)).Item1;
    }

    public string Part2(string[] input)
    {
        return "" + Run(ParseInput(input)).Item2;
    }

    private (int, int) Run((string, string, int, string, string, int)[] instructions)
    {
        var registers = new Dictionary<string, int>();
        var max = 0;
        foreach (var instruction in instructions)
        {
            var (register, op, operand, conditionReg, conditionOp, conditionVal) = instruction;
            var pass = conditionOp switch
            {
                ">=" => registers.GetValueOrDefault(conditionReg, 0) >= conditionVal,
                "<=" => registers.GetValueOrDefault(conditionReg, 0) <= conditionVal,
                ">" => registers.GetValueOrDefault(conditionReg, 0) > conditionVal,
                "<" => registers.GetValueOrDefault(conditionReg, 0) < conditionVal,
                "==" => registers.GetValueOrDefault(conditionReg, 0) == conditionVal,
                "!=" => registers.GetValueOrDefault(conditionReg, 0) != conditionVal,
                _ => throw new Exception("Bad operator " + conditionOp)
            };
            if (!pass) continue;
            registers[register] = op switch
            {
                "inc" => registers.GetValueOrDefault(register, 0) + operand,
                "dec" => registers.GetValueOrDefault(register, 0) - operand,
                _ => throw new Exception("Bad operator " + op)
            };
            max = Math.Max(max, registers.Values.Max());
        }
        
        return (registers.Values.Max(), max);
    }

    private (string, string, int, string, string, int)[] ParseInput(string[] input)
    {
        return (
            from line in input
            let match = Regex.Match(line, @"([a-z]+) (inc|dec) ([-\d]+) if ([a-z]+) ([!=<>]+) ([-\d]+)")
            select (match.Groups[1].Value, match.Groups[2].Value, int.Parse(match.Groups[3].Value), match.Groups[4].Value, match.Groups[5].Value, int.Parse(match.Groups[6].Value))
        ).ToArray();
    }
}