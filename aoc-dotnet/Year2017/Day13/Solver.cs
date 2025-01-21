namespace aoc_dotnet.Year2017.Day13;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var layers = input.Select(line => line.Split(": ").Select(int.Parse).ToArray()).ToArray();
        var severity = layers.Sum(layer => layer[0] % (2 * layer[1] - 2) == 0 ? layer[0] * layer[1] : 0);
        return "" + severity;
    }

    public string Part2(string[] input)
    {
        var layers = input.Select(line => line.Split(": ").Select(int.Parse).ToArray()).ToArray();
        var i = 1;
        while (true)
        {
            if (layers.All(layer => (layer[0] + i) % (2 * layer[1] - 2) != 0)) break;
            i++;
        }

        return "" + i;
    }
}