using static Constants;

public class DirectoriesService
{
	public IEnumerable<string> GetEntries(string path, Func<string, bool> filter, string errorType)
	{
		try
		{
			var entries = errorType == "Directory"
				? Directory.EnumerateDirectories(path)
				: Directory.EnumerateFiles(path);

			return entries
				.Where(entry => (File.GetAttributes(entry) & FileAttributes.Hidden) == 0 && filter(entry))
				.Select(Path.GetFileName)
				.Where(name => name != null)
				.ToList()!;
		}
		catch (UnauthorizedAccessException)
		{
			return new[] { $"Error: Access to the path '{path}' is denied." };
		}
		catch (Exception ex)
		{
			return new[] { $"Error: {ex.Message}" };
		}
	}

	public IEnumerable<string> GetDirectories(string path)
	{
		return GetEntries(path, _ => true, "Directory");
	}
		
	public IEnumerable<string> GetImageFiles(string path)
	{
		return GetEntries(path,
			file => FileFormats.SupportedFormats.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase),
			"File");
	}
}
