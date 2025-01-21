namespace aoc_dotnet.Year2017.Day16;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var start = Enumerable.Range('a', 16).Select(i => (char)i).ToList();
        var end = Dance(start, input);

        return new string(end.ToArray());
    }

    public string Part2(string[] input)
    {
        var iterations = 1_000_000_000;
        var start = Enumerable.Range('a', 16).Select(i => (char)i).ToList();
        var seen = new Dictionary<string, int>();
        var i = 1;
        var skipped = false;
        while (iterations > 0)
        {
            start = Dance(start, input);
            iterations--;
            
            if (!skipped && !seen.TryAdd(new string(start.ToArray()), i))
            {
                skipped = true;
                var last = seen[new string(start.ToArray())];
                var length = i - last;
                iterations %= length;
                continue;
            }

            i++;
            
        }
        return new string(start.ToArray());
    }

    private List<char> Dance(List<char> start, string[] input)
    {
        foreach (var i in input[0].Split(','))
        {
            if (i.StartsWith('s'))
            {
                var x = int.Parse(i[1..]);
                start = start[^x..].Concat(start[..^x]).ToList();
                continue;
            }

            int[] positions;
            if (i.StartsWith('p'))
            {
                positions = i[1..].Split('/').Select(x => start.IndexOf(x[0])).ToArray();
            }
            else
            {
                positions = i[1..].Split('/').Select(int.Parse).ToArray();
            }
            (start[positions[0]], start[positions[1]]) = (start[positions[1]], start[positions[0]]);
        }

        return start;
    }
}