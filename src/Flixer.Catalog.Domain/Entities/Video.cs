using FluentValidation.Results;
using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Domain.SeedWork;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Domain.Validation;
using Flixer.Catalog.Domain.ValueObject;

namespace Flixer.Catalog.Domain.Entities;

public class Video : AggregateRoot
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int YearLaunched { get; private set; }
    public bool Opened { get; private set; }
    public bool Published { get; private set; }
    public int Duration { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Rating Rating { get; private set; }
    
    public Image? Thumb { get; private set; }
    public Image? ThumbHalf { get; private set; }
    public Image? Banner { get; private set; }

    private List<Guid> _categories;
    public IReadOnlyList<Guid> Categories => _categories.AsReadOnly();

    private List<Guid> _genres;
    public IReadOnlyList<Guid> Genres => _genres.AsReadOnly();
    
    private List<Guid> _castMembers;
    public IReadOnlyList<Guid> CastMembers => _castMembers.AsReadOnly();
    
    public Media? Media { get; private set; }
    public Media? Trailer { get; private set; }

    public Video(
        string title, 
        string description, 
        int yearLaunched, 
        bool opened, 
        bool published, 
        int duration,
        Rating rating
    )
    {
        Title = title;
        Opened = opened;
        Rating = rating;
        Duration = duration;
        Published = published;
        Description = description;
        YearLaunched = yearLaunched;
        CreatedAt = DateTime.Now;
        
        _genres = new List<Guid>();
        _categories = new List<Guid>();
        _castMembers = new List<Guid>();

        ValidateAndThrow();
    }
    
    public void Update(
        string title,
        string description,
        int yearLaunched,
        bool opened,
        bool published,
        int duration,
        Rating? rating = null)
    {
        Title = title;
        Opened = opened;
        Duration = duration;
        Published = published;
        Description = description;
        YearLaunched = yearLaunched;
        
        if (rating is not null)
            Rating = (Rating) rating;

        ValidateAndThrow();
    }

    public void UpdateThumb(string path)
        => Thumb = new Image(path);
    
    public void UpdateThumbHalf(string path)
        => ThumbHalf = new Image(path);

    public void UpdateBanner(string path)
        => Banner = new Image(path);
    
    public void UpdateMedia(string path)
        => Media = new Media(path);
    
    public void UpdateTrailer(string path)
        => Trailer = new Media(path);

    public void UpdateAsSentToEncode()
    {
        if (Media is null)
            throw new EntityValidationException("There is no Media", null);
        
        Media.UpdateAsSentToEncode();
    }
    
    public void AddCategory(Guid categoryId)
        => _categories.Add(categoryId);

    public void RemoveCategory(Guid categoryId)
        => _categories.Remove(categoryId);

    public void RemoveAllCategories()
        => _categories = new List<Guid>();
    
    public void AddGenre(Guid genreId)
        => _genres.Add(genreId);

    public void RemoveGenre(Guid genreId)
        => _genres.Remove(genreId);

    public void RemoveAllGenres()
        => _genres = new List<Guid>();
    
    public void AddCastMember(Guid castMemberId)
        => _castMembers.Add(castMemberId);

    public void RemoveCastMember(Guid castMemberId)
        => _castMembers.Remove(castMemberId);

    public void RemoveAllCastMembers()
        => _castMembers = new List<Guid>();
    
    private bool Validate(out ValidationResult validationResult)
    {
        validationResult = new VideoValidator().Validate(this);
        return validationResult.IsValid;
    }

    private void ValidateAndThrow()
    {
        if (Validate(out var validationResult)) return;
        
        var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        
        throw new EntityValidationException("Video is invalid", errors);
    }
}