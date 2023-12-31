﻿namespace Flixer.Catalog.UnitTest.Application.UseCases.CreateCategoryUseCase;

public class CreateCategoryUseCaseTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
    {
        var fixture = new CreateCategoryUseCaseTestFixture();
        var invalidInputsLists = new List<object[]>();
        var totalInvalidCases = 4;

        for(int index = 0; index < times; index++)
        {
            switch(index % totalInvalidCases)
            {
                case 0:
                    invalidInputsLists.Add(new object[]
                    {
                        fixture.GetInvalidInputShortName(),
                        "Name should be at least 3 characters long"
                    });
                    break;

                case 1:
                    invalidInputsLists.Add(new object[]
                    {
                        fixture.GetInvalidInputTooLongName(),
                        "Name should be less or equal 255 characters long"
                    });
                    break;
                case 2:
                    invalidInputsLists.Add(new object[]
                    {
                        fixture.GetInvalidCategoryInputNull(),
                        "Description should not be null"
                    });
                    break;
                case 3:
                    invalidInputsLists.Add(new object[]
                    {
                        fixture.GetInvalidInputTooLongDescription(),
                        "Description should be less or equal 10000 characters long"
                    });
                    break;
                default:
                    break;
            }
        }
        return invalidInputsLists;
    }
}
