using MediatR;

namespace ImageManipulation.Application.Queries;

public class ImageDistortionQuery : IRequest<ImageDistortionQueryResult>
{
    public ImageDistortionQuery(string imageAsBase64)
    {
        ImageAsBase64 = imageAsBase64;
    }

    public string ImageAsBase64 { get; }
}