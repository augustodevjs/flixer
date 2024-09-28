using Flixer.Catalog.Application.Common.Input.Video;
using Flixer.Catalog.Domain.Extensions;

namespace Flixer.Catalog.Api.ApiModels.Video;

public class CreateVideoApiInput
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int YearLaunched { get; set; }
    public bool Opened { get; set; }
    public bool Published { get; set; }
    public int Duration { get; set; }
    public string? Rating { get; set; }
    public List<Guid>? CategoriesId { get; set; }
    public List<Guid>? GenresId { get; set; }
    public List<Guid>? CastMembersId { get; set; }

    public CreateVideoInput ToCreateVideoInput()
        => new(
            Title!,
            Description!,
            YearLaunched,
            Opened,
            Published,
            Duration,
            Rating!.ToRating(),
            CategoriesId?.AsReadOnly(),
            GenresId?.AsReadOnly(),
            CastMembersId?.AsReadOnly()
        );
}