using Flixer.Catalog.Application.Common.Input.Category;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.ListCategory;

public class DataGenerator
{
         public static IEnumerable<object[]> GetInputsListWithoutAllParameter(int times = 14)
     {
         var fixture = new ListCategoriesFixture();
         var inputListExample = fixture.DataGenerator.GetListInput();

         for (int i = 0; i < times; i++)
         {
             switch (i % 7)
             {
                 case 0:
                     yield return new object[] {
                         new ListCategoriesInput()
                     };
                     break;
                 case 1:
                     yield return new object[] {
                         new ListCategoriesInput(inputListExample.Page)
                     };
                     break;
                 case 3:
                     yield return new object[] {
                         new ListCategoriesInput(
                             inputListExample.Page,
                             inputListExample.PerPage
                         )
                     };
                     break;
                 case 4:
                     yield return new object[] {
                         new ListCategoriesInput(
                             inputListExample.Page,
                             inputListExample.PerPage,
                             inputListExample.Search
                         )
                     };
                     break;
                 case 5:
                     yield return new object[] {
                         new ListCategoriesInput(
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
                         new ListCategoriesInput()
                     };
                     break;
             }
         }
     }
}