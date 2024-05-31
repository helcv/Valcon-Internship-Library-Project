using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ValconLibrary.Entities;
using System.Diagnostics.CodeAnalysis;

namespace ValconLibrary.Data
{
    [ExcludeFromCodeCoverage]
    public class LibraryIdentityDbContext : IdentityDbContext<UserIdentity, Role, string>
    {
        public LibraryIdentityDbContext(DbContextOptions<LibraryIdentityDbContext> options) : base(options)
        {
            
        }
    }
}
