using System.Numerics;

namespace aoc_dotnet.Year2024.Day15;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var (grid, robot, steps) = ParseInput(input, 1);
        foreach (var step in steps)
        {
            var vector = step switch
            {
                '>' => 1,
                '<' => -1,
                '^' => -Complex.ImaginaryOne,
                'v' => Complex.ImaginaryOne,
                _ => 0,
            };
            (grid, robot) = DoMove(grid, robot, vector);
        }

        return "" + grid.Where(p => p.Value == 'O')
            .Select(p => 100 * p.Key.Imaginary + p.Key.Real)
            .Sum();
    }

    public string Part2(string[] input)
    {
        var (grid, robot, steps) = ParseInput(input, 2);
        foreach (var step in steps)
        {
            var vector = step switch
            {
                '>' => 1,
                '<' => -1,
                '^' => -Complex.ImaginaryOne,
                'v' => Complex.ImaginaryOne,
                _ => 0,
            };
            (grid, robot) = DoMove(grid, robot, vector);
        }
        return "" + grid.Where(p => p.Value == '[')
            .Select(p => 100 * p.Key.Imaginary + p.Key.Real)
            .Sum();
    }
    
    private (Dictionary<Complex, char>, Complex) DoMove(Dictionary<Complex, char> grid, Complex robot, Complex vector)
    {
        var me = grid[robot];
        if (me == ']') return DoMove(grid, robot - 1, vector);
        if (me == '#') return (grid, robot);
        
        var extra = me == '[' && vector.Real > 0 ? 1 : 0;
        var checkPoints = new List<Complex>{ robot + vector + extra};
        if (me == '[' && vector.Imaginary != 0)
        {
            checkPoints.Add(robot + vector + 1);
        }

        var newGrid = grid.ToDictionary();
        if (checkPoints.Any(x => grid.ContainsKey(x)))
        {
            foreach (var p in checkPoints.Where(x => newGrid.ContainsKey(x)))
            {
                (newGrid, _) = DoMove(newGrid, p, vector);
            }
        }

        if (checkPoints.All(x => !newGrid.ContainsKey(x)))
        {
            grid = newGrid;
            grid.Remove(robot);
            if (me == '[') grid.Remove(robot + 1);
            grid.Add(robot + vector, me);
            if (me == '[') grid.Add(robot + vector + 1, ']');

            return (grid, robot + vector);
        }
        return (grid, robot);
    }

    private (Dictionary<Complex, char>, Complex, char[]) ParseInput(string[] input, int boxWidth)
    {
        var parts = string.Join("\n", input).Split("\n\n");
        var grid = parts[0].Split("\n");
        if (boxWidth > 1)
        {
            grid = grid.Select(l => l.Replace("#", "##").Replace(".", "..").Replace("O", "[]").Replace("@", "@.")).ToArray();
        }
        var steps = string.Join("", parts[1].Split("\n")).ToCharArray();
        var gridItems = (
            from y in Enumerable.Range(0, grid.Length)
            from x in Enumerable.Range(0, grid[0].Length)
            where grid[y][x] != '.'
            select new KeyValuePair<Complex, char>(Complex.ImaginaryOne * y + x, grid[y][x])
        ).ToDictionary();
        var rY = grid.ToList().FindIndex(x => x.Contains('@'));
        var rX = grid[rY].IndexOf('@');
        return (gridItems, Complex.ImaginaryOne * rY + rX, steps);
    }
}