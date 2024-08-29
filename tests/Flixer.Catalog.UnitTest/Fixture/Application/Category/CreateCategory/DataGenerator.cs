namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.CreateCategory;

public class DataGenerator
{
    public static IEnumerable<object[]> GetInvalidCreateInputs(int times = 12)
     {
         var totalInvalidCases = 3;
         var invalidInputsLists = new List<object[]>();
         var fixture = new CreateCategoryCommandFixture();

         for (var index = 0; index < times; index++)
         {
             switch (index % totalInvalidCases)
             {
                 case 0:
                     invalidInputsLists.Add(new object[]
                     {
                         fixture.GetInvalidCreateInputShortName(),
                     });
                     break;
                 case 1:
                     invalidInputsLists.Add(new object[]
                     {
                         fixture.GetInvaliCreatedInputTooLongName(),
                     });
                     break;
                 case 2:
                     invalidInputsLists.Add(new object[]
                     {
                         fixture.GetInvalidCreateInputTooLongDescription(),
                     });
                     break;
             }
         }
         return invalidInputsLists;
     }
}