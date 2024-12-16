# PixPorter - Image Format Converter

## Overview
PixPorter is a versatile image format converter built on .NET 8, designed to simplify and streamline image conversions. It supports popular formats like PNG, JPG, JPEG, and WebP, providing options for both individual and batch processing.

## Features
- **Single File Conversion**: Convert specific image files to another format.
- **Batch Conversion**: Transform all images in a directory at once.
- **Directory Navigation**: Browse folders to locate and process images.
- **Drag and Drop**: Instantly convert images via drag-and-drop functionality.
- **Supported Formats**: PNG, JPG, JPEG, WebP.

## Planned Features
- **Support for Additional Formats**: Adding compatibility for image formats such as TIFF, BMP, and GIF.
- **Image Compression**: Introducing options for configurable compression levels to optimize file sizes while maintaining quality.
- **Enhanced Metadata Handling**: Providing options to preserve or strip metadata during conversion.

## Getting Started

### Download and Run
PixPorter provides pre-built executables for Windows, macOS, and Linux:

| Platform  | Download Link                                      | Instructions                  |
|-----------|----------------------------------------------------|-------------------------------|
| Windows   | [PixPorter-win-x64](Releases/PixPorter-windows-x64.exe) | Run `PixPorter.exe`.         |
| macOS     | [PixPorter-osx-x64](Releases/PixPorter-osx-x64)    | `chmod +x PixPorter` and `./PixPorter`. |
| Linux     | [PixPorter-linux-x64](Releases/PixPorter-linux-x64) | `chmod +x PixPorter` and `./PixPorter`. |

#### Windows SmartScreen Warning
When running on Windows, a SmartScreen warning may appear as the file is unsigned:
1. Click **More Info**.
2. Select **Run Anyway**.

Alternatively, build the project yourself using Visual Studio:

   ```bash
   git clone https://github.com/yourusername/PixPorter.git
   cd PixPorter
   dotnet build
   dotnet run
   ```

### Requirements
- .NET 8.0 or later
- [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp)

## Usage
When running PixPorter, the application launches an interactive prompt to navigate directories, view images, and perform conversions.

### Commands
- **`cd [path]`**: Navigate to a directory.
- **`[file]`**: Convert a specific file.
- **`--ca`**: Convert all files in a directory.
- **`--png`, `--jpg`, `--jpeg`, `--webp`**: Target format flags.
- **`help`**: Display help menu.
- **`q`**: Quit.

#### Examples
Convert all images in the current directory to WebP:
```bash
--ca --webp
```
Convert a single image to JPG:
```bash
my_image.png --jpg
```
Change the working directory:
```bash
cd C:\Users\Pictures or cd C:/Users/Pictures
```
Display the help menu:
```bash
help
```

### Drag and Drop
Drag and drop an image into the PixPorter window to automatically convert it to the default target format.

## Contributing
Contributions are welcome! To contribute:
1. Fork the repository.
2. Create a feature branch.
3. Commit your changes.
4. Push to your branch.
5. Open a pull request for review.

## License
PixPorter is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgments
Special thanks to [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) for providing a powerful and flexible image processing library.

