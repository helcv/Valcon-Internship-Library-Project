using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ValconLibrary.DTOs
{
    [ExcludeFromCodeCoverage]
    public class UpdateUserDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateOnly? DateOfBirth { get; set; }
    }
}
