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

        ValidateAndThrow();
    }
    
    public void Update(
        string title,
        string description,
        int yearLaunched,
        bool opened,
        bool published,
        int duration)
    {
        Title = title;
        Opened = opened;
        Duration = duration;
        Published = published;
        Description = description;
        YearLaunched = yearLaunched;

        ValidateAndThrow();
    }

    public void UpdateThumb(string path)
        => Thumb = new Image(path);
    
    public void UpdateThumbHalf(string path)
        => ThumbHalf = new Image(path);

    public void UpdateBanner(string path)
        => Banner = new Image(path);
    
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