using System.Collections.Immutable;
using System.Numerics;

namespace aoc_dotnet.Year2024.Day6;

public class Solver : SolverInterface
{
    static Complex Up = -Complex.ImaginaryOne;
    static Complex Down = Complex.ImaginaryOne;
    static Complex Left = -1;
    static Complex Right = 1;
    public string Part1(string[] input)
    {
        var (grid, position, direction) = ParseInput(input);
        var uniquePositions = new HashSet<Complex>();
        while (grid.ContainsKey(position))
        {
            uniquePositions.Add(position);
            var nextPosition = position + direction;
            while (grid.ContainsKey(nextPosition) && grid[nextPosition].Equals('#'))
            {
                direction = TurnRight(direction);
                nextPosition = position + direction;
            }

            position = nextPosition;
        }
        return "" + uniquePositions.Count;
    }

    public string Part2(string[] input)
    {
        var (grid, position, direction) = ParseInput(input);
        return "" + (
            from pos in grid
            where pos.Value.Equals('.') && position != pos.Key
            where DoesLoop(grid.SetItem(pos.Key, '#'), position, direction)
            select 1
        ).Sum();
    }

    private static bool DoesLoop(ImmutableDictionary<Complex, char> grid, Complex position, Complex direction)
    {
        var uniquePositions = new HashSet<(Complex, Complex)>();
        while (grid.ContainsKey(position))
        {
            if (!uniquePositions.Add((position, direction)))
            {
                return true;
            }
            var nextPosition = position + direction;
            while (grid.ContainsKey(nextPosition) && grid[nextPosition].Equals('#'))
            {
                direction = TurnRight(direction);
                nextPosition = position + direction;
            }

            position = nextPosition;
        }
        return false;
    }

    private static Complex TurnRight(Complex direction)
    {
        if (direction == Up)
        {
            return Right;
        }

        if (direction == Right)
        {
            return Down;
        }

        if (direction == Down)
        {
            return Left;
        }

        if (direction == Left)
        {
            return Up;
        }

        return direction;
    }

    private static (ImmutableDictionary<Complex, char>, Complex, Complex) ParseInput(string[] input)
    {
        var grid = new Dictionary<Complex, char>();
        var position = Complex.Zero;
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] == '^')
                {
                    position = Complex.ImaginaryOne * y + x;
                    grid.Add(Complex.ImaginaryOne * y + x, '.');
                    continue;
                }
                grid.Add(Complex.ImaginaryOne * y + x, input[y][x]);
            }
        }

        return (grid.ToImmutableDictionary(), position, Up);
    }
}