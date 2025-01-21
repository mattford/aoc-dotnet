using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;

namespace aoc_dotnet.Year2017.Day14;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + GetMap(input).Count;
    }

    public string Part2(string[] input)
    {
        var map = GetMap(input);
        var seen = new HashSet<Complex>();
        var regions = 0;
        var neighbours = new List<Complex> { Complex.ImaginaryOne, -Complex.ImaginaryOne, Complex.One, -Complex.One };
        while (map.Any(c => !seen.Contains(c)))
        {
            regions++;
            var start = map.First(c => !seen.Contains(c));
            var queue = new Queue<Complex>();
            queue.Enqueue(start);
            while (queue.Any())
            {
                var current = queue.Dequeue();
                seen.Add(current);
                foreach (var d in neighbours)
                {
                    if (!seen.Contains(current + d) && map.Contains(current + d))
                    {
                        queue.Enqueue(current + d);
                    }
                }
            }
        }

        return ""+regions;
    }

    private List<Complex> GetMap(string[] input)
    {
        var map = new List<Complex>();
        for (var i = 0; i < 128; i++)
        {
            var inputs = Enumerable.Range(0, 256).ToList();
            var lengths = $"{input[0]}-{i}".ToCharArray().Select(c => (int)c).ToList();
            lengths.AddRange(new List<int>{17, 31, 73, 47, 23});
            var hash = KnotHash(inputs, lengths);
            for (var x = 0; x < 128; x++)
            {
                if (hash[x] == '1')
                {
                    map.Add(Complex.ImaginaryOne * i + x);
                }
            }
        }

        return map;
    }
    
    private string DenseHash(List<int> sparseHash)
    {
        var hash = "";
        for (var i = 0; i < sparseHash.Count; i += 16)
        {
            var xored = sparseHash.Skip(i).Take(16).Aggregate(0, (a, c) => a ^ c);
            hash += string.Join("", xored.ToString("x2").ToCharArray().Select(c => Convert.ToString(Convert.ToInt32(""+c, 16), 2).PadLeft(4, '0')));
        }

        return hash;
    }
    
    private string KnotHash(List<int> input, List<int> lengths)
    {
        var f = 0;
        var s = 0;
        for (var i = 0; i < 64; i++)
        {
            foreach (var length in lengths)
            {
                var reversed = input[..length];
                reversed.Reverse();
                input = input[length..];
                input.AddRange(reversed);
                var ss = s % input.Count;
                input = input[ss..].ToImmutableList().AddRange(input[..ss]).ToList();
                f += s + length;
                f %= input.Count;
                s++;
            }
            
        }
        var final = input[^f..];
        final.AddRange(input[..^f]);
        return DenseHash(final);
    }
}