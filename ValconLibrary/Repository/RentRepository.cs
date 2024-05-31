using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using ValconLibrary.Data;
using ValconLibrary.Entities;
using ValconLibrary.Interfaces;

namespace ValconLibrary.Repository
{
    [ExcludeFromCodeCoverage]
    public class RentRepository : IRentRepository
    {
        private readonly LibraryDbContext _context;

        public RentRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<RentBook> GetRentAsync(Guid bookId, string userId)
        {
            return await _context.Rents.FirstOrDefaultAsync(r => r.BookId == bookId && r.UserId == userId && r.DateReturned == null);
        }

        public async Task<List<RentBook>> GetRentsForUserAsync(string userId)
        {
            return await _context.Rents.Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task<List<RentBook>> GetRentsForBookAsync(Guid bookId)
        {
            return await _context.Rents
                .Include(r => r.Book)
                .Include(r => r.User)
                .Where(r => r.BookId == bookId)
                .ToListAsync();
        }

        public async Task AddRentAsync(RentBook rent)
        {
            await _context.Rents.AddAsync(rent);
        }

        public void UpdateRentAsync(RentBook rent)
        {
            _context.Rents.Update(rent);
        }

        public async Task<int> GetNumberOfActiveRentedBooksAsync(Guid bookId)
        {
            return await _context.Rents.CountAsync(rb => rb.BookId == bookId && rb.DateReturned == null);
                
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
