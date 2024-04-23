namespace ImageManipulation.Application.Queries;

public class ImageDistortionQueryResult
{
    public ImageDistortionQueryResult(byte[] distortedImageAsBase64)
    {
        DistortedImageAsBase64 = distortedImageAsBase64;
    }

    public byte[] DistortedImageAsBase64 { get; }
}