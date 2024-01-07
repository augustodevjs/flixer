using MediatR;
using Flixer.Catalog.Application.UseCases.Category.Common;

namespace Flixer.Catalog.Application.UseCases.Category.UpdateCategory;

public interface IUpdateCategory : IRequestHandler<UpdateCategoryInput, CategoryModelOutput>
{
}
