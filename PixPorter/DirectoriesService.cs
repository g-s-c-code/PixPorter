public class DirectoriesService
{
	public IEnumerable<string> GetDirectories(string path)
	{
		var directories = new List<string>();
		try
		{
			foreach (var dir in Directory.GetDirectories(path))
			{
				if ((new DirectoryInfo(dir).Attributes & FileAttributes.Hidden) != 0)
				{
					continue;
				}
				directories.Add($"{Path.GetFileName(dir)}");
			}
		}
		catch (UnauthorizedAccessException)
		{
			directories.Add($"Error: Access to the path '{path}' is denied.");
		}
		catch (Exception ex)
		{
			directories.Add($"Error: {ex.Message}");
		}
		return directories;
	}

	public IEnumerable<string> GetFiles(string path)
	{
		var files = new List<string>();
		try
		{
			foreach (var file in Directory.GetFiles(path))
			{
				if ((new FileInfo(file).Attributes & FileAttributes.Hidden) != 0)
				{
					continue;
				}
				files.Add($"{Path.GetFileName(file)}");
			}
		}
		catch (UnauthorizedAccessException)
		{
			files.Add($"Error: Access to the path '{path}' is denied.");
		}
		catch (Exception ex)
		{
			files.Add($"Error: {ex.Message}");
		}
		return files;
	}
}
