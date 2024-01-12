﻿using MediatR;
using Flixer.Catalog.Application.UseCases.Genre.Common;

namespace Flixer.Catalog.Application.UseCases.Genre.UpdateGenre;

public class UpdateGenreInput : IRequest<GenreModelOutput>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool? IsActive { get; set; }
    public List<Guid>? CategoriesIds { get; set; }

    public UpdateGenreInput(
        Guid id,
        string name,
        bool? isActive = null,
        List<Guid>? categoriesIds = null
    )
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CategoriesIds = categoriesIds;
    }
}