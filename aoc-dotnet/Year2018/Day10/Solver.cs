using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2018.Day10;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + FindMessage(input).Item2;
    }

    public string Part2(string[] input)
    {
        return "" + FindMessage(input).Item1;
    }

    private (int, string) FindMessage(string[] input)
    {
        var points = input.Select(line => Regex.Matches(line, @"[\-\d]+").Select(m => int.Parse(m.Value)).ToArray())
            .ToList();

        int minY;
        int maxY;
        var i = 0;
        do
        {
            points = points.Select(p =>
            {
                p[0] += p[2];
                p[1] += p[3];
                return p;
            }).ToList();
            minY = points.Min(p => p[1]);
            maxY = points.Max(p => p[1]);
            i++;
        } while (maxY - minY > 9);

        // Only the letters in my input, the elves didn't give me their font definitions.
        var letters = new Dictionary<string, char>()
        {
            ["##### #    ##    ##    ###### #  #  #   # #   # #    ##    #"] = 'R',
            ["  ##   #  # #    ##    ##    ########    ##    ##    ##    #"] = 'A',
            ["#    ###   ###   ## #  ## #  ##  # ##  # ##   ###   ###    #"] = 'N',
            ["######     #     #    #    #    #    #    #     #     ######"] = 'Z',
            ["#     #     #     #     #     #     #     #     #     ######"] = 'L',
            [" #### #    ##     #     #     #     #     #     #    # #### "] = 'C',
        };
        
        var minX = points.Min(p => p[0]);
        var maxX = points.Max(p => p[0]);
        var message = "";
        for (var x = minX; x <= maxX; x += 8)
        {
            var pattern = "";
            for (var y = minY; y <= maxY; y++)
            {
                foreach (var x2 in Enumerable.Range(x, 6))
                {
                    pattern += points.Any(p => p[0] == x2 && p[1] == y) ? '#' : ' ';
                }
            }

            message += letters[pattern];
        }
        
        return (i, message);
    }
}