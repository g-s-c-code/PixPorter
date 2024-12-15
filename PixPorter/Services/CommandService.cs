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
				UI.WriteAndWait("Press any key to return...", Color.SteelBlue);
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
				UI.WriteAndWait("Unknown command.", Color.RosyBrown);
				break;
		}
	}

	private void ChangeDirectory(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			UI.WriteAndWait("Path cannot be empty. Usage: cd [[path]]", Color.RosyBrown);
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
			UI.WriteAndWait($"Directory not found: {newPath}", Color.RosyBrown);
		}
	}

	private void ConvertFile(string path, string? targetFormat)
	{
		if (!File.Exists(path))
		{
			UI.WriteAndWait($"File not found: {path}", Color.RosyBrown);
			return;
		}

		try
		{
			_converter.ConvertFile(path, targetFormat);
		}
		catch (Exception ex)
		{
			UI.WriteAndWait($"Failed to convert file: {ex.Message}", Color.RosyBrown);
		}
	}

	private void ConvertDirectory(string path, string? targetFormat)
	{
		if (!Directory.Exists(path))
		{
			UI.WriteAndWait($"Directory not found: {path}", Color.RosyBrown);
			return;
		}

		try
		{
			_converter.ConvertDirectory(path, targetFormat);
		}
		catch (Exception ex)
		{
			UI.WriteAndWait($"Failed to convert directory: {ex.Message}", Color.RosyBrown);
		}
	}
}
