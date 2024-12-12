using static Constants;

public class CommandParser
{
	private readonly string[] _supportedFileExtensions = { ".png", ".jpg", ".jpeg", ".webp" }; // ".bmp", ".gif", ".tiff"

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

			string[] checkPaths = [potentialFile, relativePath];

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