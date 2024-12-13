using Spectre.Console;
using Spectre.Console.Rendering;
using static Constants;

public static class UI
{
	public static void RenderUIs(IEnumerable<string> directoriesTree, IEnumerable<string> filesTree)
	{
		var mainLayout = new Table();

		const int width = 100;

		var directoryTable = new Table();
		directoryTable.Width = width;
		directoryTable.Border = TableBorder.None;
		directoryTable.AddColumn(new TableColumn(CreateTree("Folders:", directoriesTree)));
		directoryTable.AddColumn(new TableColumn(CreateTree("Image Files:", filesTree)));

		var currentDirectory = new Panel($"Current Directory: {Directory.GetCurrentDirectory()}");
		currentDirectory.Width = width;
		currentDirectory.Border = BoxBorder.None;
		currentDirectory.Padding = new Padding(0);

		var instructionsTable = new Table();
		instructionsTable.Width = width;
		instructionsTable.Border = TableBorder.None;
		instructionsTable.AddColumn(BuildInstructionsSection());
		instructionsTable.AddEmptyRow();
		instructionsTable.AddRow(DisplayDefaultConversions());

		var interactionsTable = new Table();
		interactionsTable.Width = width;
		interactionsTable.Border = TableBorder.None;
		interactionsTable.AddColumn(BuildCommandsSection());
		interactionsTable.AddEmptyRow();
		interactionsTable.AddRow(BuildFlagsSection());

		var layout = new Layout("Root")
			.SplitColumns(
				new Layout("Left")
					.SplitRows(
						new Layout("TopLeft").Size(5),
						new Layout("BottomLeft")),
				new Layout("Right")
					.SplitRows(
						new Layout("TopRight").Size(3),
						new Layout("BottomRight")));

		layout["TopLeft"].Update(instructionsTable);
		layout["BottomLeft"].Update(interactionsTable);
		layout["TopRight"].Update(currentDirectory);
		layout["BottomRight"].Update(directoryTable);

		mainLayout.AddColumn(new TableColumn(layout));

		AnsiConsole.Clear();
		AnsiConsole.Write(mainLayout);
	}

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

		table.AddRow(BuildCommandsSection(), DisplayDefaultConversions(), BuildInstructionsSection());
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

	private static string BuildFlagsSection()
	{
		return @"path -ca    [lightskyblue1]Convert image(s)[/]
path -png  [lightskyblue1]Convert to png[/]
-jpg             [lightskyblue1]Convert to .jpg[/]";
	}

	private static string DisplayDefaultConversions()
	{
		string conversions = string.Join("\n", DefaultConversions.Select(c => $"[steelblue]{c.Key} -> {c.Value}[/]"));

		return $@"[grey85 underline]Conversion Formats[/]
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

	public static void Write(string output)
	{
		AnsiConsole.WriteLine(output);
	}

	public static void WriteException(string output)
	{
		AnsiConsole.MarkupLine($"[darkred]{output}[/]");
		Console.ReadKey();
	}

	public static string Read(string input)
	{
		return AnsiConsole.Ask<string>(input);
	}
}