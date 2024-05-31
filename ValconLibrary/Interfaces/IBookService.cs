using CSharpFunctionalExtensions;
using ValconLibrary.DTOs;

namespace ValconLibrary.Interfaces
{
    public interface IBookService
    {
        Task<Result<SuccessCreateDto, string>> CreateBookAsync(CreateBookDto createBookDto);
        Task<Result<SuccessMessageDto, string>> UpdateBookAsync(Guid id, UpdateBookDto updateBookDto);
        Task<Result<BookDto, string>> GetBookByIdAsync(Guid id);
        Task<Result<IEnumerable<UserRentHistoryDto>, string>> GetRentHistoryForBookAsync(Guid bookId);
        Task<IEnumerable<BookDto>> GetAllBooksAsync();
        Task<Result<SuccessMessageDto, string>> DeleteBookAsync(Guid id);
    }
}
