using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
using Spectre.Console;
using static Constants;

public class ImageConverter
{
	public void ConvertFile(string filePath, string? targetFormat = null)
	{
		string extension = Path.GetExtension(filePath).ToLower();
		string outputFormat = targetFormat ?? DefaultConversions.GetValueOrDefault(extension) ?? throw new Exception($"Unsupported file type: {filePath}");

		string outputPath = Path.ChangeExtension(filePath, outputFormat);

		using var image = Image.Load(filePath);
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
				throw new Exception($"Unsupported conversion target: {outputFormat}");
		}

		AnsiConsole.MarkupLine($"[green]Converted:[/] {filePath} -> {outputPath}");
		Console.ReadKey();
	}

	public void ConvertDirectory(string directoryPath, string? targetFormat = null)
	{
		var files = Directory.GetFiles(directoryPath)
			.Where(file =>
				FileFormats.SupportedFormats.Contains(Path.GetExtension(file).ToLower()) &&
				(targetFormat == null || DefaultConversions.ContainsKey(Path.GetExtension(file).ToLower())))
			.ToList();

		if (!files.Any())
		{
			AnsiConsole.MarkupLine("[yellow]No supported image files found in the directory.[/]");
			return;
		}

		foreach (var file in files)
		{
			try
			{
				ConvertFile(file, targetFormat);
			}
			catch (Exception ex)
			{
				AnsiConsole.MarkupLine($"[red]Failed to convert {file}: {ex.Message}[/]");
			}
		}

		AnsiConsole.MarkupLine("[green]All supported files have been successfully processed![/]");
	}
}
