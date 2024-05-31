using ValconLibrary.Entities;

namespace ValconLibrary.Interfaces
{
    public interface IBookRepository
    {
        Task AddBookAsync(Book book);
        void Update(Book book);
        Task<Book> GetBookByIdAsync(Guid id);
        IQueryable<Book> GetAllBooks();
        Task<bool> DeleteBookAsync(Guid id);
        Task<bool> SaveAllAsync();
        Task<bool> IsbnExists(string isbn);
    }
}
