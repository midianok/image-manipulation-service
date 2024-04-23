namespace ImageManipulation.Application.Queries;

public class ImageDistortionQueryResult
{
    public ImageDistortionQueryResult(byte[] base64)
    {
        Base64 = base64;
    }

    public byte[] Base64 { get; }
}