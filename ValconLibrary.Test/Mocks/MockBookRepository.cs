using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValconLibrary.Entities;
using ValconLibrary.Interfaces;
using ValconLibrary.Test.Models;

namespace ValconLibrary.Test.Mocks
{
    public static class MockBookRepository
    {
        public static Mock<IBookRepository> SetupGetBookByIdAsync(this Mock<IBookRepository> mock)
        {
            mock.Setup(b => b.GetBookByIdAsync(It.IsAny<Guid>()))
                            .ReturnsAsync(BookObjectMother.CreateBook());
            return mock;
        }
    }
}
