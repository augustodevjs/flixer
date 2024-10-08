﻿using Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.Domain.Contracts.Repository;

public interface ICastMemberRepository : IRepository<CastMember>, ISearchableRepository<CastMember>
{
    public Task<IReadOnlyList<Guid>> GetIdsListByIds(List<Guid> ids);
}