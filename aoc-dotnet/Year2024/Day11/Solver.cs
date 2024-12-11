namespace aoc_dotnet.Year2024.Day11;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var stones = input[0].Split(" ").Select(c => new KeyValuePair<long, long>(long.Parse("" + c), 1)).ToDictionary();
        return "" + BlinkAtStones(stones, 25);
    }

    public string Part2(string[] input)
    {
        var stones = input[0].Split(" ").Select(c => new KeyValuePair<long, long>(long.Parse("" + c), 1)).ToDictionary();
        return "" + BlinkAtStones(stones, 75);
    }

    private static long BlinkAtStones(Dictionary<long, long> stones, int iterations)
    {
        for (var i = 0; i < iterations; i++)
        {
            var newStones = new Dictionary<long, long>();
            foreach (var stone in stones)
            {
                if (stone.Key == 0)
                {
                    newStones.TryAdd(1, 0);
                    newStones[1] += stone.Value;
                    continue;
                }

                var strNum = "" + stone.Key;
                if (strNum.Length % 2 == 0)
                {
                    newStones.TryAdd(long.Parse(strNum[..(strNum.Length / 2)]), 0);
                    newStones[long.Parse(strNum[..(strNum.Length / 2)])] += stone.Value;
                    newStones.TryAdd(long.Parse(strNum[(strNum.Length / 2)..]), 0);
                    newStones[long.Parse(strNum[(strNum.Length / 2)..])] += stone.Value;
                    continue;
                }

                newStones.TryAdd(stone.Key * 2024, 0);
                newStones[stone.Key * 2024] += stone.Value;
            }

            stones = newStones;
        }

        return stones.Select(x => x.Value).Sum();
    }
}