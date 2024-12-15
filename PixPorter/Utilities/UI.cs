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
			Title = new TableTitle("PixPorter - Image Converter and Compressor")
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
			BuildSection("Usage Instructions", new[]
			{
				("[rosybrown]Drag & Drop[/]", "Drag an image into the window, then press 'Enter' to convert it."),
				("[rosybrown]Folder Navigation[/]", "Use 'cd [[path]]' to navigate to a folder, then use the '-ca' flag to convert [underline]all[/] image files in that folder, or simply write the name of image you wish to convert.")
			}),
			BuildSection("Commands", new[]
			{
				("[rosybrown]cd[/]", "Change the current working directory."),
				("[rosybrown]q[/]", "Exit the application."),
				("[rosybrown]help[/]", "Display help information.")
			}),
			BuildSection("Flags", new[]
			{
				("[rosybrown]-ca[/]", "Convert all files in a directory."),
				("[rosybrown]-png[/]", "Set output format to PNG."),
				("[rosybrown]-jpg[/]", "Set output format to JPG."),
				("[rosybrown]-webp[/]", "Set output format to WEBP.")
			}),
			BuildSection("Default Conversion Formats",
				Constants.DefaultConversions
					.Where(c => c.Key != Constants.FileFormats.Jpeg)
					.Select(c => ($"", $"{c.Key} -> {c.Value}"))
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
			return new List<string>
			{
				"PixPorter - a simple tool for converting image files",
				"",
				"[yellow underline bold]Features:[/]",
				"- Convert images between PNG, JPG, JPEG, and WebP formats.",
				"- Process individual files or entire folders with ease.",
				"",
				"[yellow underline bold]Quick Start Guide:[/]",
				"PixPorter can be used as a command-line tool or a drag-and-drop utility. Here are the outline for each approach:",
				"- Drag & drop image files or folders containing image files into the PixPorter window and hit enter for immediate action. This will convert all images to the default conversion formats. You can also add format flags after the file or folder path to specify the output format.",
				"2. To convert a single image to a specific format, type the file path and, optionally, specify the format (e.g., 'image.png -webp').",
				"3. To batch convert a folder, provide the folder path (e.g., 'folder-path -jpg').",
				"   - If the folder path is prefixed with 'cd ', it will navigate to the directory instead.",
				"4. To convert all images in the current directory, use the '-ca' flag.",
				"",
				"[yellow underline bold]Usage Tips:[/]",
				"- Use 'cd [path]' to change directories.",
				"- Enter 'help' for this guide or 'q' to quit the application.",
				"",
				"[yellow underline bold]Flags and Their Functions:[/]",
				"- '-png': Convert to PNG format.",
				"- '-jpg': Convert to JPG format.",
				"- '-jpeg': Convert to JPEG format.",
				"- '-webp': Convert to WebP format.",
				"- '-ca': Convert all supported files in the current directory.",
				"",
				"[yellow underline bold]Default Conversion Mapping:[/]",
				string.Join(", ", Constants.DefaultConversions.Select(c => $"{c.Key} -> {c.Value}")),
				"",
				"For additional details, consult the application documentation or type 'help' at any time!"
			};
		}

	}
	private static Table HelpContentUI()
	{
		var table = new Table
		{
			Border = TableBorder.Horizontal,
			Width = LayoutWidth,
			Title = new TableTitle("PixPorter - Help Section")
		};

		table.AddColumn(new TableColumn(string.Empty)).HideHeaders();

		foreach (var detail in HelpContent.GetHelpDetails())
		{
			table.AddRow($"[white]{detail}[/]");
		}

		return table;
	}
}
