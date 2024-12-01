using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2015.Day5;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return ""+input.Count(IsNice);
    }

    public string Part2(string[] input)
    {
        return ""+input.Count(IsNicer);
    }

    private static bool IsNice(string input)
    {
        // It contains at least three vowels (aeiou only), like aei, xazegov, or aeiouaeiouaeiou.
        // It contains at least one letter that appears twice in a row, like xx, abcdde (dd), or aabbccdd (aa, bb, cc, or dd).
        // It does not contain the strings ab, cd, pq, or xy, even if they are part of one of the other requirements.
        if (Regex.IsMatch(input, "(ab|cd|pq|xy)"))
        {
            return false;
        }

        if (!Regex.IsMatch(input, "(aa|bb|cc|dd|ee|ff|gg|hh|jj|kk|ll|mm|nn|oo|pp|qq|rr|ss|tt|uu|vv|ww|xx|yy|zz)"))
        {
            return false;
        }

        return Regex.Matches(input, "(a|e|i|o|u)").Count >= 3;
    }

    private static bool IsNicer(string input)
    {
        // It contains a pair of any two letters that appears at least twice in the string without overlapping, like xyxy (xy) or aabcdefgaa (aa), but not like aaa (aa, but it overlaps).
        // It contains at least one letter which repeats with exactly one letter between them, like xyx, abcdefeghi (efe), or even aaa.
        var chars = input.ToCharArray();
        var hasRepeatedPair = false;
        var hasRepeatedLetter = false;
        for (var i = 0; i < chars.Length; i++)
        {
            if (i + 2 < chars.Length && chars[i] == chars[i + 2])
            {
                hasRepeatedLetter = true;
            }

            if (i + 4 < chars.Length)
            {
                var slice = input.Substring(i, 4);
                if (slice[..2].Equals(slice[2..]))
                {
                    hasRepeatedPair = true;
                }
            }

            if (hasRepeatedLetter && hasRepeatedPair)
            {
                break;
            }
        }
        return hasRepeatedLetter && hasRepeatedPair;
    }
}