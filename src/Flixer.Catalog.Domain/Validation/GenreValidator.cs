using FluentValidation;
using Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.Domain.Validation
{
    public class GenreValidator : AbstractValidator<Genre>
    {
        public GenreValidator()
        {
            RuleFor(genre => genre.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .NotNull()
                .WithMessage("Name is require.");
        }
    }
}