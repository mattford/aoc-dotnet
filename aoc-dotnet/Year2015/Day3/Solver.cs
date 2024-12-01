namespace aoc_dotnet.Year2015.Day3;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + simulate(input[0]);
    }

    public string Part2(string[] input)
    {
        return "" + simulate(input[0], 2);
    }

    private int simulate(string input, int actorCount = 1)
    {
        var instructions = input.ToCharArray();
        var visited = new HashSet<string>();
        var actors = new int[actorCount][];
        for (var i = 0; i < actorCount; i++)
        {
            actors[i] = [0, 0];
        }
        for (var j = 0; j < instructions.Length; j++)
        {
            var actorIdx = j % actorCount;
            var instruction = instructions[j];
            switch (instruction)
            {
                case '>':
                    actors[actorIdx][0]++;
                    break;
                case '<':
                    actors[actorIdx][0]--;
                    break;
                case '^':
                    actors[actorIdx][1]++;
                    break;
                case 'v':
                    actors[actorIdx][1]--;
                    break;
            }
            visited.Add(string.Join(",", actors[actorIdx]));
        }
        
        return visited.Count;
    }
}