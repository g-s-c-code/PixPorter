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

	public static void Write(string message)
	{
		AnsiConsole.WriteLine(message);
	}

	public static void WriteAndWait(string message, Color? color = null)
	{
		var styledMessage = new Markup(message, color ?? Color.RosyBrown);

		AnsiConsole.Write(styledMessage);
		Console.ReadKey();
	}

	public static string Read(string prompt)
	{
		return AnsiConsole.Ask<string>(prompt);
	}

	public static class HelpContent
	{
		public static List<string> GetHelpDetails()
		{
			return new List<string>
	{
		"Welcome to PixPorter, a simple yet powerful tool for converting and compressing image files!",
		"",
		"Here’s what PixPorter can do for you:",
		" - Convert images between PNG, JPG, JPEG, and WebP formats.",
		" - Compress images to save space while preserving quality.",
		"",
		"[yellow underline bold]Quick Start[/]".ToUpper(),
		"1. To convert a single image, enter the file path and specify the format you want (e.g., 'C:/path/image.png -webp').",
		"2. To convert all images in a folder, enter the folder path followed by the format (e.g., 'C:/path/to/folder -jpg').",
		"3. You can drag and drop image files or folders into the PixPorter window for instant processing.",
		"",
		"[yellow underline bold]What are 'flags' and how do they work?[/]".ToUpper(),
		" - A flag is a way to customize the conversion. Simply add the desired flag after your file or folder path.",
		" - For example: '-png' converts to PNG format, and '-webp' converts to WebP format.",
		"",
		"[yellow underline bold]Supported Format Flags:[/]",
		" - '-png'   : Convert to PNG format.",
		" - '-jpg'   : Convert to JPG format.",
		" - '-jpeg'  : Convert to JPEG format.",
		" - '-webp'  : Convert to WebP format.",
		"",
		"[yellow underline bold]Default Behaviors:[/]",
		" - PixPorter automatically applies default format conversions dependent on the input format: ",
		$"   {string.Join(", ", Constants.DefaultConversions.Select(c => $"{c.Key} -> {c.Value}"))}.",
		" - Your original images are always preserved, so you don’t have to worry about overwriting files.",
		"",
		"[yellow underline bold]Additional Features:[/]",
		" - Supports both full paths (e.g., 'C:/images/photo.png') and relative paths (e.g., './photo.png').",
		" - Exit the app anytime by typing 'q'.",
		" - Need this guide again? Just type 'help'.",
		"",
		"[yellow underline bold]Examples:[/]",
		" - To convert an image to WebP: 'C:/images/photo.png -webp'",
		" - To compress all images in a folder and convert to JPG: 'C:/images -jpg'",
		"",
		"[yellow underline bold]Tips for Beginners:[/]",
		" - Use the 'cd [[path]]' command to change the working directory if needed.",
		" - Drag and drop files or folders into the app to make processing even easier!",
		"",
		"Thank you for using PixPorter!",
		"",
		"Suppport the project by starring it on GitHub: [steelblue bold][link]https://github.com/g-s-c-code/PixPorter[/][/]",

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
