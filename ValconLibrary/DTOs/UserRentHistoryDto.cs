using System.Diagnostics.CodeAnalysis;

namespace ValconLibrary.DTOs
{
    [ExcludeFromCodeCoverage]
    public class UserRentHistoryDto
    {
        public Guid Id { get; set; }
        public UserDto User { get; set; }
        public DateTime DateRented { get; set; }
        public DateTime? DateReturned { get; set; }
    }
}
