using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using ValconLibrary.DTOs;
using ValconLibrary.Entities;
using ValconLibrary.Helpers;
using ValconLibrary.Interfaces;
using ValconLibrary.Services;
using ValconLibrary.Test.Mocks;
using ValconLibrary.Test.Models;

namespace ValconLibrary.Test.Services
{
    public class AuthorServiceTests
    {
        private readonly Mock<IAuthorRepository> _mockAuthorRepo;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<AuthorService>> _mockLogger;
        private readonly AuthorService _authorService;
        
        public AuthorServiceTests()
        {
            _mockAuthorRepo = new Mock<IAuthorRepository>();
            _mapper = MockMapper.GetMapperConfig();
            _mockLogger = new Mock<ILogger<AuthorService>>();
            _authorService = new AuthorService(_mockAuthorRepo.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public async Task Creating_An_Author_Should_Succeed()
        {
            // Arrange
            var shouldSaveAll = true;
            var authorDto = AuthorObjectMother.CreateDefaultAuthorDto();
            _mockAuthorRepo.SetupAddAuthorAsync()
                           .SetupSaveAllAsync(shouldSaveAll);

            //Act
            var result = await _authorService.CreateAuthorAsync(authorDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Message.Should().Be("Author succesfully created!");
        }

        [Fact]
        public async Task Creating_An_Author_Should_Fail_When_Save_Fails()
        {
            // Arrange
            var shouldSaveAll = false;
            var authorDto = AuthorObjectMother.CreateDefaultAuthorDto();
            _mockAuthorRepo.SetupAddAuthorAsync()
                           .SetupSaveAllAsync(shouldSaveAll);

            // Act
            var result = await _authorService.CreateAuthorAsync(authorDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Failed to add author.");
        }

        [Fact]
        public async Task Updating_An_Author_Details_Should_Succeed()
        {
            // Arrange
            var shouldSaveAll = true;
            var shouldInstantiate = true;
            var authorId = Guid.NewGuid();
            var updateAuthorDto = AuthorObjectMother.CreateDefaultAuthorDto();

            _mockAuthorRepo.SetupGetAuthorByIdAsync(shouldInstantiate)
                           .SetupUpdate()
                           .SetupSaveAllAsync(shouldSaveAll);

            //Act
            var result = await _authorService.UpdateAuthorAsync(authorId, updateAuthorDto);

            result.IsSuccess.Should().BeTrue();
            result.Value.Message.Should().Be("Author succesfully updated.");
        }

        [Fact]
        public async Task Updating_An_Author_Details_Should_Fail_When_Save_Fails()
        {
            // Arrange
            var shouldSaveAll = false;
            var shouldInstantiate = true;
            var authorId = Guid.NewGuid();
            var updateAuthorDto = AuthorObjectMother.CreateDefaultAuthorDto();

            _mockAuthorRepo.SetupGetAuthorByIdAsync(shouldInstantiate)
                           .SetupUpdate()
                           .SetupSaveAllAsync(shouldSaveAll);

            // Act
            var result = await _authorService.UpdateAuthorAsync(authorId, updateAuthorDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Failed to update author.");
        }

        [Fact]
        public async Task Updating_An_Author_Details_Should_Fail_When_Author_Does_Not_Exist()
        {
            // Arrange
            var shouldInstantiate = false;
            var authorId = Guid.NewGuid();
            var updateAuthorDto = AuthorObjectMother.CreateDefaultAuthorDto();

            _mockAuthorRepo.SetupGetAuthorByIdAsync(shouldInstantiate);

            // Act
            var result = await _authorService.UpdateAuthorAsync(authorId, updateAuthorDto);

            // Assert
            _mockAuthorRepo.Verify(ar => ar.Update(AuthorObjectMother.CreateDefaultAuthor()), Times.Never());
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Author does not exist");
        }

        [Fact]
        public async Task Deleting_An_Author_Should_Succeed()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var shouldDelete = true;
            var shouldSaveAll = true;

            _mockAuthorRepo.SetupDeleteAuthorAsync(shouldDelete)
                           .SetupSaveAllAsync(shouldSaveAll);

            // Act
            var result = await _authorService.DeleteAuthorAsync(authorId);

            // Assert
            _mockAuthorRepo.Verify(repo => repo.SaveAllAsync(), Times.Once);

            result.IsSuccess.Should().BeTrue();
            result.Value.Message.Should().Be("Author succesfully deleted.");
        }

        [Fact]
        public async Task Deleting_An_Author_Should_Fail_When_Delete_Fails()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var shouldDelete = false;

            _mockAuthorRepo.SetupDeleteAuthorAsync(shouldDelete);

            // Act
            var result = await _authorService.DeleteAuthorAsync(authorId);

            // Assert
            _mockAuthorRepo.Verify(repo => repo.SaveAllAsync(), Times.Never);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Failed to delete author");
        }

        [Fact]
        public void Retrieving_All_Authors_Should_Succeed()
        {
            // Arrange
            _mockAuthorRepo.SetupGetAllAuthors();

            // Act
            var result = _authorService.GetAllAuthors();

            // Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Retrieving_An_Author_By_Id_Should_Succeed_When_Author_Exist()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var shouldInstantiate = true;

            _mockAuthorRepo.SetupGetAuthorByIdAsync(shouldInstantiate);

            // Act
            var result = await _authorService.GetAuthorByIdAsync(authorId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task Retrieving_Author_By_Id_Should_Fail_When_Author_Does_Not_Exist()
        {
            // Arrange
            var authorId = Guid.NewGuid();

            _mockAuthorRepo.SetupGetAuthorByIdAsync();

            // Act
            var result = await _authorService.GetAuthorByIdAsync(authorId);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Author does not exist.");
        }
    }
}
