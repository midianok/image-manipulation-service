using MediatR;

namespace ImageManipulation.Application.Queries;

public class VideoDistortionQuery : IRequest<VideoDistortionQueryResult>
{
    public VideoDistortionQuery(string videoAsBase64)
    {
        VideoAsBase64 = videoAsBase64;
    }

    public string VideoAsBase64 { get; }
}