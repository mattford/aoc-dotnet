namespace aoc_dotnet.Year2025.Day6;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var nums = input.Take(input.Length - 1).Select(line =>
            line.Split(" ").Where(s => !string.IsNullOrEmpty(s)).Select(long.Parse).ToList()).ToList();
        var ops = input.Last().Split(" ").Where(s => !string.IsNullOrEmpty(s)).ToList();
        var result = 0L;
        for (var i = 0; i < nums[0].Count; i++)
        {
            var ns = nums.Select(n => n[i]).ToList();
            result += GetResult(ops[i][0], ns);
        }

        return "" + result;
    }

    public string Part2(string[] input)
    {
        var numbers = input[..^1].ToList();
        var ops = input.Last();
        var op = ' ';
        var ns = new List<long>();
        var result = 0L;
        for (var col = 0; col < input[0].Length; col++)
        {
            if (ops[col] != ' ') op = ops[col];
            var str = string.Join("", numbers.Select(n => n[col]).Where(n => n != ' '));
            if (string.IsNullOrEmpty(str))
            {
                result += GetResult(op, ns);
                ns.Clear();
                continue;
            }
            var digits = int.Parse(str);
            ns.Add(digits);
        }

        result += GetResult(op, ns);
        

        return "" + result;
    }

    private long GetResult(char op, List<long> ns)
    {
        switch (op)
        {
            case '*':
                return ns.Aggregate(1L, (a, c)  => a * c);
            case '+':
                return ns.Sum();
            default:
                throw new Exception("Unknown operator: " + op);
        }
    }
}