namespace aoc_dotnet.Year2018.Day14;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var t = int.Parse(input[0]);
        var recipes = new List<int>{ 3, 7 };
        var a = 0;
        var b = 1;
        for (var i = 0; i < t + 10; i++)
        {
            var sumString = "" + (recipes[a] + recipes[b]);
            recipes.AddRange(sumString.ToCharArray().Select(c => int.Parse(""+c)));
            a = (a + recipes[a] + 1) % recipes.Count;
            b = (b + recipes[b] + 1) % recipes.Count;
        }

        return string.Join("", recipes.Slice(t, 10));
    }

    public string Part2(string[] input)
    {
        var recipes = new List<int>{ 3, 7 };
        var a = 0;
        var b = 1;
        while (true)
        {
            var sumString = "" + (recipes[a] + recipes[b]);
            recipes.AddRange(sumString.ToCharArray().Select(c => int.Parse(""+c)));
            a = (a + recipes[a] + 1) % recipes.Count;
            b = (b + recipes[b] + 1) % recipes.Count;
            var last20 = string.Join("", recipes.TakeLast(20));
            if (!last20.Contains(input[0])) continue;
            var idx = last20.IndexOf(input[0], StringComparison.Ordinal);
            return "" + ((recipes.Count - last20.Length) + idx);
        }
    }
}