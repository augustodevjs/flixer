using Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.Domain.Contracts.Repository;

public interface IVideoRepository : IRepository<Video>, ISearchableRepository<Video>
{
    
}