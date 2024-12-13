using Spectre.Console;
using Spectre.Console.Rendering;
using static Constants;

public static class UI
{
	private const int LayoutWidth = 100;

	public static void RenderUI(IEnumerable<string> directoriesTree, IEnumerable<string> filesTree)
	{
		var layout = CreateMainLayout(directoriesTree, filesTree);

		var mainTable = new Table()
			.AddColumn(new TableColumn(layout))
			.Border(TableBorder.Horizontal);

		AnsiConsole.Clear();
		AnsiConsole.Write(mainTable);
	}

	private static Layout CreateMainLayout(IEnumerable<string> directoriesTree, IEnumerable<string> filesTree)
	{
		var infoTable = CreateInfoTable();
		var currentDirectoryPanel = CreateCurrentDirectoryPanel();
		var directoryTable = CreateDirectoryTable(directoriesTree, filesTree);

		// Create a panel for the entire Right column content
		var rightColumnPanel = new Panel(new Rows(new IRenderable[]
		{
		currentDirectoryPanel,
        directoryTable
		}))
		{
			BorderStyle = Color.SteelBlue,
			Header = new PanelHeader("Current Directory"),
			Padding = new Padding(1),
		};

		return new Layout("Root")
			.SplitColumns(
				new Layout("Left").Update(infoTable),
				new Layout("Right").Update(rightColumnPanel));
	}

	private static Table CreateInfoTable()
	{
		var table = new Table
		{
			Width = LayoutWidth,
			Border = TableBorder.None
		};

		table.AddColumn(new TableColumn(BuildInstructionsSection()));
		table.AddEmptyRow();
		table.AddRow(DisplayCommands());
		table.AddEmptyRow();
		table.AddRow(DisplayFlags());
		table.AddEmptyRow();
		table.AddRow(DisplayDefaultConversions());

		return table;
	}

	private static Panel CreateCurrentDirectoryPanel()
	{
		var currentDirectory = new TextPath(Directory.GetCurrentDirectory());

		return new Panel(currentDirectory)
		{
			Border = BoxBorder.None,
			Padding = new Padding(0, 0, 2, 0),
			Width = LayoutWidth,
		};
	}

	private static Table CreateDirectoryTable(IEnumerable<string> directoriesTree, IEnumerable<string> filesTree)
	{
		var table = new Table
		{
			Width = LayoutWidth,
			Border = TableBorder.None
		};

		table.AddColumn(new TableColumn(CreateTree("Folders:", directoriesTree)));
		table.AddColumn(new TableColumn(CreateTree("Image Files:", filesTree)));

		return table;
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

	private static string BuildInstructionsSection()
	{
		return @"[grey85 underline]Drag & Drop[/]
[lightskyblue1]Drag an image into the app window to auto-fill its path, then press 'Enter' to convert.[/]

[grey85 underline]Folder Conversion[/]
[lightskyblue1]Navigate to the folder containing your images and type 'convert' to process all images in that folder.[/]";
	}

	private static string DisplayCommands()
	{
		var commands = new Dictionary<string, string>
		{
			{ "convert", "Convert a single file to a different format." },
			{ "convert-all", "Convert all files in the current directory to a different format." },
			{ "cd", "Change the current working directory." },
			{ "q", "Quit the application." },
			{ "help", "Display help information." }
		};

		var formattedCommands = string.Join("\n", commands.Select(c => $"[steelblue]{c.Key}[/]: {c.Value}"));
		return $"[grey85 underline]Commands[/]\n{formattedCommands}";
	}

	private static string DisplayFlags()
	{
		var flags = new Dictionary<string, string>
		{
			{ "-c", "Convert a specific file." },
			{ "-ca", "Convert all files in a directory." },
			{ "-png", "Set output format to PNG." },
			{ "-jpg", "Set output format to JPG." },
			{ "-jpeg", "Set output format to JPEG." },
			{ "-webp", "Set output format to WEBP." }
		};

		var formattedFlags = string.Join("\n", flags.Select(f => $"[steelblue]{f.Key}[/]: {f.Value}"));
		return $"[grey85 underline]Flags[/]\n{formattedFlags}";
	}

	private static string DisplayDefaultConversions()
	{
		var conversions = string.Join("\n", DefaultConversions.Select(c => $"[steelblue]{c.Key} -> {c.Value}[/]"));
		return $"[grey85 underline]Conversion Formats[/]\n{conversions}";
	}

	public static void Write(string output)
	{
		AnsiConsole.WriteLine(output);
	}

	public static void WriteAndWait(string output)
	{
		AnsiConsole.MarkupLine($"[darkred]{output}[/]");
		Console.ReadKey();
	}

	public static string Read(string input)
	{
		return AnsiConsole.Ask<string>(input);
	}
}
