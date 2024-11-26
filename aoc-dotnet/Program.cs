// See https://aka.ms/new-console-template for more information

using System.Collections;
using aoc_dotnet;

var expectedArgs = new ArrayList()
{
    new Option("year", "y", true, "2024"),
    new Option("day", "d", true),
};

var arguments = new ArgumentParser(args, expectedArgs);

Console.WriteLine("Year: " + arguments.Option("year") + " Day: " + arguments.Option("day"));

var year = Convert.ToInt32(arguments.Option("year"));
var day = 0;
if (arguments.Option("day") != null)
{
    day = Convert.ToInt32(arguments.Option("day"));
}

for (var i = 1; i <= 25; i++)
{
    if (day > 0 && day != i) continue;
    var module = "aoc_dotnet.Year" + year + ".Day" + i + ".Solver";
    var type = Type.GetType(module);
    if (type == null) continue;
    if (Activator.CreateInstance(type) is not SolverInterface solver) continue;
    var input = File.ReadAllLines("Year" + year + "/Day" + i + "/input.txt");
    Console.WriteLine(year + "/" + i);
    var part1 = solver.Part1(input);
    Console.WriteLine($"Part 1: {part1}");
    var part2 = solver.Part2(input);
    Console.WriteLine($"Part 2: {part2}");
}

