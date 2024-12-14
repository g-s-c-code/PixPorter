public static class DirectoryUtility
{
	public static IEnumerable<string> GetEntries(Func<string, bool> filter, string errorType)
	{
		var dir = Directory.GetCurrentDirectory();

		try
		{
			var entries = errorType == "Directory"
				? Directory.EnumerateDirectories(dir)
				: Directory.EnumerateFiles(dir);

			return entries
				.Where(entry => (File.GetAttributes(entry) & FileAttributes.Hidden) == 0 && filter(entry))
				.Select(Path.GetFileName)
				.Where(name => name != null)
				.ToList()!;
		}
		catch (UnauthorizedAccessException)
		{
			return [$"Error: Access to the path '{dir}' is denied."];
		}
		catch (Exception ex)
		{
			return [$"Error: {ex.Message}"];
		}
	}

	public static IEnumerable<string> GetDirectories()
	{
		return GetEntries(_ => true, "Directory");
	}

	public static IEnumerable<string> GetImageFiles()
	{
		return GetEntries(file => Constants.FileFormats.SupportedFormats.Contains(Path.GetExtension(file),
			StringComparer.OrdinalIgnoreCase),
			"File");
	}
}
