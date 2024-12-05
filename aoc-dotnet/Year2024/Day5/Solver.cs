namespace aoc_dotnet.Year2024.Day5;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + GetAnswer(input, true);
    }

    public string Part2(string[] input)
    {
        return "" + GetAnswer(input);    
    }

    private int GetAnswer(string[] input, bool sortedOnly = false)
    {
        var (rules, updates) = ParseInput(input);
        var total = updates.Select(update =>
        {
            var original = string.Join(',', update);
            var sorted = update.ToList();
            sorted.Sort((x, y) =>
            {
                if (rules.FirstOrDefault(r => r[0] == x && r[1] == y) != null)
                {
                    return -1;
                }

                if (rules.FirstOrDefault(r => r[0] == y && r[1] == x) != null)
                {
                    return 1;
                }

                return 0;
            });
            var hasBeenSorted = original != string.Join(',', sorted);
            if (hasBeenSorted == sortedOnly)
            {
                return 0;
            }
            var middleIdx = (int)Math.Floor(sorted.Count / 2d);
            return sorted[middleIdx];
        }).Sum();

        return total;   
    }

    private Tuple<int[][], int[][]> ParseInput(string[] input)
    {
        var resplit = string.Join("\n", input).Split("\n\n");
        var rules = resplit[0].Split("\n")
            .Select(x => x.Split("|").Select(int.Parse).ToArray())
            .ToArray();
        var updates = resplit[1].Split("\n").Select(x => x.Split(",").Select(int.Parse).ToArray()).ToArray();
        return Tuple.Create(rules, updates);
    }
}