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
        { Constants.FileFormats.Webp, new WebpEncoder() },
        { Constants.FileFormats.Png, new PngEncoder() },
        { Constants.FileFormats.Jpg, new JpegEncoder() },
        { Constants.FileFormats.Jpeg, new JpegEncoder() }
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
			UI.WriteLine($"Converted: {filePath} -> {outputPath}");
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

        if (!files.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No supported image files found in the directory.[/]");
            return;
        }

        AnsiConsole.Progress()
            .Start(ctx =>
            {
                var conversionTask = ctx.AddTask("Converting Images", maxValue: files.Count);

                foreach (var file in files)
                {
                    try
                    {
                        ConvertFile(file, targetFormat);
                        conversionTask.Increment(1);
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[rosybrown]Conversion failed for {file}: {ex.Message}[/]");
                    }
                }
            });

        UI.WriteAndWait("All supported files have been successfully processed.");
        Console.ReadKey();
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
        return Constants.FileFormats.SupportedFormats.Contains(extension) &&
               (targetFormat == null || Constants.DefaultConversions.ContainsKey(extension));
    }
}