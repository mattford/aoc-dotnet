namespace aoc_dotnet.Year2015.Day23;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + Compute(input, new int[2]);
    }

    public string Part2(string[] input)
    {
        return "" + Compute(input, [1, 0]);
    }

    private int Compute(string[] input, int[] registers)
    {
        var instructions = input.Select(x => x.Replace(",", "").Replace("+", "").Split(" ")).ToArray();
        var ip = 0;
        while (ip >= 0 && ip < instructions.Length)
        {
            var instruction = instructions[ip];
            var opcode = instruction[0];
            var operand = instruction[1];
            switch (opcode)
            {
                case "hlf":
                    registers[operand[0] - 'a'] /= 2;
                    break;
                case "tpl":
                    registers[operand[0] - 'a'] *= 3;
                    break;
                case "inc":
                    registers[operand[0] - 'a']++;
                    break;
                case "jmp":
                    ip += int.Parse(operand);
                    continue;
                case "jie":
                    if (registers[operand[0] - 'a'] % 2 == 0)
                    {
                        ip += int.Parse(instruction[2]);
                        continue;
                    }
                    break;
                case "jio":
                    if (registers[operand[0] - 'a'] == 1)
                    {
                        ip += int.Parse(instruction[2]);
                        continue;
                    }
                    break;
            }

            ip++;
        }
        return registers[1];
    }
}