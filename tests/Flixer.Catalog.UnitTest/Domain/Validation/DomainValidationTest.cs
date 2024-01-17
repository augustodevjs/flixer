using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Domain.Validation;

namespace Flixer.Catalog.UnitTest.Domain.Validation;

public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker();

    [Fact]
    [Trait("Domain", "DomainValidation - Validation")]
    public void DomainValidation_ShouldVaidate_WhenMethodNotNullIsCalled()
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        var value = Faker.Commerce.ProductName();
        Action action =
            () => DomainValidation.NotNull(value, fieldName);
        action.Should().NotThrow();
    }

    [Fact]
    [Trait("Domain", "DomainValidation - Validation")]
    public void DomainValidation_ShouldThrowError_WhenMethodNotNullIsCalledWithValueIsNull()
    {
        string? value = null;
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.NotNull(value, fieldName);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be null");
    }

    [Theory]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void DomainValidation_ShouldThrowError_WhenMethodNotNullOrEmptyIsCalledWithParametersNullOrEmpty(string? target)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.NotNullOrEmpty(target, fieldName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be empty or null");
    }

    [Fact]
    [Trait("Domain", "DomainValidation - Validation")]
    public void DomainValidation_ShouldValidate_WhenMethodNotNullOrEmptyIsCalled()
    {
        var target = Faker.Commerce.ProductName();
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.NotNullOrEmpty(target, fieldName);

        action.Should().NotThrow();
    }

    [Theory]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesSmallerThanMin), parameters: 10)]
    public void DomainValidation_ShouldThrowError_WhenMethodMinLengthIsViolated(string target, int minLength)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.MinLength(target, minLength, fieldName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be at least {minLength} characters long");
    }

    public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberOftests = 5)
    {
        yield return new object[] { "123456", 10 };
        var faker = new Faker();
        for (int i = 0; i < (numberOftests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length + (new Random()).Next(1, 20);
            yield return new object[] { example, minLength };
        }
    }

    [Theory]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMin), parameters: 10)]
    public void DomainValidation_ShouldValidate_WhenMethodMinLengthIsCalled(string target, int minLength)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.MinLength(target, minLength, fieldName);

        action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberOftests = 5)
    {
        yield return new object[] { "123456", 6 };
        var faker = new Faker();
        for (int i = 0; i < (numberOftests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length - (new Random()).Next(1, 5);
            yield return new object[] { example, minLength };
        }
    }

    [Theory]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMax), parameters: 10)]
    public void DomainValidation_ShouldThrowError_WhenMethodMaxLengthIsViolated(string target, int maxLength)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.MaxLength(target, maxLength, fieldName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be less or equal {maxLength} characters long");
    }

    public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberOftests = 5)
    {
        yield return new object[] { "123456", 5 };
        var faker = new Faker();
        for (int i = 0; i < (numberOftests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length - (new Random()).Next(1, 5);
            yield return new object[] { example, maxLength };
        }
    }

    [Theory]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesLessThanMax), parameters: 10)]
    public void DomainValidation_ShouldValidate_WhenMethodMaxLengthIsCalled(string target, int maxLength)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.MaxLength(target, maxLength, fieldName);

        action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GetValuesLessThanMax(int numberOftests = 5)
    {
        yield return new object[] { "123456", 6 };
        var faker = new Faker();
        for (int i = 0; i < (numberOftests - 1); i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length + (new Random()).Next(0, 5);
            yield return new object[] { example, maxLength };
        }
    }
}