using Microsoft.AspNetCore.Identity;
using Moq;
using ValconLibrary.Entities;
using ValconLibrary.Test.Models;

namespace ValconLibrary.Test.Mocks
{
    public static class MockUserManager
    {
        public static Mock<UserManager<UserIdentity>> SetupFindUserByEmailAsync(this Mock<UserManager<UserIdentity>> mock, bool shouldInstantiate = false)
        {
            var user = shouldInstantiate ? UserObjectMother.CreateUserIdentity() : null;
            mock.Setup(um => um.FindByEmailAsync("test@mail.com"))
                        .ReturnsAsync(user);
            return mock;
        }

        public static Mock<UserManager<UserIdentity>> SetupCreateUserAsync(this Mock<UserManager<UserIdentity>> mock, string password, bool shouldPass = true)
        {
            var result = shouldPass ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Failed to add user to role" });
            mock.Setup(um => um.CreateAsync(It.IsAny<UserIdentity>(), password))
                            .ReturnsAsync(result);
            return mock;
        }

        public static Mock<UserManager<UserIdentity>> SetupAddToRoleAsync(this Mock<UserManager<UserIdentity>> mock, string role, bool shouldPass = true)
        {
            var result = shouldPass ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Failed to add user to role" });
            mock.Setup(um => um.AddToRoleAsync(It.IsAny<UserIdentity>(), role))
                            .ReturnsAsync(result);
            return mock;
        }

        public static Mock<UserManager<UserIdentity>> SetupFindUserByIdAsync(this Mock<UserManager<UserIdentity>> mock, bool shouldInstantiate = true)
        {
            var user = shouldInstantiate ? UserObjectMother.CreateUserIdentity() : null;
            mock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                            .ReturnsAsync(user);
            return mock;
        }

        public static Mock<UserManager<UserIdentity>> SetupUpdateUserAsync(this Mock<UserManager<UserIdentity>> mock, bool shouldPass = true)
        {
            var result = shouldPass ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Failed to add user to role" });
            mock.Setup(um => um.UpdateAsync(It.IsAny<UserIdentity>()))
                            .ReturnsAsync(result);
            return mock;
        }

        public static Mock<UserManager<UserIdentity>> SetupChangePasswordAsync(this Mock<UserManager<UserIdentity>> mock, bool shouldPass = true)
        {
            var result = shouldPass ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Failed to update user password" });
            mock.Setup(um => um.ChangePasswordAsync(It.IsAny<UserIdentity>(), It.IsAny<string>(), It.IsAny<string>()))
                            .ReturnsAsync(result);
            return mock;
        }

        public static Mock<UserManager<UserIdentity>> SetupGetUsersInRolesAsync(this Mock<UserManager<UserIdentity>> mock, string role, bool shouldBeEmpty = false)
        {
            var result = shouldBeEmpty ? new List<UserIdentity>() : UserObjectMother.CreateUsersList();
            mock.Setup(um => um.GetUsersInRoleAsync(role))
                            .ReturnsAsync(result);
            return mock;
        }

    }
}
