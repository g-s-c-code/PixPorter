public record Command(string Name, List<string> Arguments, string? TargetFormat)
{
	public Command(string name, IEnumerable<string> arguments, string? targetFormat = null) : this(name, new List<string>(arguments), targetFormat)
	{
	}
}
