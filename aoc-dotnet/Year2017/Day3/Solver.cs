using System.Numerics;

namespace aoc_dotnet.Year2017.Day3;

public class Solver : SolverInterface
{
    private Complex Up = -Complex.ImaginaryOne;
    private Complex Down = Complex.ImaginaryOne;
    private Complex Left = -Complex.One;
    private Complex Right = Complex.One;
    public string Part1(string[] input)
    {
        var target = int.Parse(input[0]);
        if (target == 1) return "" + 1;
        var i = 3;
        var y = 0;
        while (true)
        {
            y++;
            var next = i * i;
            if (next >= target) break;
            i += 2;
        }

        var rem = (i * i - target) % (i - 1);
        var sx = (int)(0 - Math.Floor(i / 2.0));
        var x = Math.Abs(sx + rem);

        return "" + (y + x);
    }

    public string Part2(string[] input)
    {
        var target = int.Parse(input[0]);
        var pos = Complex.Zero;
        var grid = new Dictionary<Complex, int>
        {
            [pos] = 1
        };
        var o = 1;
        var directions = new[] { Up, Left, Down, Right };
        var neighbours = new[] { Up, Down, Left, Right, Up + Left, Up + Right, Down + Left, Down + Right };
        while (true)
        {
            o += 2;
            pos += Complex.ImaginaryOne + 1;
            foreach (var d in directions)
                for (var i = 0; i < o - 1; i++)
                {
                    pos += d;
                    grid[pos] = neighbours.Sum(n => grid.GetValueOrDefault(pos + n, 0));
                    if (grid[pos] <= target) continue;
                    return "" + grid[pos];
                }
        }

    }
}