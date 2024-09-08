using Xunit;
using FluentAssertions;
using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.UnitTest.Fixture.Domain;

namespace Flixer.Catalog.UnitTest.Domain.Entities;

[Collection(nameof(CastMemberFixture))]
public class CastMemberTest
{
    private readonly CastMemberFixture _fixture;

    public CastMemberTest(CastMemberFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    [Trait("Domain", "CastMember - Aggregates")]
    public void CastMember_ShouldHaveExpectedProperties_WhenInstantiated()
    {
        var dateTimeBefore = DateTime.Now.AddSeconds(-1);
        var name = _fixture.DataGenerator.GetValidName();
        var type = _fixture.DataGenerator.GetRandomCastMemberType();

        var castMember = new CastMember(name, type);

        var dateTimeAfter = DateTime.Now.AddSeconds(1);
        
        castMember.Name.Should().Be(name);
        castMember.Type.Should().Be(type);
        castMember.Id.Should().NotBeEmpty();
        (castMember.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (castMember.CreatedAt <= dateTimeAfter).Should().BeTrue();
    }
    
    [Theory]
    [Trait("Domain", "CastMember - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void CastMember_ShouldThrowError_WhenInstantiatedWithNameInvalid(string? name)
    {
        var dateTimeBefore = DateTime.Now.AddSeconds(-1);
        var type = _fixture.DataGenerator.GetRandomCastMemberType();

        var action = () => new CastMember(name!, type);

        action
            .Should()
            .ThrowExactly<EntityValidationException>()
            .WithMessage("CastMember is invalid");
    }
    
    [Fact]
    [Trait("Domain", "CastMember - Aggregates")]
    public void CastMember_ShouldUpdate_WhenMethodIsCalled()
    {
        var newName = _fixture.DataGenerator.GetValidName();
        var castMember = _fixture.DataGenerator.GetValidCastMember();
        var newType = _fixture.DataGenerator.GetRandomCastMemberType();

        castMember.Update(newName, newType);

        castMember.Name.Should().Be(newName);
        castMember.Type.Should().Be(newType);
    }
    
    [Theory]
    [Trait("Domain", "CastMember - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void CastMember_ShouldThrowError_WhenUpdateMethodIsCalledWithInvalidName(string invalidName)
    {
        var castMember = _fixture.DataGenerator.GetValidCastMember();
        var newType = _fixture.DataGenerator.GetRandomCastMemberType();

        var action = () => castMember.Update(invalidName, newType);

        action
            .Should()
            .ThrowExactly<EntityValidationException>()
            .WithMessage("CastMember is invalid");
    }
}