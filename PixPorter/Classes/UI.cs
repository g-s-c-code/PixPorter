using Spectre.Console;
using Spectre.Console.Rendering;
using static Constants;

public static class UI
{
	public static void RenderUI(IEnumerable<string> directoriesTree, IEnumerable<string> filesTree)
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

		table.AddRow(BuildCommandsSection(), BuildCurrentSettingsSection(), BuildInstructionsSection());
		table.AddRow(CreateTree("Folders:", directoriesTree), CreateTree("Image Files:", filesTree));

		AnsiConsole.Clear();
		AnsiConsole.Write(table);
	}

	private static string BuildCommandsSection()
	{
		return @"[grey85]cd (path)          [lightskyblue1]Change directory[/]
convert (path)     [lightskyblue1]Convert image(s)[/]
convert -a (path)  [lightskyblue1]Convert image(s)[/]
config             [lightskyblue1]Configure settings[/]
exit               [lightskyblue1]Close PixPorter[/][/]";
	}

	private static string BuildCurrentSettingsSection()
	{
		string outputDirectory = "Same as input folder\n";
		string conversions = string.Join("\n", DefaultConversions.Select(c => $"[steelblue]{c.Key} -> {c.Value}[/]"));

		return $@"[grey85 underline]Output Directory[/]
[lightskyblue1]{outputDirectory}[/]

[grey85 underline]Conversion Formats[/]
{conversions}";
	}

	private static string BuildInstructionsSection()
	{
		return @"[grey85 underline]Drag & Drop[/]
[lightskyblue1]Drag an image into the app window to auto-fill its path, then press 'Enter' to convert.[/]

[grey85 underline]Folder Conversion[/]
[lightskyblue1]Navigate to the folder containing your images and type 'convert' to process all images in that folder.[/]";
	}

	private static IRenderable CreateTree(string header, IEnumerable<string> items)
	{
		var tree = new Tree(header)
		{
			Style = new Style(foreground: Color.RosyBrown)
		};

		foreach (var item in items)
		{
			tree.AddNode($"[bold white]{item}[/]");
		}

		return tree;
	}

	public static void WriteLine(string output)
	{
		AnsiConsole.WriteLine(output);
	}

	public static string ReadLine(string input)
	{
		return AnsiConsole.Ask<string>(input);
	}
}