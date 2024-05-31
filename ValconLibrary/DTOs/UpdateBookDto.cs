using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using ValconLibrary.Constants;

namespace ValconLibrary.DTOs
{
    [ExcludeFromCodeCoverage]
    public class UpdateBookDto
    {
        [Required]
        public List<Guid> AuthorIds { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [RegularExpression(@"\b97[89]\d{9}(\d|X)\b", ErrorMessage = "Invalid ISBN-13 number.")]
        public string ISBN { get; set; }
        [Required]
        [EnumDataType(typeof(BookGenres), ErrorMessage = "Invalid genre specified.")]
        public string Genre { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of pages can not be less than 1.")]
        public int NumberOfPages { get; set; }
        [Required]
        [Range(1, 2024)]
        public int PublishingYear { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Total copies can not be negative.")]
        public int TotalCopies { get; set; }
    }
}
