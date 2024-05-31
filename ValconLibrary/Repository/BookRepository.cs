using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using ValconLibrary.Data;
using ValconLibrary.Entities;
using ValconLibrary.Interfaces;

namespace ValconLibrary.Repository
{
    [ExcludeFromCodeCoverage]
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
        }

        public async Task<bool> DeleteBookAsync(Guid id)
        {
            var book = await _context.Books.SingleOrDefaultAsync(b => !b.IsDeleted && b.Id == id);
            if (book == null)
            {
                return false;
            }

            book.IsDeleted = true;
            return true;
        }

        public IQueryable<Book> GetAllBooks()
        {
            return _context.Books.Where(b => !b.IsDeleted).Include(a => a.Authors);
        }

        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            return await _context.Books
                .Include(a => a.Authors)
                .SingleOrDefaultAsync(a => !a.IsDeleted && a.Id == id);
        }
    
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsbnExists(string isbn)
        {
            return await _context.Books.AnyAsync(b => b.ISBN == isbn && !b.IsDeleted);
        }

        public void Update(Book book)
        {
            _context.Books.Update(book);
        }
    }
}
