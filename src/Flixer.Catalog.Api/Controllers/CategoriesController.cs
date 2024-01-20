using Flixer.Catalog.Application.Dtos.InputModel.Category;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
}
