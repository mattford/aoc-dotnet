namespace aoc_dotnet.Year2024.Day19;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var (towels, designs) = ParseInput(input);
        var cache = new Dictionary<string, long>();
        return "" + designs.Count(design => IsPossible(design, towels, cache) > 0);
    }

    public string Part2(string[] input)
    {
        var (towels, designs) = ParseInput(input);
        var cache = new Dictionary<string, long>();
        return "" + designs.Sum(design => IsPossible(design, towels, cache));
    }

    private long IsPossible(string design, string[] towels, Dictionary<string, long> cache)
    {
        if (cache.ContainsKey(design)) return cache[design];
        long current = 0;
        foreach (var towel in towels)
        {
            if (!design.StartsWith(towel)) continue;
            var rem = design[towel.Length..];
            if (rem == string.Empty)
            {
                current++;
            }
            else
            {
                current += IsPossible(rem, towels, cache);   
            }
            
        }
        cache[design] = current;
        return current;
    }

    private (string[], string[]) ParseInput(string[] input)
    {
        var parts = string.Join("\n", input).Split("\n\n");
        var towels = parts[0].Split(", ");
        var designs = parts[1].Split("\n");
        return (towels, designs);
    }
}