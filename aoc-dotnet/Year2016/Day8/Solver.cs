using System.Numerics;
using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2016.Day8;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var lights = GetSwitchedOnLights(input, 6, 50);
        return ""+lights.Count;
    }

    public string Part2(string[] input)
    {
        var lights = GetSwitchedOnLights(input, 6, 50);
        var letters = new Dictionary<string, char>()
        {
            [".##..#..#.#..#.####.#..#.#..#."] = 'A',
            ["####.#....###..#....#....####."] = 'E',
            [".##..#..#.#..#.#..#.#..#..##.."] = 'O',
            ["###..#..#.#..#.###..#.#..#..#."] = 'R',
            [".##..#..#.#....#.##.#..#..###."] = 'G',
            ["###..#..#.#..#.###..#....#...."] = 'P',
            ["#..#.#..#.####.#..#.#..#.#..#."] = 'H',
            ["#...##...#.#.#...#....#....#.."] = 'Y',
        };
        var solution = "";
        for (var x = 0; x < 50; x += 5)
        {
            var pattern = string.Join("",
                Enumerable.Range(0, 6).Select(
                    y => string.Join("", Enumerable.Range(x, 5).Select(x2 => lights.Contains(y * Complex.ImaginaryOne + x2) ? '#' : '.'))));
            if (!letters.TryGetValue(pattern, out var letter))
            {
                // Fallback to manual decoding, since the elves didn't provide examples of all letters in their special font.
                Console.WriteLine("Error: Manual decoding required!");
                Console.WriteLine(pattern);
                Display(lights);
                break;
            }

            solution += letter;
        }

        return solution;
    }

    private void Display(List<Complex> lights)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        for (var y = 0; y < 6; y++)
        {
            for (var x = 0; x < 50; x++)
            {
                Console.Write(lights.Contains(y * Complex.ImaginaryOne + x) ? "#" : " ");
            }
            Console.WriteLine();
        }
    }

    private static List<Complex> GetSwitchedOnLights(string[] input, int maxY, int maxX)
    {
        var lights = new List<Complex>();
        foreach (var line in input)
        {
            if (line.StartsWith("rect"))
            {
                var dims = line.Split(" ")[1].Split("x").Select(int.Parse).ToArray();
                for (var y = 0; y < dims[1]; y++)
                {
                    for (var x = 0; x < dims[0]; x++) lights.Add(Complex.ImaginaryOne * y + x);
                }
                lights = lights.Distinct().ToList();
                continue;
            }

            if (line.StartsWith("rotate"))
            {
                var dims = Regex.Match(line, "(y|x)=([0-9]+) by ([0-9]+)");
                var dir = dims.Groups[1].Value;
                var target = int.Parse(dims.Groups[2].Value);
                var moves = int.Parse(dims.Groups[3].Value);
                lights = lights.Select(light =>
                {
                    if (dir == "y" && (int)light.Imaginary == target)
                    {
                        return light.Imaginary * Complex.ImaginaryOne + (light.Real + moves) % maxX;
                    }

                    if (dir == "x" && (int)light.Real == target)
                    {
                        return (light.Imaginary + moves) % maxY * Complex.ImaginaryOne + light.Real;
                    }

                    return light;
                }).Distinct().ToList();
            }
        } 
        return lights;
    }
}