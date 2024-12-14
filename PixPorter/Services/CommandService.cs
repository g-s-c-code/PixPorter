using System.Text;
using Spectre.Console;
using static Constants;

public class CommandService(ImageConverter converter)
{
	private readonly ImageConverter _converter = converter;

	public void ExecuteCommand(Command command)
	{
		switch (command.Name.ToLower())
		{
			case Commands.Exit:
				Environment.Exit(0);
				break;
			case Commands.Help:
				UI.RenderUI(DirectoryUtility.GetDirectories(), DirectoryUtility.GetImageFiles(), true);
				UI.WriteAndWait("Press any key to return...", Color.SteelBlue);
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
				UI.WriteLine("[rosybrown]Unknown command.[/]");
				break;
		}
	}

	private void ChangeDirectory(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			AnsiConsole.WriteLine("Path cannot be empty. Usage: cd [[path]]");
			return;
		}

		string newPath = Path.IsPathRooted(path)
			? path
			: Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), path));

		if (Directory.Exists(newPath))
		{
			Directory.SetCurrentDirectory(newPath);
		}
		else
		{
			AnsiConsole.WriteLine($"Directory not found: {newPath}");
			Console.ReadKey();
		}
	}

	private void ConvertFile(string path, string? targetFormat)
	{
		if (!File.Exists(path))
		{
			UI.WriteLine($"File not found: {path}");
			return;
		}

		try
		{
			_converter.ConvertFile(path, targetFormat);
		}
		catch (Exception ex)
		{
			UI.WriteLine($"Failed to convert file: {ex.Message}");
		}
	}

	private void ConvertDirectory(string path, string? targetFormat)
	{
		if (!Directory.Exists(path))
		{
			UI.WriteLine($"Directory not found: {path}");
			return;
		}

		try
		{
			_converter.ConvertDirectory(path, targetFormat);
		}
		catch (Exception ex)
		{
			UI.WriteLine($"Failed to convert directory: {ex.Message}");
		}
	}
}
