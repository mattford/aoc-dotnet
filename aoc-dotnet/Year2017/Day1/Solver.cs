namespace aoc_dotnet.Year2017.Day1;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var captcha = input[0];
        var t = 0;
        if (captcha[0] == captcha[^1]) t += int.Parse(""+captcha[0]);
        for (var i = 0; i < captcha.Length - 1; i++)
        {
            if (captcha[i] == captcha[i + 1]) t += int.Parse(""+captcha[i]);
        }

        return "" + t;
    }

    public string Part2(string[] input)
    {
        var captcha = input[0];
        var t = 0;
        var h = captcha.Length / 2;
        for (var i = 0; i < captcha.Length; i++)
        {
            var o = i + h;
            if (o > captcha.Length -1) o -= captcha.Length;
            if (captcha[i] == captcha[o]) t += int.Parse(""+captcha[i]);
        }

        return "" + t;
    }
}