using MediatR;
using Flixer.Catalog.Application.UseCases.Category.Common;

namespace Flixer.Catalog.Application.UseCases.Category.GetCategory;

public interface IGetCategory : IRequestHandler<GetCategoryInput, CategoryModelOutput>
{
}
