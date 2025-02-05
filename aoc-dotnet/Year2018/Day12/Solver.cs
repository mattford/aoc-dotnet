namespace aoc_dotnet.Year2018.Day12;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var (state, recipes) = ParseInput(input);
        for (var i = 0; i < 20; i++)
        {
            var min = state.Keys.Min();
            var max = state.Keys.Max();
            var next = new Dictionary<long, char>();
            for (var j = min - 4; j <= max + 4; j++)
            {
                var k = "";
                for (var l = 0; l <= 4; l++) k += state.GetValueOrDefault(j + l, '.');
                if (recipes.Contains(k)) next.Add(j + 2, '#');
            }

            state = next;
        }

        return ""+state.Keys.Sum();
    }

    public string Part2(string[] input)
    {
        var (state, recipes) = ParseInput(input);
        var last = 0L;
        var lastIncrement = 0L;
        for (var i = 0L; i < 50000000000; i++)
        {
            var min = state.Keys.Min();
            var max = state.Keys.Max();
            var next = new Dictionary<long, char>();
            for (var j = min - 4; j <= max + 4; j++)
            {
                var k = "";
                for (var l = 0; l <= 4; l++) k += state.GetValueOrDefault(j + l, '.');
                if (recipes.Contains(k)) next.Add(j + 2, '#');
            }

            state = next;
            var increment = state.Keys.Sum() - last;
            if (increment == lastIncrement)
            {
                return "" + (state.Keys.Sum() + (50000000000 - (i + 1)) * increment);
            }

            lastIncrement = increment;
            last = state.Keys.Sum();
        }
        // Won't get here before the end of time.
        return ""+state.Keys.Sum();
    }

    private (Dictionary<long, char>, List<string>) ParseInput(string[] input)
    {
        var parts = input[0].Split(": ")[1].ToCharArray();
        var state = Enumerable.Range(0, parts.Length).Where(i => parts[i] == '#').ToDictionary<int, long, char>(i => i, _ => '#');
        var recipes = (from line in input[2..] select line.Split(" => ") into p where p[1] == "#" select p[0]).ToList();

        return (state, recipes);
    }
}