namespace aoc_dotnet.Year2024.Day21;

public class Solver: SolverInterface
{
    public string Part1(string[] input)
    {
        var total = (
            from code in input 
            let length = Translate(code, 2)
            let numericPart = int.Parse(code[..^1]) 
            select length * numericPart
        ).Sum();

        return ""+total;
    }

    public string Part2(string[] input)
    {
        var total = (
            from code in input 
            let length = Translate(code, 25) 
            let numericPart = long.Parse(code[..^1]) 
            select length * numericPart
        ).Sum();

        return ""+total;
    }

    private long Translate(string code, int directionals)
    {
        var numeric = "789\n456\n123\n 0A".Split("\n");
        var directional = new Keypad(" ^A\n<v>".Split("\n"));
        var outp = new Keypad(numeric).GetCommandString(code);
        return directional.GetCommandLength(outp, directionals);
    }
}