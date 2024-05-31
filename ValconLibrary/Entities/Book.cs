using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ValconLibrary.Constants;

namespace ValconLibrary.Entities
{
    [ExcludeFromCodeCoverage]
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public BookGenres Genre { get; set; }
        public int NumberOfPages { get; set; }
        public int PublishingYear { get; set; }
        public int TotalCopies { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        [JsonIgnore]
        public ICollection<Author> Authors { get; set; }
        public ICollection<RentBook> Rents { get; set; }
    }
}
