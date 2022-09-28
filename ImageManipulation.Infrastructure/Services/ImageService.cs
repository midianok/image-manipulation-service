using ImageManipulation.Domain.Services;
using ImageMagick;

namespace ImageManipulation.Infrastructure.Services;

public class ImageService : IImageService
{
    public byte[] Distort(byte[] imageBytes)
    {
        using var image = new MagickImage(imageBytes);
        var imageWidth = image.Width;
        var imageHeight = image.Height;

        image.LiquidRescale(new MagickGeometry("40x40%!"));
        image.Resize(imageWidth, imageHeight);
        return image.ToByteArray();
    }
}