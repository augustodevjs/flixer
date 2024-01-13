using MediatR;
using Flixer.Catalog.Application.Dtos.ViewModel.Category;
using Flixer.Catalog.Application.Dtos.InputModel.Category;

namespace Flixer.Catalog.Application.Contracts.UseCases.Category;

public interface IUpdateCategory : IRequestHandler<UpdateCategoryInputModel, CategoryViewModel>
{
}
