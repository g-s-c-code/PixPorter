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
		RenderUI();

		Queue<string> commandHistory = new Queue<string>();

		while (true)
		{
			string input = AnsiConsole.Ask<string>("Enter command:");

			if (string.IsNullOrWhiteSpace(input))
				continue;

			commandHistory.Enqueue(input);

			if (commandHistory.Count > 2)
			{
				commandHistory.Clear();
				RenderUI();
			}

			_commandProcessor.ProcessCommand(input);
		}
	}

	public void RenderUI()
	{
		var leftTableColumn = new Table();
		leftTableColumn.AddColumn(new TableColumn("[bold]PixPorter - Image Format Converter and Compressor[/]"));
		leftTableColumn.AddRow("");
		leftTableColumn.AddRow("[rosybrown bold underline]INSTRUCTIONS[/]");
		leftTableColumn.AddRow("Drag and drop an image, or manually enter path of an image, and press enter to convert it.");
		leftTableColumn.AddRow("");
		leftTableColumn.AddRow("[rosybrown bold underline]COMMANDS[/]");
		leftTableColumn.AddRow("cd (path)       - Change current directory");
		leftTableColumn.AddRow("convert (path)  - Convert image or directory");
		leftTableColumn.AddRow("config          - Configure settings");
		leftTableColumn.AddRow("exit            - Close PixPorter");
		leftTableColumn.Border = TableBorder.None;

		var rightTableColumn = new Table();
		rightTableColumn.AddColumn(new TableColumn(""));
		rightTableColumn.AddRow("");
		rightTableColumn.AddRow("[rosybrown bold underline]CURRENT SETTINGS[/]");
		rightTableColumn.AddRow("");
		rightTableColumn.AddRow($"[white]Current Directory:[/] [steelblue]{_config.CurrentDirectory}[/]");
		rightTableColumn.AddRow("");
		rightTableColumn.AddRow($"[white]Output Directory:[/] [steelblue]{(_config.OutputDirectory ?? "Same as image input folder (default)")}[/]");
		rightTableColumn.AddRow("");
		rightTableColumn.AddRow("[white]Conversion Formats:[/]");
		foreach (var conversion in _config.DefaultConversions)
		{
			rightTableColumn.AddRow($"[steelblue]  {conversion.Key} -> {conversion.Value}[/]");
		}
		rightTableColumn.Border = TableBorder.None;

		var mainLayout = new Table();
		mainLayout.AddColumn(new TableColumn(leftTableColumn));
		mainLayout.AddColumn(new TableColumn(rightTableColumn));
		mainLayout.Border = TableBorder.None;
		mainLayout.BorderColor(Color.White);

		AnsiConsole.Clear();
		AnsiConsole.Write(mainLayout);
	}
}
