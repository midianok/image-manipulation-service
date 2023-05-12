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
    [Route("distort")]
    public async Task<IActionResult> Distort(DistortDto distort)
    {
        if (string.IsNullOrEmpty(distort.ImageAsBase64))
        {
            return BadRequest($"{nameof(DistortDto.ImageAsBase64)} is empty");
        }

        var result = await _mediator.Send(new ImageDistortionQuery(distort.ImageAsBase64));
        return Ok(result);
    }
}