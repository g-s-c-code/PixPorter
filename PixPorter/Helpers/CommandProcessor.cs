using Spectre.Console;

public class CommandProcessor(CommandParser commandParser, CommandService commandService)
{
	private readonly CommandParser _commandParser = commandParser;
	private readonly CommandService _commandExecutor = commandService;

	public void ProcessCommand(string input)
	{
		try
		{
			var command = _commandParser.Parse(input.ToLower());
			_commandExecutor.ExecuteCommand(command);
		}
		catch (CommandException ex)
		{
			UI.WriteAndWait($"Command Error: {ex.Message}", Color.RosyBrown);
		}
		catch (Exception ex)
		{
			UI.WriteAndWait($"Unexpected Error: {ex.Message}", Color.RosyBrown);
		}
	}
}