namespace aoc_dotnet;

public class Option(string name, string? shortName = null, bool expectsValue = false, string defaultValue = "")
{
    private string _value = "";
    
    public string Name => name;
    public string? ShortName => shortName;
    public bool ExpectsValue => expectsValue;
    public string Value => _value ?? defaultValue;
    
    public void AppendValue(string append)
    {
        _value += append;
    }
}