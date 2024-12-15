# PixPorter - Image Format Converter

## Overview

PixPorter is a versatile image format converter built on .NET 8, designed to make converting image files simple and efficient. It supports popular formats such as PNG, JPG, JPEG, and WebP, and allows both single and batch conversions.

## Acknowledgments

This project utilizes the excellent [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) library for image processing. A huge thanks to the SixLabors team for their work.

## Features

- **Convert Individual Files**: Convert specific image files to another format.
- **Batch Conversion**: Transform all images in a directory to the desired format.
- **Directory Navigation**: Navigate through folders to access and process images.
- **Drag and Drop Support**: Instantly convert images via drag-and-drop functionality.
- **Supported Formats**: PNG, JPG, JPEG, and WebP.

## Getting Started

### Download and Run

PixPorter offers pre-built, runnable files for Windows, macOS, and Linux. These can be downloaded directly from the [releases page](#).

Alternatively, you can download the source code and build the project yourself using Visual Studio:

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/PixPorter.git
   cd PixPorter
   ```

2. Build the project:
   ```bash
   dotnet build
   ```

3. Run the tool:
   ```bash
   dotnet run
   ```

### Dependencies

- .NET 8.0 or later
- SixLabors.ImageSharp

## Usage

When running PixPorter, the application launches an interactive prompt where you can navigate directories, view image files, and execute conversions.

### Commands

- **`cd [path]`**: Change the current directory to the specified path.
- **`ca`**: Convert all images in the current directory.
- **`[file]`**: Convert a specific image file.
- **`q`**: Exit the application.
- **`help`**: Display a list of available commands.
- **`-png`, `-jpg`, `-jpeg`, `-webp`**: Optional format flags. Specify the target format for conversion.

### Example Usage

Convert all images in the current directory to WebP:
```bash
ca -webp
```

Convert a single image to JPG, using the optional -jpg flag:
```bash
my_image.png -jpg
```

Change the working directory:
```bash
cd C:\Users\Pictures
```

Display the help menu:
```bash
help
```

### Drag and Drop Support

Drag and drop an image into the PixPorter window to automatically convert it to a default target format.

## Supported Formats

PixPorter supports the following image format conversions:

- PNG
- JPG
- JPEG
- WebP

## Contributing

We welcome contributions to PixPorter! Here’s how you can get involved:

1. Fork the repository.
2. Create a feature branch for your changes.
3. Commit your updates.
4. Push to your branch.
5. Open a pull request for review.

## License

PixPorter is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgments

Special thanks to the [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) team for providing a powerful and flexible image processing library.

