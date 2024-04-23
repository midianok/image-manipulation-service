namespace ImageManipulation.Domain.Services;

public interface IDistortionService
{
    byte[] DistortImageAsync(byte[] image);

    Task<byte[]> DistortVideoAsync(byte[] video);
}