
namespace aoc_dotnet.Year2025.Day2;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var a = input[0]
            .Split(',')
            .Select(r => r.Split('-').Select(long.Parse).ToList())
            .Sum(r =>
            {
                var c = 0L;
                for (var i = r[0]; i <= r[1]; i++)
                {
                    if (IsInvalid(i)) c += i;
                }

                return c;
            });

        return "" + a;
    }

    public string Part2(string[] input)
    {
        var a = input[0]
            .Split(',')
            .Select(r => r.Split('-').Select(long.Parse).ToList())
            .Sum(r =>
            {
                var c = 0L;
                for (var i = r[0]; i <= r[1]; i++)
                {
                    if (IsInvalid(i, -1)) c += i;
                }

                return c;
            });

        return "" + a;
    }
    
    private bool IsInvalid(long n, int maxGroups = 2)
    {
        var str = "" + n;
        if (maxGroups < 0) maxGroups = str.Length;
        return Enumerable.Range(2, maxGroups - 1).Any(g =>
        {
            if (str.Length % g != 0) return false;
            var gLen = str.Length / g;
            return Enumerable
                .Range(0, g)
                .Select(i => str.Substring(i * gLen, gLen))
                .Distinct()
                .Count() == 1;
        });
    }
}