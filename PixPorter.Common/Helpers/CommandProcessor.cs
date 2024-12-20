using Spectre.Console;

public class CommandProcessor(CommandParser commandParser, CommandService commandService, IUserInterace ui)
{
	private readonly CommandParser _commandParser = commandParser;
	private readonly CommandService _commandExecutor = commandService;
	private readonly IUserInterace _ui = ui;

	public void ProcessCommand(string input)
	{
		try
		{
			var command = _commandParser.ParseInput(input.ToLower());
			_commandExecutor.ExecuteCommand(command);
		}
		catch (CommandException ex)
		{
			_ui.DisplayErrorMessage($"Command Error: {ex.Message}");
		}
		catch (Exception ex)
		{
			_ui.DisplayErrorMessage($"Unexpected Error: {ex.Message}");
		}
	}
}