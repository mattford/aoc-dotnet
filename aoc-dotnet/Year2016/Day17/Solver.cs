using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace aoc_dotnet.Year2016.Day17;

public class Solver: SolverInterface
{
    Dictionary<char, Complex> Directions = new()
    {
        ['U'] = -Complex.ImaginaryOne,
        ['D'] = Complex.ImaginaryOne,
        ['L'] = -1,
        ['R'] = 1,
    };
    public string Part1(string[] input)
    {
        return ""+FindMinAndMax(input[0]).Item1;
    }

    public string Part2(string[] input)
    {
        return ""+FindMinAndMax(input[0]).Item2;
    }

    private (string, int) FindMinAndMax(string passcode)
    {
        var queue = new PriorityQueue<(Complex, string), int>();
        queue.Enqueue((Complex.Zero, ""), 0);
        var visited = new HashSet<(Complex, string)>();
        var max = 0;
        var min = "";
        while (queue.Count > 0)
        {
            var (pos, path) = queue.Dequeue();
            if (!visited.Add((pos, path))) continue;
            var hash = Hash(passcode + path);
            foreach (var dir in Directions.Where((kv, i) =>
                         pos + kv.Value is { Imaginary: >= 0 and <= 3, Real: >= 0 and <= 3 } &&
                         hash[i] is 'b' or 'c' or 'd' or 'e' or 'f'))
            {
                if (visited.Contains((pos + dir.Value, path + dir.Key))) continue;
                if (pos + dir.Value is { Imaginary: 3, Real: 3 })
                {
                    if (min == string.Empty) min = path + dir.Key;
                    max = Math.Max(max, path.Length + 1);
                    continue;
                }
                queue.Enqueue((pos + dir.Value, path + dir.Key), path.Length + 1);
            }
        }

        return (min,  max);
    }
    
    private string Hash(string input)
    {
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var output = Convert.ToHexString(MD5.HashData(inputBytes)).ToLower();
        return output;
    }
}