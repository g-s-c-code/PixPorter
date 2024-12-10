using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;

class PixPorter
{
	public static string currentDirectory = AppContext.BaseDirectory;
	public static string? outputDirectory = null; // null is same as input folder
	public static readonly Dictionary<string, string> defaultConversions = new()
	{
		{ ".png", ".webp" },
		{ ".jpg", ".webp" },
		{ ".jpeg", ".webp" },
		{ ".webp", ".png" }
	};

	static void Main(string[] args)
	{
		Console.Title = "PixPorter - Image Format Converter";
		Console.WriteLine("PixPorter - Image Format Converter");
		Console.WriteLine("Type 'help' for available commands");

		while (true)
		{
			Console.Write($"{currentDirectory}> ");
			string input = (Console.ReadLine() ?? "").Trim();
			if (string.IsNullOrWhiteSpace(input))
				continue;

			ProcessCommand(input);
		}
	}

	static void ProcessCommand(string input)
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
						ConvertPath(parts[1]);
					break;
				case "help":
					DisplayHelp();
					break;
				case "config":
					ConfigureConversions();
					break;
				case "exit":
					Environment.Exit(0);
					break;
				default:
					// Check for explicit format flag
					string? targetFormat = parts.FirstOrDefault(p => p.StartsWith("."));

