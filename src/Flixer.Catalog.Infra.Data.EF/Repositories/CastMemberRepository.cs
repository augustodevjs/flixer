using Flixer.Catalog.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Infra.Data.EF.Context;
using Flixer.Catalog.Infra.Data.EF.Abstractions;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Domain.SeedWork.SearchableRepository;

namespace Flixer.Catalog.Infra.Data.EF.Repositories;

public class CastMemberRepository  : Repository<CastMember>, ICastMemberRepository
{
    public CastMemberRepository(FlixerCatalogDbContext context) : base(context)
    {
    }

    public async Task<SearchOutput<CastMember>> Search(SearchInput input)
    {
        var toSkip = (input.Page - 1) * input.PerPage;
        var query = Context.CastMembers.AsNoTracking();

        query = AddOrderToQuery(query, input.OrderBy, input.Order);

        if (!String.IsNullOrWhiteSpace(input.Search))
            query = query.Where(x => x.Name.Contains(input.Search));

        var total = await query.CountAsync();

        var items = await query
            .Skip(toSkip)
            .Take(input.PerPage)
            .ToListAsync();

        return new SearchOutput<CastMember>(total, input.PerPage, input.Page, items);
    }
    
    private IQueryable<CastMember> AddOrderToQuery(
        IQueryable<CastMember> query,
        string orderProperty,
        SearchOrder order
    ) => (orderProperty.ToLower(), order) switch
    {
        ("name", SearchOrder.Asc) => query.OrderBy(x => x.Name).ThenBy(x => x.Id),
        ("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name).ThenByDescending(x => x.Id),
        ("id", SearchOrder.Asc) => query.OrderBy(x => x.Id),
        ("id", SearchOrder.Desc) => query.OrderByDescending(x => x.Id),
        ("createdat", SearchOrder.Asc) => query.OrderBy(x => x.CreatedAt),
        ("createdat", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedAt),
        _ => query.OrderBy(x => x.Name).ThenBy(x => x.Id)
    };
}