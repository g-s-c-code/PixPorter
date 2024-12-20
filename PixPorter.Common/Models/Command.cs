public record Command(string Name, List<string> Arguments, string? TargetFormat, int? Quality)
{
	public Command(string name, IEnumerable<string> arguments, string? targetFormat = null, int? quality = null) : this(name, new List<string>(arguments), targetFormat, quality)
	{
	}
}
