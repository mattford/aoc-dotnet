namespace aoc_dotnet.Year2024.Day7;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var equations = ParseInput(input);
        var operators = new[]{'*', '+'};
        return "" + equations.Select(e => TryOperators(e, operators) ? e.Item1 : 0).Sum();
    }

    public string Part2(string[] input)
    {
        var equations = ParseInput(input);
        var operators = new[]{'*', '+', '|'};
        return "" + equations.Select(e => TryOperators(e, operators) ? e.Item1 : 0).Sum();
    }

    private bool TryOperators((long, long[]) equation, char[] operators)
    {
        var (target, operands) = equation;
        var current = operands.Take(1).First();
        operands = operands.Skip(1).ToArray();

        if (operands.Length == 0)
        {
            return current == target;
        }
        var operand = operands.Take(1).First();
        operands = operands.Skip(1).ToArray();

        return operators.Any(x =>
        {
            var nextValue = x switch
            {
                '+' => current + operand,
                '*' => current * operand,
                '|' => long.Parse(current + "" + operand),
                _ => current
            };
            var newOperands = new[]{nextValue};
            newOperands = newOperands.Concat(operands).ToArray();
            return TryOperators((target, newOperands), operators);
        });
    }

    private (long, long[])[] ParseInput(string[] input)
    {
        return input.Select(line => line.Split(": "))
            .Select(x => (long.Parse(x[0]), x[1].Split(" ").Select(long.Parse).ToArray()))
            .ToArray();
    }
}