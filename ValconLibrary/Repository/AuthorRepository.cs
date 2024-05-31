using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using ValconLibrary.Data;
using ValconLibrary.Entities;
using ValconLibrary.Interfaces;

namespace ValconLibrary.Repository
{
    [ExcludeFromCodeCoverage]
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext _context;

        public AuthorRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public async Task AddAuthorAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
        }

        public void Update(Author author)
        {
            _context.Authors.Update(author);
        }

        public async Task<bool> DeleteAuthorAsync(Guid id)
        {
            var author = await _context.Authors.SingleOrDefaultAsync(a => !a.IsDeleted && a.Id == id);
            if (author == null)
            {
                return false;
            }

            author.IsDeleted = true;
            return true;
        }

        public IQueryable<Author> GetAllAuthors()
        {
            return _context.Authors.Where(a => !a.IsDeleted);
        }

        public async Task<IEnumerable<Author>> GetAuthorsByIdsAsync(List<Guid> authorIds)
        {
            return await _context.Authors.Where(a => authorIds.Contains(a.Id) && !a.IsDeleted).ToListAsync();
        }

        public async Task<Author> GetAuthorByIdAsync(Guid id)
        {
            return await _context.Authors.SingleOrDefaultAsync(a => !a.IsDeleted && a.Id == id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
