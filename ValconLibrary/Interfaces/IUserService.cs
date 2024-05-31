using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ValconLibrary.DTOs;
using ValconLibrary.Entities;

namespace ValconLibrary.Interfaces
{
    public interface IUserService
    {
        Task<Result<SuccessCreateDto, IEnumerable<string>>> CreateUserAsync(RegisterDto registerDto, string role);
        Task<IEnumerable<UserDetailsDto>> GetUsersAsync();
        Task<Result<SuccessMessageDto, IEnumerable<string>>> UpdateUserDetailsAsync(string userId, UpdateUserDto updateUserDto);
        Task<Result<SuccessMessageDto, IEnumerable<string>>> PasswordUpdateAsync(string userId, PasswordUpdateDto passwordUpdateDto);
        Task<Result<UserDto, string>> GetProfileAsync(string userId);
        Task<Result<IEnumerable<BookRentHistory>, string>> GetUserRentHistoryByIdAsync(string userId);
    }
}
