using Moq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValconLibrary.Interfaces;
using ValconLibrary.Test.Models;

namespace ValconLibrary.Test.Mocks
{
    public static class MockRentRepository
    {
        public static Mock<IRentRepository> SetupAddAuthorAsync(this Mock<IRentRepository> mock)
        {
            mock.Setup(r => r.GetRentsForUserAsync(It.IsAny<string>()))
                            .ReturnsAsync(RentObjectMother.CreateRentsList);
            return mock;
        }
    }
}
