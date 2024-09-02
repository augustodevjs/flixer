namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.CreateCategory;

public class DataGenerator
{
    public static IEnumerable<object[]> GetInvalidCreateInputs(int times = 12)
     {
         var totalInvalidCases = 3;
         var invalidInputsLists = new List<object[]>();
         var fixture = new CreateCategoryFixture();

         for (var index = 0; index < times; index++)
         {
             switch (index % totalInvalidCases)
             {
                 case 0:
                     invalidInputsLists.Add(new object[]
                     {
                         fixture.DataGenerator.GetInvalidCreateInputShortName(),
                     });
                     break;
                 case 1:
                     invalidInputsLists.Add(new object[]
                     {
                         fixture.DataGenerator.GetInvaliCreatedInputTooLongName(),
                     });
                     break;
                 case 2:
                     invalidInputsLists.Add(new object[]
                     {
                         fixture.DataGenerator.GetInvalidCreateInputTooLongDescription(),
                     });
                     break;
             }
         }
         
         return invalidInputsLists;
     }
}