using ValconLibrary.Entities;

namespace ValconLibrary.Interfaces
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAuthorsByIdsAsync(List<Guid> authorIds);
        Task AddAuthorAsync(Author author);
        void Update(Author author);
        Task<Author> GetAuthorByIdAsync(Guid id);
        IQueryable<Author> GetAllAuthors();
        Task<bool> DeleteAuthorAsync(Guid id);
        Task<bool> SaveAllAsync();
    }
}
