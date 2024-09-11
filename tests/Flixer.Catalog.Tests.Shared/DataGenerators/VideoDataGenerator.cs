using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Domain.Enums;

namespace Flixer.Catalog.Tests.Shared.DataGenerators;

public class VideoDataGenerator : DataGeneratorBase
{
    public Video GetValidVideo() => new(
        GetValidTitle(),
        GetValidDescription(),
        GetValidYearLaunched(),
        GetRandomBoolean(),
        GetRandomBoolean(),
        GetValidDuration(),
        GetRandomRating()
    );

    public string GetValidTitle()
        => Faker.Lorem.Letter(100);
    
    public Rating GetRandomRating()
    {
        var enumValue = Enum.GetValues<Rating>();
        var random = new Random();
        return enumValue[random.Next(enumValue.Length)];
    }

    public string GetValidDescription()
        => Faker.Commerce.ProductDescription();

    public string GetTooLongDescription()
        => Faker.Lorem.Letter(4001);

    public int GetValidYearLaunched()
        => Faker.Date.BetweenDateOnly(
            new DateOnly(1960, 1, 1), 
            new DateOnly(2022, 1, 1)
        ).Year;

    public int GetValidDuration()
        => (new Random()).Next(100, 300);

    public string GetTooLongTitle()
        => Faker.Lorem.Letter(400);
    
    public string GetValidImagePath()
        => Faker.Image.PlaceImgUrl();
}