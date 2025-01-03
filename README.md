# PixPorter - Image Format Converter

![PixPorter Logo](https://raw.githubusercontent.com/g-s-c-code/PixPorter/refs/heads/master/pixporter.webp)

## Overview
PixPorter is a versatile image format converter built on .NET 8, designed to simplify and streamline image conversions. It supports popular formats like PNG, JPG, JPEG, WebP, GIF, TIFF, and BMP, providing options for both individual and batch processing.

## Features
- **Single File Conversion**: Convert specific image files to another format.
- **Batch Conversion**: Transform all images in a directory at once.
- **Directory Navigation**: Browse folders to locate and process images.
- **Drag and Drop**: Instantly convert images via drag-and-drop functionality.
- **Supported Formats**: PNG, JPG, JPEG, WebP, GIF, TIFF, BMP.

## Planned Features - *Work In Progress*
- **Support for Additional Formats**: Adding further support for image processing features.
- **Image Compression**: Introducing options for configurable compression levels to optimize file sizes while maintaining quality.
- **Enhanced Metadata Handling**: Providing options to preserve or strip metadata during conversion.

## Getting Started

### Build and Create a Runnable

PixPorter provides source code that you can build and package into an executable file for Windows, macOS, and Linux. Before building and running PixPorter, make sure you have the **[.NET 8 SDK or later](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)** as it is required to build and run the application. Follow these steps to create and run your own executable:

1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/PixPorter.git
   cd PixPorter
   ```

2. **Build the project** (this creates the project output, but not yet an executable):
   ```bash
   dotnet build
   ```

3. **Publish the project to create a runnable executable**:
   - For **Windows**:
     ```bash
     dotnet publish -c Release -r win-x64 --self-contained
     ```
   - For **macOS**:
     ```bash
     dotnet publish -c Release -r osx-x64 --self-contained
     ```
   - For **Linux**:
     ```bash
     dotnet publish -c Release -r linux-x64 --self-contained
     ```

   This will generate a self-contained executable for your platform in the `bin/Release/net8.0/[platform]/publish/` directory.

4. **Run the application** by executing the generated file:
   - On **Windows**, it will be `PixPorter.exe` in the output folder.
   - On **macOS** or **Linux**, it will be a binary that you can run directly (`chmod +x PixPorter` may be required on Linux/macOS).

## Usage
When running PixPorter, the application launches an interactive prompt to navigate directories, view images, and perform conversions.

### Commands
- **`cd [path]`**: Navigate to a directory.
- **`[file]`**: Convert a specific file.
- **`--ca`**: Convert all files in a directory.
- **`--png`, `--jpg`, `--jpeg`, `--webp`, `--gif`, `--tiff`, `--bmp`**: Target format flags.
- **`help`**: Display help menu.
- **`q`**: Quit.

#### Examples

Convert a single image to default format (no flag needed):
```bash
vacation_photo.png
```

Convert a single image to WebP:
```bash
family_photo.jpg --webp
```

Convert all images in current directory to default format:
```bash
--ca
```

Convert all images in current directory to PNG:
```bash
--ca --png
```

Convert all images in a specific directory to JPEG:
```bash
C:\Users\Pictures --jpeg
```

Navigate to a directory:
```bash
cd C:\Users\Pictures
```
or
```bash
cd C:/Users/Pictures
```

Convert a file by dragging and dropping:
```
Simply drag any image file into the PixPorter window
```

Convert a directory by dragging and dropping:
```
Drag a folder into the PixPorter window to convert all images inside
```

Display the help menu:
```bash
help
```

Exit the application:
```bash
q
```

### Drag and Drop
Just drag and drop any image or folder into the PixPorter window:
- Single image: Converts to default format
- Directory: Converts all supported images inside

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
