public static class Constants
{
	public static class Commands
	{
		public const string ConvertFile = "convert";
		public const string ConvertAll = "convert-all";
		public const string ChangeDirectory = "cd";
		public const string Quit = "q";
		public const string Help = "h";
	}

	public static class FileFormats
	{
		public const string Png = ".png";
		public const string Jpg = ".jpg";
		public const string Jpeg = ".jpeg";
		public const string Webp = ".webp";

		public static readonly string[] SupportedFormats = { Png, Jpg, Jpeg, Webp };
	}
}
