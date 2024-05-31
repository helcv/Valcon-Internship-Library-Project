using System.Diagnostics.CodeAnalysis;

namespace ValconLibrary.DTOs
{
    [ExcludeFromCodeCoverage]
    public class BookRentHistory
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string ISBN { get; set; }
        public int NumberOfPages { get; set; }
        public int PublishingYear { get; set; }
        public ICollection<BookAuthorDto> Authors { get; set; }
        public DateTime DateRented { get; set; }
        public DateTime? DateReturned { get; set; }
    }
}
