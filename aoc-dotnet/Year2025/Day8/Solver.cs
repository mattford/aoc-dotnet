namespace aoc_dotnet.Year2025.Day8;

public class Solver : SolverInterface
{
    public string Part1(string[] input)
    {
        var boxes = input.Select(line => line.Split(",").Select(long.Parse).ToList()).ToList();
        var dists = new List<(string, string, double)>();
        while (boxes.Count > 0)
        {
            var first = boxes.First();
            boxes =  boxes.Skip(1).ToList();
            foreach (var other in boxes)
            {
                dists.Add((string.Join(",", first), string.Join(",", other), Dist(first, other)));
            }
        }

        dists = dists.OrderBy(x => x.Item3).Take(1000).ToList();

        var circuits = new List<List<string>>();
        
        foreach (var (first, other, _) in dists)
        {
            // Console.WriteLine($"Connecting {first} to {other}");
            var circuit = circuits
                .Where(x => x.Contains(first) || x.Contains(other))
                .SelectMany(x => x)
                .ToList();
            circuit.AddRange([first, other]);
            circuits = circuits.Where(x => !x.Contains(first) && !x.Contains(other)).ToList();
            circuits.Add(circuit.Distinct().ToList());
        }
        return "" + circuits.OrderByDescending(c => c.Count).Take(3).Aggregate(1L, (t, c) => t * c.Count);
    }

    public string Part2(string[] input)
    {
        var boxes = input.Select(line => line.Split(",").Select(long.Parse).ToList()).ToList();
        var dists = new List<(string, string, double)>();
        while (boxes.Count > 0)
        {
            var first = boxes.First();
            boxes =  boxes.Skip(1).ToList();
            foreach (var other in boxes)
            {
                dists.Add((string.Join(",", first), string.Join(",", other), Dist(first, other)));
            }
        }

        dists = dists.OrderBy(x => x.Item3).ToList();

        var circuits = new List<List<string>>();
        foreach (var (first, other, _) in dists)
        {
            var circuit = circuits
                .Where(x => x.Contains(first) || x.Contains(other))
                .SelectMany(x => x)
                .ToList();
            circuit.AddRange([first, other]);
            circuits = circuits.Where(x => !x.Contains(first) && !x.Contains(other)).ToList();
            circuits.Add(circuit.Distinct().ToList());
            if (circuit.Distinct().Count() == input.Length)
            {
                return "" + long.Parse(first.Split(",")[0]) * long.Parse(other.Split(",")[0]);
            }
        }
        return "Didn't find solution";

    }

    private double Dist(List<long> a, List<long> b)
    {
        return Math.Sqrt(a.Zip(b).Select(x => Math.Pow(Math.Abs(x.First - x.Second), 2)).Sum());
    }
}