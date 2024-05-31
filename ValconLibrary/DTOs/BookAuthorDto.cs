using System.Diagnostics.CodeAnalysis;

namespace ValconLibrary.DTOs
{
    [ExcludeFromCodeCoverage]
    public class BookAuthorDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int YearOfBirth { get; set; }
    }
}
