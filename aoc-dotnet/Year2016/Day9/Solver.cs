using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2016.Day9;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        return "" + Decompress(input[0], 1);
    }

    public string Part2(string[] input)
    {
        return "" + Decompress(input[0], 2);
    }

    private static long Decompress(string compressed, int version)
    {
        var output = 0L;
        while (compressed.Length > 0)
        {
            var nextMarker = Regex.Match(compressed, @"\(([0-9]+)x([0-9]+)\)");
            if (!nextMarker.Success) return output + compressed.Length;
            output += nextMarker.Index;
            var markerLength = int.Parse(nextMarker.Groups[1].Value);
            var markerRepeats = int.Parse(nextMarker.Groups[2].Value);
            var compressedData = compressed.Substring(nextMarker.Index + nextMarker.Value.Length, markerLength);
            output += (version == 2 ? Decompress(compressedData, version) : compressedData.Length) * markerRepeats;
            compressed = compressed.Substring(nextMarker.Index + nextMarker.Value.Length + markerLength);
        }

        return output;
    }
}