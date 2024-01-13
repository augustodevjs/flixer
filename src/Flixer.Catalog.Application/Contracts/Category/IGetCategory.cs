using MediatR;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;
using Flixer.Catalog.Application.Dtos.InputModel.Category;

namespace Flixer.Catalog.Application.Contracts.Category;

public interface IGetCategory : IRequestHandler<GetCategoryInputModel, CategoryViewModel>
{
}
