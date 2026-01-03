using System.Numerics;

namespace aoc_dotnet.Year2025.Day7;

public class Solver : SolverInterface
{
    public string Part1(string[] input)
    {
        var beams = new List<Complex>();
        var splitters = new List<Complex>();
        foreach (var y in Enumerable.Range(0, input.Length))
        {
            foreach (var x in Enumerable.Range(0, input[y].Length))
            {
                if (input[y][x] is 'S') beams.Add(Complex.ImaginaryOne * y + x);
                if (input[y][x] is '^') splitters.Add(Complex.ImaginaryOne * y + x);
            }
        }

        var visited = new HashSet<Complex>();
        var splits = 0;
        while (beams.Count > 0)
        {
            var nextBeams = new List<Complex>();
            foreach (var beam in beams)
            {
                var current = beam;
                while (current.Imaginary < input.Length && visited.Add(current))
                {
                    if (splitters.Contains(current))
                    {
                        nextBeams.AddRange(new List<Complex>
                        {
                            current - 1,
                            current + 1
                        }.Where(n => !visited.Contains(n)));
                        splits++;
                        break;
                    }

                    current += Complex.ImaginaryOne;
                }
            }

            beams = nextBeams;
        }

        return "" + splits;
    }

    public string Part2(string[] input)
    {
        var beams = new List<Complex>();
        var splitters = new List<Complex>();
        foreach (var y in Enumerable.Range(0, input.Length))
        {
            foreach (var x in Enumerable.Range(0, input[y].Length))
            {
                if (input[y][x] is 'S') beams.Add(Complex.ImaginaryOne * y + x);
                if (input[y][x] is '^') splitters.Add(Complex.ImaginaryOne * y + x);
            }
        }

        return "" + QuantumTachyon(beams[0], splitters, []);
    }

    private long QuantumTachyon(Complex beam, List<Complex> splitters, Dictionary<Complex, long> cache)
    {
        if (cache.TryGetValue(beam, out var tachyon)) return tachyon;
        var maxY = splitters.Max(s => s.Imaginary);
        var routes = 0L;

        var current = beam;
        while (current.Imaginary <= maxY)
        {
            if (splitters.Contains(current))
            {
                var nexts = new List<Complex>
                {
                    current - 1,
                    current + 1
                };
                var thisRoutes = 0L;
                foreach (var n in nexts) thisRoutes += QuantumTachyon(n, splitters, cache);
                cache.Add(beam, thisRoutes);
                return thisRoutes;
            }

            current += Complex.ImaginaryOne;
        }

        routes++;
        cache.Add(beam, routes);
        return routes;
    }
}