using Spectre.Console;

public class PixPorter
{
	private readonly CommandProcessor _commandProcessor;

	public PixPorter()
	{
		var config = new PixPorterConfig();
		var converter = new ImageConverter(config);
		_commandProcessor = new CommandProcessor(config, converter);
	}

	public void Run()
	{
		Console.Title = "PixPorter - Image Format Converter";
		Console.WriteLine("PixPorter - Image Format Converter");
		Console.WriteLine("Type 'help' for available commands");

		while (true)
		{
			string input = AnsiConsole.Ask<string>($"{_commandProcessor.CurrentDirectory}> ");
			if (string.IsNullOrWhiteSpace(input))
				continue;

			_commandProcessor.ProcessCommand(input);
		}
	}
}
