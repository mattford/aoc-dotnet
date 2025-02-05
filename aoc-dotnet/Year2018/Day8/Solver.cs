namespace aoc_dotnet.Year2018.Day8;

public class Solver : SolverInterface
{
    public string Part1(string[] input)
    {
        var ints = input[0].Split(' ').Select(int.Parse).ToArray();
        return ""+ParseNode(ints).Item1;
    }

    public string Part2(string[] input)
    {
        var ints = input[0].Split(' ').Select(int.Parse).ToArray();
        return ""+ParseNode(ints).Item2;
    }

    private (int, int, int[]) ParseNode(int[] input)
    {
        var metadataTotal = 0;
        var nodeValueTotal = 0;
        var cnodes = input[0];
        var mentries = input[1];
        input = input[2..];

        var cnodeValues = new List<int>();
        foreach (var _ in Enumerable.Range(0, cnodes))
        {
            (var nm, var nv, input) = ParseNode(input);
            cnodeValues.Add(nv);
            metadataTotal += nm;
        }
        if (cnodes > 0)
        {
            nodeValueTotal += input[..mentries].Where(m => m > 0 && m <= cnodeValues.Count).Select(m => cnodeValues[m-1]).Sum();
        }
        else
        {
            nodeValueTotal += input[..mentries].Sum();
        }

        metadataTotal += input[..mentries].Sum();
        input = input.Skip(mentries).ToArray();

        return (metadataTotal, nodeValueTotal, input);
    }
}