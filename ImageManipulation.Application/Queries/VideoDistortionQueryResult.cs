namespace ImageManipulation.Application.Queries;

public class VideoDistortionQueryResult
{
    public VideoDistortionQueryResult(byte[] distortedVideoAsBase64)
    {
        DistortedVideoAsBase64 = distortedVideoAsBase64;
    }

    public byte[] DistortedVideoAsBase64 { get; }
}