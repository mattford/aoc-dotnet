using System.Diagnostics;
using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2018.Day9;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var ints = Regex.Matches(input[0], @"\d+").Select(m => int.Parse(m.Value)).ToArray();
        return "" + GetMaxScore(ints[0], ints[1]);
    }

    public string Part2(string[] input)
    {
        var ints = Regex.Matches(input[0], @"\d+").Select(m => int.Parse(m.Value)).ToArray();
        return "" + GetMaxScore(ints[0], ints[1] * 100);
    }

    private static long GetMaxScore(int elves, int maxMarble)
    {
        var scores = Enumerable.Repeat(0L, elves).ToArray();
        var circle = new LinkedList<long>();
        circle.AddFirst(0);
        var current = circle.First;
        Debug.Assert(current != null, nameof(current) + " != null");
        for (var i = 1; i < maxMarble; i++)
        {
            if (i % 23 == 0)
            {
                for (var j = 0; j < 7; j++)
                {
                    Debug.Assert(current != null, nameof(current) + " != null");
                    current = current.Previous ?? circle.Last;
                }
                scores[i % elves] += (current?.Value ?? 0) + i;
                var old = current;
                Debug.Assert(old != null, nameof(old) + " != null");
                current = old.Next;
                circle.Remove(old);
            }
            else
            {
                Debug.Assert(current != null, nameof(current) + " != null");
                current = circle.AddAfter((current.Next ?? circle.First)!, i);
            }
        }
        
        return scores.Max();
    }
}