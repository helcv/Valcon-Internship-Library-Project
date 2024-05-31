using System.Diagnostics.CodeAnalysis;
using ValconLibrary.Data;
using ValconLibrary.DTOs;
using ValconLibrary.Entities;
using ValconLibrary.Interfaces;

namespace ValconLibrary.Repository
{
    [ExcludeFromCodeCoverage]
    public class UserRepository : IUserRepository
    {
        private readonly LibraryDbContext _context;

        public UserRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }
        public async Task<User> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
