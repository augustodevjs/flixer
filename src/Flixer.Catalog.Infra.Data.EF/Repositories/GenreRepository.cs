using Flixer.Catalog.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Infra.Data.EF.Models;
using Flixer.Catalog.Infra.Data.EF.Context;
using Flixer.Catalog.Infra.Data.EF.Abstractions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;

namespace Flixer.Catalog.Infra.Data.EF.Repositories;

public class GenreRepository : Repository<Genre>, IGenreRepository
{
    public GenreRepository(FlixerCatalogDbContext context) : base(context)
    {
    }

    public override async Task<Genre?> GetById(Guid? id)
    {
        var genre = await Context.Genres
            .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        
        var categoryIds = await Context.GenresCategories
            .Where(x => x.GenreId == genre!.Id)
            .Select(x => x.CategoryId)
            .ToListAsync();

        foreach (var categoryId in categoryIds)
        {
            genre?.AddCategory(categoryId);
        }

        return genre;
    }

    public override void Create(Genre genre)
    {
        Context.Genres.Add(genre);

        if (genre.Categories.Count <= 0) return;
        
        var relations = genre.Categories
            .Select(categoryId => new GenresCategories(
                categoryId,
                genre.Id
            ));
            
        Context.GenresCategories.AddRange(relations);
    }

    public override void Update(Genre genre)
    {
        var genresCategories = Context.GenresCategories
            .Where(x => x.GenreId == genre.Id);
        
        Context.Genres.Update(genre);
        Context.GenresCategories.RemoveRange(genresCategories);

        if (genre.Categories.Count <= 0) return;
        
        var relations = genre.Categories
            .Select(categoryId => new GenresCategories(categoryId, genre.Id));
            
        Context.GenresCategories.AddRange(relations);
    }

    public override void Delete(Genre genre)
    {
        var genresCategories = Context.GenresCategories
            .Where(x => x.GenreId == genre.Id);
        
        Context.GenresCategories.RemoveRange(genresCategories);

        Context.Genres.Remove(genre);
    }
    
    public async Task<IReadOnlyList<Guid>> GetIdsListByIdsAsync(List<Guid> ids)
    {
        return await Context.Genres.AsNoTracking()
            .Where(genre => ids.Contains(genre.Id))
            .Select(genre => genre.Id)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Genre>> GetListByIdsAsync(List<Guid> ids)
    {
        return await Context.Genres.AsNoTracking()
            .Where(genre => ids.Contains(genre.Id))
            .ToListAsync();
    }

    public async Task<SearchOutput<Genre>> Search(SearchInput input)
    {
        var toSkip = (input.Page - 1) * input.PerPage;

        var query = Context.Genres.AsNoTracking();

        query = AddOrderToQuery(query, input.OrderBy, input.Order);

        if (string.IsNullOrEmpty(input.Search) is not true)
            query = query.Where(genre => genre.Name!.Contains(input.Search));

        var total = await query.CountAsync();

        var genres = await query
            .Skip(toSkip)
            .Take(input.PerPage)
            .ToListAsync();

        var genresIds = genres.Select(genre => genre.Id);

        var relations = await Context.GenresCategories
            .Where(relation => genresIds.Contains(relation.GenreId))
            .ToArrayAsync();

        var relationsByGenreIdGroup = relations.GroupBy(x => x.GenreId).ToList();
        
        relationsByGenreIdGroup.ForEach(relationGroup =>
        {
            var genre = genres.Find(genre => genre.Id == relationGroup.Key);
            
            if (genre is null) return;

            relationGroup.ToList()
                .ForEach(relation => genre.AddCategory(relation.CategoryId));
        });

        return new SearchOutput<Genre>(total, input.PerPage, input.Page, genres);
    }

    private IQueryable<Genre> AddOrderToQuery(IQueryable<Genre> query, string orderProperty, SearchOrder order)
    {
        var orderedQuery = (orderProperty.ToLower(), order) switch
        {
            ("name", SearchOrder.Asc) => query.OrderBy(x => x.Name).ThenBy(x => x.Id.ToString()),
            ("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name).ThenByDescending(x => x.Id.ToString()),
            ("id", SearchOrder.Asc) => query.OrderBy(x => x.Id.ToString()),
            ("id", SearchOrder.Desc) => query.OrderByDescending(x => x.Id.ToString()),
            ("createdat", SearchOrder.Asc) => query.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderBy(x => x.Name).ThenBy(x => x.Id.ToString())
        };

        return orderedQuery;
    }
}