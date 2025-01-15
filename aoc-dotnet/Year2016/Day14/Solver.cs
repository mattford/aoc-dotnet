using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2016.Day14;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + SolveWithStretch(input[0], 0);
    }

    public string Part2(string[] input)
    {
        return "" + SolveWithStretch(input[0], 2016);
    }

    private int SolveWithStretch(string salt, int stretch)
    {
        var queue = new Queue<string>();
        foreach (var x in Enumerable.Range(0, 1001))
        {
            queue.Enqueue(Hash(salt + x, stretch));
        }
        var i = 0;
        var found = 0;
        while (found < 64)
        {
            var hash = queue.Dequeue();
            if (Regex.IsMatch(hash, @"([0-9a-z])\1\1"))
            {
                var m = Regex.Match(hash, @"([0-9a-z])\1\1");
                // now check next 1000
                if (queue.Any(hash2 => hash2.Contains(new string(m.Groups[1].Value[0], 5))))
                {
                    found++;
                }
            }

            queue.Enqueue(Hash(salt + (i + queue.Count + 1), stretch));
            i++;
        }

        return i - 1;
    }

    private string Hash(string input, int stretchCount)
    {
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var output = Convert.ToHexString(MD5.HashData(inputBytes)).ToLower();
        if (stretchCount > 0) output = Hash(output, stretchCount - 1);
        return output;
    }
}