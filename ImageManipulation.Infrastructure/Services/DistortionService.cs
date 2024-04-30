using System.Diagnostics;
using ImageManipulation.Domain.Services;
using ImageMagick;
using Microsoft.Extensions.Logging;

namespace ImageManipulation.Infrastructure.Services;

public class DistortionService : IDistortionService
{
    private readonly ILogger<DistortionService> _logger;
    private const string TempDirectory = "Temp";

    public DistortionService(ILogger<DistortionService> logger)
    {
        _logger = logger;
    }

    public byte[] DistortImageAsync(byte[] imageBytes)
    {
        var stopwatch = new Stopwatch();
        _logger.LogInformation("Start distroting image");
        stopwatch.Start();
        using var image = new MagickImage(imageBytes);
        var imageWidth = image.Width;
        var imageHeight = image.Height;
        image.LiquidRescale(new Percentage(40), new Percentage(40), 1, 0);
        image.Resize(imageWidth, imageHeight);
        var result = image.ToByteArray();
        stopwatch.Stop();
        _logger.LogInformation("Image distorted. Elapsed time: {Elapsed} sec", stopwatch.ElapsedMilliseconds / 1000.0);
        return result;
    }

    public async Task<byte[]> DistortVideoAsync(byte[] video)
    {
        var distortVideoStopwatch = new Stopwatch();
        distortVideoStopwatch.Start();
        var stopwatch = new Stopwatch();

        _logger.LogInformation("Start distroting video");
        var id = Guid.NewGuid();
        var fileTempDir = Path.Combine(TempDirectory, id.ToString());
        try
        {
            if (!Directory.Exists(fileTempDir))
            {
                Directory.CreateDirectory(fileTempDir);
                _logger.LogInformation("Frames directory created: {FileTempDir}", fileTempDir);
            }

            var videFilePath = Path.Combine(fileTempDir, $"{id.ToString()}.mp4");


            stopwatch.Start();
            await File.WriteAllBytesAsync(videFilePath, video);
            stopwatch.Stop();

            _logger.LogInformation("File saved to file system : {VideFilePath} {Length} Kb. Elapsed time: {Elapsed} sec", fileTempDir, video.Length / 1000, stopwatch.ElapsedMilliseconds / 1000.0);

            using (var process = new Process())
            {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.FileName = "ffmpeg";
                process.StartInfo.Arguments = $"-i {videFilePath} -r 15 {Path.Combine(fileTempDir, id.ToString())}_%d.png";

                stopwatch.Restart();
                process.Start();
                await process.WaitForExitAsync();
                stopwatch.Stop();

                _logger.LogInformation("Frames extracted. Elapsed time: {Elapsed} sec", stopwatch.ElapsedMilliseconds / 1000.0);
            }

            var frames = Directory.GetFiles(fileTempDir, $"{id.ToString()}*.png")
                .OrderBy(x => x.Length)
                .ThenBy(x => x)
                .Select(x => new MagickImage(x))
                .ToList();

            using var result = new MagickImageCollection();
            _logger.LogInformation("Starting distorting frames. Total frames: {Frames}", frames.Count);
            var frameNum = 1;
            foreach (var image in frames)
            {
                stopwatch.Restart();
                var imageWidth = image.Width;
                var imageHeight = image.Height;
                image.LiquidRescale(new Percentage(40), new Percentage(40), 1, 0);
                image.Resize(imageWidth, imageHeight);
                result.Add(image);
                stopwatch.Stop();
                _logger.LogInformation("Frame distorted. Frame number: {FrameNumber}. Elapsed time: {Elapsed} sec", frameNum++, stopwatch.ElapsedMilliseconds / 1000.0);
            }

            _logger.LogInformation("Frames destorted. Total frames: {Frames}", frames.Count);

            using (var memoryStream = new MemoryStream())
            {
                await result.WriteAsync(memoryStream, MagickFormat.Mp4);
                distortVideoStopwatch.Stop();
                _logger.LogInformation("Distorting video finished. Elapsed time: {Elapsed} sec", distortVideoStopwatch.ElapsedMilliseconds / 1000.0);
                return memoryStream.ToArray();
            }
        }
        finally
        {
            Directory.Delete(fileTempDir, true);
        }
    }
}