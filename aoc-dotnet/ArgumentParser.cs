using System.Collections;

namespace aoc_dotnet;

public class ArgumentParser
{
    ArrayList requiredArguments;
    private ArrayList options;
    private string arguments = "";
    
    public ArgumentParser(string[] args, ArrayList requiredArgs)
    {
        requiredArguments = requiredArgs;
        options = new ArrayList();
        Parse(args);
    }
    
    private void Parse (string[] args)
    {
        // --year 2024 --day 1
        // --year=2024 --day=1 --verbose somearg
        Option? lastOption = null;
        foreach (var arg in args)
        {
            if (arg.StartsWith("--"))
            {
                // is --option
                var option = FindOption(arg.Substring(2), requiredArguments);
                if (option != null)
                {
                    options.Add(option);
                    lastOption = option;
                }
                else
                {
                    arguments += arg + " ";
                }
            }
            else if (arg.StartsWith('-') || arg.StartsWith('/'))
            {
                // is -o or /o shorthand
                var option = FindOption(arg.Substring(1), requiredArguments);
                if (option != null)
                {
                    options.Add(option);
                    lastOption = option;
                }
                else
                {
                    arguments += arg + " ";
                }
            }
            else
            {
                // is value or argument
                if (lastOption is { ExpectsValue: true })
                {
                    lastOption.AppendValue(arg);
                }
                else
                {
                    arguments += arg + " ";
                }
            }
        }
    }

    private Option? FindOption(string arg, ArrayList list)
    {
        return list.Cast<Option>().FirstOrDefault(o => o.Name == arg || o.ShortName == arg);
    }

    public string? Option(string name, string? defaultValue = null)
    {
        return FindOption(name, options)?.Value ?? defaultValue;
    }

    public string Arguments()
    {
        return arguments;
    }
}