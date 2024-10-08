﻿using MediatR;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Application.Exceptions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Application.Common.Input.Video;
using Flixer.Catalog.Application.Common.Output.Video;

namespace Flixer.Catalog.Application.Commands.Video;

public class UpdateVideo : IRequestHandler<UpdateVideoInput, VideoOutput>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVideoRepository _videoRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICastMemberRepository _castMemberRepository;

    public UpdateVideo(
        IUnitOfWork unitOfWork,
        IVideoRepository videoRepository,
        IGenreRepository genreRepository, 
        ICategoryRepository categoryRepository, 
        ICastMemberRepository castMemberRepository
    ) 
    {
        _unitOfWork = unitOfWork;
        _videoRepository = videoRepository;
        _genreRepository = genreRepository;
        _categoryRepository = categoryRepository;
        _castMemberRepository = castMemberRepository;
    }

    public async Task<VideoOutput> Handle(UpdateVideoInput input, CancellationToken cancellationToken)
    {
        var video = await _videoRepository.GetById(input.VideoId);
        
        if (video == null)
            NotFoundException.ThrowIfNull(video, $"Video '{video!.Id}' not found.");
        
        video.Update(
            input.Title,
            input.Description,
            input.YearLaunched,
            input.Opened,
            input.Published,
            input.Duration,
            input.Rating);

        await ValidateAndAddRelations(input, video);

        _videoRepository.Update(video);
        await _unitOfWork.Commit();
        
        return VideoOutput.FromVideo(video);
    }
    
     private async Task ValidateAndAddRelations(
        UpdateVideoInput input, 
        Domain.Entities.Video video
    )
    {
        if(input.GenresIds is not null)
        {
            video.RemoveAllGenres();
            if(input.GenresIds.Count > 0)
            {
                await ValidateGenresIds(input);
                input.GenresIds!.ToList().ForEach(video.AddGenre);
            }
        }

        if(input.CategoriesIds is not null)
        {
            video.RemoveAllCategories();
            if(input.CategoriesIds.Count > 0)
            {
                await ValidateCategoriesIds(input);
                input.CategoriesIds!.ToList().ForEach(video.AddCategory);
            }
        }

        if(input.CastMembersIds is not null)
        {
            video.RemoveAllCastMembers();
            if(input.CastMembersIds.Count > 0)
            {
                await ValidateCastMembersIds(input);
                input.CastMembersIds!.ToList().ForEach(video.AddCastMember);
            }
        }
    }

    private async Task ValidateGenresIds(UpdateVideoInput input)
    {
        var persistenceIds = await _genreRepository
            .GetIdsListByIdsAsync(input.GenresIds!.ToList());
        
        if (persistenceIds.Count < input.GenresIds!.Count)
        {
            var notFoundIds = input.GenresIds!.ToList()
                .FindAll(id => !persistenceIds.Contains(id));
            
            throw new RelatedAggregateException(
                $"Related genre id (or ids) not found: {string.Join(',', notFoundIds)}.");
        }
    }

    private async Task ValidateCategoriesIds(UpdateVideoInput input)
    {
        var persistenceIds = await _categoryRepository
            .GetIdsListByIds(input.CategoriesIds!.ToList());
        
        if (persistenceIds.Count < input.CategoriesIds!.Count)
        {
            var notFoundIds = input.CategoriesIds!.ToList()
                .FindAll(id => !persistenceIds.Contains(id));
            
            throw new RelatedAggregateException(
                $"Related category id (or ids) not found: {string.Join(',', notFoundIds)}.");
        }
    }

    private async Task ValidateCastMembersIds(UpdateVideoInput input)
    {
        var persistenceIds = await _castMemberRepository
            .GetIdsListByIds(input.CastMembersIds!.ToList());
        
        if (persistenceIds.Count < input.CastMembersIds!.Count)
        {
            var notFoundIds = input.CastMembersIds!.ToList()
                .FindAll(id => !persistenceIds.Contains(id));
            
            throw new RelatedAggregateException(
                $"Related cast member(s) id (or ids) not found: {string.Join(',', notFoundIds)}.");
        }
    }
}