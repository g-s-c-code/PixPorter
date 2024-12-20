using System.Text;
using Spectre.Console;
public class CommandService(ImageConverter converter)
{
	private readonly ImageConverter _converter = converter;

	public void ExecuteCommand(Command command)
	{
		switch (command.Name.ToLower())
		{
			case Constants.Quit:
				Environment.Exit(0);
				break;
			case Constants.Help:
				UI.RenderUI(DirectoryUtility.GetDirectories(), DirectoryUtility.GetImageFiles(), true);
				UI.WriteAndWait("Press any key to return...");
				break;
			case Constants.ChangeDirectory:
				ChangeDirectory(command.Arguments.FirstOrDefault() ?? "");
				break;
			case Constants.ConvertFile:
				ConvertFile(command.Arguments.FirstOrDefault() ?? "", command.TargetFormat);
				Console.ReadKey();
				break;
			case Constants.ConvertAll:
				ConvertDirectory(command.Arguments.FirstOrDefault() ?? Directory.GetCurrentDirectory(), command.TargetFormat);
				break;
			default:
				UI.DisplayErrorMessage("Unknown command.");
				break;
		}
	}

	private void ChangeDirectory(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			UI.DisplayErrorMessage("Path cannot be empty. Usage: cd [[path]]");
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
			UI.DisplayErrorMessage($"Directory not found: {newPath}");
		}
	}

	private void ConvertFile(string path, string? targetFormat)
	{
		if (!File.Exists(path))
		{
			UI.DisplayErrorMessage($"File not found: {path}");
			return;
		}

		try
		{
			_converter.ConvertFile(path, targetFormat);
		}
		catch (Exception ex)
		{
			UI.DisplayErrorMessage($"Failed to convert file: {ex.Message}");
		}
	}

	private void ConvertDirectory(string path, string? targetFormat)
	{
		if (!Directory.Exists(path))
		{
			UI.DisplayErrorMessage($"Directory not found: {path}");
			return;
		}

		try
		{
			_converter.ConvertDirectory(path, targetFormat);
		}
		catch (Exception ex)
		{
			UI.DisplayErrorMessage($"Failed to convert directory: {ex.Message}");
		}
	}
}
