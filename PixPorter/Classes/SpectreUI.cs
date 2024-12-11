using Spectre.Console;

public class SpectreUI
{
	private readonly PixPorterConfig _config;

	public SpectreUI(PixPorterConfig config) => _config = config;

	public void RenderUI()
	{
		var table = new Table
		{
			Border = TableBorder.Square,
			UseSafeBorder = true,
			ShowRowSeparators = true,
			Title = new TableTitle("PixPorter - Image Format Converter").SetStyle("white bold")
		};

		table.AddColumn(new TableColumn("[white bold]COMMANDS[/]").Width(50));
		table.AddColumn(new TableColumn("[white bold]CURRENT SETTINGS[/]").Width(50));
		table.AddColumn(new TableColumn("[white bold]INSTRUCTIONS[/]").Width(50));

		// Commands section
		string commands =
@"[grey85]cd (path)       - [rosybrown]Change directory[/]
convert (path)  - [rosybrown]Convert image(s)[/]
config          - [rosybrown]Configure settings[/]
exit            - [rosybrown]Close PixPorter[/][/]";

		// Current settings section
		string outputDirectory = _config.OutputDirectory ?? "Same as input folder\n";
		string conversions = string.Join("\n", _config.DefaultConversions.Select(c => $"[steelblue]{c.Key} -> {c.Value}[/]"));
		string currentSettings =
$@"[grey85 underline]Output Directory[/]
[rosybrown]{outputDirectory}[/]
[grey85 underline]Conversion Formats[/]
{conversions}";

		// Instructions section
		string instructions =
@"[grey85 underline]Drag & Drop[/]
[rosybrown]Drag an image into the app window to auto-fill its path, then press 'Enter' to convert.[/]

[grey85 underline]Folder Conversion[/]
[rosybrown]Navigate to the folder containing your images and type 'convert' to process all images in that folder.[/]";

		table.AddRow(commands, currentSettings, instructions);

		AnsiConsole.Clear();
		AnsiConsole.Write(table);
	}
}