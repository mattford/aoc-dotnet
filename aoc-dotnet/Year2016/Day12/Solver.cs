namespace aoc_dotnet.Year2016.Day12;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + Run(input, 0);
    }

    public string Part2(string[] input)
    {
        return "" + Run(input, 1);
    }

    private int Run(string[] input, int c)
    {
        var ip = 0;
        var registers = new Dictionary<char, int>
        {
            ['a'] = 0,
            ['b'] = 0,
            ['c'] = c,
            ['d'] = 0,
        };

        while (ip >= 0 && ip < input.Length)
        {
            var parts = input[ip].Split(' ');
            var instruction = parts[0];
            if (!int.TryParse(parts[1], out var x))
            {
                x = registers[parts[1][0]];
            }

            switch (instruction)
            {
                case "cpy":
                    registers[parts[2][0]] = x;
                    break;
                case "inc":
                    registers[parts[1][0]]++;
                    break;
                case "dec":
                    registers[parts[1][0]]--;
                    break;
                case "jnz":
                    if (x != 0)
                    {
                        ip += int.Parse(parts[2]);
                        continue;
                    }
                    break;
            }
            ip++;
            
        }

        return registers['a'];
    }
}