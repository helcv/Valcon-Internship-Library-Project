using CSharpFunctionalExtensions;
using ValconLibrary.DTOs;
using ValconLibrary.Entities;

namespace ValconLibrary.Interfaces
{
    public interface IRentService
    {
        Task<Result<SuccessMessageDto, string>> RentBookAsync(RentBookDto rentBookDto);
        Task<Result<SuccessMessageDto, string>> ReturnBookAsync(ReturnBookDto returnBookDto);
    }
}
