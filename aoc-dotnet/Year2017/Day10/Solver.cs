using System.Collections.Immutable;

namespace aoc_dotnet.Year2017.Day10;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var output = KnotHash(Enumerable.Range(0, 256).ToList(), input[0].Split(',').Select(int.Parse).ToList()).Item1;
        return "" + (output[0] * output[1]);
    }

    public string Part2(string[] input)
    {
        var inputs = Enumerable.Range(0, 256).ToList();
        var lengths = input[0].ToCharArray().Select(x => (int)x).ToList();
        lengths.AddRange(new List<int> { 17, 31, 73, 47, 23 });
        var c = 0;
        for (var i = 0; i < 64; i++)
        {
            (inputs, c) = KnotHash(inputs, lengths, i * lengths.Count, c);
        }

        return DenseHash(inputs);
    }

    private string DenseHash(List<int> sparseHash)
    {
        var hash = "";
        for (var i = 0; i < sparseHash.Count; i += 16)
        {
            var xored = sparseHash.Skip(i).Take(16).Aggregate(0, (a, c) => a ^ c);
            hash += xored.ToString("x2");
        }

        return hash;
    }

    private (List<int>, int) KnotHash(List<int> input, List<int> lengths, int s = 0, int f = 0)
    {
        if (f != 0)
        {
            input = input[f..].ToImmutableList().AddRange(input[..f]).ToList();
        }
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

        var final = input[^f..];
        final.AddRange(input[..^f]);
        return (final, f);
    }
}