using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using ValconLibrary.Authentication;
using ValconLibrary.DTOs;
using ValconLibrary.Entities;
using ValconLibrary.Interfaces;

namespace ValconLibrary.Data
{
    [ExcludeFromCodeCoverage]
    public class Seed
    {
        public static async Task SeedRolesAndAdmin(UserManager<UserIdentity> userManager, RoleManager<Role> roleManager, 
                                                   IConfiguration config, LibraryDbContext context)
        {
            if (await userManager.Users.AnyAsync()) return;

            var roles = new List<Role>()
            {
                new Role {Name = "Admin"},
                new Role {Name = "User"},
                new Role {Name = "Librarian"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new UserIdentity
            {
                Name = "Admin",
                UserName = "admin",
                LastName = "Admin",
                Email = config["Admin:Email"]
            };

            var result = await userManager.CreateAsync(admin, config["Admin:Password"]);
            context.Users.Add(new User { Id = admin.Id, Email = admin.Email, Username = admin.UserName});
            await context.SaveChangesAsync();

            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}
