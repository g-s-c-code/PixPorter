using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
using Spectre.Console;

public class ImageConverter
{
	private readonly Dictionary<string, IImageEncoder> _encoders = new()
	{
		{ Constants.WebpFileFormat, new WebpEncoder() },
		{ Constants.PngFileFormat, new PngEncoder() },
		{ Constants.JpegFileFormat, new JpegEncoder() },
		{ Constants.JpgFileFormat, new JpegEncoder() }
	};

	public void ConvertFile(string filePath, string? targetFormat = null)
	{
		string extension = Path.GetExtension(filePath).ToLower();
		string outputFormat = DetermineOutputFormat(extension, targetFormat);
		string outputPath = Path.ChangeExtension(filePath, outputFormat);

		try
		{
			using var image = Image.Load(filePath);
			image.Save(outputPath, GetEncoder(outputFormat));
			UI.Write($"Converted: {filePath} -> {outputPath}", Spectre.Console.Color.SteelBlue);
		}
		catch (Exception ex)
		{
			AnsiConsole.MarkupLine($"[rosybrown]Conversion failed: {ex.Message}[/]");
		}
	}

	public void ConvertDirectory(string directoryPath, string? targetFormat = null)
	{
		var files = Directory.GetFiles(directoryPath)
			.Where(file => IsSupported(file, targetFormat))
			.ToList();

		if (files.Count == 0)
		{
			UI.WriteAndWait("No supported image files found in the directory.", Spectre.Console.Color.Yellow);
			return;
		}

		AnsiConsole.Progress()
			.Start(ctx =>
			{
				var conversionTask = ctx.AddTask("[steelblue]Converting Images[/]", maxValue: files.Count);

				foreach (var file in files)
				{
					try
					{
						conversionTask.Increment(1);
						ConvertFile(file, targetFormat);
					}
					catch (Exception ex)
					{
						UI.WriteAndWait($"Conversion failed for {file}: {ex.Message}[/]", Spectre.Console.Color.RosyBrown);
					}
				}
			});

		UI.WriteAndWait("All supported files have been successfully processed.");
	}

	private string DetermineOutputFormat(string inputExtension, string? targetFormat)
	{
		return targetFormat ??
			   Constants.DefaultConversions.GetValueOrDefault(inputExtension) ??
			   throw new Exception($"Unsupported file type: {inputExtension}");
	}

	private IImageEncoder GetEncoder(string format)
	{
		return _encoders.TryGetValue(format, out var encoder)
			? encoder
			: throw new Exception($"Unsupported conversion target: {format}");
	}

	private bool IsSupported(string filePath, string? targetFormat)
	{
		string extension = Path.GetExtension(filePath).ToLower();
		return Constants.SupportedFileFormats.Contains(extension) &&
			   (targetFormat == null || Constants.DefaultConversions.ContainsKey(extension));
	}
}