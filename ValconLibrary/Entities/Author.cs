using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ValconLibrary.Entities
{
    [ExcludeFromCodeCoverage]
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int YearOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Book> Books { get; set; }  
    }
}
