public class CommandParser
{
	public Command Parse(string input)
	{
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

		// Handle -ca (convert all)
		if (parts.Contains("-ca"))
		{
			string? targetFormat = parts.FirstOrDefault(p =>
				p == "-png" || p == "-jpg" || p == "-jpeg" || p == "-webp");

			return new Command(Constants.Commands.ConvertAll,
				Array.Empty<string>(),
				MapFormatFlag(targetFormat));
		}

		// Handle single file conversion
		var formatFlag = parts.FirstOrDefault(p =>
			p == "-png" || p == "-jpg" || p == "-jpeg" || p == "-webp");

		var potentialFiles = parts.Where(p => !p.StartsWith("-")).ToList();
		string? resolvedFilePath = null;

		foreach (var potentialFile in potentialFiles)
		{
			string relativePath = Path.Combine(Directory.GetCurrentDirectory(), potentialFile);

			string[] checkPaths = [potentialFile, relativePath];

			resolvedFilePath = checkPaths.FirstOrDefault(File.Exists);

			if (resolvedFilePath != null)
			{
				break;
			}
		}

		// Single file conversion
		if (resolvedFilePath != null)
		{
			return new Command(Constants.Commands.ConvertFile,
				[resolvedFilePath],
				MapFormatFlag(formatFlag));
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
