using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2015.Day11;

public class Solver : SolverInterface
{
    public string Part1(string[] input)
    {
        return NextPassword(input[0]);
    }
    
    public string Part2(string[] input)
    {
        return NextPassword(NextPassword(input[0]));
    }

    private string NextPassword(string password)
    {
        // aim to create a string of the form xxx11233, changing as few chars as possible
        // first, check if the password contains any forbidden chars and bump them up if so
        var freeFrom = password.IndexOfAny(['i', 'o', 'l']);
        password = password.Replace('i', 'j').Replace('o', 'p').Replace('l', 'm');
        if (freeFrom >= 0) password = password[..(freeFrom + 1)] + new string('a', password.Length - freeFrom - 1);
        // now check if we already have a run
        var pattern =
            $"({string.Join("|", Enumerable.Range('a', 24).Select(x => ""+ (char)x + (char)(x + 1) + (char)(x + 2)).ToArray())})";
        var m = Regex.IsMatch(password[..^5], pattern);
        var len = m ? 4 : 5;
        var lastChars = password[^len..];
        
        if (len == 4)
        {
            var firstPair = lastChars[..2];
            if (firstPair.Distinct().Count() == 2)
            {
                firstPair = new string(firstPair.Max(), 2);
                return password[..^len] + firstPair + "aa";
            }
            var secondPair = lastChars[2..];
            if (secondPair.Distinct().Count() == 1)
            {
                secondPair = new string(Increment(secondPair[0]), 2);
                return password[..^len] + firstPair + secondPair;
            }

            secondPair = new string(secondPair.Max(), 2);
            return password[..^len] + firstPair + secondPair;
        }
        
        var existing = password[^len];
        var next = password[^(len - 1)];
        if (freeFrom >= 5)
        {
            return password[..^5] + "aabcc";
        }
        var first = (char)Math.Max(existing, next);
        if (existing == next) first = Increment(first);
        if (first > 'x')
        {
            first = 'a';
            password = Increment(password, password.Length - 6);
        }
        var second = (char)(first + 1);
        var third = (char)(second + 1);
        return password[..^len] + first + first + second + third + third;
    }

    private static char Increment(char c)
    {
        var next = (char)(c + 1);
        if (next > 'z') next = 'a';
        if (next is 'o' or 'i' or 'l') next = Increment(next);
        return next;
    }
    
    private static string Increment(string input, int pos)
    {
        char next;
        do
        {
            var current = input[pos];
            next = (char)(current + 1);
            if (next is 'i' or 'o' or 'l') next = (char)(next + 1);
            if (next > 'z') next = 'a';
            input = input[..pos] + next + input[(pos + 1)..];
            pos--;
        } while (next == 'a' && pos >= 0);

        if (next == 'a') return 'a' + input;
        return input;
    }
}