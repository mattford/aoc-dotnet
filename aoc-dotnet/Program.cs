// See https://aka.ms/new-console-template for more information

using System.Net;
using aoc_dotnet;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddCommandLine(args)
    .Build();

var year = Convert.ToInt32(config["year"] ?? "2024");
var day = Convert.ToInt32(config["day"] ?? "0");
Console.WriteLine($"Year: {year} Day: {day}");

for (var i = 1; i <= 25; i++)
{
    if (day > 0 && day != i) continue;
    var module = "aoc_dotnet.Year" + year + ".Day" + i + ".Solver";
    var type = Type.GetType(module);
    if (type == null) continue;
    if (Activator.CreateInstance(type) is not SolverInterface solver) continue;
    var inputPath = "Year" + year + "/Day" + i + "/input.txt";
    if (!File.Exists(inputPath))
    {
        DownloadInput(inputPath, year, i, config["token"]);
    }
    var input = File.ReadAllLines("Year" + year + "/Day" + i + "/input.txt");
    Console.WriteLine(year + "/" + i);
    var part1 = solver.Part1(input);
    Console.WriteLine($"Part 1: {part1}");
    var part2 = solver.Part2(input);
    Console.WriteLine($"Part 2: {part2}");
}

void DownloadInput(string inputPath, int year, int day, string? token)
{
    Console.WriteLine($"Downloading input for {year}/{day}");
    if (token == null)
    {
        Console.WriteLine("No token provided.");
        return;
    }
    var url = "https://adventofcode.com/" + year + "/day/" + day + "/input";
    var request = new HttpRequestMessage(HttpMethod.Get, url);
    var baseAddress = new Uri("https://adventofcode.com");
    var cookieContainer = new CookieContainer();
    using var handler = new HttpClientHandler { CookieContainer = cookieContainer };
    using var httpClient = new HttpClient(handler);
    cookieContainer.Add(baseAddress, new Cookie("session", token));
    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent",
        "github.com/mattford/aoc-dotnet by mrford157@gmail.com");
    var task = httpClient.SendAsync(request)
        .ContinueWith((taskwithmsg) =>
        {
            var response = taskwithmsg.Result;
            response.EnsureSuccessStatusCode();
            var responseTextTask = response.Content.ReadAsStringAsync();
            responseTextTask.Wait();
            File.WriteAllText(inputPath, responseTextTask.Result);
        });
    task.Wait();
}

