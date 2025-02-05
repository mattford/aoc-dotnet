namespace aoc_dotnet.Year2018.Day5;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return ""+React(input[0]);
    }

    public string Part2(string[] input)
    {
        var min = Enumerable.Range('a', 26).Select(c => React(input[0], (char)c)).Prepend(int.MaxValue).Min();
        return "" + min;
    }
    
    private int React(string polymer, char ignore = '.')
    {
        var last = '.';
        var end = polymer.ToCharArray().ToList();
        var start = new List<char>();
        while (end.Count > 0)
        {
            var c = end.First();
            end.RemoveAt(0);
            if (char.ToLower(c) == char.ToLower(ignore)) continue;
            if (last != c && char.ToUpper(last) == char.ToUpper(c))
            {
                last = '.';
                if (start.Count == 0) continue;
                last = start[^1];
                start.RemoveAt(start.Count - 1);
            }
            else 
            {
                if (last != '.') start.Add(last);
                last = c;
            }
        }

        start.Add(last);

        return start.Count;
    }
}