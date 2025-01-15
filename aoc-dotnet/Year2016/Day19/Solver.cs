namespace aoc_dotnet.Year2016.Day19;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var list = new LinkedList<int>();
        for (var i = 0; i < int.Parse(input[0]); i++) list.AddLast(i + 1);

        while (list.Count > 1)
        {
            list.AddLast(list.First());
            list.RemoveFirst();
            list.RemoveFirst();
        }

        return "" + list.First();
    }

    public string Part2(string[] input)
    {
        var front = new LinkedList<int>();
        var back = new LinkedList<int>();
        var elfCount = int.Parse(input[0]);
        var halfway = (int)Math.Floor((double)elfCount / 2);
        for (var i = 1; i <= halfway; i++) front.AddLast(i);
        for (var i = halfway + 1; i <= elfCount; i++) back.AddLast(i);
        var isOdd = elfCount % 2 != 0;
        while (front.Count > 0 && back.Count > 0)
        {
            back.AddLast(front.First());
            front.RemoveFirst();
            back.RemoveFirst();
            if (isOdd)
            {
                front.AddLast(back.First());
                back.RemoveFirst();
            }
            isOdd = !isOdd;
        }

        return ""+back.First();
    }
}