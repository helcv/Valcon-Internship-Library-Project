using CSharpFunctionalExtensions;
using ValconLibrary.DTOs;

namespace ValconLibrary.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserTokenDto, string>> Login(LoginDto loginDto);
    }
}
