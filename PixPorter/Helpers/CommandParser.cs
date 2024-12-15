public class CommandParser
{
	public Command Parse(string input)
	{
		if (input.StartsWith("\"") && input.EndsWith("\""))
		{
			input = input.Substring(1, input.Length - 2);
		}

		if (input == "exit" || input == "q" || input == "quit")
		{
			return new Command(Constants.Commands.Exit, []);
		}

		if (input == "help")
		{
			return new Command(Constants.Commands.Help, []);
		}

		if (input.StartsWith("cd "))
		{
			string path = input.Substring(3).Trim();
			return new Command(Constants.Commands.ChangeDirectory, [path]);
		}

		var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		string? formatFlag = parts.FirstOrDefault(p =>
			p == "-png" || p == "-jpg" || p == "-jpeg" || p == "-webp");

		if (parts.Contains("-ca"))
		{
			return new Command(Constants.Commands.ConvertAll,
				Array.Empty<string>(),
				MapFormatFlag(formatFlag));
		}

		var potentialPaths = parts.Where(p => !p.StartsWith("-")).ToList();
		foreach (var potentialPath in potentialPaths)
		{
			string relativePath = Path.Combine(Directory.GetCurrentDirectory(), potentialPath);
			string[] checkPaths = [potentialPath, relativePath];

			string? resolvedDirectoryPath = checkPaths.FirstOrDefault(Directory.Exists);
			if (resolvedDirectoryPath != null)
			{
				return new Command(Constants.Commands.ConvertAll,
					[resolvedDirectoryPath],
					MapFormatFlag(formatFlag));
			}

			string? resolvedFilePath = checkPaths.FirstOrDefault(File.Exists);
			if (resolvedFilePath != null)
			{
				return new Command(Constants.Commands.ConvertFile,
					[resolvedFilePath],
					MapFormatFlag(formatFlag));
			}
		}

		throw new CommandException("Invalid command or path.");
	}

	private static string? MapFormatFlag(string? flag)
	{
		return flag switch
		{
			Constants.Flags.Png => Constants.FileFormats.Png,
			Constants.Flags.Jpg => Constants.FileFormats.Jpg,
			Constants.Flags.Jpeg => Constants.FileFormats.Jpeg,
			Constants.Flags.Webp => Constants.FileFormats.Webp,
			_ => null
		};
	}
}