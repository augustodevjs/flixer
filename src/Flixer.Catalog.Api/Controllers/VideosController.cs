using MediatR;
using Microsoft.AspNetCore.Mvc;
using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Api.ApiModels.Video;
using Flixer.Catalog.Api.ApiModels.Response;
using Flixer.Catalog.Application.Common.Input.Video;
using Flixer.Catalog.Application.Common.Output.Video;

namespace Flixer.Catalog.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class VideosController : ControllerBase
{
    private readonly IMediator _mediator;

    public VideosController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseList<VideoOutput>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        CancellationToken cancellationToken,
        [FromQuery] int? page = null,
        [FromQuery(Name = "per_page")] int? perPage = null,
        [FromQuery] string? search = null,
        [FromQuery] string? sort = null,
        [FromQuery] SearchOrder? dir = null
    )
    {
        var input = new ListVideosInput();
        
        if (page is not null) input.Page = page.Value;
        if (perPage is not null) input.PerPage = perPage.Value;
        if (!string.IsNullOrWhiteSpace(search)) input.Search = search;
        if (!string.IsNullOrWhiteSpace(sort)) input.Sort = sort;
        if (dir is not null) input.Dir = dir.Value;

        var result = await _mediator.Send(input, cancellationToken);
        
        var output = new ApiResponseList<VideoOutput>(result);
        
        return Ok(output);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VideoOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var output = await _mediator.Send(new GetVideoInput(id), cancellationToken);
        
        return Ok(new ApiResponse<VideoOutput>(output));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<VideoOutput>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateVideo([FromBody] CreateVideoApiInput request, CancellationToken cancellationToken)
    {
        var input = request.ToCreateVideoInput();
        var output = await _mediator.Send(input, cancellationToken);
        
        return CreatedAtAction(
            nameof(CreateVideo),
            new { id = output.Id },
            new ApiResponse<VideoOutput>(output)
        );
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VideoOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateVideo(
        [FromRoute] Guid id,
        [FromBody] UpdateVideoApiInput apiInput,
        CancellationToken cancellationToken)
    {
        var output = await _mediator.Send(
            apiInput.ToInput(id),
            cancellationToken
        );
        
        return Ok(new ApiResponse<VideoOutput>(output));
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVideo([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteVideoInput(id), cancellationToken);
        return NoContent();
    }
    
    [HttpPost("{id:guid}/medias/{type}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UploadMedia(
        [FromRoute] Guid id,
        [FromRoute] string type,
        [FromForm] UploadMediaApiInput apiInput,
        CancellationToken cancellationToken)
    {
        var input = apiInput.ToUploadMediasInput(id, type);
        await _mediator.Send(input, cancellationToken);
        
        return NoContent();
    }
}