using ImageManipulation.Application.Queries;
using ImageManipulation.Domain.Services;
using MediatR;

namespace ImageManipulation.Application.Handlers;

public class ImageDistortionHandler : IRequestHandler<ImageDistortionQuery, ImageDistortionQueryResult>
{
    private readonly IImageService _imageService;

    public ImageDistortionHandler(IImageService imageService)
    {
        _imageService = imageService;
    }

    public Task<ImageDistortionQueryResult> Handle(ImageDistortionQuery request, CancellationToken cancellationToken)
    {
        var buffer = new Span<byte>(new byte[request.ImageAsBase64.Length]);
        if (!Convert.TryFromBase64String(request.ImageAsBase64, buffer, out _))
        {
            throw new Exception($"{nameof(ImageDistortionQuery.ImageAsBase64)} is not valid Base64 string");
        }

        var result = _imageService.Distort(buffer.ToArray());
        return Task.FromResult(new ImageDistortionQueryResult(result));
    }
}