public class PixPorter
{
	private readonly CommandProcessor _commandProcessor;

	public PixPorter()
	{
		var imageConverter = new ImageConverter();
		var commandParser = new CommandParser();
		var commandService = new CommandService(imageConverter);
		_commandProcessor = new CommandProcessor(commandParser, commandService);
	}

	public void Run()
	{
		Console.Title = "PixPorter - Image Format Converter";

		while (true)
		{
			UI.RenderUI(DirectoryUtility.GetDirectories(),
						DirectoryUtility.GetImageFiles());

			var input = UI.Read("Enter command:");

			if (string.IsNullOrWhiteSpace(input))
				continue;

			_commandProcessor.ProcessCommand(input);
		}
	}
}