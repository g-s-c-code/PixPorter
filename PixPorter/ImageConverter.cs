using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;

public class ImageConverter
{
	private readonly PixPorterConfig _config;

	public ImageConverter(PixPorterConfig config)
	{
		_config = config;
	}

	public void ConvertFile(string filePath, string? explicitFormat = null)
	{
		string extension = Path.GetExtension(filePath).ToLower();
		string? outputFormat = explicitFormat ?? _config.DefaultConversions.GetValueOrDefault(extension);

		if (outputFormat == null)
		{
			Console.WriteLine($"Unsupported file type: {filePath}");
			return;
		}

		outputFormat = outputFormat.StartsWith('.') ? outputFormat.ToLower() : $".{outputFormat.ToLower()}";
		string outputPath = _config.OutputDirectory != null
			? Path.Combine(_config.OutputDirectory, Path.GetFileNameWithoutExtension(filePath) + outputFormat)
			: Path.ChangeExtension(filePath, outputFormat);

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

	public void ConvertDirectory(string directoryPath, string? explicitFormat = null)
	{
		string[] supportedExtensions = { ".webp", ".png", ".jpg", ".jpeg" };

		var files = Directory.GetFiles(directoryPath)
			.Where(f => supportedExtensions.Contains(Path.GetExtension(f).ToLower()));

		foreach (var file in files)
		{
			ConvertFile(file, explicitFormat);
		}
	}
}
