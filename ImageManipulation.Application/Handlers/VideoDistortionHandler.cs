using ImageManipulation.Application.Queries;
using ImageManipulation.Domain.Services;
using MediatR;

namespace ImageManipulation.Application.Handlers;

public class VideoDistortionHandler : IRequestHandler<VideoDistortionQuery, VideoDistortionQueryResult>
{
    private readonly IDistortionService _distortionService;

    public VideoDistortionHandler(IDistortionService distortionService)
    {
        _distortionService = distortionService;
    }

    public async Task<VideoDistortionQueryResult> Handle(VideoDistortionQuery request, CancellationToken cancellationToken)
    {
        var bytes = Convert.FromBase64String(request.VideoAsBase64);
        var result = await _distortionService.DistortVideoAsync(bytes);
        return new VideoDistortionQueryResult(result);
    }
}