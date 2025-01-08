using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2015.Day15;

using Ingredient = List<int>;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var ingredients = GetIngredients(input);
        return "" + GetMaxScore(ingredients, false);
    }

    public string Part2(string[] input)
    {
        var ingredients = GetIngredients(input);
        return "" + GetMaxScore(ingredients, true);
    }

    private int GetMaxScore(Ingredient[] ingredients, bool limitCalories)
    {
        return GetMaxScore(ingredients, 100, [0, 0, 0, 0, 0], limitCalories);
    }

    private int GetMaxScore(Ingredient[] ingredients, int maxSpoons, int[] values, bool limitCalories)
    {
        var m = 0;
        if (maxSpoons == 0)
        {
            if (limitCalories && values[4] != 500) return 0;
            return values[..^1].Aggregate(1, (current, x) => current * Math.Max(0, x));
        }
        foreach (var ingredient in ingredients)
        {
            var nextIngredients = ingredients.Where(x => x != ingredient).ToArray();
            for (var i = 1; i <= maxSpoons; i++)
            {
                var nextValues = values.Select((v, j) => v + i * ingredient[j]).ToArray();
                m = Math.Max(m, GetMaxScore(nextIngredients, maxSpoons - i, nextValues, limitCalories));
            }
        }

        return m;
    }

    private static Ingredient[] GetIngredients(string[] input)
    {
        return (
            from line in input
            let matches = Regex.Matches(line, @"([\d\-]+)")
            select matches.Select(m => int.Parse(m.Value)).ToList()
        ).ToArray();
    }
}