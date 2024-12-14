public static class Constants
{
	public static class Commands
	{
		public const string ConvertFile = "convert";
		public const string ConvertAll = "convert-all";
		public const string ChangeDirectory = "cd";
		public const string Exit = "q";
		public const string Help = "help";
	}

	public static class Flags
	{
		public const string ConvertAll = "-ca";
		public const string Png = "-png";
		public const string Jpg = "-jpg";
		public const string Jpeg = "-jpeg";
		public const string Webp = "-webp";
	}

	public static class FileFormats
	{
		public const string Png = ".png";
		public const string Jpg = ".jpg";
		public const string Jpeg = ".jpeg";
		public const string Webp = ".webp";

		public static readonly HashSet<string> SupportedFormats = new(StringComparer.OrdinalIgnoreCase)
		{
			Png, Jpg, Jpeg, Webp
		};
	}

	public static Dictionary<string, string> DefaultConversions { get; } = new()
	{
		{ FileFormats.Png, FileFormats.Webp },
		{ FileFormats.Jpg, FileFormats.Webp },
		{ FileFormats.Jpeg, FileFormats.Webp },
		{ FileFormats.Webp, FileFormats.Png }
	};

	public static class CompressionDefaults
	{
		public const int PngQuality = 100;
		public const int JpgQuality = 85;
		public const int WebpQuality = 80;

		public static readonly Dictionary<string, int> FormatQualityDefaults = new()
		{
			{ FileFormats.Png, PngQuality },
			{ FileFormats.Jpg, JpgQuality },
			{ FileFormats.Jpeg, JpgQuality },
			{ FileFormats.Webp, WebpQuality }
		};
	}
}
