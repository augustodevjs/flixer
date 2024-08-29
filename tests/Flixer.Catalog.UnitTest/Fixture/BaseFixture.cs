using Bogus;

namespace Flixer.Catalog.UnitTest.Fixture;

public abstract class BaseFixture
{
    public Faker Faker { get; set; }
    protected BaseFixture() => Faker = new Faker("pt_BR");
}