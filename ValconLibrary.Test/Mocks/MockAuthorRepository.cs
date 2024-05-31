using Moq;
using System.Linq;
using System.Linq.Expressions;
using ValconLibrary.Entities;
using ValconLibrary.Interfaces;
using ValconLibrary.Test.Models;

namespace ValconLibrary.Test.Mocks
{
    public static class MockAuthorRepository
    {
        public static Mock<IAuthorRepository> SetupAddAuthorAsync(this Mock<IAuthorRepository> mock)
        {
            mock.Setup(repo => repo.AddAuthorAsync(It.IsAny<Author>()))
                .Returns(Task.CompletedTask);
            return mock;
        }

        public static Mock<IAuthorRepository> SetupSaveAllAsync(this Mock<IAuthorRepository> mock, bool shouldSaveAll)
        {
            mock.Setup(repo => repo.SaveAllAsync())
                .ReturnsAsync(shouldSaveAll);
            return mock;
        }

        public static Mock<IAuthorRepository> SetupUpdate(this Mock<IAuthorRepository> mock)
        {
            mock.Setup(repo => repo.Update(It.IsAny<Author>()));
            return mock;
        }

        public static Mock<IAuthorRepository> SetupDeleteAuthorAsync(this Mock<IAuthorRepository> mock, bool shouldDelete)
        {
            mock.Setup(repo => repo.DeleteAuthorAsync(It.IsAny<Guid>()))
                .ReturnsAsync(shouldDelete);
            return mock;
        }

        public static Mock<IAuthorRepository> SetupGetAuthorByIdAsync(this Mock<IAuthorRepository> mock, bool? shouldInstantiate = null)
        {
            var author = shouldInstantiate.HasValue && shouldInstantiate.Value ? AuthorObjectMother.CreateDefaultAuthor() : null;
            mock.Setup(repo => repo.GetAuthorByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(author);
            return mock;
        }

        public static Mock<IAuthorRepository> SetupGetAllAuthors(this Mock<IAuthorRepository> mock)
        {
            mock.Setup(repo => repo.GetAllAuthors())
               .Returns(() => new List<Author>
               {
                    AuthorObjectMother.CreateDefaultAuthor(),
                    AuthorObjectMother.CreateDefaultAuthor()
               }.AsQueryable());

            return mock;
        }
    }
}
