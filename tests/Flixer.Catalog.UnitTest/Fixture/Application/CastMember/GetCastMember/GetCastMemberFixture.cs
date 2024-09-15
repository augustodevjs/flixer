using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Application.CastMember.GetCastMember;

[CollectionDefinition(nameof(GetCastMemberFixture))]
public class GetCastMemberFixtureCollection : ICollectionFixture<GetCastMemberFixture>
{
    
}

public class GetCastMemberFixture
{
    public CastMemberDataGenerator DataGenerator { get; } = new();
    public Mock<ICastMemberRepository> GetRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Queries.CastMember.GetCastMember>> GetLoggerMock() => new();
}