using Spectre.Console;

public class CommandExecutor
{
	private readonly PixPorterConfig _config;
	private readonly ImageConverter _converter;

	public CommandExecutor(PixPorterConfig config, ImageConverter converter)
	{
		_config = config;
		_converter = converter;
	}

	public void Execute(Command command)
	{
		switch (command.Name)
		{
			case "cd":
				ChangeDirectory(command.Arguments.FirstOrDefault() ?? "");
				break;
			case "convert":
				ConvertFile(command.Arguments.FirstOrDefault() ?? "", command.TargetFormat);
				break;
			case "convert-all":
				ConvertDirectory(command.Arguments.FirstOrDefault() ?? _config.CurrentDirectory, command.TargetFormat);
				break;
			default:
				AnsiConsole.MarkupLine("[red]Unknown command.[/]");
				break;
		}
	}

	private void ChangeDirectory(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			AnsiConsole.WriteLine("Path cannot be empty. Usage: cd [path]");
			return;
		}

		string newPath = Path.IsPathRooted(path)
			? path
			: Path.GetFullPath(Path.Combine(_config.CurrentDirectory, path));

		if (Directory.Exists(newPath))
		{
			_config.CurrentDirectory = newPath;
		}
		else
		{
			AnsiConsole.WriteLine($"Directory not found: {newPath}");
		}
	}

	private void ConvertFile(string path, string? targetFormat)
	{
		if (!File.Exists(path))
		{
			AnsiConsole.WriteLine($"File not found: {path}");
			return;
		}

		try
		{
			_converter.ConvertFile(path, targetFormat);
		}
		catch (Exception ex)
		{
			AnsiConsole.WriteLine($"Failed to convert file: {ex.Message}");
		}
	}

	private void ConvertDirectory(string path, string? targetFormat)
	{
		if (!Directory.Exists(path))
		{
			AnsiConsole.WriteLine($"Directory not found: {path}");
			return;
		}

		try
		{
			_converter.ConvertDirectory(path, targetFormat);
		}
		catch (Exception ex)
		{
			AnsiConsole.WriteLine($"Failed to convert directory: {ex.Message}");
		}
	}
}