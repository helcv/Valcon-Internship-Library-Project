using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValconLibrary.Constants;
using ValconLibrary.DTOs;
using ValconLibrary.Entities;
using ValconLibrary.Interfaces;
using ValconLibrary.Services;
using ValconLibrary.Test.Mocks;
using ValconLibrary.Test.Models;

namespace ValconLibrary.Test.Services
{
    public  class UserServiceTests
    {
        private readonly Mock<UserManager<UserIdentity>> _mockUserManager;
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IRentRepository> _mockRentRepo;
        private readonly Mock<IBookRepository> _mockBookRepo;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<UserService>> _mockLogger;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserManager = GetUserManagerMock();
            _mockUserRepo = new Mock<IUserRepository>();
            _mockRentRepo = new Mock<IRentRepository>();
            _mockBookRepo = new Mock<IBookRepository>();
            _mapper = MockMapper.GetMapperConfig();
            _mockLogger = new Mock<ILogger<UserService>>();
            _userService = new UserService(_mockUserManager.Object, _mockUserRepo.Object, _mapper, _mockRentRepo.Object, _mockBookRepo.Object, _mockLogger.Object);
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
        public async Task Creating_A_User_Should_Succeed()
        {
            // Arrange
            var shouldSaveAll = true;
            var role = Roles.User;
            var registerDto = UserObjectMother.CreateUserRegisterDto();

            _mockUserManager.SetupCreateUserAsync(registerDto.Password);
            _mockUserRepo.SetupAddUserAsync()
                         .SetupSaveAllAsync(shouldSaveAll);
            _mockUserManager.SetupAddToRoleAsync(role);

            //Act
            var result = await _userService.CreateUserAsync(registerDto, role);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Message.Should().Be("User successfully created!");
        }

