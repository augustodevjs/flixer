using FluentValidation;
using Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.Domain.Validation;

public class VideoValidator : AbstractValidator<Video>
{
    public VideoValidator()
    {
        RuleFor(video => video.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(255).WithMessage($"Title should be less or equal {255} characters long");

        RuleFor(video => video.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage($"Description should be less or equal {500} characters long");
    }
}