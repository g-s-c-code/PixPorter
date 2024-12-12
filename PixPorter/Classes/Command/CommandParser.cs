public class CommandParser
{
	public Command Parse(string input)
	{
		if (string.IsNullOrWhiteSpace(input))
			throw new CommandException("No command entered.");

		// Check for CD command first
		if (input.StartsWith("cd "))
		{
			string path = input.Substring(3).Trim();
			return new Command("cd", new[] { path });
		}

		// Split input into parts
		var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		// Supported image extensions
		string[] supportedExtensions = new[] { ".png", ".jpg", ".jpeg", ".webp", ".bmp", ".gif", ".tiff" };

		// Handle convert-all with optional format
		if (parts.Contains("-ca"))
		{
			string? targetFormat = parts.FirstOrDefault(p =>
				p == "-png" || p == "-jpg" || p == "-jpeg" || p == "-webp");

			return new Command("convert-all",
				Array.Empty<string>(),
				MapFormatFlag(targetFormat));
		}

		// Find format flag
		var formatFlag = parts.FirstOrDefault(p =>
			p == "-png" || p == "-jpg" || p == "-jpeg" || p == "-webp");

		// Find potential file (try current directory and full path)
		var potentialFiles = parts.Where(p => !p.StartsWith("-")).ToList();
		string? resolvedFilePath = null;

		foreach (var potentialFile in potentialFiles)
		{
			// Check relative to current directory
			string relativePath = Path.Combine(Directory.GetCurrentDirectory(), potentialFile);

			// Check full path
			string[] checkPaths = new[]
			{
				potentialFile,  // As-is
                relativePath    // Relative to current directory
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

		// File with format
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