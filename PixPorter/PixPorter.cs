public class PixPorter
{
	private readonly CommandParser _commandParser;
	private readonly CommandService _commandExecutor;

	public PixPorter()
	{
		var converter = new ImageConverter();
		_commandParser = new CommandParser();
		_commandExecutor = new CommandService(converter);
	}

	public void ProcessCommand(string input)
	{
		try
		{
			var command = _commandParser.Parse(input.ToLower());
			_commandExecutor.ExecuteCommand(command);
		}
		catch (CommandException ex)
		{
			UI.WriteAndWait($"Command Error: {ex.Message}");
		}
		catch (Exception ex)
		{
			UI.WriteAndWait($"Unexpected Error: {ex.Message}");
		}
	}

	public void Run()
	{
		Console.Title = "PixPorter - Image Format Converter";

		while (true)
		{
			UI.RenderUI(DirectoryReader.GetDirectories(), DirectoryReader.GetImageFiles());
			var input = UI.Read("Enter command:");

			if (string.IsNullOrWhiteSpace(input))
				continue;

			ProcessCommand(input);
		}
	}
}