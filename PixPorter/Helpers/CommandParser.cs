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
			string path = input.Substring(3).Trim();
			return new Command(Constants.ChangeDirectory, [path]);
		}

		var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		string? formatFlag = parts.FirstOrDefault(p =>
			p == "-png" || p == "-jpg" || p == "-jpeg" || p == "-webp");

		if (parts.Contains("ca"))
		{
			return new Command(Constants.ConvertAll,
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
				return new Command(Constants.ConvertAll,
					[resolvedDirectoryPath],
					MapFormatFlag(formatFlag));
			}

			string? resolvedFilePath = checkPaths.FirstOrDefault(File.Exists);
			if (resolvedFilePath != null)
			{
				return new Command(Constants.ConvertFile,
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
			Constants.PngFlag => Constants.PngFileFormat,
			Constants.JpgFlag => Constants.JpgFileFormat,
			Constants.JpegFlag => Constants.JpegFileFormat,
			Constants.WebpFlag => Constants.WebpFileFormat,
			_ => null
		};
	}
}