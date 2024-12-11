using Spectre.Console;

public class SpectreUI
{
	private readonly PixPorterConfig _config;

	public SpectreUI(PixPorterConfig config) => _config = config;

	public void RenderUI()
	{
		var leftTableColumn = new Table();
		leftTableColumn.AddColumn(new TableColumn("[bold]PixPorter - Image Format Converter and Compressor[/]"));
		leftTableColumn.AddRow("");
		leftTableColumn.AddRow("[steelblue1_1 bold]INSTRUCTIONS[/]");
		leftTableColumn.AddRow("[white underline]Drag & Drop[/]");
		leftTableColumn.AddRow("[grey85]Drag an image into the app window to auto-fill its path, then press 'Enter' to convert.[/]");
		leftTableColumn.AddRow("");
		leftTableColumn.AddRow("[white underline]Folder Conversion[/]");
		leftTableColumn.AddRow("[grey85]Navigate to the folder containing your images and type 'convert' to process all images in that folder.[/]");
		leftTableColumn.AddRow("");
		leftTableColumn.AddRow("");
		leftTableColumn.AddRow("[steelblue1_1 bold]COMMANDS[/]");
		leftTableColumn.AddRow("[grey85]cd (path)       - Change current directory[/]");
		leftTableColumn.AddRow("[grey85]convert (path)  - Convert image or directory[/]");
		leftTableColumn.AddRow("[grey85]config          - Configure settings[/]");
		leftTableColumn.AddRow("[grey85]exit            - Close PixPorter[/]");
		leftTableColumn.Border = TableBorder.None;

		var rightTableColumn = new Table();
		rightTableColumn.AddColumn(new TableColumn(""));
		rightTableColumn.AddRow("");
		rightTableColumn.AddRow("[steelblue1_1 bold]CURRENT SETTINGS[/]");
		rightTableColumn.AddRow($"[white]Current Directory:[/] [steelblue]{_config.CurrentDirectory}[/]");
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
		mainLayout.Border = TableBorder.Minimal;
		mainLayout.BorderColor(Color.White);

		AnsiConsole.Clear();
		AnsiConsole.Write(mainLayout);
	}
}