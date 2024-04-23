namespace ImageManipulation.Application.Queries;

public class VideoDistortionQueryResult
{
    public VideoDistortionQueryResult(byte[] base64)
    {
        Base64 = base64;
    }

    public byte[] Base64 { get; }
}