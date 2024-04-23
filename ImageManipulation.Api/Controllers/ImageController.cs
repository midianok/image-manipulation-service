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

    public ImageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("distort-image")]
    public async Task<IActionResult> DistortImage(DistortDto distort)
    {
        if (string.IsNullOrEmpty(distort.Base64))
        {
            return BadRequest($"{nameof(DistortDto.Base64)} is empty");
        }

        var result = await _mediator.Send(new ImageDistortionQuery(distort.Base64));
        return Ok(result);
    }

    [HttpPost]
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