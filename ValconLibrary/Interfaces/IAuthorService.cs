using CSharpFunctionalExtensions;
using ValconLibrary.DTOs;

namespace ValconLibrary.Interfaces
{
    public interface IAuthorService
    {
        Task<Result<SuccessCreateDto, string>> CreateAuthorAsync(AuthorDto authorDto);
        Task<Result<SuccessMessageDto, string>> UpdateAuthorAsync(Guid id, AuthorDto updateAuthorDto);
        Task<Result<AuthorDetailsDto, string>> GetAuthorByIdAsync(Guid id);
        IEnumerable<AuthorDetailsDto> GetAllAuthors();
        Task<Result<SuccessMessageDto, string>> DeleteAuthorAsync(Guid id);
    }
}
