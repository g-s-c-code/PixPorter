public static class Constants
{
	public const string SingleQuotationMark = "\"";
	public const string ConvertFile = "-cf";
	public const string ConvertAll = "-ca";
	public const string ChangeDirectory = "cd ";
	public const string Exit = "exit";
	public const string Q = "q";
	public const string Quit = "quit";
	public const string Help = "help";
	public const string PngFlag = "-png";
	public const string JpgFlag = "-jpg";
	public const string JpegFlag = "-jpeg";
	public const string WebpFlag = "-webp";
	public const string PngFileFormat = ".png";
	public const string JpgFileFormat = ".jpg";
	public const string JpegFileFormat = ".jpeg";
	public const string WebpFileFormat = ".webp";

	public static readonly HashSet<string> SupportedFileFormats = new(StringComparer.OrdinalIgnoreCase)
	{
		PngFileFormat, JpgFileFormat, JpegFileFormat, WebpFileFormat
	};

	public static Dictionary<string, string> DefaultConversions { get; } = new()
	{
		{ PngFileFormat, WebpFileFormat },
		{ JpgFileFormat, WebpFileFormat },
		{ JpegFileFormat, WebpFileFormat },
		{ WebpFileFormat, PngFileFormat }
	};
}
