namespace aoc_dotnet.Year2016.Day16;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return GetChecksum(input[0], 272);
    }

    public string Part2(string[] input)
    {
        return GetChecksum(input[0], 35651584);
    }

    private string GetChecksum(string a, int length)
    {
        while (a.Length < length)
        {
            var b = new string(a.Reverse().Select(c => c == '1' ? '0' : '1').ToArray());
            a = a + "0" + b;
        }

        a = a[..length];
        do
        {
            a = new string(Enumerable.Range(0, a.Length / 2)
                .Select(i => a.Substring(i * 2, 2) is "11" or "00" ? '1' : '0')
                .ToArray()
            );
        } while (a.Length % 2 == 0);

        return a;
    }
}