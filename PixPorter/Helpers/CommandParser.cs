public class CommandParser
{
	public Command Parse(string input)
	{
		if (input.StartsWith(Constants.SingleQuotationMark) && input.EndsWith(Constants.SingleQuotationMark))
		{
			input = input[1..^1];
		}

		if (input == Constants.Exit || input == Constants.Q || input == Constants.Quit)
		{
			return new Command(Constants.Quit, []);
		}

		if (input == Constants.Help)
		{
			return new Command(Constants.Help, []);
		}

		if (input.StartsWith(Constants.ChangeDirectory))
		{
			string path = input.Substring(Constants.ChangeDirectory.Length).Trim();
			return new Command(Constants.ChangeDirectory, [path]);
		}

		string[] validExtensions = { ".png", ".jpg", ".jpeg", ".webp" };
		string? filePath = ExtractFilePath(input, validExtensions);

		if (filePath != null)
		{
			string remainingInput = input.Substring(input.IndexOf(filePath) + filePath.Length).Trim();

			if (string.IsNullOrWhiteSpace(remainingInput))
			{
				return new Command(Constants.ConvertFile, [filePath], null);
			}

			var remainingParts = remainingInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			string? formatFlag = remainingParts.FirstOrDefault(p =>
				p == "--png" || p == "--jpg" || p == "--jpeg" || p == "--webp");

			if (remainingParts.Contains("--ca"))
			{
				return new Command(Constants.ConvertAll, [filePath], MapFormatFlag(formatFlag));
			}
		}

		var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		string? formatFlagGeneral = parts.FirstOrDefault(p =>
			p == "--png" || p == "--jpg" || p == "--jpeg" || p == "--webp");

		if (parts.Contains("--ca"))
		{
			return new Command(Constants.ConvertAll, [], MapFormatFlag(formatFlagGeneral));
		}

		var potentialPaths = parts.Where(p => !p.StartsWith("-")).ToList();
		foreach (var potentialPath in potentialPaths)
		{
			string relativePath = Path.Combine(Directory.GetCurrentDirectory(), potentialPath);
			string[] checkPaths = { potentialPath, relativePath };

			string? resolvedDirectoryPath = checkPaths.FirstOrDefault(Directory.Exists);
			if (resolvedDirectoryPath != null)
			{
				return new Command(Constants.ConvertAll,
					[resolvedDirectoryPath],
					MapFormatFlag(formatFlagGeneral));
			}

			string? resolvedFilePath = checkPaths.FirstOrDefault(File.Exists);
			if (resolvedFilePath != null)
			{
				return new Command(Constants.ConvertFile,
					[resolvedFilePath],
					MapFormatFlag(formatFlagGeneral));
			}
		}

		throw new CommandException("Invalid command or path.");
	}

	private static string? ExtractFilePath(string input, string[] validExtensions)
	{
		foreach (string ext in validExtensions)
		{
			int extIndex = input.IndexOf(ext, StringComparison.OrdinalIgnoreCase);
			if (extIndex > 0)
			{
				int endIndex = extIndex + ext.Length;
				return input[..endIndex].Trim();
			}
		}
		return null;
	}

	private static string? MapFormatFlag(string? flag)
	{
		return flag switch
		{
			Constants.PngFlag => Constants.PngFileFormat,
			Constants.JpgFlag => Constants.JpgFileFormat,
			Constants.JpegFlag => Constants.JpegFileFormat,
			Constants.WebpFlag => Constants.WebpFileFormat,
			_ => null
		};
	}
}
