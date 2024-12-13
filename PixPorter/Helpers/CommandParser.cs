using static Constants;

public class CommandParser
{
	private readonly string[] _supportedFileExtensions = { ".png", ".jpg", ".jpeg", ".webp" };

	public Command Parse(string input)
	{
		if (input == "q" || input == "quit")
		{
			return new Command(Commands.Quit, Array.Empty<string>());
		}

		if (input.StartsWith("cd "))
		{
			string path = input.Substring(3).Trim();
			return new Command(Commands.ChangeDirectory, [path]);
		}

		var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		// Handle -ca (convert all)
		if (parts.Contains("-ca"))
		{
			string? targetFormat = parts.FirstOrDefault(p =>
				p == "-png" || p == "-jpg" || p == "-jpeg" || p == "-webp");

			return new Command(Commands.ConvertAll,
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
			return new Command(Commands.ConvertFile,
				[resolvedFilePath],
				MapFormatFlag(formatFlag));
		}

		throw new CommandException("Invalid command or path.");
	}

	private string? MapFormatFlag(string? flag)
	{
		return flag switch
		{
			"-png" => ".png",
			"-jpg" => ".jpg",
			"-jpeg" => ".jpeg",
			"-webp" => ".webp",
			_ => null
		};
	}
}
