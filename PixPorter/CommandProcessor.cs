public class CommandProcessor
{
	private readonly PixPorterConfig _config;
	private readonly ImageConverter _converter;

	public CommandProcessor(PixPorterConfig config, ImageConverter converter)
	{
		_config = config;
		_converter = converter;
	}

	public string CurrentDirectory => _config.CurrentDirectory;

	public void ProcessCommand(string input)
	{
		string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		string command = parts[0].ToLower();

		try
		{
			switch (command)
			{
				case "cd":
					ChangeDirectory(parts.Length > 1 ? parts[1] : "");
					break;
				case "convert":
					if (parts.Length > 1)
						_converter.ConvertFile(parts[1]);
					break;
				case "help":
					DisplayHelp();
					break;
				case "config":
					Configure();
					break;
				case "exit":
					Environment.Exit(0);
					break;
				default:
					Console.WriteLine("Unknown command. Type 'help' for available commands.");
					break;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error: {ex.Message}");
		}
	}

	private void ChangeDirectory(string path)
	{
		string newPath = Path.IsPathRooted(path)
			? path
			: Path.GetFullPath(Path.Combine(CurrentDirectory, path));

		if (Directory.Exists(newPath))
		{
			_config.CurrentDirectory = newPath;
		}
		else
		{
			Console.WriteLine($"Directory not found: {newPath}");
		}
	}

	private void DisplayHelp()
	{
		Console.WriteLine("PixPorter Commands:");
		Console.WriteLine("  cd [path]       - Change current directory");
		Console.WriteLine("  convert [path]  - Convert image or directory");
		Console.WriteLine("  config          - Configure conversions and output directory");
		Console.WriteLine("  help            - Show this help menu");
		Console.WriteLine("  exit            - Close PixPorter");
	}

	private void Configure()
	{
		Console.WriteLine("Configuration not yet implemented.");
	}
}
