public class CommandParser
{
	public Command Parse(string input)
	{
		if (string.IsNullOrWhiteSpace(input))
			throw new CommandException("No command entered.");

		if (input == "-ca")
			return new Command("convert-all", Array.Empty<string>());

		if (Path.IsPathRooted(input))
		{
			return Directory.Exists(input)
				? new Command("convert-all", new[] { input })
				: new Command("convert", new[] { input });
		}

		if (input.StartsWith("cd "))
		{
			string path = input.Substring(3).Trim();
			return new Command("cd", new[] { path });
		}

		throw new CommandException("Invalid command or path.");
	}
}