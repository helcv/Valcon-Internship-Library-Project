using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValconLibrary.Entities;

namespace ValconLibrary.Test.Models
{
    public static class RentObjectMother
    {
        public static List<RentBook> CreateRentsList()
        {
            return new List<RentBook>
        {
            new RentBook
            {
                UserId = Guid.NewGuid().ToString(),
                BookId = Guid.NewGuid(),
                DateRented = new DateTime(2023, 1, 1),
                DateReturned = new DateTime(2023, 1, 15)
            },
            new RentBook
            {
                UserId = Guid.NewGuid().ToString(),
                BookId = Guid.NewGuid(),
                DateRented = new DateTime(2023, 2, 1),
                DateReturned = new DateTime(2023, 2, 15)
            }
        };
        }
    }
}
