public class PixPorter
{
	private readonly CommandProcessor _commandProcessor;

	public PixPorter()
	{
		var converter = new ImageConverter();
		var commandParser = new CommandParser();
		var commandExecutor = new CommandExecutor(converter);

		_commandProcessor = new CommandProcessor(commandParser, commandExecutor);
	}

	public void Run()
	{
		Console.Title = "PixPorter - Image Format Converter";

		while (true)
		{
			UI.RenderUI(DirectoryReader.GetDirectories(), DirectoryReader.GetImageFiles());
			UI.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");
			var input = UI.ReadLine("Enter command:");

			if (string.IsNullOrWhiteSpace(input))
				continue;

			_commandProcessor.ProcessCommand(input);
		}
	}
}