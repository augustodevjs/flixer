using FluentValidation;
using Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.Domain.Validation;

public class CastMemberValidator : AbstractValidator<CastMember>
{
    public CastMemberValidator()
    {
        RuleFor(castMember => castMember.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .Length(3, 255).WithMessage("Name must be between 3 and 255 characters.");
    }
}