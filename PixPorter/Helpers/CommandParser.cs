public class CommandParser
{
	private const string QuotationMark = "\"";
	private const string Exit = "exit";
	private const string Q = "q";
	private const string Quit = "quit";
	private const string Help = "help";
	private const string ChangeDirectory = "cd ";

	public Command Parse(string input)
	{
		if (input.StartsWith(QuotationMark) && input.EndsWith(QuotationMark))
		{
			input = input[1..^1];
		}

		if (input == Exit || input == Q || input == Quit)
		{
			return new Command(Constants.Exit, []);
		}

		if (input == Help)
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

		if (parts.Contains("-ca"))
		{
			return new Command(Constants.ConvertAllFlag,
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
				return new Command(Constants.ConvertAllFlag,
					[resolvedDirectoryPath],
					MapFormatFlag(formatFlag));
			}

			string? resolvedFilePath = checkPaths.FirstOrDefault(File.Exists);
			if (resolvedFilePath != null)
			{
				return new Command(Constants.ConvertFileFlag,
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