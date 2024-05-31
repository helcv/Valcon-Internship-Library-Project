using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ValconLibrary.Constants;
using ValconLibrary.Entities;

namespace ValconLibrary.DTOs
{
    [ExcludeFromCodeCoverage]
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public int NumberOfPages { get; set; }
        public int PublishingYear { get; set; }
        public int TotalCopies { get; set; }
        public ICollection<BookAuthorDto> Authors { get; set; }
    }
}
