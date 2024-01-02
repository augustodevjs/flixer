using Flixer.Catalog.Domain.Exceptions;
using Flixer.Catalog.Domain.Validation;

namespace Flixer.Catalog.UnitTest.Domain.Validation
{
    public class DomainValidationTest
    {
        private Faker Faker {  get; set; } = new();

        [Fact(DisplayName = nameof(NotNullOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOk()
        {
            var value = Faker.Commerce.ProductName();
            Action action = () => DomainValidation.NotNull(value, "Value");

            action.Should().NotThrow();
        }

        [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullThrowWhenNull()
        {
            string? value = null;
            Action action = () => DomainValidation.NotNull(value, "FieldName");

            action.Should().Throw<EntityValidationException>().WithMessage("FieldName should not be null.");
        }
    }
}
