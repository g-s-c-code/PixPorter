using Spectre.Console;

public class PixPorter
{
	private readonly PixPorterConfig _config;
	private readonly CommandProcessor _commandProcessor;

	public PixPorter()
	{
		_config = new PixPorterConfig();
		var converter = new ImageConverter(_config);
		var commandParser = new CommandParser();
		var commandExecutor = new CommandExecutor(_config, converter);

		_commandProcessor = new CommandProcessor(commandParser, commandExecutor);
	}

	public void Run()
	{
		Console.Title = "PixPorter - Image Format Converter";

		while (true)
		{
			AnsiConsole.WriteLine($"Current Directory: {_config.CurrentDirectory}");
			string input = AnsiConsole.Ask<string>("Enter file path, directory, or command:");

			if (string.IsNullOrWhiteSpace(input))
				continue;

			_commandProcessor.ProcessCommand(input);
		}
	}
}