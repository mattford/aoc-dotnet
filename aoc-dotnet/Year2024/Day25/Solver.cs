namespace aoc_dotnet.Year2024.Day25;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var (locks, keys) = ParseInputs(input);
        var total = locks.Sum(l => keys.Count(k => l.Zip(k).All(lk => lk.First >= lk.Second)));
        return "" + total;
    }

    public string Part2(string[] input)
    {
        return "Merry Christmas!";
    }

    private (int[][], int[][]) ParseInputs(string[] input)
    {
        var locks = new List<int[]>();
        var keys = new List<int[]>();
        var parts = string.Join("\n", input).Split("\n\n");
        foreach (var part in parts)
        {
            var lines = part.Split('\n');
            var isLock = lines[0] == new string('#', lines[0].Length);
            var thisHeights = new int[lines[0].Length];
            for (var x = 0; x < lines[0].Length; x++)
            {
                var h = 0;
                for (var y = 0; y < lines.Length; y++)
                {
                    if (lines[y][x] == '#') h++;
                }
                thisHeights[x] = isLock ? lines.Length - h : h;
            }

            if (isLock)
            {
                locks.Add(thisHeights);
            }
            else
            {
                keys.Add(thisHeights);
            }
        }
        return (locks.ToArray(), keys.ToArray());
    }
}