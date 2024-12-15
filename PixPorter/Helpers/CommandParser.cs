public class CommandParser
{
	public Command Parse(string input)
	{
		// Exit command
		if (input == "exit" || input == "q" || input == "quit")
		{
			return new Command(Constants.Commands.Exit, []);
		}

		// Help command
		if (input == "help")
		{
			return new Command(Constants.Commands.Help, []);
		}

		// Change directory command
		if (input.StartsWith("cd "))
		{
			string path = input.Substring(3).Trim();
			return new Command(Constants.Commands.ChangeDirectory, [path]);
		}

		var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		// Handle format flag extraction
		string? formatFlag = parts.FirstOrDefault(p =>
			p == "-png" || p == "-jpg" || p == "-jpeg" || p == "-webp");

		// Handle -ca (convert all) command when in a directory
		if (parts.Contains("-ca"))
		{
			return new Command(Constants.Commands.ConvertAll,
				Array.Empty<string>(),
				MapFormatFlag(formatFlag));
		}

		// Handle single file or directory conversion
		var potentialPaths = parts.Where(p => !p.StartsWith("-")).ToList();
		foreach (var potentialPath in potentialPaths)
		{
			string relativePath = Path.Combine(Directory.GetCurrentDirectory(), potentialPath);
			string[] checkPaths = [potentialPath, relativePath];

			// Check if it's a directory
			string? resolvedDirectoryPath = checkPaths.FirstOrDefault(Directory.Exists);
			if (resolvedDirectoryPath != null)
			{
				return new Command(Constants.Commands.ConvertAll,
					[resolvedDirectoryPath],
					MapFormatFlag(formatFlag));
			}

			// Check if it's a file
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