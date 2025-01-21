using System.Collections.Immutable;

namespace aoc_dotnet.Year2017.Day18;

using State = (int, long[], long, bool, int);

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var initialState = (0, new long[26], 0, false, 0);
        return "" + ResumeableRun(input, initialState, [], false).Item1.Item3;
    }

    public string Part2(string[] input)
    {
        var programs = new State[]
        {
            (0, new long[26], 0, false, 0),
            (0, new long[26].ToImmutableList().SetItem('p'-'a', 1).ToArray(), 0, false, 0),
        };

        var outputs = new[]
        {
            new Queue<long>(),
            new Queue<long>(),
        };
        var current = 0;
        while (programs.Any(p => !p.Item4))
        {
            var other = (current + 1) % 2;
            (programs[current], outputs[current]) = ResumeableRun(input, programs[current], outputs[other], true);
            current = other;
            if (outputs.All(o => o.Count == 0)) break; // deadlock
        }

        return "" + programs[1].Item5;
    }

    private (State, Queue<long>) ResumeableRun(string[] instructions, State state, Queue<long> received, bool multiprocessingEnabled)
    {
        var outputs = new Queue<long>();
        var (ip, registers, lastPlayed, halted, totalOutputs) = state;
        if (halted) return (state, outputs);
        while (ip >= 0 && ip < instructions.Length)
        {
            var instruction = instructions[ip];
            var parts = instruction.Split(' ');
            var y = 0L;
            if (!long.TryParse(parts[1], out var x)) x = registers[parts[1][0] - 'a'];
            if (parts.Length > 2)
            {
                if (!long.TryParse(parts[2], out y)) y = registers[parts[2][0] - 'a'];
            }
            switch (parts[0])
            {
                case "snd":
                    lastPlayed = x;
                    outputs.Enqueue(x);
                    totalOutputs++;
                    break;
                case "set":
                    registers[parts[1][0] - 'a'] = y;
                    break;
                case "add":
                    registers[parts[1][0] - 'a'] += y;
                    break;
                case "mul":
                    registers[parts[1][0] - 'a'] *= y;
                    break;
                case "mod":
                    registers[parts[1][0] - 'a'] %= y;
                    break;
                case "rcv":
                    if (!multiprocessingEnabled && x == 0) break;
                    if (received.Count == 0) return ((ip, registers, lastPlayed, false, totalOutputs), outputs);
                    registers[parts[1][0] - 'a'] = received.Dequeue();
                    break;
                case "jgz":
                    if (x > 0)
                    {
                        ip += (int)y;
                        continue;
                    }

                    break;
            }

            ip++;
        }

        return ((ip, registers, lastPlayed, true, totalOutputs), outputs);
    }
}