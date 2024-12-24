using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2024.Day21;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var numeric = "789\n456\n123\n 0A".Split("\n");
        var directional = " ^A\n<v>".Split("\n");
        var pipeline = new []{
            new Keypad(numeric),
            new Keypad(directional),
            new Keypad(directional),
        };
        var total = (
            from code in input 
            let sequence = Translate(pipeline, code).OrderBy(x => x.Length).First()
            let numericPart = int.Parse(Regex.Replace(code, "[^0-9]+", "")) 
            let length = sequence.Length 
            select length * numericPart
        ).Sum();

        return ""+total;
    }

    public string Part2(string[] input)
    {
        var numeric = "789\n456\n123\n 0A".Split("\n");
        var directional = " ^A\n<v>".Split("\n");
        var total = 0;
        var keypads = new List<Keypad> { new(numeric) };
        var dir = new Keypad(directional);
        keypads.AddRange(Enumerable.Range(0, 25).Select(_ => dir));
        var pipeline = keypads.ToArray();
        foreach (var code in input)
        {
            var sequence = Translate(pipeline, code).OrderBy(x => x.Length).First();
            var numericPart = int.Parse(Regex.Replace(code, "[^0-9]+", ""));
            var length = sequence.Length;
            // Console.WriteLine($"{code}: {sequence} - {length} * {numericPart} = {length * numericPart}");
            total += length * numericPart;
        }
        return ""+total;
    }

    private string[] Translate(Keypad[] pipeline, string code)
    {
        var output = new[]{code};
        var i = 0;
        var cache = new Dictionary<string, string[]>();
        foreach (var p in pipeline)
        {
            i++;
            Console.WriteLine($"Processing robot {i}, keeping track of {output.Length}");
            var next = new List<string>();
            foreach (var c in output)
            {
                if (!cache.TryGetValue(c, out var cached))
                {
                    cached = p.GetCommandStrings(c);
                    cache.Add(c, cached);
                }
                next = next.Union(cached).ToList();
            }
            output = next.ToArray();
        }
        
        return output;
    }
}