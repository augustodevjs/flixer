﻿namespace Flixer.Catalog.UnitTest.Fixture.Application.Category.UpdateCategory;

public class DataGenerator
{    
     public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
     {
         var fixture = new UpdateCategoryFixture();

         for (var indice = 0; indice < times; indice++)
         {
             var exampleCategory = fixture.DataGenerator.GetValidCategory();
             var exampleInput = fixture.DataGenerator.GetInputUpdate(exampleCategory.Id);

             yield return new object[] {
                 exampleCategory, exampleInput
             };
         }
     }

     public static IEnumerable<object[]> GetInvalidUpdateInputs(int times = 12)
     {
         var totalInvalidCases = 3;
         var invalidInputsList = new List<object[]>();
         var fixture = new UpdateCategoryFixture();

         for (var index = 0; index < times; index++)
         {
             switch (index % totalInvalidCases)
             {
                 case 0:
                     invalidInputsList.Add(new object[] {
                         fixture.DataGenerator.GetInvalidUpdateInputShortName(),
                     });
                     break;
                 case 1:
                     invalidInputsList.Add(new object[] {
                         fixture.DataGenerator.GetInvalidUpdateInputTooLongName(),
                     });
                     break;
                 case 2:
                     invalidInputsList.Add(new object[] {
                         fixture.DataGenerator.GetInvalidUpdateInputTooLongDescription(),
                     });
                     break;
             }
         }

         return invalidInputsList;
     }
}