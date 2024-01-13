using MediatR;
using Flixer.Catalog.Application.Dtos.InputModel.Category;

namespace Flixer.Catalog.Application.Contracts.Category;

public interface IDeleteCategory : IRequestHandler<DeleteCategoryInputModel>
{

}