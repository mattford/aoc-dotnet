using System.Numerics;

namespace aoc_dotnet.Year2017.Day11;

public class Solver: SolverInterface
{
    private Dictionary<string, Complex> Directions = new()
    {
        ["n"] = Complex.ImaginaryOne,
        ["s"] = -Complex.ImaginaryOne,
        ["ne"] = Complex.ImaginaryOne * 0.5 + 1,
        ["nw"] = Complex.ImaginaryOne * 0.5 - 1,
        ["se"] = -Complex.ImaginaryOne * 0.5 + 1,
        ["sw"] = -Complex.ImaginaryOne * 0.5 - 1
    };
    
    public string Part1(string[] input)
    {
        var pos = input[0].Split(',').Aggregate(Complex.Zero, (current, direction) => current + Directions[direction]);
        return ""+DistFromHome(pos);
    }

    public string Part2(string[] input)
    {
        var pos = Complex.Zero;
        var max = 0;
        foreach (var direction in input[0].Split(','))
        {
            pos += Directions[direction];
            max = Math.Max(max, DistFromHome(pos));
        }

        return ""+max;
    }

    private int DistFromHome(Complex point)
    {
        // In practice in my input just using the x coord always works, however
        // if you had a lower x coord than y you'd have to do extra moves
        if (point.Real == 0) return (int)Math.Abs(point.Imaginary);
        var d = Math.Abs(point.Real);
        var y = Math.Abs(point.Imaginary) - Math.Abs(point.Real) / 2;
        if (y > 0) d += y;
        return (int)d;
    }
}