﻿using Flixer.Catalog.Application.Dtos.InputModel.Category;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Category.UpdateCategoryUseCase;

[Collection(nameof(UpdateCategoryUseCaseTestFixture))]
public class UpdateCategoryTestInputValidator
{
    private readonly UpdateCategoryUseCaseTestFixture _fixture;

    public UpdateCategoryTestInputValidator(UpdateCategoryUseCaseTestFixture fixture)
        => _fixture = fixture;

    [Fact]
    [Trait("Application", "UpdateCategoryInputValidator - Use Cases")]
    public void Validator_ShouldDontValidate_WhenGuidIsEmpty()
    {
        var input = _fixture.GetValidInput(Guid.Empty);
        var validator = new UpdateCategoryValidatorInputModel();

        var validateResult = validator.Validate(input);

        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeFalse();
        validateResult.Errors.Should().HaveCount(1);
        validateResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }

    [Fact]
    [Trait("Application", "UpdateCategoryInputValidator - Use Cases")]
    public void Validator_ShouldValidate_WhenIdIsProvided()
    {
        var input = _fixture.GetValidInput();
        var validator = new UpdateCategoryValidatorInputModel();

        var validateResult = validator.Validate(input);

        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeTrue();
        validateResult.Errors.Should().HaveCount(0);
    }
}