using System.Collections.Immutable;
using System.Numerics;

namespace aoc_dotnet.Year2017.Day19;

public class Solver: SolverInterface
{
    Complex Up = -Complex.ImaginaryOne;
    Complex Down = Complex.ImaginaryOne;
    Complex Left = -Complex.One;
    Complex Right = Complex.One;
    
    public string Part1(string[] input)
    {
        return "" + FollowPath(input).Item1;
    }

    public string Part2(string[] input)
    {
        return "" + FollowPath(input).Item2;
    }

    private (string, int) FollowPath(string[] input)
    {
        var grid = (
            from y in Enumerable.Range(0, input.Length)
            from x in Enumerable.Range(0, input[y].Length)
            where input[y][x] != ' '
            select new KeyValuePair<Complex, char>(Complex.ImaginaryOne * y + x, input[y][x])
        ).ToImmutableDictionary();
        var pos = Complex.Zero + input[0].IndexOf('|');
        
        var dir = Down;
        var steps = 0;
        var path = "";

        while (true)
        {
            steps++;
            var thisValue = grid[pos];
            if (thisValue is not '|' and not '-' and not '+') path += thisValue;
            // if the current value is a '|' or a '-' we can blindly go to the next pos
            if (thisValue is '-' or '|')
            {
                pos += dir;
                continue;
            }
            
            // we are on a + or a letter tile, so we have to feel around a bit for our next position
            var next = pos + dir;
            var nextValue = grid.GetValueOrDefault(next, '.');
            if (nextValue != '.' && (
                    thisValue is not '+' ||
                    nextValue is not '|' and not '-' || 
                    (dir.Imaginary != 0 && nextValue is '|') || 
                    (dir.Imaginary == 0 && nextValue is '-')
            )) {
                pos = next;
                continue;
            }
            // we need to change direction
            var toTry = dir.Imaginary != 0 ? new[]{Left, Right} : new[]{Up, Down};
            var found = false;
            foreach (var nextDir in toTry)
            {
                var dirValue = grid.GetValueOrDefault(pos + nextDir, '.');
                if (dirValue is not '-' and not '|' and not '.' || 
                    (nextDir.Imaginary != 0 && dirValue is '|') || 
                    (nextDir.Imaginary == 0 && dirValue is '-')) {
                    pos += nextDir;
                    dir = nextDir;
                    found = true;
                    break;
                }
            }

            if (!found) break;
        }

        return (path, steps);
    }
}