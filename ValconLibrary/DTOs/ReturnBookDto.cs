using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ValconLibrary.DTOs
{
    [ExcludeFromCodeCoverage]
    public class ReturnBookDto
    {
        [Required]
        public Guid BookId { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime? DateReturned { get; set; }
    }
}
