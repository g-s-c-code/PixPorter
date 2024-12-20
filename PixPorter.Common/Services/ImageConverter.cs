using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Tiff;

public class ImageConverter
{
	private readonly IUserInterace _ui;

	public ImageConverter(IUserInterace ui)
	{
		_ui = ui;
	}

	private readonly Dictionary<string, IImageEncoder> _encoders = new()
	{
		{ Constants.WebpFileFormat, new WebpEncoder() },
		{ Constants.PngFileFormat, new PngEncoder() },
		{ Constants.JpegFileFormat, new JpegEncoder() },
		{ Constants.JpgFileFormat, new JpegEncoder() },
		{ Constants.GifFileFormat, new GifEncoder() },
		{ Constants.TiffFileFormat, new TiffEncoder() },
		{ Constants.BmpFileFormat, new BmpEncoder() }
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
			_ui.Write($"Converted: {filePath} -> {outputPath}");
		}
		catch (Exception ex)
		{
			_ui.WriteAndWait($"Conversion failed: {ex.Message}");
		}
	}

	public void ConvertDirectory(string directoryPath, string? targetFormat = null)
	{
		var files = Directory.GetFiles(directoryPath)
			.Where(file => IsSupported(file, targetFormat))
			.ToList();

		if (files.Count == 0)
		{
			_ui.DisplayErrorMessage("No supported image files found in the directory.");
			return;
		}

		_ui.RenderProgress(files, targetFormat, ConvertFile);
		_ui.WriteAndWait("All supported files have been successfully processed.");
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