using System.Diagnostics;
using ImageManipulation.Domain.Services;
using ImageMagick;

namespace ImageManipulation.Infrastructure.Services;

public class DistortionService : IDistortionService
{
    private const string TempDirectory = "Temp";

    public byte[] DistortImageAsync(byte[] imageBytes)
    {
        using var image = new MagickImage(imageBytes);
        var imageWidth = image.Width;
        var imageHeight = image.Height;

        image.LiquidRescale(new MagickGeometry("40x40%!"));
        image.Resize(imageWidth, imageHeight);
        return image.ToByteArray();
    }

    public async Task<byte[]> DistortVideoAsync(byte[] video)
    {
        var id = Guid.NewGuid();
        var fileTempDir = Path.Combine(TempDirectory, id.ToString());
        try
        {
            if (!Directory.Exists(fileTempDir))
            {
                Directory.CreateDirectory(fileTempDir);
            }

            var videFilePath = Path.Combine(fileTempDir, $"{id.ToString()}.mp4");
            await File.WriteAllBytesAsync(videFilePath, video);

            using (var process = new Process())
            {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "ffmpeg";
                process.StartInfo.Arguments =
                    $"-i {videFilePath} -r 15 {Path.Combine(fileTempDir, id.ToString())}_%d.png";
                process.Start();
                await process.WaitForExitAsync();
            }

            var frames = Directory.GetFiles(fileTempDir, $"{id.ToString()}*.png")
                .OrderBy(x => x.Length)
                .ThenBy(x => x)
                .Select(x => new MagickImage(x));

            using var images = new MagickImageCollection(frames);


            var result = new MagickImageCollection();
            foreach (var image in images)
            {
                var imageWidth = image.Width;
                var imageHeight = image.Height;
                image.LiquidRescale(new MagickGeometry("40x40%!"));
                image.Resize(imageWidth, imageHeight);
                result.Add(image);
            }

            using (var memoryStream = new MemoryStream())
            {
                await result.WriteAsync(memoryStream, MagickFormat.Mp4);
                return memoryStream.ToArray();
            }
        }
        finally
        {
            Directory.Delete(fileTempDir, true);
        }
    }
}