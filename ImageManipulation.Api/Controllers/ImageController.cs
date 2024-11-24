using ImageManipulation.Application.Dto;
using ImageManipulation.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ImageManipulation.Api.Controllers;

[ApiController]
[Route("image")]
public class ImageController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ImageController> _logger;

    public ImageController(IMediator mediator, ILogger<ImageController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [Produces(typeof(ImageDistortionQueryResult))]
    [Route("distort-image")]
    public async Task<IActionResult> DistortImage(DistortDto distort)
    {
        _logger.LogInformation("wpwpwp");
        if (string.IsNullOrEmpty(distort.Base64))
        {
            return BadRequest($"{nameof(DistortDto.Base64)} is empty");
        }

        var result = await _mediator.Send(new ImageDistortionQuery(distort.Base64));
        return Ok(result);
    }

    [HttpPost]
    [Produces(typeof(VideoDistortionQueryResult))]
    [Route("distort-video")]
    public async Task<IActionResult> DistortVideo(DistortDto distort)
    {
        if (string.IsNullOrEmpty(distort.Base64))
        {
            return BadRequest($"{nameof(DistortDto.Base64)} is empty");
        }

        var result = await _mediator.Send(new VideoDistortionQuery(distort.Base64));
        return Ok(result);
    }
}