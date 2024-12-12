public class Command
{
    public string Name { get; }
    public List<string> Arguments { get; }
    public string? TargetFormat { get; }

    public Command(string name, IEnumerable<string> arguments, string? targetFormat = null)
    {
        Name = name;
        Arguments = new List<string>(arguments);
        TargetFormat = targetFormat;
    }
}
