using Spectre.Console;

public class PixPorter
{
	private readonly PixPorterConfig _config;
	private readonly SpectreUI _ui;
	private readonly CommandProcessor _commandProcessor;

	public PixPorter()
	{
		_config = new PixPorterConfig();
		_ui = new SpectreUI(_config);

		var converter = new ImageConverter(_config);
		var commandParser = new CommandParser();
		var commandExecutor = new CommandExecutor(_config, converter);

		_commandProcessor = new CommandProcessor(commandParser, commandExecutor);
	}

	public void Run()
	{
		Console.Title = "PixPorter - Image Format Converter";
		_ui.RenderUI();

		Queue<string> commandHistory = new Queue<string>();

		while (true)
		{
			AnsiConsole.WriteLine(_config.CurrentDirectory);
			string input = AnsiConsole.Ask<string>("Enter command:");

			if (string.IsNullOrWhiteSpace(input))
				continue;

			commandHistory.Enqueue(input);

			if (commandHistory.Count > 0)
			{
				commandHistory.Clear();
				_ui.RenderUI();
			}

			_commandProcessor.ProcessCommand(input);
		}
	}
}
