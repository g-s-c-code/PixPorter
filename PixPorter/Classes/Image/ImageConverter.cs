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

	public void ConvertFile(string filePath, string? explicitFormat = null)
	{
		string extension = Path.GetExtension(filePath).ToLower();
		string? outputFormat = explicitFormat ?? _config.DefaultConversions.GetValueOrDefault(extension);

		if (outputFormat == null)
		{
			AnsiConsole.MarkupLine($"[red]Unsupported file type: {filePath}[/]");
			return;
		}

		outputFormat = outputFormat.StartsWith('.') ? outputFormat.ToLower() : $".{outputFormat.ToLower()}";
		string outputPath = _config.OutputDirectory != null
			? Path.Combine(_config.OutputDirectory, Path.GetFileNameWithoutExtension(filePath) + outputFormat)
			: Path.ChangeExtension(filePath, outputFormat);

		AnsiConsole.Progress()
			.AutoRefresh(true)
			.AutoClear(false)
			.Columns(new ProgressColumn[]
			{
			new TaskDescriptionColumn(),
			new ProgressBarColumn(),
			new PercentageColumn(),
			new SpinnerColumn()
			})
			.Start(ctx =>
			{
				var task = ctx.AddTask($"[green]Processing {Path.GetFileName(filePath)}[/]");

				try
				{
					task.Increment(20);
					System.Threading.Thread.Sleep(200);

					using (var image = Image.Load(filePath))
					{
						task.Increment(30);
						System.Threading.Thread.Sleep(200);

						task.Increment(30);

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
								throw new NotSupportedException($"Output format {outputFormat} is not supported.");
						}

						task.Increment(20);
					}

					AnsiConsole.MarkupLine($"[green]Converted:[/] {filePath} -> {outputPath}");
				}
				catch (Exception ex)
				{
					AnsiConsole.MarkupLine($"[red]Error processing file {filePath}: {ex.Message}[/]");
				}
			});
	}

	public void ConvertDirectory(string directoryPath, string? explicitFormat = null)
	{
		string[] supportedExtensions = { ".webp", ".png", ".jpg", ".jpeg" };

		var files = Directory.GetFiles(directoryPath)
			.Where(f => supportedExtensions.Contains(Path.GetExtension(f).ToLower()))
			.ToList();

		if (!files.Any())
		{
			AnsiConsole.MarkupLine("[red]No supported image files found in the directory.[/]");
			return;
		}

		AnsiConsole.Progress()
			.AutoRefresh(true)
			.AutoClear(false)
			.HideCompleted(true)
			.Columns(new ProgressColumn[]
			{
			new TaskDescriptionColumn(),
			new ProgressBarColumn(),
			new PercentageColumn(),
			new RemainingTimeColumn(),
			new SpinnerColumn()
			})
			.Start(ctx =>
			{
				var overallTask = ctx.AddTask("[green]Converting images...[/]", maxValue: files.Count);

				foreach (var file in files)
				{
					System.Threading.Thread.Sleep(200);

					try
					{
						ConvertFile(file, explicitFormat);
					}
					catch (Exception ex)
					{
						AnsiConsole.MarkupLine($"[red]Error converting file {file}: {ex.Message}[/]");
					}

					overallTask.Increment(1);
				}
			});

		AnsiConsole.MarkupLine("[green]All files have been successfully processed![/]");
	}

}
