using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;

class PixPorter
{
	private static string _currentDirectory;

	// Configuration for default conversions
	private static Dictionary<string, string> _defaultConversions = new Dictionary<string, string>
	{
		{ ".png", ".webp" },
		{ ".jpg", ".webp" },
		{ ".jpeg", ".webp" },
		{ ".webp", ".png" }
	};

	static void Main(string[] args)
	{
		_currentDirectory = Directory.GetCurrentDirectory();
		Console.Title = "PixPorter - Image Format Converter";

		Console.WriteLine("PixPorter - Image Format Converter");
		Console.WriteLine("Type 'help' for available commands");

		while (true)
		{
			Console.Write($"{_currentDirectory}> ");
			string input = Console.ReadLine().Trim();

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
					ChangeDirectory(parts.Length > 1 ? parts[1] : null);
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
					string targetFormat = parts.FirstOrDefault(p => p.StartsWith("."));

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
			_currentDirectory = Directory.GetCurrentDirectory();
			return;
		}

		string newPath = Path.IsPathRooted(path)
			? path
			: Path.GetFullPath(Path.Combine(_currentDirectory, path));

		if (Directory.Exists(newPath))
		{
			_currentDirectory = newPath;
		}
		else
		{
			Console.WriteLine($"Directory not found: {newPath}");
		}
	}

	static void ConvertPath(string path, string explicitFormat = null)
	{
		// Remove quotes if present
		path = path.Trim('"');

		// Resolve full path
		path = Path.IsPathRooted(path)
			? path
			: Path.GetFullPath(Path.Combine(_currentDirectory, path));

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

	static void ConvertFile(string filePath, string explicitFormat = null)
	{
		string extension = Path.GetExtension(filePath).ToLower();
		string outputFormat = explicitFormat;

		// If no explicit format, use default conversion
		if (string.IsNullOrEmpty(outputFormat))
		{
			if (!_defaultConversions.TryGetValue(extension, out outputFormat))
			{
				Console.WriteLine($"Unsupported file type: {filePath}");
				return;
			}
		}

		// Ensure output format starts with a dot and is lowercase
		outputFormat = outputFormat.StartsWith('.')
			? outputFormat.ToLower()
			: $".{outputFormat.ToLower()}";

		using (var image = Image.Load(filePath))
		{
			string outputPath = Path.ChangeExtension(filePath, outputFormat);

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

	static void ConvertDirectory(string directoryPath, string explicitFormat = null)
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
		Console.WriteLine("Current Default Conversions:");
		foreach (var conversion in _defaultConversions)
		{
			Console.WriteLine($"{conversion.Key} -> {conversion.Value}");
		}

		Console.WriteLine("\nEnter new conversion (e.g., '.png .jpg' to change PNG conversion)");
		Console.WriteLine("Or press Enter to keep current settings");

		string input = Console.ReadLine().Trim();
		if (string.IsNullOrEmpty(input)) return;

		var parts = input.Split(' ');
		if (parts.Length == 2)
		{
			string sourceFormat = parts[0].StartsWith('.') ? parts[0] : $".{parts[0]}";
			string targetFormat = parts[1].StartsWith('.') ? parts[1] : $".{parts[1]}";

			_defaultConversions[sourceFormat] = targetFormat;
			Console.WriteLine($"Updated: {sourceFormat} -> {targetFormat}");
		}
		else
		{
			Console.WriteLine("Invalid input format");
		}
	}

	static void DisplayHelp()
	{
		Console.WriteLine("PixPorter Commands:");
		Console.WriteLine("  cd [path]       - Change current directory");
		Console.WriteLine("  convert [path]  - Convert image or directory");
		Console.WriteLine("  config          - Configure default conversions");
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