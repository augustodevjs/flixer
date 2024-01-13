using Flixer.Catalog.Application.Dtos.InputModel.Genre;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Genre.DeleteGenreUseCase;
internal class DeleteGenreInput : DeleteGenreInputModel
{
    public DeleteGenreInput(Guid id) : base(id)
    {
    }
}