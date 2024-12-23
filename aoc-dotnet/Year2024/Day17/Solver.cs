using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2024.Day17;

public class Solver : SolverInterface
{
    public string Part1(string[] input)
    {
        var (registers, program) = ParseInput(input);
        return Compute(registers['A'], program);
    }

    public string Part2(string[] input)
    {
        var (_, program) = ParseInput(input);
        var output = FindNext(0, program, 0);

        if (output == null)
        {
            throw new Exception("It's all gone wrong!");
        }
        return ""+output;
    }

    private long? FindNext(long output, int[] program, int counter)
    {
        counter++;
        var aim = string.Join(",", program.TakeLast(counter));
        for (var j = 0; j <= 7; j++)
        {
            var a = output + j;
            if (Compute(a, program) != aim) continue;
            if (counter == program.Length)
            {
                return a;
            }
            var thisPathResult = FindNext(a << 3, program, counter);
            if (thisPathResult == null) continue;
            return thisPathResult;
        }
        return null;
    }

    private string Compute(long aValue, int[] program)
    {
        long ip = 0;
        var outputs = new List<long>();
        var registers = new Dictionary<char, long>
        {
            ['A'] = aValue,
            ['B'] = 0,
            ['C'] = 0
        };

        while (ip < program.Length)
        {
            var instruction = program[ip];
            long operand = program[ip + 1];
            if (new[] { 0, 2, 5, 6, 7 }.Any(a => a == instruction))
            {
                operand = operand switch
                {
                    4 => registers['A'],
                    5 => registers['B'],
                    6 => registers['C'],
                    _ => operand
                };
            }

            switch (instruction)
            {
                case 0:
                    // The adv instruction (opcode 0) performs division. The numerator is the value in the A register. The denominator is found by raising 2 to the power of the instruction's combo operand. (So, an operand of 2 would divide A by 4 (2^2); an operand of 5 would divide A by 2^B.) The result of the division operation is truncated to an integer and then written to the A register.
                    registers['A'] = (long)(registers['A'] / Math.Pow(2, operand));
                    break;
                case 1:
                    // The bxl instruction (opcode 1) calculates the bitwise XOR of register B and the instruction's literal operand, then stores the result in register B
                    registers['B'] ^= operand;
                    break;
                case 2:
                    // The bst instruction (opcode 2) calculates the value of its combo operand modulo 8 (thereby keeping only its lowest 3 bits), then writes that value to the B register.
                    registers['B'] = operand % 8;
                    break;
                case 3:
                    // The jnz instruction (opcode 3) does nothing if the A register is 0. However, if the A register is not zero, it jumps by setting the instruction pointer to the value of its literal operand; if this instruction jumps, the instruction pointer is not increased by 2 after this instruction.
                    if (registers['A'] != 0)
                    {
                        ip = operand;
                        continue;
                    }

                    break;
                case 4:
                    // The bxc instruction (opcode 4) calculates the bitwise XOR of register B and register C, then stores the result in register B. (For legacy reasons, this instruction reads an operand but ignores it.)
                    registers['B'] ^= registers['C'];
                    break;
                case 5:
                    // The out instruction (opcode 5) calculates the value of its combo operand modulo 8, then outputs that value. (If a program outputs multiple values, they are separated by commas.)
                    outputs.Add(operand % 8);
                    break;
                case 6:
                    // The bdv instruction (opcode 6) works exactly like the adv instruction except that the result is stored in the B register. (The numerator is still read from the A register.)
                    registers['B'] = (long)(registers['A'] / Math.Pow(2, operand));
                    break;
                case 7:
                    // The cdv instruction (opcode 7) works exactly like the adv instruction except that the result is stored in the C register. (The numerator is still read from the A register.)
                    registers['C'] = (long)(registers['A'] / Math.Pow(2, operand));
                    break;
            }
            ip += 2;
        }

        return string.Join(",", outputs);
    }

    private (Dictionary<char, int>, int[]) ParseInput(string[] input)
    {
        var matches = Regex.Matches(string.Join("\n", input), "([0-9-]+)").ToArray();
        // first 3 are registers, then its the company
        var registers = new Dictionary<char, int>();
        registers.Add('A', int.Parse(matches[0].Value));
        registers.Add('B', int.Parse(matches[1].Value));
        registers.Add('C', int.Parse(matches[2].Value));

        return (registers, matches.Skip(3).Select(match => int.Parse(match.Value)).ToArray());
    }
}