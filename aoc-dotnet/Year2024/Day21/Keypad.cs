using System.Collections.Immutable;
using System.Numerics;

namespace aoc_dotnet.Year2024.Day21;

public class Keypad
{
    private char _currentKey = 'A';
    private readonly ImmutableDictionary<char, ImmutableDictionary<char, string[]>> _buttonMap;
    public Keypad(string[] rows)
    {
        var map = (
            from y in Enumerable.Range(0, rows.Length)
            from x in Enumerable.Range(0, rows[y].Length)
            where rows[y][x] != ' '
            select new KeyValuePair<Complex, char>(Complex.ImaginaryOne * y + x, rows[y][x])
        ).ToImmutableDictionary();
        _buttonMap = (
            from x in map
            select new KeyValuePair<char, ImmutableDictionary<char, string[]>>(x.Value, BuildMap(map, x.Key))
        ).ToImmutableDictionary();
        // foreach (var (btn, paths) in _buttonMap)
        // {
        //     Console.WriteLine(btn);
        //     foreach (var path in paths)
        //     {
        //         Console.WriteLine($" - {path.Key} => {path.Value}");
        //     }
        // }
    }

    public string[] GetCommandStrings(string command)
    {
        var chars = command.ToCharArray();
        var commandStrings = new List<string> { "" };
        foreach (var commandChar in chars)
        {
            var nextStrings = new List<string>();
            if (commandChar != _currentKey)
            {
                foreach (var ch in _buttonMap[_currentKey][commandChar])
                {
                    nextStrings = nextStrings.Union(commandStrings.Select(x => x + ch + "A")).ToList();
                }
            }
            else
            {
                nextStrings = commandStrings;
            }
            _currentKey = commandChar;
            commandStrings = nextStrings;
            
        }
        
        _currentKey = 'A';
        var min = commandStrings.Min(x => x.Length);
        return commandStrings.Distinct().Where(x => x.Length == min).ToArray();
    }

    private ImmutableDictionary<char, string[]> BuildMap(ImmutableDictionary<Complex, char> map, Complex from)
    {
        var paths = new Dictionary<char, string[]>();

        foreach (var point in map)
        {
            if (point.Key == from)
            {
                paths.TryAdd(point.Value, [""]);
                continue;
            }
            var routes = new List<string>();
            var horizontal = point.Key.Real - from.Real;
            var vertical = point.Key.Imaginary - from.Imaginary;
            var hString = new string(horizontal > 0 ? '>' : '<', (int)Math.Abs(horizontal));
            var vString = new string(vertical > 0 ? 'v' : '^', (int)Math.Abs(vertical));
            if (map.ContainsKey(from + horizontal))
            {
                routes.Add(hString + vString);
            }
            if (map.ContainsKey(from + vertical * Complex.ImaginaryOne))
            {
                routes.Add(vString + hString);
            }

            paths.TryAdd(point.Value, routes.Distinct().ToArray());
        }
        
        return paths.ToImmutableDictionary();
    }
}