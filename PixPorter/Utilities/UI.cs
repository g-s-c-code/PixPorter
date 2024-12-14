using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;
using static Constants;

public static class UI
{
	private const int LayoutWidth = 100;

	public static void RenderUI(IEnumerable<string> directoryTree, IEnumerable<string> fileTree)
	{
		var infoTable = CreateInformationTable();
		var currentDirectoryPanel = CreateCurrentDirectoryPanel();
		var directoryTable = CreateDirectoryContentTable(directoryTree, fileTree);

		var rightColumnPanel = CreateRightColumnPanel(currentDirectoryPanel, directoryTable);
		var layoutTable = CreateLayoutTable(infoTable, rightColumnPanel);

		AnsiConsole.Clear();
		AnsiConsole.Write(layoutTable);
	}

	private static Panel CreateRightColumnPanel(Panel currentDirectoryPanel, Table directoryTable)
	{
		return new Panel(new Rows([currentDirectoryPanel, directoryTable]))
		{
			BorderStyle = Color.LightSkyBlue1,
			Header = new PanelHeader("Current Directory".ToUpper()),
			Padding = new Padding(1, 1, 1, 0),
		};
	}

	private static Table CreateLayoutTable(Table infoTable, Panel rightColumnPanel)
	{
		var layoutTable = new Table();
		layoutTable.AddColumn(new TableColumn(infoTable));
		layoutTable.AddColumn(new TableColumn(rightColumnPanel));
		layoutTable.Columns[0].Padding = new Padding(0);
		layoutTable.Columns[1].Padding = new Padding(0);
		layoutTable.Border = TableBorder.Horizontal;
		return layoutTable;
	}

	private static Table CreateInformationTable()
	{
		var table = new Table
		{
			Border = TableBorder.None,
			Width = LayoutWidth
		};

		table.AddColumn(new TableColumn("")).HideHeaders();

		var sections = new[]
		{
		CreateSection("Usage Instructions".ToUpper(), new[]
		{
			("Drag & Drop", "Drag an image into the window, then press 'Enter' to convert it."),
			("Folder Navigation", "Use 'cd [[path]]' to navigate to a folder, then use flag '-ca' to convert all files.")
		}),
		CreateSection("Commands".ToUpper(), new[]
		{
			("cd", "Change the current working directory."),
			("q", "Quit the application."),
			("help", "Display help information.")
		}),
		CreateSection("Flags".ToUpper(), new[]
		{
			("-ca", "Convert all files in a directory."),
			("-png", "Set output format to PNG."),
			("-jpg", "Set output format to JPG."),
			("-jpeg", "Set output format to JPEG."),
			("-webp", "Set output format to WEBP.")
		}),
		CreateSection("Default Conversion Formats".ToUpper(),
			DefaultConversions.Select(c => (c.Key, $"{c.Key} -> {c.Value}")).ToArray())
	};

		foreach (var section in sections)
		{
			table.AddRow(section);
		}

		return table;
	}

	private static IRenderable CreateSection(string header, (string Key, string Description)[] items)
	{
		var sectionContent = new StringBuilder();
		sectionContent.AppendLine($"[lightskyblue1 bold]{header}[/]");

		foreach (var (key, description) in items)
		{
			sectionContent.AppendLine($"[grey85]{key}[/]: {description}");
		}

		return new Markup(sectionContent.ToString());
	}

	private static Panel CreateCurrentDirectoryPanel()
	{
		var currentDirectory = new TextPath(Directory.GetCurrentDirectory());

		return new Panel(currentDirectory)
		{
			Border = BoxBorder.None,
			Padding = new Padding(2, 1),
			Width = LayoutWidth
		};
	}

	private static Table CreateDirectoryContentTable(IEnumerable<string> directoryTree, IEnumerable<string> fileTree)
	{
		var table = new Table
		{
			Border = TableBorder.Minimal,
			Width = LayoutWidth
		};

		table.AddColumn(new TableColumn(CreateDirectoryTree("Folders:", directoryTree)));
		table.AddColumn(new TableColumn(CreateDirectoryTree("Image Files:", fileTree)));
		table.Columns[0].PadBottom(0);
		table.Columns[1].PadBottom(0);

		return table;
	}
	private static IRenderable CreateDirectoryTree(string header, IEnumerable<string> items)
	{
		var tree = new Tree(header)
		{
			Style = new Style(foreground: Color.RosyBrown)
		};

		foreach (var node in !items.Any()
			? new[] { "[dim italic]None[/]" }
			: items.Select(item => $"[bold white]{item}[/]"))
		{
			tree.AddNode(node);
		}

		return tree;
	}

	public static void Write(string output)
	{
		AnsiConsole.WriteLine(output);
	}

	public static void WriteAndWait(string output)
	{
		AnsiConsole.MarkupLine($"[rosybrown]{output}[/]");
		Console.ReadKey();
	}

	public static string Read(string prompt)
	{
		return AnsiConsole.Ask<string>(prompt);
	}
}