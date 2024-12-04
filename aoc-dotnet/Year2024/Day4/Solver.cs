namespace aoc_dotnet.Year2024.Day4;

public class Solver : SolverInterface
{
    public string Part1(string[] input)
    {
        var grid = ParseInput(input);
        var total = 0;
        for (var x = 0; x < grid.Length; x++)
        {
            for (var y = 0; y < grid[x].Length; y++)
            {
                total += FindChristmas(grid, Tuple.Create(x, y), "XMAS");
            }
        }

        return "" + total;
    }

    public string Part2(string[] input)
    {
        var grid = ParseInput(input);
        var total = 0;
        for (var x = 0; x < grid.Length; x++)
        {
            for (var y = 0; y < grid[x].Length; y++)
            {
                var mas = new List<Tuple<int, int>>();
                foreach (var n in GetNeighbours(true))
                {
                    var found = FindChristmas(grid, Tuple.Create(x + n.Item1, y + n.Item2), "MAS",
                        Tuple.Create(-n.Item1, -n.Item2));
                    if (found < 1) continue;
                    if (mas.Any(ma => IsPerpendicular(ma, n)))
                    {
                        total++;
                        break;
                    }

                    mas.Add(n);
                }
            }
        }
        return "" + total;
    }

    private static bool IsPerpendicular(Tuple<int, int> vector1, Tuple<int, int> vector2)
    {
        return int.Abs(vector1.Item1 - vector2.Item1) + int.Abs(vector1.Item2 - vector2.Item2) == 2;
    }

    private int FindChristmas(char[][] grid, Tuple<int, int> position, string target,
        Tuple<int, int>? lastVector = null)
    {
        if (position.Item1 < 0 || position.Item1 > grid.Length - 1 || position.Item2 < 0 ||
            position.Item2 > grid[0].Length - 1)
        {
            return 0;
        }

        var current = grid[position.Item1][position.Item2];
        var targetChar = target.Take(1).First();
        var newTarget = target[1..];
        if (current != targetChar)
        {
            return 0;
        }

        if (newTarget.Length == 0)
        {
            return 1;
        }

        var neighbours = lastVector != null ? [lastVector] : GetNeighbours();

        return neighbours.Select(x =>
            FindChristmas(grid, Tuple.Create(position.Item1 + x.Item1, position.Item2 + x.Item2), newTarget, x)).Sum();
    }

    private static Tuple<int, int>[] GetNeighbours(bool cornersOnly = false)
    {
        if (cornersOnly)
        {
            return
            [
                Tuple.Create(1, -1), // NE
                Tuple.Create(-1, -1), // NW
                Tuple.Create(1, 1), // SE
                Tuple.Create(-1, 1) // SW
            ];
        }

        return
        [
            Tuple.Create(0, -1), // N
            Tuple.Create(1, 0), // E
            Tuple.Create(0, 1), // S
            Tuple.Create(-1, 0), // W
            Tuple.Create(1, -1), // NE
            Tuple.Create(-1, -1), // NW
            Tuple.Create(1, 1), // SE
            Tuple.Create(-1, 1) // SW
        ];
    }

    private static char[][] ParseInput(string[] input)
    {
        var grid = new char[input.Length][];
        for (var i = 0; i < input.Length; i++)
        {
            grid[i] = new char[input[i].Length];
            for (var j = 0; j < input[i].Length; j++)
            {
                grid[i][j] = input[i][j];
            }
        }

        return grid;
    }
}