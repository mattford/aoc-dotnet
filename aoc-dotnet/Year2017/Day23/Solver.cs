namespace aoc_dotnet.Year2017.Day23;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + Run(input, 0).Item1;
    }

    public string Part2(string[] input)
    {
        // Analysing the source code reveals it is counting prime numbers
        // between a lower and upper bound, I suspect the bounds are different
        // between inputs, so extracting them should generalise this solution.
        var registers = Run(input[..8], 1).Item2;
        var u = registers[2];
        var l = registers[1];
        var h = 0;
        for (var i = l; i <= u; i += 17)
        {
            for (var a = 2; a <= i / 2; a++)
            {
                if (i % a != 0) continue;
                h++;
                break;
            }
        }
        return "" + h;
    }

    private (int, long[]) Run(string[] instructions, int a)
    {
        var registers = new long[26];
        registers[0] = a;
        var ip = 0;
        var muls = 0;
        while (ip >= 0 && ip < instructions.Length)
        {
            var instruction = instructions[ip].Split(' ');
            if (!long.TryParse(instruction[1], out var x)) x = registers[instruction[1][0] - 'a'];
            if (!long.TryParse(instruction[2], out var y)) y = registers[instruction[2][0] - 'a'];
            switch (instruction[0])
            {
                case "set":
                    registers[instruction[1][0] - 'a'] = y;
                    ip++;
                    break;
                case "sub":
                    registers[instruction[1][0] - 'a'] -= y;
                    ip++;
                    break;
                case "mul":
                    registers[instruction[1][0] - 'a'] *= y;
                    ip++;
                    muls++;
                    break;
                case "jnz":
                    if (x != 0) ip += (int)y;
                    if (x == 0) ip++;
                    break;
                
            }
        }

        return (muls, registers);
    }
}