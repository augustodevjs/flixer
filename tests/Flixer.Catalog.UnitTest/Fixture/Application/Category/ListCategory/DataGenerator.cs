using Flixer.Catalog.Application.Queries.Category.ListCategories;

namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.ListCategory;

public class DataGenerator
{
         public static IEnumerable<object[]> GetInputsListWithoutAllParameter(int times = 14)
     {
         var fixture = new ListCategoriesQueryFixture();
         var inputListExample = fixture.GetListInput();

         for (int i = 0; i < times; i++)
         {
             switch (i % 7)
             {
                 case 0:
                     yield return new object[] {
                         new ListCategoriesQuery()
                     };
                     break;
                 case 1:
                     yield return new object[] {
                         new ListCategoriesQuery(inputListExample.Page)
                     };
                     break;
                 case 3:
                     yield return new object[] {
                         new ListCategoriesQuery(
                             inputListExample.Page,
                             inputListExample.PerPage
                         )
                     };
                     break;
                 case 4:
                     yield return new object[] {
                         new ListCategoriesQuery(
                             inputListExample.Page,
                             inputListExample.PerPage,
                             inputListExample.Search
                         )
                     };
                     break;
                 case 5:
                     yield return new object[] {
                         new ListCategoriesQuery(
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
                         new ListCategoriesQuery()
                     };
                     break;
             }
         }
     }
}