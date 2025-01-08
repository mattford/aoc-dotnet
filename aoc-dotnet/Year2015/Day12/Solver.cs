using System.Text.Json.Nodes;

namespace aoc_dotnet.Year2015.Day12;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var json = JsonNode.Parse(input[0]);
        if (json == null) return "";
        return "" + GetSumOf(json, []);
    }

    public string Part2(string[] input)
    {
        var json = JsonNode.Parse(input[0]);
        if (json == null) return "";
        return "" + GetSumOf(json, ["red"]);
    }

    private int GetSumOf(JsonNode json, string[] ignoreValues)
    {
        var total = 0;
        if (json is JsonObject jObject)
        {
            foreach (var item in jObject)
            {
                if (item.Value is JsonObject or JsonArray) total += GetSumOf(item.Value, ignoreValues);
                if (item.Value is JsonValue jValue && jValue.TryGetValue<string>(out var s) && ignoreValues.Contains(s)) return 0;
                if (item.Value is JsonValue jValue2 && jValue2.TryGetValue<int>(out var i)) total += i;
            }
        }

        if (json is JsonArray jArray)
        {
            foreach (var item in jArray)
            {
                if (item == null) continue;
                if (item is JsonObject or JsonArray) total += GetSumOf(item, ignoreValues);
                if (item is JsonValue jValue && jValue.TryGetValue<int>(out var i)) total += i;
            }
        }

        return total;
    }
}