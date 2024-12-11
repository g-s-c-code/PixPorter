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
@"[grey85]cd (path)          [lightskyblue1]Change directory[/]
convert (path)     [lightskyblue1]Convert image(s)[/]
convert -a (path)  [lightskyblue1]Convert image(s)[/]
config             [lightskyblue1]Configure settings[/]
exit               [lightskyblue1]Close PixPorter[/][/]";

		// Current settings section
		string outputDirectory = (_config.OutputDirectory ?? "Same as input folder") + "\n";
		string conversions = string.Join("\n", _config.DefaultConversions.Select(c => $"[steelblue]{c.Key} -> {c.Value}[/]"));
		string currentSettings =
$@"[grey85 underline]Output Directory[/]
[lightskyblue1]{outputDirectory}[/]
[grey85 underline]Conversion Formats[/]
{conversions}";

		// Instructions section
		string instructions =
@"[grey85 underline]Drag & Drop[/]
[lightskyblue1]Drag an image into the app window to auto-fill its path, then press 'Enter' to convert.[/]

[grey85 underline]Folder Conversion[/]
[lightskyblue1]Navigate to the folder containing your images and type 'convert' to process all images in that folder.[/]";

		table.AddRow(commands, currentSettings, instructions);

		AnsiConsole.Clear();
		AnsiConsole.Write(table);
	}
}