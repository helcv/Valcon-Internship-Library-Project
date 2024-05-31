using Microsoft.AspNetCore.Identity;

namespace ValconLibrary.Entities
{
    public class UserIdentity : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
