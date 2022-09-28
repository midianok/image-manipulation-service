namespace ImageManipulation.Application.Queries;

public class ImageDistortionQueryResult
{
    public ImageDistortionQueryResult(byte[] distortImageAsBase64)
    {
        DistortImageAsBase64 = distortImageAsBase64;
    }

    public byte[] DistortImageAsBase64 { get; }
}