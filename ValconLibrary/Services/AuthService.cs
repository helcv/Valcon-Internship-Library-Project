using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
using ValconLibrary.Authentication;
using ValconLibrary.DTOs;
using ValconLibrary.Entities;
using ValconLibrary.Interfaces;

namespace ValconLibrary.Services
{
    [ExcludeFromCodeCoverage]
    public class AuthService : IAuthService
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IAuthenticationTokenHandler _tokenHandler;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<UserIdentity> userManager, IAuthenticationTokenHandler tokenHandler, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _logger = logger;
        }

        public async Task<Result<UserTokenDto, string>> Login(LoginDto loginDto)
        {
            _logger.LogInformation($"User (Email: {loginDto.Email}) is logging in.");
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                _logger.LogWarning("Invalid email address.");
                return Result.Failure<UserTokenDto, string>("Invalid email address.");
            }      

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
            {
                _logger.LogWarning("Invalid password.");
                return Result.Failure<UserTokenDto, string>("Invalid password.");
            }  

            var userToReturn = new UserTokenDto

            {
                Token = await _tokenHandler.CreateToken(user)
            };

            _logger.LogInformation($"User (Email: {loginDto.Email}) succesfully logged in.");
            return Result.Success<UserTokenDto, string>(userToReturn);
        }
    }
}
