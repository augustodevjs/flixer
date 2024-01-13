using Flixer.Catalog.Application.Dtos.InputModel.Category;

namespace Flixer.Catalog.UnitTest.Application.UseCases.Category.ListCategoriesUseCase;

public class ListCategoriesUseCaseTestDataGenerator
{
    public static IEnumerable<object[]> GetInputsWithoutAllParameter(int times = 14)
    {
        var fixture = new ListCategoriesUseCaseTestFixture();
        var inputExample = fixture.GetExampleInput();

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
                        new ListCategoriesInputModel(inputExample.Page)
                    };
                    break;
                case 3:
                    yield return new object[] {
                        new ListCategoriesInputModel(
                            inputExample.Page,
                            inputExample.PerPage
                        )
                    };
                    break;
                case 4:
                    yield return new object[] {
                        new ListCategoriesInputModel(
                            inputExample.Page,
                            inputExample.PerPage,
                            inputExample.Search
                        )
                    };
                    break;
                case 5:
                    yield return new object[] {
                        new ListCategoriesInputModel(
                            inputExample.Page,
                            inputExample.PerPage,
                            inputExample.Search,
                            inputExample.Sort
                        )
                    };
                    break;
                case 6:
                    yield return new object[] { inputExample };
                    break;
                default:
                    yield return new object[] {
                        new ListCategoriesInputModel()
                    };
                    break;
            }
        }
    }
}
