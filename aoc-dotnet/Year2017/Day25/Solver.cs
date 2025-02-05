using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace aoc_dotnet.Year2017.Day25;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var (state, iterations, map) = GetStateMap(input);
        var tape = new Dictionary<int, bool>();
        var cursor = 0;
        while (iterations > 0)
        {
            (tape[cursor], var move, state) = map[state][tape.GetValueOrDefault(cursor, false)];
            cursor += move;
            iterations--;
        }

        return ""+tape.Count(x => x.Value);
    }

    public string Part2(string[] input)
    {
        return "Merry Christmas!";
    }

    private (char, long, ImmutableDictionary<char, ImmutableDictionary<bool, (bool, int, char)>>) GetStateMap(string[] input)
    {
        var parts = string.Join("\n", input).Split("\n\n");
        var matches = Regex.Match(parts[0],
            @"Begin in state ([A-Z]+).\nPerform a diagnostic checksum after (\d+) steps.");
        var state = matches.Groups[1].Value[0];
        var iterations = long.Parse(matches.Groups[2].Value);
        var map = new Dictionary<char, ImmutableDictionary<bool, (bool, int, char)>>();
        foreach (var stateDefinition in parts[1..])
        {
            var thisStateMap = new Dictionary<bool, (bool, int, char)>();
            // get the last word on each line since it's the only one which matters.
            var words = stateDefinition.Split('\n').Select(line => line.Trim(':', '.').Split(' ')[^1]).ToArray();
            thisStateMap[false] = (words[2] == "1", words[3] == "left" ? -1 : 1, words[4][0]);
            thisStateMap[true] = (words[6] == "1", words[7] == "left" ? -1 : 1, words[8][0]);
            map[words[0][0]] = thisStateMap.ToImmutableDictionary();
        }
        return (state, iterations, map.ToImmutableDictionary());
    }
}