using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Flixer.Catalog.Domain.Contracts.Repository;
using Flixer.Catalog.Tests.Shared.DataGenerators;

namespace Flixer.Catalog.UnitTest.Fixture.Application.CastMember.DeleteCastMember;

[CollectionDefinition(nameof(DeleteCastMemberFixture))]
public class DeleteCastMemberFixtureCollection : ICollectionFixture<DeleteCastMemberFixture>
{
    
}

public class DeleteCastMemberFixture
{
    public CastMemberDataGenerator DataGenerator { get; } = new();
    public Mock<ICastMemberRepository> GetRepositoryMock() => new();
    public Mock<ILogger<Catalog.Application.Commands.CastMember.DeleteCastMember>> GetLoggerMock() => new();
}