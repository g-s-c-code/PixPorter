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
				ConvertFile(command.Arguments.FirstOrDefault() ?? "");
				break;
			case "help":
				DisplayHelp();
				break;
			case "config":
				Configure();
				break;
			case "exit":
				ExitApplication();
				break;
			default:
				AnsiConsole.WriteLine("Unknown command. Type 'help' for available commands.");
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

	private void ConvertFile(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			AnsiConsole.WriteLine("File path cannot be empty. Usage: convert [file_path]");
			return;
		}

		if (File.Exists(path))
		{
			try
			{
				_converter.ConvertFile(path);
				AnsiConsole.WriteLine($"File successfully converted: {path}");
			}
			catch (Exception ex)
			{
				AnsiConsole.WriteLine($"Failed to convert file: {ex.Message}");
			}
		}
		else
		{
			AnsiConsole.WriteLine($"File not found: {path}");
		}
	}

	private void DisplayHelp()
	{
		AnsiConsole.WriteLine("PixPorter Commands:");
		AnsiConsole.WriteLine("  cd [path]       - Change current directory");
		AnsiConsole.WriteLine("  convert [path]  - Convert image or directory");
		AnsiConsole.WriteLine("  config          - Configure conversions and output directory");
		AnsiConsole.WriteLine("  help            - Show this help menu");
		AnsiConsole.WriteLine("  exit            - Close PixPorter");
	}

	private void Configure()
	{
		while (true)
		{
			AnsiConsole.WriteLine("\nConfiguration Menu:");
			AnsiConsole.WriteLine("1. Modify Default Conversions");
			AnsiConsole.WriteLine("2. Set Permanent Output Directory");
			AnsiConsole.WriteLine("3. Clear Output Directory");
			AnsiConsole.WriteLine("4. View Current Configuration");
			AnsiConsole.WriteLine("0. Exit Configuration");

			string choice = AnsiConsole.Ask<string>("Choose an option: ");

			switch (choice)
			{
				case "1":
					ConfigureConversionRules();
					break;
				case "2":
					SetOutputDirectory();
					break;
				case "3":
					ClearOutputDirectory();
					break;
				case "4":
					DisplayCurrentConfiguration();
					break;
				case "0":
					return;
				default:
					AnsiConsole.WriteLine("Invalid option. Please try again.");
					break;
			}
		}
	}

	private void ConfigureConversionRules()
	{
		AnsiConsole.WriteLine("Current Default Conversions:");
		foreach (var conversion in _config.DefaultConversions)
		{
			AnsiConsole.WriteLine($"{conversion.Key} -> {conversion.Value}");
		}

		AnsiConsole.WriteLine("\nEnter new conversion (e.g., '.png .jpg' to change PNG conversion)");
		AnsiConsole.WriteLine("Or press Enter to keep current settings");

		string input = (Console.ReadLine() ?? "").Trim();
		if (string.IsNullOrEmpty(input)) return;

		var parts = input.Split(' ');
		if (parts.Length == 2)
		{
			string sourceFormat = parts[0].StartsWith('.') ? parts[0] : $".{parts[0]}";
			string targetFormat = parts[1].StartsWith('.') ? parts[1] : $".{parts[1]}";

			_config.DefaultConversions[sourceFormat] = targetFormat;
			AnsiConsole.WriteLine($"Updated: {sourceFormat} -> {targetFormat}");
		}
		else
		{
			AnsiConsole.WriteLine("Invalid input format");
		}
	}

	private void SetOutputDirectory()
	{
		AnsiConsole.WriteLine("Enter full path for permanent output directory:");
		string path = (Console.ReadLine() ?? "").Trim();

		if (string.IsNullOrEmpty(path))
		{
			AnsiConsole.WriteLine("Operation cancelled.");
			return;
		}

		if (Directory.Exists(path))
		{
			_config.OutputDirectory = path;
			AnsiConsole.WriteLine($"Output directory set to: {path}");
		}
		else
		{
			AnsiConsole.WriteLine("Directory does not exist. Create it? (Y/N)");
			if ((Console.ReadLine() ?? "").Trim().ToUpper() == "Y")
			{
				try
				{
					Directory.CreateDirectory(path);
					_config.OutputDirectory = path;
					AnsiConsole.WriteLine($"Created and set output directory to: {path}");
				}
				catch (Exception ex)
				{
					AnsiConsole.WriteLine($"Could not create directory: {ex.Message}");
				}
			}
		}
	}

	private void ClearOutputDirectory()
	{
		_config.OutputDirectory = null;
		AnsiConsole.WriteLine("Output directory reset to default (same as input folder).\n");
	}

	private void DisplayCurrentConfiguration()
	{
		AnsiConsole.WriteLine("\nCurrent Configuration:");

		AnsiConsole.WriteLine("Default Conversions:");
		foreach (var conversion in _config.DefaultConversions)
		{
			AnsiConsole.WriteLine($"{conversion.Key} -> {conversion.Value}");
		}

		AnsiConsole.WriteLine("\nOutput Directory:");
		if (_config.OutputDirectory == null)
			AnsiConsole.WriteLine("Same as input folder (default)");
		else
			AnsiConsole.WriteLine(_config.OutputDirectory);
	}

	private void ExitApplication()
	{
		AnsiConsole.WriteLine("Exiting application...");
		Environment.Exit(0);
	}
}
