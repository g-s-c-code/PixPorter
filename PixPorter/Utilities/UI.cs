using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;

public static class UI
{
	private const int LayoutWidth = 150;

	public static void RenderUI(IEnumerable<string> directories, IEnumerable<string> files, bool displayHelp = false)
	{
		var leftPanel = InformationContentUI();
		var rightPanel = DirectoryContentUI(CurrentDirectoryPathUI(), CurrentDirectoryContentUI(directories, files));
		var pixPorterUI = displayHelp ? HelpContentUI() : PixPorterUI(leftPanel, rightPanel);

		AnsiConsole.Clear();
		AnsiConsole.Write(pixPorterUI);
	}

	private static Table PixPorterUI(Table leftPanel, Panel rightPanel)
	{
		var layoutTable = new Table
		{
			Border = TableBorder.Horizontal,
			Title = new TableTitle("PixPorter - A Simple Image Conversion Tool")
		};

		layoutTable.AddColumn(new TableColumn(leftPanel).Padding(0, 0));
		layoutTable.AddColumn(new TableColumn(rightPanel).Padding(0, 0));

		return layoutTable;
	}

	private static Panel DirectoryContentUI(Panel currentDirectoryPathUI, Table currentDirectoryContentUI)
	{
		return new Panel(new Rows([currentDirectoryPathUI, currentDirectoryContentUI]))
		{
			BorderStyle = Color.LightSkyBlue1,
			Header = new PanelHeader("[[ Current Directory ]]".ToUpper()),
			Padding = new Padding(0, 1, 0, 0),
		};
	}

	private static Table InformationContentUI()
	{
		var table = new Table
		{
			Border = TableBorder.None,
			Width = LayoutWidth
		};

		table.AddColumn(new TableColumn(string.Empty)).HideHeaders();
		table.Columns[0].Padding(0, 0, 0, 0);

		var sections = new[]
		{
		BuildSection("Usage Quick Guide", new[]
		{
			("[rosybrown]Drag & Drop[/]", "Drag an image, press [[ENTER]] to convert."),
			("[rosybrown]Navigation[/]", "Use 'cd [[path]]' to navigate to a folder. Convert all with '-ca'."),
			("[rosybrown]Conversion Flags[/]",
				"[rosybrown]-png[/]: Convert to PNG\n[rosybrown]-jpg[/]: Convert to JPG\n[rosybrown]-webp[/]: Convert to WebP\n[rosybrown]-ca[/]: Convert all files in current directory")
		}),
		BuildSection("Quick Commands", new[]
		{
			("[rosybrown]'cd [[path]]'[/]", "Change directory"),
			("[rosybrown]'q'[/]", "Exit application"),
			("[rosybrown]'help'[/]", "Show detailed instructions")
		}),
		BuildSection("Default Conversion Formats", Constants.DefaultConversions
			.Where(c => c.Key != Constants.FileFormats.Jpeg)
			.Select(c => (c.Key.ToString(), $"-> {c.Value}"))
			.ToArray())
	};

		foreach (var section in sections)
		{
			table.AddRow(section);
		}

		return table;
	}


	private static IRenderable BuildSection(string title, (string Key, string Value)[] items)
	{
		var sectionContent = new StringBuilder().AppendLine($"[lightskyblue1 underline bold]{title.ToUpper()}[/]");

		foreach (var (key, value) in items)
		{
			sectionContent.AppendLine($"[grey85]{key}[/]: {value}");
		}

		return new Markup(sectionContent.ToString());
	}

	private static Panel CurrentDirectoryPathUI()
	{
		var currentDirectory = new TextPath(Directory.GetCurrentDirectory().ToUpper())
			.RootColor(Color.White)
			.SeparatorColor(Color.RosyBrown)
			.StemColor(Color.White)
			.LeafColor(Color.LightSkyBlue1);

		return new Panel(currentDirectory)
		{
			Border = BoxBorder.None,
			Width = LayoutWidth
		};
	}

	private static Table CurrentDirectoryContentUI(IEnumerable<string> directories, IEnumerable<string> files)
	{
		var table = new Table
		{
			Border = TableBorder.Simple,
			Width = LayoutWidth
		};

		table.AddColumn(new TableColumn(BuildTree("Folders:".ToUpper(), directories)));
		table.AddColumn(new TableColumn(BuildTree("Image Files:".ToUpper(), files)));
		table.Columns[0].Padding(0, 0);
		table.Columns[1].Padding(0, 0);

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

	public static void Write(string message, Color? color = null)
	{
		var styledMessage = new Markup(message, color ?? Color.White);

		AnsiConsole.Write(styledMessage);
	}

	public static void WriteLine(string message)
	{
		AnsiConsole.WriteLine(message);
	}

	public static void WriteAndWait(string message, Color? color = null)
	{
		var styledMessage = new Markup(message, color ?? Color.White);

		AnsiConsole.Write(styledMessage);
		Console.ReadKey();
	}

	public static string ReadLine(string prompt)
	{
		return AnsiConsole.Ask<string>(prompt);
	}

	public static class HelpContent
	{
		public static List<string> GetHelpDetails()
		{
			return
[
"[lightskyblue1 underline bold]HOW TO USE:[/]",
"- Add format flags [italic]if[/] you need a specific output format.",
"- Format flags are optional. Default conversion mappings are pre-configured (see below).",
$"Default mappings: {string.Join(" | ", Constants.DefaultConversions.Select(c => $"[steelblue]{c.Key}[/] -> [steelblue]{c.Value}[/]"))}",
"",
"[rosybrown bold]DRAG & DROP[/]:",
"- Drag a file or folder into the PixPorter window. Add an optional format flag if desired.",
"   [bold]EXAMPLE: 'my_photo.png' + [[ENTER]] -> Converts to the default format (e.g., 'my_photo.webp').[/]",
"   [bold]EXAMPLE: 'my_photo.png -jpg' + [[ENTER]] -> Converts to JPG (e.g., 'my_photo.jpg').[/]",
"",
"[rosybrown bold]DIRECT FILE/FOLDER CONVERSION[/]:",
"- Enter a full file path or a folder path (and an optional format flat) + [[ENTER]] for automatic conversion.",
"   [bold]EXAMPLE: 'C:\\Users\\Pictures -webp' + [[ENTER]] -> Converts all images in the folder to WebP.[/]",
"",
"[rosybrown bold]CURRENT DIRECTORY CONVERSION[/]:",
"- Use the command line to navigate to a directory and perform conversions.",
"- 'cd [[path]]' -> Navigate to the desired directory.",
"- '-ca' -> Converts all images in the current directory.",
"   [bold]EXAMPLE: 'cd C:\\Users\\Photos' + [[ENTER]] -> Navigate to the directory.[/]",
"   [bold]EXAMPLE: '-ca -jpg' + [[ENTER]] -> Converts all images in the current directory to JPG.[/]",
"",
"[lightskyblue1 underline bold]CONVERSION FLAGS:[/]",
"'-png': Convert to PNG",
"'-jpg': Convert to JPG",
"'-webp': Convert to WebP",
"'-ca': Convert all files in the current directory",
			];
		}
	}
	private static Table HelpContentUI()
	{
		var table = new Table
		{
			Border = TableBorder.Horizontal,
			Width = LayoutWidth,
			Title = new TableTitle("PixPorter - A Simple Image Conversion Tool")
		};

		table.AddColumn(new TableColumn(string.Empty)).HideHeaders();

		foreach (var detail in HelpContent.GetHelpDetails())
		{
			table.AddRow($"[white]{detail}[/]");
		}

		return table;
	}
}
