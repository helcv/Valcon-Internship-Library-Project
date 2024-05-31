using System.Diagnostics.CodeAnalysis;

namespace ValconLibrary.Entities
{
    [ExcludeFromCodeCoverage]
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public ICollection<RentBook> Rents { get; set; }
    }
}
