public class CommandParser
{
	public Command Parse(string input)
	{
		input = input.Trim();
		var potentialQuoationMarksRemoved = string.Empty;

		foreach (char c in input)
		{
			if (c != '"')
			{
				potentialQuoationMarksRemoved += c;
			}
		}

		input = potentialQuoationMarksRemoved;

		if (input is Constants.Q or Constants.Quit or Constants.Exit)
		{
			return new Command(Constants.Quit, []);
		}

		if (input is Constants.Help)
		{
			return new Command(Constants.Help, []);
		}

		if (input.StartsWith(Constants.ChangeDirectory))
		{
			string path = input[Constants.ChangeDirectory.Length..].Trim();
			return new Command(Constants.ChangeDirectory, [path]);
		}

		string? filePath = ExtractFilePath(input, [.. Constants.SupportedFileFormats]);

		if (filePath != null)
		{
			string remainingInput = input[(input.IndexOf(filePath) + filePath.Length)..].Trim();

			if (string.IsNullOrWhiteSpace(remainingInput))
			{
				return new Command(Constants.ConvertFile, [filePath], null);
			}

			var remainingParts = remainingInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);

			string? formatFlag = remainingParts.FirstOrDefault(p =>
				p is Constants.PngFlag
					or Constants.JpgFlag
					or Constants.JpegFlag
					or Constants.WebpFlag
					or Constants.GifFlag
					or Constants.TiffFlag
					or Constants.BmpFlag);

			if (remainingParts.Contains(Constants.ConvertAll))
			{
				return new Command(Constants.ConvertAll, [filePath], MapFormatFlag(formatFlag));
			}
			else
			{
				return new Command(Constants.ConvertFile, [filePath], MapFormatFlag(formatFlag));
			}
		}

		var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		string? formatFlagGeneral = parts.FirstOrDefault(p =>
				p is Constants.PngFlag
					or Constants.JpgFlag
					or Constants.JpegFlag
					or Constants.WebpFlag
					or Constants.WebpFlag
					or Constants.GifFlag
					or Constants.TiffFlag
					or Constants.BmpFlag);

		if (parts.Contains(Constants.ConvertAll))
		{
			return new Command(Constants.ConvertAll, [], MapFormatFlag(formatFlagGeneral));
		}

		if (parts.Contains(formatFlagGeneral) && Directory.GetCurrentDirectory().Any())
		{
			return new Command(Constants.ConvertAll, [], MapFormatFlag(formatFlagGeneral));
		}

		var potentialPaths = parts.Where(p => !p.StartsWith("-")).ToList();
		foreach (var potentialPath in potentialPaths)
		{
			string relativePath = Path.Combine(Directory.GetCurrentDirectory(), potentialPath);
			string[] checkPaths = [potentialPath, relativePath];

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
			Constants.GifFlag => Constants.GifFileFormat,
			Constants.TiffFlag => Constants.TiffFileFormat,
			Constants.BmpFlag => Constants.BmpFileFormat,
			_ => null
		};
	}
}
