using static Constants;

public class CommandParser
{
	public Command Parse(string input)
	{
		if (string.IsNullOrWhiteSpace(input))
			throw new CommandException("No command entered.");

		if (input == "q" || input == "quit")
		{
			return new Command(Commands.Quit, Array.Empty<string>());
		}

		if (input.StartsWith("cd "))
		{
			string path = input.Substring(3).Trim();
			return new Command(Commands.ChangeDirectory, new[] { path });
		}

		var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		string[] supportedExtensions = new[] { ".png", ".jpg", ".jpeg", ".webp" }; // ".bmp", ".gif", ".tiff"

		if (parts.Contains("-ca"))
		{
			string? targetFormat = parts.FirstOrDefault(p =>
				p == "-png" || p == "-jpg" || p == "-jpeg" || p == "-webp");

			return new Command("convert-all",
				Array.Empty<string>(),
				MapFormatFlag(targetFormat));
		}

		var formatFlag = parts.FirstOrDefault(p =>
			p == "-png" || p == "-jpg" || p == "-jpeg" || p == "-webp");

		var potentialFiles = parts.Where(p => !p.StartsWith("-")).ToList();
		string? resolvedFilePath = null;

		foreach (var potentialFile in potentialFiles)
		{
			string relativePath = Path.Combine(Directory.GetCurrentDirectory(), potentialFile);

			string[] checkPaths = new[]
			{
				potentialFile,
				relativePath
			};

			resolvedFilePath = checkPaths.FirstOrDefault(File.Exists);

			if (resolvedFilePath != null)
			{
				break;
			}
		}

		// Pure format flag (implies convert all)
		if (formatFlag != null && resolvedFilePath == null)
		{
			return new Command("convert-all",
				Array.Empty<string>(),
				MapFormatFlag(formatFlag));
		}

		if (resolvedFilePath != null && formatFlag != null)
		{
			return new Command("convert",
				new[] { resolvedFilePath },
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