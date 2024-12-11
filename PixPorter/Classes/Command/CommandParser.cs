public class CommandParser
{
	public Command Parse(string input)
	{
		var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		if (parts.Length == 0)
		{
			throw new CommandException("No command entered.");
		}

		string commandName = parts[0].ToLower();
		var arguments = parts.Skip(1);

		if (IsImagePath(input))
		{
			return new Command("convert", new[] { input });
		}

		return new Command(commandName, arguments);
	}

	private bool IsImagePath(string input)
	{
		string[] supportedExtensions = { ".png", ".jpg", ".jpeg", ".webp" };
		string extension = Path.GetExtension(input).ToLower();

		if (!supportedExtensions.Contains(extension))
			return false;

		return File.Exists(input);
	}
}
