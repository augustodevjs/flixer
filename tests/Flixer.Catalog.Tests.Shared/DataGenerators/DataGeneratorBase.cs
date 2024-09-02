using Bogus;

namespace Flixer.Catalog.Tests.Shared.DataGenerators;

public abstract class DataGeneratorBase
{
    public Faker Faker { get; set; }

    protected DataGeneratorBase()
        => Faker = new Faker("pt_BR");

    protected bool GetRandomBoolean()
        => new Random().NextDouble() < 0.5;
}