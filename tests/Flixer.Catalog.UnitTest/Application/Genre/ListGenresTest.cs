﻿// using Xunit;
// using Flixer.Catalog.UnitTest.Fixture.Application.Genre.ListGenre;
//
// namespace Flixer.Catalog.UnitTest.Application.Genre;
//
// [Collection(nameof(ListGenreFixture))]
// public class ListGenresTest
// {
//     private readonly ListGenreFixture _fixture;
//
//     public ListGenresTest(ListGenreFixture fixture)
//     {
//         _fixture = fixture;
//     }
//
//     [Fact]
//     [Trait("Application", "ListGenre - Query")]
//     public async Task Query_ShouldReturnListOfGenres()
//     {
//          var genreRepositoryMock = _fixture.();
//         var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
//         var genresListExample = _fixture.GetExampleGenresList();
//         var input = _fixture.GetExampleInput();
//         var outputRepositorySearch = new SearchOutput<DomainEntity.Genre>(
//             currentPage: input.Page,
//             perPage: input.PerPage,
//             items: (IReadOnlyList<DomainEntity.Genre>)genresListExample,
//             total: new Random().Next(50, 200)
//         );
//         genreRepositoryMock.Setup(x => x.Search(
//             It.IsAny<SearchInput>(),
//             It.IsAny<CancellationToken>()
//         )).ReturnsAsync(outputRepositorySearch);
//         var useCase = new UseCase
//             .ListGenres(genreRepositoryMock.Object, categoryRepositoryMock.Object);
//
//         UseCase.ListGenresOutput output =
//             await useCase.Handle(input, CancellationToken.None);
//
//         output.Page.Should().Be(outputRepositorySearch.CurrentPage);
//         output.PerPage.Should().Be(outputRepositorySearch.PerPage);
//         output.Total.Should().Be(outputRepositorySearch.Total);
//         output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
//         ((List<GenreModelOutput>)output.Items).ForEach(outputItem =>
//         {
//             var repositoryGenre = outputRepositorySearch.Items
//                 .FirstOrDefault(x => x.Id == outputItem.Id);
//             outputItem.Should().NotBeNull();
//             repositoryGenre.Should().NotBeNull();
//             outputItem.Name.Should().Be(repositoryGenre!.Name);
//             outputItem.IsActive.Should().Be(repositoryGenre.IsActive);
//             outputItem.CreatedAt.Should().Be(repositoryGenre!.CreatedAt);
//             outputItem.Categories.Should()
//                 .HaveCount(repositoryGenre.Categories.Count);
//             foreach (var expectedId in repositoryGenre.Categories)
//                 outputItem.Categories.Should().Contain(relation => relation.Id == expectedId);
//         });
//         genreRepositoryMock.Verify(
//             x => x.Search(
//                 It.Is<SearchInput>(searchInput => 
//                     searchInput.Page == input.Page
//                     && searchInput.PerPage == input.PerPage
//                     && searchInput.Search == input.Search
//                     && searchInput.OrderBy == input.Sort
//                     && searchInput.Order == input.Dir
//                 ),
//                 It.IsAny<CancellationToken>()
//             ),
//             Times.Once
//         );
//         var expectedIds = genresListExample
//             .SelectMany(genre => genre.Categories)
//             .Distinct().ToList();
//         categoryRepositoryMock.Verify(
//             x => x.GetListByIds(
//                 It.Is<List<Guid>>(parameterList =>
//                     parameterList.All(id => expectedIds.Contains(id)
//                     && parameterList.Count == expectedIds.Count
//                 )), 
//                 It.IsAny<CancellationToken>()
//             ),
//             Times.Once
//         );
//     }
// }