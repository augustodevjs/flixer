using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Application.CastMember.UpdateCastMember;

[CollectionDefinition(nameof(UpdateCastMemberFixture))]
public class UpdateCastMemberFixtureCollection : ICollectionFixture<UpdateCastMemberFixture>
{
    
}

public class UpdateCastMemberFixture
{
    public CastMemberDataGenerator DataGenerator { get; } = new();
    public Mock<ICastMemberRepository> GetRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Commands.CastMember.UpdateCastMember>> GetLoggerMock() => new();
}