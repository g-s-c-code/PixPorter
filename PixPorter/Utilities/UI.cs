using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;
using static Constants;

public static class UI
{
	private const int LayoutWidth = 150;

	public static void RenderUI(IEnumerable<string> directories, IEnumerable<string> files)
	{
		var informationTable = BuildInformationTable();
		var currentDirectoryPanel = BuildCurrentDirectoryPanel();
		var directoryContentTable = BuildDirectoryContentTable(directories, files);

		var rightPanel = BuildRightPanel(currentDirectoryPanel, directoryContentTable);
		var layoutTable = BuildLayoutTable(informationTable, rightPanel);

		AnsiConsole.Clear();
		AnsiConsole.Write(layoutTable);
	}

	public static void RenderHelpUI(IEnumerable<string> directories, IEnumerable<string> files)
	{
		AnsiConsole.Clear();
		AnsiConsole.Write(BuildHelpTable());
		AnsiConsole.Prompt(new SelectionPrompt<string>()
			   .Title("Navigate:")
			   .AddChoices("Back to Main Menu")); 
		RenderUI(directories, files);
	}

	private static Panel BuildRightPanel(Panel directoryPanel, Table contentTable)
	{
		return new Panel(new Rows([directoryPanel, contentTable]))
		{
			BorderStyle = Color.LightSkyBlue1,
			Header = new PanelHeader("Current Directory".ToUpper()),
			Padding = new Padding(1, 1, 1, 0),
		};
	}

	private static Table BuildLayoutTable(Table informationTable, Panel rightPanel)
	{
		var layoutTable = new Table
		{
			Border = TableBorder.Horizontal
		};

		layoutTable.AddColumn(new TableColumn(informationTable) { Padding = new Padding(0) });
		layoutTable.AddColumn(new TableColumn(rightPanel) { Padding = new Padding(0) });

		return layoutTable;
	}

	private static Table BuildInformationTable()
	{
		var table = new Table
		{
			Border = TableBorder.None,
			Width = LayoutWidth
		};

		table.AddColumn(new TableColumn(string.Empty)).HideHeaders();

		var sections = new[]
		{
			BuildSection("Usage Instructions", new[]
			{
				("[rosybrown]Drag & Drop[/]", "Drag an image into the window, then press 'Enter' to convert it."),
				("[rosybrown]Folder Navigation[/]", "Use 'cd [[path]]' to navigate to a folder, then use flag '-ca' to convert all image files in that folder.")
			}),
			BuildSection("Commands", new[]
			{
				("[rosybrown]cd[/]", "Change the current working directory."),
				("[rosybrown]q | e[/]", "Exit the application."),
				("[rosybrown]help[/]", "Display help information.")
			}),
			BuildSection("Flags", new[]
			{
				("[rosybrown]-ca[/]", "Convert all files in a directory."),
				("[rosybrown]-png[/]", "Set output format to PNG."),
				("[rosybrown]-jpg[/]", "Set output format to JPG."),
				("[rosybrown]-jpeg[/]", "Set output format to JPEG."),
				("[rosybrown]-webp[/]", "Set output format to WEBP.")
			}),
			BuildSection("Default Conversion Formats", DefaultConversions.Select(c => ($"", $"{c.Key} -> {c.Value}")).ToArray())
		};

		foreach (var section in sections)
		{
			table.AddRow(section);
		}

		return table;
	}

	private static IRenderable BuildSection(string title, (string Key, string Value)[] items)
	{
		var sectionContent = new StringBuilder().AppendLine($"[lightskyblue1 bold]{title.ToUpper()}[/]");

		foreach (var (key, value) in items)
		{
			sectionContent.AppendLine($"[grey85]{key}[/]: {value}");
		}

		return new Markup(sectionContent.ToString());
	}

	private static Panel BuildCurrentDirectoryPanel()
	{
		return new Panel(new TextPath(Directory.GetCurrentDirectory()))
		{
			Border = BoxBorder.None,
			Padding = new Padding(2, 1),
			Width = LayoutWidth
		};
	}

	private static Table BuildDirectoryContentTable(IEnumerable<string> directories, IEnumerable<string> files)
	{
		var table = new Table
		{
			Border = TableBorder.Minimal,
			Width = LayoutWidth
		};

		table.AddColumn(new TableColumn(BuildTree("Folders:", directories)));
		table.AddColumn(new TableColumn(BuildTree("Image Files:", files)));

		return table;
	}

	private static IRenderable BuildTree(string header, IEnumerable<string> items)
	{
		var tree = new Tree(header)
		{
			Style = new Style(foreground: Color.RosyBrown)
		};

		foreach (var node in items.DefaultIfEmpty("[dim italic]None[/]").Select(item => $"[bold white]{item}[/]"))
		{
			tree.AddNode(node);
		}

		return tree;
	}

	public static void Write(string message)
	{
		AnsiConsole.WriteLine(message);
	}

	public static void WriteAndWait(string message)
	{
		AnsiConsole.MarkupLine($"[rosybrown]{message}[/]");
		Console.ReadKey();
	}

	public static string Read(string prompt)
	{
		return AnsiConsole.Ask<string>(prompt);
	}

	public static class HelpContent
	{
		public static List<(string Title, string[] Details)> GetHelpSections()
		{
			return new List<(string, string[])>
			{
				("Overview", new[]
				{
					"PixPorter is a versatile image format conversion tool",
					"Convert images between PNG, JPG, JPEG, and WebP formats with ease"
				}),
				("Basic Commands", new[]
				{
					"'cd [[path]]': Change the current working directory",
					"'q': Exit the application",
					"'help': Display help information"
				}),
				("Conversion Commands", new[]
				{
					"Single File: Enter a valid image file path, followed by an optional format flag",
					"  Example: C:/path/to/image/image.png -webp",
					"Convert All Files: Enter a valid folder path containing one or more images, followed by an optional format flag",
					"  Example: C:/path/to/folder/containing/images -webp"
				}),
				("Supported Format Flags", new[]
				{
					"[[-png]]: Convert to PNG",
					"[[-jpg]]: Convert to JPG",
					"[[-jpeg]]: Convert to JPEG",
					"[[-webp]]: Convert to WebP"
				}),
				("Default Conversion Formats", DefaultConversions.Select(c => $"{c.Key} -> {c.Value}").ToArray()),
				("Tips", new[]
				{
					"Drag and drop images into the window",
					"Use full or relative paths",
					"Original images are preserved during conversion"
				})
			};
		}
	}

	private static Table BuildHelpTable()
	{
		var helpTable = new Table
		{
			Border = TableBorder.Horizontal,
			Width = LayoutWidth,
			Title = new TableTitle("PixPorter - Help Section".ToUpper())
		};

		helpTable.AddColumn(new TableColumn(string.Empty)).HideHeaders();

		foreach (var (title, details) in HelpContent.GetHelpSections())
		{
			helpTable.AddRow($"[bold yellow]{title}[/]");

			foreach (var detail in details)
			{
				helpTable.AddRow($"[white]{detail}[/]");
			}

			helpTable.AddRow(string.Empty);
		}

		return helpTable;
	}
}
