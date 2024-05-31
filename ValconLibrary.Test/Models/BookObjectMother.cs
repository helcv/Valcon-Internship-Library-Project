using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValconLibrary.Entities;

namespace ValconLibrary.Test.Models
{
    public static class BookObjectMother
    {
        public static List<Book> CreateBooksList()
        {
            return new List<Book>
            {
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "Book 1",
                    Authors = new List<Author> {
                        AuthorObjectMother.CreateDefaultAuthor(),
                        AuthorObjectMother.CreateDefaultAuthor()
                    }
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "Book 2",
                    Authors = new List<Author> {
                        AuthorObjectMother.CreateDefaultAuthor(),
                        AuthorObjectMother.CreateDefaultAuthor()
                    }
                }
            };
        }

        public static Book CreateBook()
        {
            return new Book
            {
                Id = Guid.NewGuid(),
                Title = "Book 1",
                Authors = new List<Author> {
                    AuthorObjectMother.CreateDefaultAuthor(),
                    AuthorObjectMother.CreateDefaultAuthor()
                }
            };
        }
    }
}
