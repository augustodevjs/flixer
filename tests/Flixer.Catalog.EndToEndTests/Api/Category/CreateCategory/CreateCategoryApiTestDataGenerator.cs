namespace Flixer.Catalog.EndToEndTests.Api.Category.CreateCategory;

public class CreateCategoryApiTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new CreateCategoryApiTestFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 3;

        for (int index = 0; index < totalInvalidCases; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    var inputNameShort = fixture.GetExampleInput();
                    inputNameShort.Name = fixture.GetInvalidInputShortName();

                    invalidInputsList.Add(new object[] {
                        inputNameShort,
                        "Name should be at least 3 characters long"
                    });
                    break;
                case 1:
                    var inputTooLong = fixture.GetExampleInput();
                    inputTooLong.Name = fixture.GetInvalidTooLongName();

                    invalidInputsList.Add(new object[] {
                        inputTooLong,
                        "Name should be less or equal 255 characters long"
                    });
                    break;
                case 2:
                    var inputDescriptionTooLong = fixture.GetExampleInput();
                    inputDescriptionTooLong.Description = fixture.GetInvalidTooLongDescription();

                    invalidInputsList.Add(new object[] {
                        inputDescriptionTooLong,
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