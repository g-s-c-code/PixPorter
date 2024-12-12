using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
using Spectre.Console;

public class ImageConverter
{
	private readonly PixPorterConfig _config;

	public ImageConverter(PixPorterConfig config)
	{
		_config = config;
	}

	public void ConvertFile(string filePath)
	{
		string extension = Path.GetExtension(filePath).ToLower();
		string? outputFormat = _config.DefaultConversions.GetValueOrDefault(extension);

		if (outputFormat == null)
		{
			AnsiConsole.MarkupLine($"[red]Unsupported file type: {filePath}[/]");
			return;
		}

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
		}

		AnsiConsole.MarkupLine($"[green]Converted:[/] {filePath} -> {outputPath}");
	}

	public void ConvertDirectory(string directoryPath)
	{
		var files = Directory.GetFiles(directoryPath)
			.Where(file => _config.DefaultConversions.ContainsKey(Path.GetExtension(file).ToLower()))
			.ToList();

		if (!files.Any())
		{
			AnsiConsole.MarkupLine("[red]No supported image files found in the directory.[/]");
			return;
		}

		foreach (var file in files)
		{
			ConvertFile(file);
		}

		AnsiConsole.MarkupLine("[green]All files have been successfully processed![/]");
	}
}
