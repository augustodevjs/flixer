using MediatR;
using Microsoft.AspNetCore.Mvc;
using Flixer.Catalog.Application.Dtos.InputModel.Category;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;

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

    [HttpPost]
    [ProducesResponseType(typeof(CategoryViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryInputModel input, CancellationToken cancellationToken)
    {
        var output = await _mediator.Send(input, cancellationToken);
        
        return CreatedAtAction(nameof(Create), new { output.Id }, output);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CategoryViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var output = await _mediator.Send(new GetCategoryInputModel(id), cancellationToken);

        return Ok(output);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCategoryInputModel(id), cancellationToken);

        return NoContent();
    }
}
