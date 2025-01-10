using System.Numerics;

namespace aoc_dotnet.Year2016.Day1;

public class Solver: SolverInterface
{
    private Complex North = -Complex.ImaginaryOne;
    private Complex TurnRight = Complex.ImaginaryOne;
    
    public string Part1(string[] input)
    {
        var pos = Complex.Zero;
        var dir = North;
        foreach (var instr in input[0].Split(", "))
        {
            var t = instr[0];
            var v = int.Parse(instr[1..]);
            dir *= t == 'R' ? TurnRight : -TurnRight;
            pos += dir * v;
        }
        return "" + (Math.Abs(pos.Imaginary) + Math.Abs(pos.Real));
    }

    public string Part2(string[] input)
    {
        var seen = new HashSet<Complex>();
        var pos = Complex.Zero;
        var dir = North;
        foreach (var instr in input[0].Split(", "))
        {
            var t = instr[0];
            var v = int.Parse(instr[1..]);
            dir *= t == 'R' ? TurnRight : -TurnRight;
            for (var i = 0; i < v; i++)
            {
                pos += dir;
                if (!seen.Add(pos)) return "" + (Math.Abs(pos.Imaginary) + Math.Abs(pos.Real));
            }
        }
        return "No location visited twice";
    }
}