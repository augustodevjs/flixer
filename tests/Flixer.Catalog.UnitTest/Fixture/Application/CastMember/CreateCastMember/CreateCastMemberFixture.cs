using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Application.CastMember.CreateCastMember;

[CollectionDefinition(nameof(CreateCastMemberFixture))]
public class CreateCastMemberFixtureCollection : ICollectionFixture<CreateCastMemberFixture>
{
    
}

public class CreateCastMemberFixture
{
    public CastMemberDataGenerator DataGenerator { get; } = new();
    public Mock<ICastMemberRepository> GetRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Commands.CastMember.CreateCastMember>> GetLoggerMock() => new();
}