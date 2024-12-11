public class CommandProcessor
{
	private readonly CommandParser _commandParser;
	private readonly CommandExecutor _commandExecutor;

	public CommandProcessor(CommandParser commandParser, CommandExecutor commandExecutor)
	{
		_commandParser = commandParser;
		_commandExecutor = commandExecutor;
	}

	public void ProcessCommand(string input)
	{
		try
		{
			var command = _commandParser.Parse(input);
			_commandExecutor.Execute(command);
		}
		catch (CommandException ex)
		{
			Console.WriteLine($"Command Error: {ex.Message}");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Unexpected Error: {ex.Message}");
		}
	}
}