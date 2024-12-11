public class Command
{
	public string Name { get; }
	public List<string> Arguments { get; }

	public Command(string name, IEnumerable<string> arguments)
	{
		Name = name;
		Arguments = new List<string>(arguments);
	}
}
