namespace Flixer.Catalog.UnitTest.Common;

// remover após resolver o commom fixture da domain
public abstract class BaseFixture
{
    public Faker Faker { get; set; }
    protected BaseFixture() => Faker = new Faker("pt_BR");
    public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
}
