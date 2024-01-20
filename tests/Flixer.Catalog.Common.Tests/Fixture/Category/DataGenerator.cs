using Flixer.Catalog.Application.Dtos.InputModel.Category;

namespace Flixer.Catalog.Common.Tests.Fixture.Category;

public class DataGenerator
{
    public static IEnumerable<object[]> GetInvalidCreateInputs(int times = 12)
    {
        var fixture = new CategoryTestFixture();
        var invalidInputsLists = new List<object[]>();
        var totalInvalidCases = 4;

        for (int index = 0; index < times; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    invalidInputsLists.Add(new object[]
                    {
                        fixture.GetInvalidCreateInputShortName(),
                        "Name should be at least 3 characters long"
                    });
                    break;

                case 1:
                    invalidInputsLists.Add(new object[]
                    {
                        fixture.GetInvaliCreatedInputTooLongName(),
                        "Name should be less or equal 255 characters long"
                    });
                    break;
                case 2:
                    invalidInputsLists.Add(new object[]
                    {
                        fixture.GetInvalidCreateInputNull(),
                        "Description should not be null"
                    });
                    break;
                case 3:
                    invalidInputsLists.Add(new object[]
                    {
                        fixture.GetInvalidCreateInputTooLongDescription(),
                        "Description should be less or equal 10000 characters long"
                    });
                    break;
                default:
                    break;
            }
        }
        return invalidInputsLists;
    }

    public static IEnumerable<object[]> GetInputsListWithoutAllParameter(int times = 14)
    {
        var fixture = new CategoryTestFixture();
        var inputListExample = fixture.GetListInput();

        for (int i = 0; i < times; i++)
        {
            switch (i % 7)
            {
                case 0:
                    yield return new object[] {
                        new ListCategoriesInputModel()
                    };
                    break;
                case 1:
                    yield return new object[] {
                        new ListCategoriesInputModel(inputListExample.Page)
                    };
                    break;
                case 3:
                    yield return new object[] {
                        new ListCategoriesInputModel(
                            inputListExample.Page,
                            inputListExample.PerPage
                        )
                    };
                    break;
                case 4:
                    yield return new object[] {
                        new ListCategoriesInputModel(
                            inputListExample.Page,
                            inputListExample.PerPage,
                            inputListExample.Search
                        )
                    };
                    break;
                case 5:
                    yield return new object[] {
                        new ListCategoriesInputModel(
                            inputListExample.Page,
                            inputListExample.PerPage,
                            inputListExample.Search,
                            inputListExample.Sort
                        )
                    };
                    break;
                case 6:
                    yield return new object[] { inputListExample };
                    break;
                default:
                    yield return new object[] {
                        new ListCategoriesInputModel()
                    };
                    break;
            }
        }
    }

    public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
    {
        var fixture = new CategoryTestFixture();

        for (int indice = 0; indice < times; indice++)
        {
            var exampleCategory = fixture.GetInputUpdate();
            var exampleInput = fixture.GetInputUpdate(exampleCategory.Id);

            yield return new object[] {
                exampleCategory, exampleInput
            };
        }
    }

    public static IEnumerable<object[]> GetInvalidUpdateInputs(int times = 12)
    {
        var fixture = new CategoryTestFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 3;

        for (int index = 0; index < times; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    invalidInputsList.Add(new object[] {
                        fixture.GetInvalidUpdateInputShortName(),
                        "Name should be at least 3 characters long"
                    });
                    break;
                case 1:
                    invalidInputsList.Add(new object[] {
                        fixture.GetInvalidUpdateInputTooLongName(),
                        "Name should be less or equal 255 characters long"
                    });
                    break;
                case 2:
                    invalidInputsList.Add(new object[] {
                        fixture.GetInvalidUpdateInputTooLongDescription(),
                        "Description should be less or equal 10000 characters long"
                    });
                    break;
                default:
                    break;
            }
        }

        return invalidInputsList;
    }
}
