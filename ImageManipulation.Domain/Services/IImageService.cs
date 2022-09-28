namespace ImageManipulation.Domain.Services;

public interface IImageService
{
    byte[] Distort(byte[] image);
}