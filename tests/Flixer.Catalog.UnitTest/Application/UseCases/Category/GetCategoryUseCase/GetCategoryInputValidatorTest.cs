﻿using Flixer.Catalog.Application.Dtos.InputModel.Category;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Category.GetCategoryUseCase;

[Collection(nameof(GetCategoryUseCaseTestFixture))]
public class GetCategoryInputValidatorTest
{
    private readonly GetCategoryUseCaseTestFixture _fixture;

    public GetCategoryInputValidatorTest(GetCategoryUseCaseTestFixture fixture)
        => _fixture = fixture;

    [Fact]
    [Trait("Application", "GetCategoryInputValidation - UseCases")]
    public void Validator_ShouldValidate_WhenMethodValidateIsCalled()
    {
        var validInput = new GetCategoryInputModel(Guid.NewGuid());
        var validator = new GetCategoryValidatorInputModel();

        var validationResult = validator.Validate(validInput);

        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().HaveCount(0);
    }

    [Fact]
    [Trait("Application", "GetCategoryInputValidation - UseCases")]
    public void Validator_ShouldShowErrorMessage_WhenGuidIsEmpty()
    {
        var invalidInput = new GetCategoryInputModel(Guid.Empty);
        var validator = new GetCategoryValidatorInputModel();

        var validationResult = validator.Validate(invalidInput);

        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().HaveCount(1);
        validationResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }
}
