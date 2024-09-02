using MediatR;
using Microsoft.AspNetCore.Mvc;
using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Api.Response;
using Flixer.Catalog.Application.Common.Input.CastMember;
using Flixer.Catalog.Application.Common.Output.CastMember;

namespace Flixer.Catalog.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CastMemberController : ControllerBase
{
    private readonly IMediator _mediator;

    public CastMemberController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseList<CastMemberOutput>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        CancellationToken cancellationToken,
        [FromQuery] int? page = null,
        [FromQuery(Name = "per_page")] int? perPage = null,
        [FromQuery] string? search = null,
        [FromQuery] string? sort = null,
        [FromQuery] SearchOrder? dir = null
    )
    {
        var input = new ListCastMembersInput();

        if (page is not null) input.Page = page.Value;
        if (perPage is not null) input.PerPage = perPage.Value;
        if (string.IsNullOrEmpty(search) is false) input.Search = search;
        if (string.IsNullOrEmpty(sort) is false) input.Sort = sort;
        if (dir is not null) input.Dir = dir.Value;

        var result = await _mediator.Send(input, cancellationToken);
        
        var output = new ApiResponseList<CastMemberOutput>(result);

        return Ok(output);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CastMemberOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCastMemberInput(id), cancellationToken);
        var output = new ApiResponse<CastMemberOutput>(result);
        
        return Ok(output);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CastMemberOutput>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateCastMemberInput input, CancellationToken cancellationToken)
    {
        var output = await _mediator.Send(input, cancellationToken);
        return CreatedAtAction(nameof(Create), new { output.Id }, new ApiResponse<CastMemberOutput>(output));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CastMemberOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromBody] UpdateCastMemberInput input, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(input, cancellationToken);
        var output = new ApiResponse<CastMemberOutput>(result);
        return Ok(output);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCastMemberInput(id), cancellationToken);
        return NoContent();
    }
}