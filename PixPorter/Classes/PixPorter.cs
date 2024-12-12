public class PixPorter
{
	private readonly CommandProcessor _commandProcessor;
	private readonly DirectoriesService _directoriesService;

	public PixPorter()
	{
		var converter = new ImageConverter();
		var commandParser = new CommandParser();
		var commandExecutor = new CommandExecutor(converter);

		_directoriesService = new DirectoriesService();
		_commandProcessor = new CommandProcessor(commandParser, commandExecutor);
	}

	public void Run()
	{
		Console.Title = "PixPorter - Image Format Converter";

		while (true)
		{
			UI.RenderUI(_directoriesService.GetDirectories(Directory.GetCurrentDirectory()), _directoriesService.GetFiles(Directory.GetCurrentDirectory()));
			UI.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");
			var input = UI.ReadLine("Enter command:");

			if (string.IsNullOrWhiteSpace(input))
				continue;

			_commandProcessor.ProcessCommand(input);
		}
	}
}