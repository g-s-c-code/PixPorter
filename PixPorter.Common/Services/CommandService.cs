public class CommandService(ImageConverter converter, IUserInterace ui)
{
	private readonly ImageConverter _converter = converter;
	private readonly IUserInterace _ui = ui;

	public void ExecuteCommand(Command command)
	{
		switch (command.Name.ToLower())
		{
			case Constants.Quit:
				Environment.Exit(0);
				break;
			case Constants.Help:
				_ui.RenderUI(DirectoryUtility.GetDirectories(), DirectoryUtility.GetImageFiles(), true);
				_ui.WriteAndWait("Press any key to return...");
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
				_ui.DisplayErrorMessage("Unknown command.");
				break;
		}
	}

	private void ChangeDirectory(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			_ui.DisplayErrorMessage("Path cannot be empty. Usage: cd [[path]]");
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
			_ui.DisplayErrorMessage($"Directory not found: {newPath}");
		}
	}

	private void ConvertFile(string path, string? targetFormat)
	{
		if (!File.Exists(path))
		{
			_ui.DisplayErrorMessage($"File not found: {path}");
			return;
		}

		try
		{
			_converter.ConvertFile(path, targetFormat);
		}
		catch (Exception ex)
		{
			_ui.DisplayErrorMessage($"Failed to convert file: {ex.Message}");
		}
	}

	private void ConvertDirectory(string path, string? targetFormat)
	{
		if (!Directory.Exists(path))
		{
			_ui.DisplayErrorMessage($"Directory not found: {path}");
			return;
		}

		try
		{
			_converter.ConvertDirectory(path, targetFormat);
		}
		catch (Exception ex)
		{
			_ui.DisplayErrorMessage($"Failed to convert directory: {ex.Message}");
		}
	}
}
