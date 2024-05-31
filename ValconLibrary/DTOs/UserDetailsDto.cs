using System.Diagnostics.CodeAnalysis;

namespace ValconLibrary.DTOs
{
    [ExcludeFromCodeCoverage]
    public class UserDetailsDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
