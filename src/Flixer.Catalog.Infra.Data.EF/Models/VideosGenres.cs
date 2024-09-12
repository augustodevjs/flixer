using Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.Infra.Data.EF.Models;

public class VideosGenres
{
    public Guid GenreId { get; set; }
    public Guid VideoId { get; set; }
    
    public Genre? Genre { get; set; }
    public Video? Video { get; set; }

    public VideosGenres(Guid genreId, Guid videoId)
    {
        GenreId = genreId;
        VideoId = videoId;
    }
}