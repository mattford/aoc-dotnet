using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2016.Day7;

public partial class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var t = (
            from line in input 
            let split = SplitIp(line)
            where split.Item1.Any(l => AbbaPattern().Matches(l).Any(m => m.Value.Distinct().Count() > 1))
            where !split.Item2.Any(l => AbbaPattern().Matches(l).Any(m => m.Value.Distinct().Count() > 1))
            select 1
        ).Count();
        return "" + t;
    }

    public string Part2(string[] input)
    {
        var t = (
            from line in input 
            let split = SplitIp(line)
            let abas = split.Item1.SelectMany(l => AbaPattern().Matches(l).Where(m => m.Groups[1].Value.Distinct().Count() > 1).Select(m => m.Groups[1].Value))
            where abas.Any(aba => split.Item2.Any(ht => ht.Contains("" + aba[1] + aba[0] + aba[1])))
            select 1
        ).Count();
        return "" + t;
    }

    private (string[], string[]) SplitIp(string ip)
    {
        var hypertexts = HypertextPattern().Matches(ip)
            .Select(m => m.Groups[1].Value)
            .ToArray();
        var normals = HypertextPattern().Replace(ip, "|").Split("|");
        return (normals, hypertexts);
    }

    [GeneratedRegex(@"\[([a-z]*)\]")]
    private static partial Regex HypertextPattern();
    
    [GeneratedRegex(@"([a-z])([a-z])\2\1")]
    private static partial Regex AbbaPattern();
    
    [GeneratedRegex(@"(?=(([a-z])[a-z]\2))")]
    private static partial Regex AbaPattern();
}