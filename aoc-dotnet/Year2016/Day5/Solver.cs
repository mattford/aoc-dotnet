using System.Security.Cryptography;
using System.Text;

namespace aoc_dotnet.Year2016.Day5;

public class Solver : SolverInterface
{
    public string Part1(string[] input)
    {
        var pw = "";
        var i = 0;
        var target = new string('0', 5);
        while (pw.Length < 8)
        {
            var inputBytes = Encoding.ASCII.GetBytes(input[0] + i);
            var hash = Convert.ToHexString(MD5.HashData(inputBytes));
            if (hash.StartsWith(target)) pw += hash[5];
            i++;
        }

        return pw.ToLower();
    }

    public string Part2(string[] input)
    {
        var pw = new char[8];
        var f = new bool[8];
        var i = 0;
        var target = new string('0', 5);
        while (f.Any(x => !x))
        {
            var inputBytes = Encoding.ASCII.GetBytes(input[0] + i);
            var hash = Convert.ToHexString(MD5.HashData(inputBytes));
            if (hash.StartsWith(target))
            {
                if (int.TryParse(""+hash[5], out var pos) && pos is >= 0 and <= 7 && !f[pos])
                {
                    pw[pos] = hash[6];
                    f[pos] = true;
                }
            }

            i++;
        }

        return new string(pw).ToLower();
    }
}