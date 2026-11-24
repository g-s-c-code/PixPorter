using ImageMagick;
using PixPorter.Common.Core;

namespace PixPorter.Common.Helpers;

public static class ConversionHelper
{
    public static string ConvertFile(
        string inputPath,
        string? targetExtension,
        int? quality = null,
        bool stripMetadata = false,
        string? outputDirectory = null,
        IReadOnlySet<string>? sourcesToPreserve = null)
    {
        string sourceExtension = Path.GetExtension(inputPath);
        string outputExtension = targetExtension ?? Constants.GetDefaultTarget(sourceExtension);
        string outputPath = ResolveOutputPath(inputPath, outputExtension, outputDirectory, sourcesToPreserve);

        // Decode with the coder that matches the extension, so only PixPorter's own formats are ever
        // parsed. Without this, ImageMagick sniffs the content and could route a renamed file into
        // any of its 200+ coders.
        var readSettings = new MagickReadSettings { Format = ToMagickFormat(sourceExtension) };
        MagickFormat outputFormat = ToMagickFormat(outputExtension);

        // A collection keeps every frame, so animated GIF/WebP and multi-page TIFF survive.
        using var frames = new MagickImageCollection(inputPath, readSettings);

        foreach (IMagickImage<byte> frame in frames)
        {
            if (stripMetadata)
                frame.Strip();

            if (quality.HasValue)
                frame.Quality = MapQuality(outputExtension, quality.Value);

            if (!SupportsTransparency(outputFormat) && frame.HasAlpha)
            {
                frame.BackgroundColor = MagickColors.White;
                frame.Alpha(AlphaAction.Remove);
            }
        }

        bool keepAllFrames = frames.Count > 1 && MagickFormatInfo.Create(outputFormat)?.SupportsMultipleFrames == true;

        // Writing several frames to a single-frame format would make ImageMagick emit name-0, name-1, ...
        if (keepAllFrames)
            frames.Write(outputPath, outputFormat);
        else
            frames[0].Write(outputPath, outputFormat);

        return outputPath;
    }

    private static string ResolveOutputPath(
        string inputPath,
        string outputExtension,
        string? outputDirectory,
        IReadOnlySet<string>? sourcesToPreserve)
    {
        string directory = string.IsNullOrWhiteSpace(outputDirectory)
            ? Path.GetDirectoryName(inputPath) ?? string.Empty
            : outputDirectory;

        Directory.CreateDirectory(directory);

        string baseName = Path.GetFileNameWithoutExtension(inputPath);
        string candidate = Path.Combine(directory, baseName + outputExtension);

        return WouldDestroyASource(candidate, inputPath, sourcesToPreserve)
            ? Path.Combine(directory, $"{baseName}-converted{outputExtension}")
            : candidate;
    }

    private static bool WouldDestroyASource(
        string candidate,
        string inputPath,
        IReadOnlySet<string>? sourcesToPreserve)
    {
        string resolved = Path.GetFullPath(candidate);

        return string.Equals(resolved, Path.GetFullPath(inputPath), StringComparison.OrdinalIgnoreCase)
            || (sourcesToPreserve?.Contains(resolved) ?? false);
    }

    public static IEnumerable<string> GetConvertibleFiles(string directory) =>
        Directory.GetFiles(directory).Where(f => Constants.SupportedExtensions.Contains(Path.GetExtension(f)));

    private static MagickFormat ToMagickFormat(string extension) => extension.ToLowerInvariant() switch
    {
        ".png" => MagickFormat.Png,
        ".jpg" or ".jpeg" => MagickFormat.Jpeg,
        ".webp" => MagickFormat.WebP,
        ".gif" => MagickFormat.Gif,
        ".bmp" => MagickFormat.Bmp,
        ".tiff" => MagickFormat.Tiff,
        ".tga" => MagickFormat.Tga,
        ".qoi" => MagickFormat.Qoi,
        ".pbm" => MagickFormat.Pbm,
        _ => throw new NotSupportedException($"{extension} files are not supported.")
    };

    private static bool SupportsTransparency(MagickFormat format) =>
        format is not (MagickFormat.Jpeg or MagickFormat.Pbm);

    private static uint MapQuality(string extension, int quality) => extension.ToLowerInvariant() switch
    {
        ".jpg" or ".jpeg" or ".webp" => (uint)quality,
        // For PNG, ImageMagick reads quality as (zlib level * 10) + filter; 5 is adaptive filtering.
        ".png" => (uint)(MapQualityToPngCompressionLevel(quality) * 10 + 5),
        _ => throw new NotSupportedException($"Quality is not supported for {extension} files.")
    };

    private static int MapQualityToPngCompressionLevel(int quality) =>
        (int)Math.Round(9.0 * (1.0 - ((quality - 1) / 99.0)));
}
