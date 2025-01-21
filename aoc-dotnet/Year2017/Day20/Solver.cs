using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2017.Day20;

internal record struct Particle(int id, long x, long y, long z, long vX, long vY, long vZ, long aX, long aY, long aZ);

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var particles = GetParticles(input);
        return "" + particles.OrderBy(p => Math.Abs(p.aX) + Math.Abs(p.aY) + Math.Abs(p.aZ))
            .ThenBy(p => Math.Abs(p.vX) + Math.Abs(p.vY) + Math.Abs(p.vZ))
            .ThenBy(p => Math.Abs(p.x) + Math.Abs(p.y) + Math.Abs(p.z))
            .First().id;
    }

    public string Part2(string[] input)
    {
        var particles = GetParticles(input).ToList();
        // All collisions happen before t = 50 in my input.
        for (var t = 0; t < 50; t++)
        {
            for (var i = 0; i < particles.Count; i++)
            {
                var particle = particles[i];
                particle.vX += particle.aX;
                particle.vY += particle.aY;
                particle.vZ += particle.aZ;
                particle.x += particle.vX;
                particle.y += particle.vY;
                particle.z += particle.vZ;
                particles[i] = particle;
            }

            var collisions = particles.GroupBy(p => (p.x, p.y, p.z)).Where(g => g.Count() > 1);
            foreach (var collision in collisions)
            {
                foreach (var c in collision) particles.Remove(c);
            }
        }

        return "" + particles.Count;
    }

    private Particle[] GetParticles(string[] input)
    {
        return (
            from i in Enumerable.Range(0, input.Length)
            let matches = Regex.Matches(input[i], @"([\-\d]+)").Select(m => long.Parse(m.Value)).ToArray()
            select new Particle(i, matches[0], matches[1], matches[2], matches[3], matches[4], matches[5], matches[6],
                matches[7], matches[8])
        ).ToArray();
    }
}