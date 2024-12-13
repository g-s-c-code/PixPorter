using Spectre.Console;
using Spectre.Console.Rendering;
using System.Collections.Generic;
using System.Linq;
using static Constants;

public static class UI
{
	private const int LayoutWidth = 100;

	public static void RenderUI(IEnumerable<string> directoriesTree, IEnumerable<string> filesTree)
	{
		var layout = CreateMainLayout(directoriesTree, filesTree);

		var mainTable = new Table()
			.AddColumn(new TableColumn(layout));

		AnsiConsole.Clear();
		AnsiConsole.Write(mainTable);
	}

	private static Layout CreateMainLayout(IEnumerable<string> directoriesTree, IEnumerable<string> filesTree)
	{
		var instructionsTable = CreateInstructionsTable();
		var currentDirectoryPanel = CreateCurrentDirectoryPanel();
		var directoryTable = CreateDirectoryTable(directoriesTree, filesTree);

		return new Layout("Root")
			.SplitColumns(
				new Layout("Left").Update(instructionsTable),
				new Layout("Right")
					.SplitRows(
						new Layout("TopRight").Size(3).Update(currentDirectoryPanel),
						new Layout("BottomRight").Update(directoryTable)));
	}

	private static Table CreateInstructionsTable()
	{
		var table = new Table
		{
			Width = LayoutWidth,
			Border = TableBorder.None
		};

		table.AddColumn(new TableColumn(BuildInstructionsSection()));
		table.AddEmptyRow();
		table.AddRow(DisplayDefaultConversions());

		return table;
	}

	private static Panel CreateCurrentDirectoryPanel()
	{
		return new Panel($"Current Directory: {Directory.GetCurrentDirectory()}")
		{
			Width = LayoutWidth,
			Border = BoxBorder.None,
			Padding = new Padding(0)
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
