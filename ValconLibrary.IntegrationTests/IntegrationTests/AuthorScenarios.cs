using Azure;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ValconLibrary.DTOs;
using ValconLibrary.Entities;
using ValconLibrary.IntegrationTests.Factory;
using ValconLibrary.IntegrationTests.Helpers;



namespace ValconLibrary.Test.IntegrationTests
{
    public class AuthorScenarios : IClassFixture<WebAppFactory>
    {
        private HttpClient _client;
        private readonly GetRequestContent _requestContent;
        private readonly WebAppFactory _factory;

        public AuthorScenarios(WebAppFactory factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost/");
            _requestContent = new GetRequestContent();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        }

        [Fact]
        public async Task Create_Author_And_Response_Status_Code_Ok()
        {
            // Arrange
            var authorDto = new AuthorDto
            {
                Name = "Author",
                LastName = "Author",
                YearOfBirth = 1999
            };

            // Act
            var response = await _client.PostAsync("api/authors", JsonContent.Create(authorDto));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(3000)]
        public async Task Create_Author_With_Invalid_Data_Should_Return_Bad_Request(int invalidYearOfBirth)
        {
            // Arrange
            var invalidAuthorDto = new AuthorDto
            {
                Name = "William",
                LastName = "Shakespeare",
                YearOfBirth = invalidYearOfBirth
            };

            // Act
            var response = await _client.PostAsync("api/authors", JsonContent.Create(invalidAuthorDto));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Get_All_Authors_And_Response_Status_Code_Ok()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("api/authors");

            // Assert
            var content = _requestContent.GetRequestContentAsync<IEnumerable<AuthorDetailsDto>>(response);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_Author_By_Id_And_Response_Status_Code_Ok()
        {
            // Arrange
            var authorDto = new AuthorDto
            {
                Name = "William",
                LastName = "Shakespeare",
                YearOfBirth = 1564
            };

            // Act
            var createResponse = await _client.PostAsync("api/authors", JsonContent.Create(authorDto));

            var createdAuthor = await _requestContent.GetRequestContentAsync<SuccessCreateDto>(createResponse);
            var authorId = createdAuthor?.Id;

            var response = await _client.GetAsync($"api/authors/{authorId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Author_By_Invalid_Id_Should_Return_Not_Found()
        {
            // Arrange
            var invalidAuthorId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"api/authors/{invalidAuthorId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_Author_By_Id_And_Response_Status_Code_Ok()
        {
            // Arrange
            var authorDto = new AuthorDto
            {
                Name = "William",
                LastName = "Shakespeare",
                YearOfBirth = 1564
            };

            var createResponse = await _client.PostAsync("api/authors", JsonContent.Create(authorDto));

            var createdAuthor = await _requestContent.GetRequestContentAsync<SuccessCreateDto>(createResponse);
            var authorId = createdAuthor?.Id;

            // Act
            var deleteResponse = await _client.DeleteAsync($"api/authors/{authorId}");

            // Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_Author_With_Invalid_Id_Should_Return_Not_Found()
        {
            // Arrange
            var invalidAuthorId = Guid.NewGuid();

            // Act
            var response = await _client.DeleteAsync($"api/authors/{invalidAuthorId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_Author_With_Valid_Data_Should_Return_Ok()
        {
            // Arrange
            var authorDto = new AuthorDto
            {
                Name = "William",
                LastName = "Shakespeare",
                YearOfBirth = 1564
            };

            // Act 
            var createResponse = await _client.PostAsync("api/authors", JsonContent.Create(authorDto));
            createResponse.EnsureSuccessStatusCode();

            var createdAuthor = await _requestContent.GetRequestContentAsync<SuccessCreateDto>(createResponse);
            var authorId = createdAuthor?.Id;

            var updatedAuthorDto = new AuthorDto
            {
                Name = "William",
                LastName = "Shakespeare",
                YearOfBirth = 2000
            };

            // Act 
            var updateResponse = await _client.PutAsync($"api/authors/{authorId}", JsonContent.Create(updatedAuthorDto));

            var response = await _client.GetAsync($"api/authors/{authorId}");
            var updatedAuthor = await _requestContent.GetRequestContentAsync<AuthorDetailsDto>(response);

            // Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            updatedAuthor?.YearOfBirth.Should().Be(2000);
        }

        [Fact]
        public async Task Update_Author_With_Invalid_Data_Should_Return_BadRequest()
        {
            // Arrange
            var authorDto = new AuthorDto
            {
                Name = "William",
                LastName = "Shakespeare",
                YearOfBirth = 1564
            };

            // Act
            var createResponse = await _client.PostAsync("api/authors", JsonContent.Create(authorDto));
            createResponse.EnsureSuccessStatusCode();

            var createdAuthor = await _requestContent.GetRequestContentAsync<SuccessCreateDto>(createResponse);
            var authorId = createdAuthor?.Id;

            var invalidAuthorDto = new AuthorDto
            {
                Name = "",
                LastName = "Updated Author",
                YearOfBirth = 2000
            };

            // Act 
            var updateResponse = await _client.PutAsync($"api/authors/{authorId}", JsonContent.Create(invalidAuthorDto));

            // Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
