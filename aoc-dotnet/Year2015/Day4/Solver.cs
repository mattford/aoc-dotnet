using System.Security.Cryptography;
using System.Text;

namespace aoc_dotnet.Year2015.Day4;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return ""+FindLowestInt(input[0], 5);
    }

    public string Part2(string[] input)
    {
        return ""+FindLowestInt(input[0], 6);
    }

    private static int FindLowestInt(string key, int zeroCount)
    {
        var zeroString = "";
        for (var j = 0; j < zeroCount; j++)
        {
            zeroString += "0";
        }

        var i = 0;
        string hash;
        do
        {
            i++;
            var inputBytes = Encoding.ASCII.GetBytes(key + i);
            hash = Convert.ToHexString(MD5.HashData(inputBytes));
        } while (!hash.StartsWith(zeroString));

        return i;
    }
}