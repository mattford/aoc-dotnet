namespace aoc_dotnet.Year2017.Day17;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var j = int.Parse(input[0]);
        var list = new LinkedList<int>();
        list.AddLast(0);
        var nextVal = 1;
        var pos = list.First;
        while (nextVal <= 2017)
        {
            for (var k = 0; k < j; k++) pos = pos?.Next ?? list.First;
            if (pos == null) break;
            list.AddAfter(pos, nextVal);
            pos = pos.Next;
            nextVal++;
        }
        return "" + (pos?.Next ?? list.First)?.Value;
    }

    public string Part2(string[] input)
    {
        var j = int.Parse(input[0]);
        var nextVal = 1;
        var pos = 0;
        var result = 0;
        while (nextVal <= 50_000_000)
        {
            pos = (pos + j) % nextVal;
            pos++;
            if (pos == 1) result = nextVal;
            nextVal++;
        }
        return "" + result;
    }
}