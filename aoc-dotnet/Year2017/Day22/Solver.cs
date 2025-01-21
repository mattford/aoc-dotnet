using System.Numerics;

namespace aoc_dotnet.Year2017.Day22;

public class Solver: SolverInterface
{
    private Complex Up = -Complex.ImaginaryOne;
    private Complex TurnRight = Complex.ImaginaryOne;
    public string Part1(string[] input)
    {
        var pos = Math.Floor(input.Length / 2d) * -Up + Math.Floor(input[0].Length / 2d);
        var dir = Up;
        var infected = (
            from y in Enumerable.Range(0, input.Length)
            from x in Enumerable.Range(0, input[y].Length)
            where input[y][x] == '#'
            select new KeyValuePair<Complex,bool>(-Up * y + x, true)
        ).ToDictionary();
        const int iterations = 10000;
        var infections = 0;
        for (var i = 0; i < iterations; i++)
        {
            var isInfected = infected.GetValueOrDefault(pos, false);
            dir *= isInfected ? TurnRight : -TurnRight;
            if (!isInfected) infections++;
            infected[pos] = !isInfected;
            pos += dir;
        }

        return "" + infections;
    }

    public string Part2(string[] input)
    {
        var pos = Math.Floor(input.Length / 2d) * -Up + Math.Floor(input[0].Length / 2d);
        var dir = Up;
        var infected = (
            from y in Enumerable.Range(0, input.Length)
            from x in Enumerable.Range(0, input[y].Length)
            where input[y][x] == '#'
            select new KeyValuePair<Complex,char>(-Up * y + x, 'I')
        ).ToDictionary();
        const int iterations = 10000000;
        var infections = 0;
        for (var i = 0; i < iterations; i++)
        {
            var state = infected.GetValueOrDefault(pos, 'C');
            (dir, state) = state switch
            {
                'C' => (dir * -TurnRight, 'W'),
                'W' => (dir, 'I'),
                'I' => (dir * TurnRight, 'F'),
                'F' => (dir * TurnRight * TurnRight, 'C'),
                _ => throw new Exception("Invalid state")
            };
            if (state == 'I') infections++;
            infected[pos] = state;
            pos += dir;
        }

        return "" + infections;
    }
}