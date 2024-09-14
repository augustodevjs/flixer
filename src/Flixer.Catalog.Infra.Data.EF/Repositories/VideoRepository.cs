using Flixer.Catalog.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Infra.Data.EF.Models;
using Flixer.Catalog.Infra.Data.EF.Context;
using Flixer.Catalog.Infra.Data.EF.Abstractions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;

namespace Flixer.Catalog.Infra.Data.EF.Repositories;

public class VideoRepository : Repository<Video>, IVideoRepository
    
{
    public VideoRepository(FlixerCatalogDbContext context) : base(context)
    {
    }

    public override void Create(Video video)
    {
        Context.Videos.AddAsync(video);

        if (video.Categories.Count > 0)
        {
            var relations = video.Categories
                .Select(categoryId => new VideosCategories(
                    categoryId,
                    video.Id
                ));

            Context.VideosCategories.AddRangeAsync(relations);
        }

        if (video.Genres.Count > 0)
        {
            var relations = video.Genres
                .Select(genreId => new VideosGenres(
                    genreId,
                    video.Id
                ));

            Context.VideosGenres.AddRangeAsync(relations);
        }

        if (video.CastMembers.Count <= 0) return;
        {
            var relations = video.CastMembers
                .Select(castMemberId => new VideosCastMembers(
                    castMemberId,
                    video.Id
                ));
            Context.VideosCastMembers.AddRangeAsync(relations);
        }
    }

    public override void Update(Video video)
    {
        Context.Videos.Update(video);
        
        Context.VideosCategories.RemoveRange(
            Context.VideosCategories
                .Where(x => x.VideoId == video.Id)
        );
        
        Context.VideosCastMembers.RemoveRange(
            Context.VideosCastMembers
                .Where(x => x.VideoId == video.Id)
        );
        
        Context.VideosGenres.RemoveRange(
            Context.VideosGenres.Where(x => x.VideoId == video.Id)    
        );
        
        if (video.Categories.Count > 0)
        {
            var relations = video.Categories
                .Select(categoryId => new VideosCategories(
                    categoryId,
                    video.Id
                ));
            
            Context.VideosCategories.AddRangeAsync(relations);
        }
        
        if (video.Genres.Count > 0)
        {
            var relations = video.Genres
                .Select(genreId => new VideosGenres(
                    genreId,
                    video.Id
                ));
            Context.VideosGenres.AddRangeAsync(relations);
        }
        
        if (video.CastMembers.Count > 0)
        {
            var relations = video.CastMembers
                .Select(castMemberId => new VideosCastMembers(
                    castMemberId,
                    video.Id
                ));
            Context.VideosCastMembers.AddRangeAsync(relations);
        }

        DeleteOrphanMedias(video);
    }

    public override void Delete(Video video)
    {
        Context.VideosCategories.RemoveRange(
            Context.VideosCategories
                .Where(x => x.VideoId == video.Id)
        );
        
        Context.VideosCastMembers.RemoveRange(
            Context.VideosCastMembers
                .Where(x => x.VideoId == video.Id)
        );
        
        Context.VideosGenres.RemoveRange(
            Context.VideosGenres
                .Where(x => x.VideoId == video.Id)
        );

        if (video.Trailer is not null)
            Context.Medias.Remove(video.Trailer);

        if (video.Media is not null)
            Context.Medias.Remove(video.Media);

        Context.Videos.Remove(video);
    }

    public override async Task<Video?> GetById(Guid? id)
    {
        var video = await Context.Videos
            .FirstOrDefaultAsync(video => video.Id == id);

        var categoryIds = await Context.VideosCategories
            .Where(x => x.VideoId == video!.Id)
            .Select(x => x.CategoryId)
            .ToListAsync();
        
        categoryIds.ForEach(video!.AddCategory);

        var genresIds = await Context.VideosGenres
            .Where(x => x.VideoId == video!.Id)
            .Select(x => x.GenreId)
            .ToListAsync();
        
        genresIds.ForEach(video!.AddGenre);

        var castMembersIds = await Context.VideosCastMembers
            .Where(x => x.VideoId == video!.Id)
            .Select(x => x.CastMemberId)
            .ToListAsync();
        
        castMembersIds.ForEach(video!.AddCastMember);

        return video;
    }
    
