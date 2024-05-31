using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ValconLibrary.DTOs
{
    [ExcludeFromCodeCoverage]
    public class AuthorDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [Range(1,2024)]
        public int YearOfBirth { get; set; }
    }
}