        [Fact]
        public async Task Creating_A_User_Should_Fail_When_Identity_Create_Async_Fails()
        {
            // Arrange
            var shouldPass = false;
            var registerDto = UserObjectMother.CreateUserRegisterDto();
            var role = Roles.User;

            _mockUserManager.SetupCreateUserAsync(registerDto.Password, shouldPass);

            // Act
            var result = await _userService.CreateUserAsync(registerDto, role);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task Creating_A_User_Should_Fail_When_Save_Fails()
        {
            // Arrange
            var shouldSaveAll = false;
            var registerDto = UserObjectMother.CreateUserRegisterDto();
            var role = Roles.User;

            _mockUserManager.SetupCreateUserAsync(registerDto.Password);
            _mockUserRepo.SetupAddUserAsync()
                         .SetupSaveAllAsync(shouldSaveAll);

            // Act
            var result = await _userService.CreateUserAsync(registerDto, role);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task Creating_A_User_Should_Fail_When_Add_To_Role_Fails()
        {
            // Arrange
            var shouldSaveAll = true;
            var sholdPass = false;
            var registerDto = UserObjectMother.CreateUserRegisterDto();
            var role = Roles.User;

            _mockUserManager.SetupCreateUserAsync(registerDto.Password);
            _mockUserRepo.SetupAddUserAsync()
                         .SetupSaveAllAsync(shouldSaveAll);
            _mockUserManager.SetupAddToRoleAsync(role, sholdPass);

            // Act
            var result = await _userService.CreateUserAsync(registerDto, role);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task Updating_A_User_Details_Should_Succeed()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var updateUserDto = UserObjectMother.CreateUpdateUserDto();

            _mockUserManager.SetupFindUserByIdAsync()
                            .SetupUpdateUserAsync();

            // Act
            var result = await _userService.UpdateUserDetailsAsync(userId ,updateUserDto);

            // Assert
            result.IsSuccess.Should().BeTrue();

        }

        [Fact]
        public async Task Updating_A_User_Details_Should_Fail_When_User_Does_Not_Exist()
        {
            // Arrange
            var shouldInstantiate = false;
            var userId = Guid.NewGuid().ToString();
            var updateUserDto = UserObjectMother.CreateUpdateUserDto();
            var user = UserObjectMother.CreateUserIdentity();
            _mockUserManager.SetupFindUserByIdAsync(shouldInstantiate);

            // Act
            var result = await _userService.UpdateUserDetailsAsync(userId, updateUserDto);

            // Assert
            result.IsFailure.Should().BeTrue();
            _mockUserManager.Verify(um => um.UpdateAsync(user), Times.Never);
        }

        [Fact]
        public async Task Updating_A_User_Details_Should_Fail_When_Update_Async_Fails()
        {
            // Arrange
            var shouldPass = false;
            var userId = Guid.NewGuid().ToString();
            var updateUserDto = UserObjectMother.CreateUpdateUserDto();

            _mockUserManager.SetupFindUserByIdAsync()
                            .SetupUpdateUserAsync(shouldPass);

            // Act
            var result = await _userService.UpdateUserDetailsAsync(userId, updateUserDto);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Updating_A_User_Password_Should_Succeed()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var passwordUpdateDto = UserObjectMother.CreatePasswordUpdateDto();

            _mockUserManager.SetupFindUserByIdAsync()
                            .SetupChangePasswordAsync();

            // Act
            var result = await _userService.PasswordUpdateAsync(userId, passwordUpdateDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Message.Should().Be("User password successfully updated!");
        }

        [Fact]
        public async Task Updating_A_User_Password_Should_Fail_When_User_Does_Not_Exist()
        {
            // Arrange
            var shouldInstantiate = false;
            var userId = Guid.NewGuid().ToString();
            var passwordUpdateDto = UserObjectMother.CreatePasswordUpdateDto();

            _mockUserManager.SetupFindUserByIdAsync(shouldInstantiate);

            // Act
            var result = await _userService.PasswordUpdateAsync(userId, passwordUpdateDto);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("User does not exist.");
            _mockUserManager.Verify(um => um.ChangePasswordAsync(It.IsAny<UserIdentity>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Updating_A_User_Password_Should_Fail_When_Change_Password_Async_Fails()
        {
            // Arrange
            var shouldPass = false;
            var userId = Guid.NewGuid().ToString();
            var passwordUpdateDto = UserObjectMother.CreatePasswordUpdateDto();

            _mockUserManager.SetupFindUserByIdAsync()
                            .SetupChangePasswordAsync(shouldPass);

            // Act
            var result = await _userService.PasswordUpdateAsync(userId, passwordUpdateDto);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Retrieving_User_Profile_Should_SucceedWhen_User_Exist()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            _mockUserManager.SetupFindUserByIdAsync();

            // Act
            var result = await _userService.GetProfileAsync(userId);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Retrieving_User_Profile_Should_Fail_When_User_Does_Not_Exist()
        {
            // Arrange
            var shouldInstantiate = false;
            var userId = Guid.NewGuid().ToString();
            _mockUserManager.SetupFindUserByIdAsync(shouldInstantiate);

            // Act
            var result = await _userService.GetProfileAsync(userId);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("User does not exist.");
        }

        [Fact]
        public async Task Retrieving_All_Users_Should_Succeed()
        {
            // Arrange
            var role = Roles.User;

            _mockUserManager.SetupGetUsersInRolesAsync(role);

            // Act
            var result = await _userService.GetUsersAsync();

            // Assert
            result.Should().NotBeEmpty().And.HaveCount(2);
        }

        [Fact]
        public async Task Retrieving_All_Users_Should_Return_Empty_List_When_Users_Do_Not_Exist()
        {
            // Arrange
            var shouldBeEmpty = true;
            var role = Roles.User;

            _mockUserManager.SetupGetUsersInRolesAsync(role, shouldBeEmpty);

            // Act
            var result = await _userService.GetUsersAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Retrieving_User_History_Should_Succeed()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();

            _mockUserManager.SetupFindUserByIdAsync();
            _mockRentRepo.SetupAddAuthorAsync();
            _mockBookRepo.SetupGetBookByIdAsync();

            // Act
            var result = await _userService.GetUserRentHistoryByIdAsync(userId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Retrieving_User_History_Should_Fail_When_User_Does_Not_Exist()
        {
            // Arrange
            var shouldInstantiate = false;
            var userId = Guid.NewGuid().ToString();

            _mockUserManager.SetupFindUserByIdAsync(shouldInstantiate);

            // Act
            var result = await _userService.GetUserRentHistoryByIdAsync(userId);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("User does not exist.");
        }
    }
}
