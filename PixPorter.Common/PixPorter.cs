public class PixPorter
{
	private readonly CommandProcessor _commandProcessor;
	private readonly IUserInterace _ui;

	public PixPorter(IUserInterace ui)
	{
		_ui = ui;
		var imageConverter = new ImageConverter(_ui);
		var commandParser = new CommandParser();
		var commandService = new CommandService(imageConverter, _ui);
		_commandProcessor = new CommandProcessor(commandParser, commandService, _ui);
	}

	public void Run()
	{
		_ui.DisplayTitle("PixPorter - Image Format Converter");

		while (true)
		{
			_ui.RenderUI(DirectoryUtility.GetDirectories(),
						DirectoryUtility.GetImageFiles());

			var input = _ui.Read("Enter command:");

			if (string.IsNullOrWhiteSpace(input))
				continue;

			_commandProcessor.ProcessCommand(input);
		}
	}
}