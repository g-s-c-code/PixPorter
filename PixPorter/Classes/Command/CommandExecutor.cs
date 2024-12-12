using Spectre.Console;
using static Constants;

public class CommandExecutor
{
	private readonly PixPorterConfig _config;
	private readonly ImageConverter _converter;

	public CommandExecutor(PixPorterConfig config, ImageConverter converter)
	{
		_config = config;
		_converter = converter;
	}

	public void ExecuteCommand(Command command)
	{
		switch (command.Name.ToLower())
		{
			case Commands.Quit:
				Environment.Exit(0);
				break;
			case Commands.ChangeDirectory:
				ChangeDirectory(command.Arguments.FirstOrDefault() ?? "");
				break;
			case Commands.ConvertFile:
				ConvertFile(command.Arguments.FirstOrDefault() ?? "", command.TargetFormat);
				break;
			case Commands.ConvertAll:
				ConvertDirectory(command.Arguments.FirstOrDefault() ?? Directory.GetCurrentDirectory(), command.TargetFormat);
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
			: Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), path));

		if (Directory.Exists(newPath))
		{
			Directory.SetCurrentDirectory(path);
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