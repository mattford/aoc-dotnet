namespace aoc_dotnet.Year2025.Day1;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var d = 50;
        const int maxD = 100;
        var a = 0;
        foreach (var line in input)
        {
            var v = line[0] == 'R' ? 1 : -1;
            if (!int.TryParse(line[1..], out var c)) throw new Exception("Invalid number detected!");
            d += (v * c);
            d %= maxD;
            if (d < 0) d += maxD;
            if (d == 0) a++;
        }

        return "" + a;
    }

    public string Part2(string[] input)
    {
        var d = 50;
        const int maxD = 100;
        var a = 0;
        foreach (var line in input)
        {
            var v = line[0] == 'R' ? 1 : -1;
            if (!int.TryParse(line[1..], out var c)) throw new Exception("Invalid number detected!");
            var distToZero = v == 1 ? maxD - d : d;
            if (distToZero > 0 && c >= distToZero) a++;
            a += (c - distToZero) / maxD;
            
            d += (v * c);
            d %= maxD;
            if (d < 0) d += maxD;
        }

        return "" + a;
    }
}