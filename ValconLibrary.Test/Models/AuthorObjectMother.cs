using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValconLibrary.DTOs;
using ValconLibrary.Entities;

namespace ValconLibrary.Test.Models
{
    public class AuthorObjectMother
    {
        public static Author CreateDefaultAuthor()
        {
            return new Author
            {
                Id = Guid.NewGuid(),
                Name = "Default",
                LastName = "Author",
                YearOfBirth = 1970,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                IsDeleted = false
            };
        }

        public static AuthorDto CreateDefaultAuthorDto()
        {
            return new AuthorDto
            {
                Name = "Default",
                LastName = "Author",
                YearOfBirth = 1970
            };
        }

        public static AuthorDetailsDto CreateDefaultAuthorDetailsDto()
        {
            return new AuthorDetailsDto
            {
                Id = Guid.NewGuid(),
                Name = "Default",
                LastName= "Author",
                YearOfBirth = 1970,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };
        }
    }
}
