namespace Flixer.Catalog.EndToEndTests.Api.Category.Common;
internal class DataGenerator
{
    public static IEnumerable<object[]> GetInvalidCreateInputs()
    {
        var fixture = new CategoryFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 3;

        for (int index = 0; index < totalInvalidCases; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    var inputNameShort = fixture.GetExampleCreateInput();
                    inputNameShort.Name = fixture.GetInvalidInputShortName();

                    invalidInputsList.Add(new object[] {
                        inputNameShort,
                        "Name should be at least 3 characters long"
                    });
                    break;
                case 1:
                    var inputTooLong = fixture.GetExampleCreateInput();
                    inputTooLong.Name = fixture.GetInvalidTooLongName();

                    invalidInputsList.Add(new object[] {
                        inputTooLong,
                        "Name should be less or equal 255 characters long"
                    });
                    break;
                case 2:
                    var inputDescriptionTooLong = fixture.GetExampleCreateInput();
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

    public static IEnumerable<object[]> GetInvalidUpdateInputs()
    {
        var fixture = new CategoryFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 3;

        for (int index = 0; index < totalInvalidCases; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    var input1 = fixture.GetExampleUpdateInput();
                    input1.Name = fixture.GetInvalidInputShortName();
                    invalidInputsList.Add(new object[] {
                        input1,
                        "Name should be at least 3 characters long"
                    });
                    break;
                case 1:
                    var input2 = fixture.GetExampleUpdateInput();
                    input2.Name = fixture.GetInvalidTooLongName();
                    invalidInputsList.Add(new object[] {
                        input2,
                        "Name should be less or equal 255 characters long"
                    });
                    break;
                case 2:
                    var input3 = fixture.GetExampleUpdateInput();
                    input3.Description = fixture.GetInvalidTooLongDescription();
                    invalidInputsList.Add(new object[] {
                        input3,
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
