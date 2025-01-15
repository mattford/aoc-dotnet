using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2016.Day21;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return Scramble("abcdefgh", input);
    }

    public string Part2(string[] input)
    {
        return Scramble("fbgdceah", input.Reverse().ToArray(), true);
    }

    private string Scramble(string start, string[] instructions, bool reverse = false)
    {
        foreach (var instruction in instructions)
        {
            var next = start.ToCharArray();
            if (instruction.StartsWith("swap"))
            {
                int[] positions;
                if (instruction.StartsWith("swap position"))
                {
                    positions = instruction.Split(' ').Where(x => int.TryParse(x, out _)).Select(int.Parse)
                        .ToArray();
                }
                else
                {
                    positions = Regex.Matches(instruction, "letter ([a-z])")
                        .Select(m => start.IndexOf(m.Groups[1].Value[0])).ToArray();
                }
                next[positions[0]] = start[positions[1]];
                next[positions[1]] = start[positions[0]];
            }

            if (instruction.StartsWith("rotate"))
            {
                bool right;
                int steps;
                if (instruction.StartsWith("rotate based on position"))
                {
                    var match = Regex.Match(instruction, @"letter ([a-z])");
                    var position = start.IndexOf(match.Groups[1].Value[0]);
                    if (reverse)
                    {
                        right = false;
                        // worked out on paper
                        steps = position switch
                        {
                            0 => 1,
                            1 => 1,
                            2 => 6,
                            3 => 2,
                            4 => 7,
                            5 => 3,
                            6 => 0,
                            7 => 4,
                            _ => throw new Exception("Weird position!")
                        };
                    } else {
                        right = true;
                        steps = position + 1;
                        if (position >= 4) steps++;
                        steps %= start.Length;  
                    }
                }
                else
                {
                    var match = Regex.Match(instruction, @"rotate (left|right) (\d+) steps?");
                    right = match.Groups[1].Value == "right";
                    if (reverse) right = !right;
                    steps = int.Parse(match.Groups[2].Value) % start.Length;
                }
                if (right) next = (start[^steps..] + start[..^steps]).ToCharArray();
                if (!right) next = (start[steps..] + start[..steps]).ToCharArray();
            }
            
            if (instruction.StartsWith("reverse"))
            {
                var positions = instruction.Split(' ').Where(x => int.TryParse(x, out _)).Select(int.Parse)
                    .ToArray();
                
                next = (
                    start[..positions[0]] + 
                    string.Join("", start[positions[0]..(positions[1]+1)].Reverse()) +
                    (positions[1] < start.Length ? start[(positions[1] + 1)..] : "")
                ).ToCharArray();
            }

            if (instruction.StartsWith("move"))
            {
                var positions = instruction.Split(' ').Where(x => int.TryParse(x, out _)).Select(int.Parse)
                    .ToArray();
                if (reverse)
                {
                    positions = positions.Reverse().ToArray();
                }
                var nextTemp = next.Except([start[positions[0]]]).ToList();
                nextTemp.Insert(positions[1], start[positions[0]]);
                next = nextTemp.ToArray();
            }
            start = new string(next);
        }

        return start;
    }
}