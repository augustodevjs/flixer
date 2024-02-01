using MediatR;
using Microsoft.AspNetCore.Mvc;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;
using Flixer.Catalog.Application.Dtos.InputModel.Category;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;

namespace Flixer.Catalog.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListCategoriesViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
           CancellationToken cancellationToken,
           [FromQuery] int? page = null,
           [FromQuery] int? perPage = null,
           [FromQuery] string? sort = null,
           [FromQuery] string? search = null,
           [FromQuery] SearchOrder? dir = null
       )
    {
        var input = new ListCategoriesInputModel();

        if (dir is not null) input.Dir = dir.Value;
        if (page is not null) input.Page = page.Value;
        if (perPage is not null) input.PerPage = perPage.Value;
        if (!String.IsNullOrWhiteSpace(sort)) input.Sort = sort;
        if (!String.IsNullOrWhiteSpace(search)) input.Search = search;

        var output = await _mediator.Send(input, cancellationToken);

        return Ok(output);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CategoryViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var output = await _mediator.Send(new GetCategoryInputModel(id), cancellationToken);
        return Ok(output);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryInputModel input, CancellationToken cancellationToken)
    {
        var output = await _mediator.Send(input, cancellationToken);
        return CreatedAtAction(nameof(Create), new { output.Id }, output);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CategoryViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryInputModel input, CancellationToken cancellationToken)
    {
        var output = await _mediator.Send(input, cancellationToken);
        return Ok(output);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(CategoryViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCategoryInputModel(id), cancellationToken);
        return NoContent();
    }
}