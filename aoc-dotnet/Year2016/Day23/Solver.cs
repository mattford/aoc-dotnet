namespace aoc_dotnet.Year2016.Day23;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return ""+Run(input.ToArray(), 7);
    }

    public string Part2(string[] input)
    {
        return ""+Run(input.ToArray(), 12);
    }
    
    private long Run(string[] input, int a)
    {
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

                        if (y < 0)
                        {
                            var loopedInstructions = input[(ip + y)..ip];
                            if (loopedInstructions.Length == 5)
                            {
                                // optimise the multiply
                                var multiplier = loopedInstructions[0].Split(' ')[1];
                                if (!int.TryParse(multiplier, out var f))
                                {
                                    f = registers[multiplier[0]];
                                };
                                // always c and d involved in my input
                                registers['a'] += f * x;
                                registers['c'] = 0;
                                registers['d'] = 0;
                                break;
                            }
                            if (loopedInstructions.Length == 2)
                            {
                                var changes = new Dictionary<char, int>();
                                foreach (var loopedInstruction in loopedInstructions)
                                {
                                    var i = loopedInstruction.Split(' ')[0];
                                    if (i is not "inc" and not "dec") throw new Exception("Panic!");
                                    changes.Add(loopedInstruction.Split(' ')[1][0], i == "inc" ? 1 : -1);
                                }
                                foreach (var change in changes)
                                {
                                    registers[change.Key] += change.Value * x;
                                }
                        
                                break;
                            }
                        }

                        ip += y;
                        continue;
                    }
                    break;
                case "tgl":
                    var target = ip + x;
                    if (target < 0 || target >= input.Length) break;
                    var other = input[target];
                    if (other.StartsWith("inc"))
                    {
                        input[target] = input[target].Replace("inc", "dec");
                    } else if (other.StartsWith("dec") || other.StartsWith("tgl"))
                    {
                        input[target] = input[target].Replace("tgl", "inc").Replace("dec", "inc");
                    } else if (other.StartsWith("jnz"))
                    {
                        input[target] = input[target].Replace("jnz", "cpy");
                    } else if (other.StartsWith("cpy"))
                    {
                        input[target] = input[target].Replace("cpy", "jnz");
                    }
                    break;
            }
            ip++;
            
        }

        return registers['a'];
    }
}