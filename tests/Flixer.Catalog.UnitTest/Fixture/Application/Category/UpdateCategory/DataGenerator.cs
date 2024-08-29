namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.UpdateCategory;

public class DataGenerator
{    
     public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
     {
         var fixture = new UpdateCategoryCommandFixture();

         for (var indice = 0; indice < times; indice++)
         {
             var exampleCategory = fixture.CategoryFixture.GetValidCategory();
             var exampleInput = fixture.GetInputUpdate(exampleCategory.Id);

             yield return new object[] {
                 exampleCategory, exampleInput
             };
         }
     }

     public static IEnumerable<object[]> GetInvalidUpdateInputs(int times = 12)
     {
         var totalInvalidCases = 3;
         var invalidInputsList = new List<object[]>();
         var fixture = new UpdateCategoryCommandFixture();

         for (var index = 0; index < times; index++)
         {
             switch (index % totalInvalidCases)
             {
                 case 0:
                     invalidInputsList.Add(new object[] {
                         fixture.GetInvalidUpdateInputShortName(),
                     });
                     break;
                 case 1:
                     invalidInputsList.Add(new object[] {
                         fixture.GetInvalidUpdateInputTooLongName(),
                     });
                     break;
                 case 2:
                     invalidInputsList.Add(new object[] {
                         fixture.GetInvalidUpdateInputTooLongDescription(),
                     });
                     break;
             }
         }

         return invalidInputsList;
     }
}