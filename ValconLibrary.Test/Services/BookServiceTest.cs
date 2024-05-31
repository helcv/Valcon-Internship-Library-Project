using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValconLibrary.Entities;
using ValconLibrary.Interfaces;
using ValconLibrary.Services;
using ValconLibrary.Test.Mocks;

namespace ValconLibrary.Test.Services
{
    public class BookServiceTest
    {
        private readonly Mock<UserManager<UserIdentity>> _mockUserManager;
        private readonly Mock<IAuthorRepository> _mockAuthorRepo;
        private readonly Mock<IRentRepository> _mockRentRepo;
        private readonly Mock<IBookRepository> _mockBookRepo;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<BookService>> _mockLogger;
        private readonly BookService _bookService;

        public BookServiceTest()
        {
            _mockUserManager = GetUserManagerMock();
            _mockAuthorRepo = new Mock<IAuthorRepository>();
            _mockRentRepo = new Mock<IRentRepository>();
            _mockBookRepo = new Mock<IBookRepository>();
            _mapper = MockMapper.GetMapperConfig();
            _mockLogger = new Mock<ILogger<BookService>>();
            _bookService = new BookService(_mapper, _mockBookRepo.Object, _mockAuthorRepo.Object , _mockRentRepo.Object, _mockUserManager.Object, _mockLogger.Object);
        }
        private Mock<UserManager<UserIdentity>> GetUserManagerMock()
        {
            return new Mock<UserManager<UserIdentity>>(
                new Mock<IUserStore<UserIdentity>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<UserIdentity>>().Object,
                new IUserValidator<UserIdentity>[0],
                new IPasswordValidator<UserIdentity>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<UserIdentity>>>().Object);
        }

        [Fact]
        public async Task Retrieving_A_Book_By_Id_Should_Succeed_When_Book_Exist()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            _mockBookRepo.SetupGetBookByIdAsync();

            // Act
            var result = await _bookService.GetBookByIdAsync(bookId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }
    }
}
