using Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.Infra.Data.EF.Models;

public class VideosCategories
{
    public Guid VideoId { get; set; }
    public Guid CategoryId { get; set; }
    
    public Video? Video { get; set; }
    public Category? Category { get; set; }

    public VideosCategories(Guid categoryId, Guid videoId)
    {
        VideoId = videoId;
        CategoryId = categoryId;
    }
}