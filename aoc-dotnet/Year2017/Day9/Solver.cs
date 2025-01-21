namespace aoc_dotnet.Year2017.Day9;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return ""+Parse(input[0]).Item1;
    }

    public string Part2(string[] input)
    {
        return ""+Parse(input[0]).Item2;
    }

    private (int, int) Parse(string input)
    {
        var i = 0;
        var groupScore = 0;
        var garbageCount = 0;
        var inGarbage = false;
        var groupDepth = 0;
        while (i < input.Length - 1)
        {
            var c = input[i];
            i++;
            if (c == '!')
            {
                i++;
                continue;
            }

            inGarbage = inGarbage && c != '>';
            if (inGarbage)
            {
                garbageCount++;
                continue;
            }
            if (c == '<')
            {
                inGarbage = true;
                continue;
            }
            
            switch (c)
            {
                case '{':
                    groupDepth++;
                    groupScore += groupDepth;
                    break;
                case '}':
                    if (groupDepth > 0) groupDepth--;
                    break;
            }
        }
        return (groupScore, garbageCount);
    }
}