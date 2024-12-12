public static class Constants
{
	public static class Commands
	{
		public const string ConvertFile = "convert";
		public const string ConvertAll = "convert-all";
		public const string ChangeDirectory = "cd";
		public const string Quit = "q";
		public const string Help = "help";
	}

	public static class Flags
	{
		public const string ConvertFile = "-c";
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

		public static readonly string[] SupportedFormats = { Png, Jpg, Jpeg, Webp };
	}

	public static Dictionary<string, string> DefaultConversions { get; } = new()
	{
		{ FileFormats.Png, FileFormats.Webp },
		{ FileFormats.Jpg, FileFormats.Webp },
		{ FileFormats.Jpeg, FileFormats.Webp },
		{ FileFormats.Webp, FileFormats.Png }
	};
}
