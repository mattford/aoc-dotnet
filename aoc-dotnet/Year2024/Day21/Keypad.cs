using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Numerics;

namespace aoc_dotnet.Year2024.Day21;

public class Keypad
{
    private char _currentKey = 'A';
    private readonly ImmutableDictionary<char, ImmutableDictionary<char, string>> _buttonMap;
    private readonly ConcurrentDictionary<(char, char, int), long> cache;
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
            select new KeyValuePair<char, ImmutableDictionary<char, string>>(x.Value, BuildMap(map, x.Key))
        ).ToImmutableDictionary();
        cache = new ConcurrentDictionary<(char, char, int), long>();
    }

    public long GetCommandLength(string command, int iterations)
    {
        if (iterations == 0)
        {
            return command.Length;
        }

        var length = 0L;
        var current = 'A';
        foreach (var c in command)
        {
            length += GetCommandLength(current, c, iterations);
            current = c;
        }
        return length;
    }

    private long GetCommandLength(char current, char next, int iterations)
    {
        return cache.GetOrAdd((current, next, iterations), _ =>
        {
            var nextString = _buttonMap[current][next] + "A";
            return GetCommandLength(nextString, iterations - 1);
        });
    }
    
    
    public string GetCommandString(string command)
    {
        var chars = command.ToCharArray();
        var commandString = "";
        foreach (var commandChar in chars)
        {
            commandString += _buttonMap[_currentKey][commandChar] + "A";
            _currentKey = commandChar;
        }
        
        _currentKey = 'A';
        return commandString;
    }

    private ImmutableDictionary<char, string> BuildMap(ImmutableDictionary<Complex, char> map, Complex origin)
    {
        var paths = new Dictionary<char, string>();

        foreach (var point in map)
        {
            if (point.Key == origin)
            {
                paths.TryAdd(point.Value, "");
                continue;
            }
            
            var route = "";
            var from = origin;
            while (from != point.Key)
            {
                var horizontal = point.Key.Real - from.Real;
                var vertical = point.Key.Imaginary - from.Imaginary;
                if (horizontal < 0 && map.ContainsKey(from + horizontal))
                {
                    // go left first
                    route += new string('<', (int)Math.Abs(horizontal));
                    from += horizontal;
                    horizontal = 0;
                }

                if (vertical != 0 && map.ContainsKey(from + vertical * Complex.ImaginaryOne))
                {
                    // down now
                    route += new string(vertical > 0 ? 'v' : '^', (int)Math.Abs(vertical));
                    from += vertical * Complex.ImaginaryOne;
                }

                if (horizontal > 0 && map.ContainsKey(from + horizontal))
                {
                    route += new string('>', (int)Math.Abs(horizontal));
                    from += horizontal;
                }
            }

            paths.TryAdd(point.Value, route);
        }
        
        return paths.ToImmutableDictionary();
    }
}