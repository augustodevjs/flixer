using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Application.CastMember.ListCastMember;

[CollectionDefinition(nameof(ListCastMemberFixture))]
public class ListCastMemberFixtureCollection : ICollectionFixture<ListCastMemberFixture>
{
    
}

public class ListCastMemberFixture
{
    public CastMemberDataGenerator DataGenerator { get; } = new();
    public Mock<ICastMemberRepository> GetRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Queries.CastMember.ListCastMember>> GetLoggerMock() => new();
}