					if (parts.Length > 1 && targetFormat != null)
					{
						// Remove the flag from parts and process path with specified format
						var pathParts = parts.Where(p => p != targetFormat).ToArray();
						ConvertPath(string.Join(" ", pathParts), targetFormat);
					}
					else
					{
						// Assume drag-and-drop or direct path conversion
						ConvertPath(input);
					}
					break;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error: {ex.Message}");
		}
	}

	static void ChangeDirectory(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			currentDirectory = Directory.GetCurrentDirectory();
			return;
		}

		string newPath = Path.IsPathRooted(path)
			? path
			: Path.GetFullPath(Path.Combine(currentDirectory, path));

		if (Directory.Exists(newPath))
		{
			currentDirectory = newPath;
		}
		else
		{
			Console.WriteLine($"Directory not found: {newPath}");
		}
	}

	static void ConvertPath(string path, string? explicitFormat = null)
	{
		// Remove quotes if present
		path = path.Trim('"');

		// Resolve full path
		path = Path.IsPathRooted(path)
			? path
			: Path.GetFullPath(Path.Combine(currentDirectory, path));

		if (File.Exists(path))
		{
			ConvertFile(path, explicitFormat);
		}
		else if (Directory.Exists(path))
		{
			ConvertDirectory(path, explicitFormat);
		}
		else
		{
			Console.WriteLine($"Path not found: {path}");
		}
	}

	static void ConvertFile(string filePath, string? explicitFormat = null)
	{
		string extension = Path.GetExtension(filePath).ToLower();
		string? outputFormat = explicitFormat;

		// If no explicit format, use default conversion
		if (string.IsNullOrEmpty(outputFormat))
		{
			if (!defaultConversions.TryGetValue(extension, out outputFormat))
			{
				Console.WriteLine($"Unsupported file type: {filePath}");
				return;
			}
		}

		// Ensure output format starts with a dot and is lowercase
		outputFormat = outputFormat.StartsWith('.')
			? outputFormat.ToLower()
			: $".{outputFormat.ToLower()}";

		// Determine output path
		string outputPath;
		if (outputDirectory != null)
		{
			// Use permanent output directory
			string fileName = Path.GetFileNameWithoutExtension(filePath) + outputFormat;
			outputPath = Path.Combine(outputDirectory, fileName);
		}
		else
		{
			// Use same directory as input file
			outputPath = Path.ChangeExtension(filePath, outputFormat);
		}

		using (var image = Image.Load(filePath))
		{
			switch (outputFormat)
			{
				case ".webp":
					image.Save(outputPath, new WebpEncoder());
					break;
				case ".png":
					image.Save(outputPath, new PngEncoder());
					break;
				case ".jpg":
				case ".jpeg":
					image.Save(outputPath, new JpegEncoder());
					break;
				default:
					Console.WriteLine($"Unsupported output format: {outputFormat}");
					return;
			}

			Console.WriteLine($"Converted: {filePath} -> {outputPath}");
		}
	}

	static void ConvertDirectory(string directoryPath, string? explicitFormat = null)
	{
		string[] supportedExtensions = { ".webp", ".png", ".jpg", ".jpeg" };

		var files = Directory.GetFiles(directoryPath)
			.Where(f => supportedExtensions.Contains(Path.GetExtension(f).ToLower()));

		foreach (var file in files)
		{
			ConvertFile(file, explicitFormat);
		}
	}

	static void ConfigureConversions()
	{
		while (true)
		{
			Console.WriteLine("\nConfiguration Menu:");
			Console.WriteLine("1. Modify Default Conversions");
			Console.WriteLine("2. Set Permanent Output Directory");
			Console.WriteLine("3. Clear Output Directory");
			Console.WriteLine("4. View Current Configuration");
			Console.WriteLine("0. Exit Configuration");

			Console.Write("Choose an option: ");
			string choice = (Console.ReadLine() ?? "").Trim();

			switch (choice)
			{
				case "1":
					ConfigureConversionRules();
					break;
				case "2":
					SetOutputDirectory();
					break;
				case "3":
					outputDirectory = null;
					Console.WriteLine("Output directory reset to default (same as input folder).");
					break;
				case "4":
					DisplayCurrentConfiguration();
					break;
				case "0":
					return;
				default:
					Console.WriteLine("Invalid option. Please try again.");
					break;
			}
		}
	}

	static void ConfigureConversionRules()
	{
		Console.WriteLine("Current Default Conversions:");
		foreach (var conversion in defaultConversions)
		{
			Console.WriteLine($"{conversion.Key} -> {conversion.Value}");
		}

		Console.WriteLine("\nEnter new conversion (e.g., '.png .jpg' to change PNG conversion)");
		Console.WriteLine("Or press Enter to keep current settings");

		string input = (Console.ReadLine() ?? "").Trim();
		if (string.IsNullOrEmpty(input)) return;

		var parts = input.Split(' ');
		if (parts.Length == 2)
		{
			string sourceFormat = parts[0].StartsWith('.') ? parts[0] : $".{parts[0]}";
			string targetFormat = parts[1].StartsWith('.') ? parts[1] : $".{parts[1]}";

			defaultConversions[sourceFormat] = targetFormat;
			Console.WriteLine($"Updated: {sourceFormat} -> {targetFormat}");
		}
		else
		{
			Console.WriteLine("Invalid input format");
		}
	}

	static void SetOutputDirectory()
	{
		Console.WriteLine("Enter full path for permanent output directory:");
		string path = (Console.ReadLine() ?? "").Trim();

		if (string.IsNullOrEmpty(path))
		{
			Console.WriteLine("Operation cancelled.");
			return;
		}

		if (Directory.Exists(path))
		{
			outputDirectory = path;
			Console.WriteLine($"Output directory set to: {path}");
		}
		else
		{
			Console.WriteLine("Directory does not exist. Create it? (Y/N)");
			if ((Console.ReadLine() ?? "").Trim().ToUpper() == "Y")
			{
				try
				{
					Directory.CreateDirectory(path);
					outputDirectory = path;
					Console.WriteLine($"Created and set output directory to: {path}");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Could not create directory: {ex.Message}");
				}
			}
		}
	}

	static void DisplayCurrentConfiguration()
	{
		Console.WriteLine("\nCurrent Configuration:");

		Console.WriteLine("Default Conversions:");
		foreach (var conversion in defaultConversions)
		{
			Console.WriteLine($"{conversion.Key} -> {conversion.Value}");
		}

		Console.WriteLine("\nOutput Directory:");
		if (outputDirectory == null)
			Console.WriteLine("Same as input folder (default)");
		else
			Console.WriteLine(outputDirectory);
	}

	static void DisplayHelp()
	{
		Console.WriteLine("PixPorter Commands:");
		Console.WriteLine("  cd [path]       - Change current directory");
		Console.WriteLine("  convert [path]  - Convert image or directory");
		Console.WriteLine("  config          - Configure conversions and output directory");
		Console.WriteLine("  help            - Show this help menu");
		Console.WriteLine("  exit            - Close PixPorter");
		Console.WriteLine("\nSupported Conversions:");
		Console.WriteLine("  .webp <-> .png");
		Console.WriteLine("  .png <-> .jpg");
		Console.WriteLine("\nConversion Flags:");
		Console.WriteLine("  Drag or navigate to file/folder with .format");
		Console.WriteLine("  Example: somefile.png .jpg  (converts to jpg)");
	}
}