namespace aoc_dotnet.Year2016.Day20;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var blocks = input.Select(x => x.Split("-").Select(long.Parse).ToArray()).OrderBy(x => x[0]).ToArray();
        var i = 0L;
        while (blocks.Any(b => b[0] <= i && b[1] >= i))
        {
            var block = blocks.First(b => b[0] <= i && b[1] >= i);
            i = block[1] + 1;
        }

        return "" + i;
    }

    public string Part2(string[] input)
    {
        var blocks = input.Select(x => x.Split("-").Select(long.Parse).ToArray()).OrderBy(x => x[0]).ToArray();
        var t = 0L;
        for (var i = 0L; i <= 4294967295; i++)
        {
            var myBlock = blocks.FirstOrDefault(b => b[0] <= i && b[1] >= i);
            if (myBlock != null)
            {
                i = myBlock[1];
                continue;
            }
            // how many til the next block?
            var nextblock = blocks.FirstOrDefault(b => b[0] > i);
            if (nextblock == null)
            {
                t += 4294967295 - i;
                break;
            }

            t += nextblock[0] - i;
            i = nextblock[1];
        }
        return "" + t;
    }
}