    public async Task<SearchOutput<Video>> Search(SearchInput input)
    {
        var toSkip = (input.Page - 1) * input.PerPage;
        var query = Context.Videos.AsNoTracking();

        if(!string.IsNullOrWhiteSpace(input.Search))
            query = query.Where(video => video.Title.Contains(input.Search));
        
        query = InsertOrderBy(input, query);

        var count = query.Count();
        var items = await query.Skip(toSkip).Take(input.PerPage)
            .ToListAsync();

        var videosIds = items.Select(video => video.Id).ToList();
        await AddCategoriesToVideos(items, videosIds);
        await AddGenresToVideos(items, videosIds);
        await AddCastMembersToVideos(items, videosIds);
        
        return new(
            input.Page,
            input.PerPage,
            count,
            items);
    }

    private async Task AddCategoriesToVideos(List<Video> items, List<Guid> videosIds)
    {
        var categoriesRelations = await Context.VideosCategories
            .Where(relation => videosIds.Contains(relation.VideoId))
            .ToListAsync();
        
        var relationsWithCategoriesByVideoId =
            categoriesRelations.GroupBy(x => x.VideoId).ToList();
        
        relationsWithCategoriesByVideoId.ForEach(relationGroup =>
        {
            var video = items.Find(video => video.Id == relationGroup.Key);
            
            if (video is null) return;
            
            relationGroup.ToList()
                .ForEach(relation => video.AddCategory(relation.CategoryId));
        });
    }

    private async Task AddGenresToVideos(List<Video> items, List<Guid> videosIds)
    {
        var genresRelations = await Context.VideosGenres
            .Where(relation => videosIds.Contains(relation.VideoId))
            .ToListAsync();
        
        var relationsWithGenresByVideoId =
            genresRelations.GroupBy(x => x.VideoId).ToList();
        
        relationsWithGenresByVideoId.ForEach(relationGroup =>
        {
            var video = items.Find(video => video.Id == relationGroup.Key);
            
            if (video is null) return;
            
            relationGroup.ToList()
                .ForEach(relation => video.AddGenre(relation.GenreId));
        });
    }

    private async Task AddCastMembersToVideos(List<Video> items, List<Guid> videosIds)
    {
        var castMembersRelations = await Context.VideosCastMembers
            .Where(relation => videosIds.Contains(relation.VideoId))
            .ToListAsync();
        
        var relationsWithCastMembersByVideoId =
            castMembersRelations.GroupBy(x => x.VideoId).ToList();
        
        relationsWithCastMembersByVideoId.ForEach(relationGroup =>
        {
            var video = items.Find(video => video.Id == relationGroup.Key);
            
            if (video is null) return;
            
            relationGroup.ToList()
                .ForEach(relation => video.AddCastMember(relation.CastMemberId));
        });
    }

    private static IQueryable<Video> InsertOrderBy(SearchInput input, IQueryable<Video> query)
    {
        return input switch
        {
            { Order: SearchOrder.Asc } when input.OrderBy.ToLower() is "title"
                => query.OrderBy(video => video.Title).ThenBy(video => video.Id),
            { Order: SearchOrder.Desc } when input.OrderBy.ToLower() is "title"
                => query.OrderByDescending(video => video.Title).ThenByDescending(video => video.Id),
            { Order: SearchOrder.Asc } when input.OrderBy.ToLower() is "id"
                => query.OrderBy(video => video.Id),
            { Order: SearchOrder.Desc } when input.OrderBy.ToLower() is "id"
                => query.OrderByDescending(video => video.Id),
            { Order: SearchOrder.Asc } when input.OrderBy.ToLower() is "createdat"
                => query.OrderBy(video => video.CreatedAt),
            { Order: SearchOrder.Desc } when input.OrderBy.ToLower() is "createdat"
                => query.OrderByDescending(video => video.CreatedAt),
            _ => query.OrderBy(video => video.Title).ThenBy(video => video.Id)
        };
    }

    private void DeleteOrphanMedias(Video video)
    {
        if (Context.Entry(video).Reference(v => v.Trailer).IsModified)
        {
            var oldTrailerId = Context.Entry(video)
                .OriginalValues.GetValue<Guid?>($"{nameof(Video.Trailer)}Id");
            
            if (oldTrailerId != null && oldTrailerId != video.Trailer?.Id)
            {
                var oldTrailer = Context.Medias.Find(oldTrailerId);
                Context.Medias.Remove(oldTrailer!);
            }
        }

        if (Context.Entry(video).Reference(v => v.Media).IsModified)
        {
            var oldMediaId = Context.Entry(video)
                .OriginalValues.GetValue<Guid?>($"{nameof(Video.Media)}Id");
            
            if (oldMediaId != null && oldMediaId != video.Media?.Id)
            {
                var oldMedia = Context.Medias.Find(oldMediaId);
                Context.Medias.Remove(oldMedia!);
            }
        }
    }
}