using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2015.Day14;

using Reindeer = (int, int, int);

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var reindeer = GetReindeer(input);
        return ""+reindeer.Max(deer => DistAfter(deer, 2503));
    }

    public string Part2(string[] input)
    {
        var reindeer = GetReindeer(input);
        var points = new int[reindeer.Length];
        for (var t = 1; t <= 2503; t++)
        {
            var dists = reindeer.Select(r => DistAfter(r, t)).ToList();
            var max = dists.Max();
            for (var i = 0; i < dists.Count; i++)
            {
                if (dists[i] == max) points[i]++;
            }
        }
        return ""+points.Max();
    }

    private int DistAfter(Reindeer reindeer, int time)
    {
        var (speed, running, resting) = reindeer;
        var cycleTime = running + resting;
        var dist = (int)Math.Floor((double)time / cycleTime) * speed * running;
        if (time % cycleTime > 0)
        {
            dist += Math.Min(running, time % cycleTime) * speed;
        }

        return dist;
    }

    private Reindeer[] GetReindeer(string[] input)
    {
        return (
            from line in input
            let matches = Regex.Matches(line, @"(\d+)")
            let ints = matches.Select(x => int.Parse(x.Value)).ToArray()
            select (ints[0], ints[1], ints[2])
        ).ToArray();
    }
}