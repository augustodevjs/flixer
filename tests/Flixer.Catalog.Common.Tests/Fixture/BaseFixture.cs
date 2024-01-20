using Bogus;

namespace Flixer.Catalog.Common.Tests.fixture;

public abstract class BaseFixture
{
    public Faker Faker { get; set; }
    protected BaseFixture() => Faker = new Faker("pt_BR");
}