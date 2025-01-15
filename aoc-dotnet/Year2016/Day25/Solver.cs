namespace aoc_dotnet.Year2016.Day25;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var i = 0;
        do
        {
            i++;
        } while (!Output(input.ToArray(), i));
        return "" + i;
    }

    public string Part2(string[] input)
    {
        return "Merry Christmas!";
    }
    
    private bool Output(string[] input, int a)
    {
        var last = -1;
        var seen = new HashSet<string>();
        var ip = 0;
        var registers = new Dictionary<char, int>
        {
            ['a'] = a,
            ['b'] = 0,
            ['c'] = 0,
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
                    if (!registers.ContainsKey(parts[2][0])) break;
                    registers[parts[2][0]] = x;
                    break;
                case "inc":
                    if (!registers.ContainsKey(parts[1][0])) break;
                    registers[parts[1][0]]++;
                    break;
                case "dec":
                    if (!registers.ContainsKey(parts[1][0])) break;
                    registers[parts[1][0]]--;
                    break;
                case "jnz":
                    if (x != 0)
                    {
                        if (!int.TryParse(parts[2], out var y))
                        {
                            y = registers[parts[2][0]];
                        }

                        ip += y;
                        continue;
                    }
                    break;
                case "out":
                    if (registers[parts[1][0]] == last) return false;
                    if (!seen.Add(string.Join("/", registers.Values))) return true;
                    last = registers[parts[1][0]];
                    break;
            }
            ip++;
            
        }
        return true;
    }
}