using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValconLibrary.Entities;
using ValconLibrary.Interfaces;

namespace ValconLibrary.Test.Mocks
{
    public static class MockUserRepo
    {
        public static Mock<IUserRepository> SetupAddUserAsync(this Mock<IUserRepository> mock)
        {
            mock.Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);
            return mock;
        }

        public static Mock<IUserRepository> SetupSaveAllAsync(this Mock<IUserRepository> mock, bool shouldSaveAll)
        {
            mock.Setup(repo => repo.SaveAllAsync())
                .ReturnsAsync(shouldSaveAll);
            return mock;
        }
    }
}
