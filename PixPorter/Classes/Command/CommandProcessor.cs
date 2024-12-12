using Spectre.Console;

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
			_commandExecutor.ExecuteCommand(command);
		}
		catch (CommandException ex)
		{
			AnsiConsole.WriteLine($"Command Error: {ex.Message}");
		}
		catch (Exception ex)
		{
			AnsiConsole.WriteLine($"Unexpected Error: {ex.Message}");
		}
	}
}