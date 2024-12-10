public class PixPorterConfig
{
	public string CurrentDirectory { get; set; } = AppContext.BaseDirectory;
	public string? OutputDirectory { get; set; } = null;

	public Dictionary<string, string> DefaultConversions { get; } = new()
	{
		{ ".png", ".webp" },
		{ ".jpg", ".webp" },
		{ ".jpeg", ".webp" },
		{ ".webp", ".png" }
	};
}
