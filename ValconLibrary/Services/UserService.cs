using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using ValconLibrary.Constants;
using ValconLibrary.DTOs;
using ValconLibrary.Entities;
using ValconLibrary.Interfaces;

namespace ValconLibrary.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IRentRepository _rentRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<UserIdentity> userManager, IUserRepository userRepo, 
                           IMapper mapper, IRentRepository rentRepository, 
                           IBookRepository bookRepository, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _userRepo = userRepo;
            _mapper = mapper;
            _rentRepository = rentRepository;
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<Result<SuccessCreateDto, IEnumerable<string>>> CreateUserAsync(RegisterDto registerDto, string role)
        {
            _logger.LogInformation($"Creating user (Email: {registerDto.Email}).");
            var errMessages = new List<string>();

            var userToRegister = new UserIdentity
            {
                Email = registerDto.Email,
                Name = registerDto.Name,
                LastName = registerDto.LastName,
                UserName = registerDto.UserName,
                DateOfBirth = registerDto.DateOfBirth ?? DateOnly.MinValue
            };

            var result = await _userManager.CreateAsync(userToRegister, registerDto.Password);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed to add user - IdentityDbContext");
                errMessages.AddRange(result.Errors.Select(error => error.Description));
                return Result.Failure<SuccessCreateDto, IEnumerable<string>>(errMessages);
            }

            var user = new User
            {
                Username = userToRegister.UserName,
                Id = userToRegister.Id,
                Email = registerDto.Email
            };

            await _userRepo.AddUserAsync(user);
            var success = await _userRepo.SaveAllAsync();
            if (!success)
            {
                errMessages.Add("Failed to add user");
                _logger.LogWarning("Failed to add user - DbContext");
                return Result.Failure<SuccessCreateDto, IEnumerable<string>>(errMessages);
            }

            var addToRole = await _userManager.AddToRoleAsync(userToRegister, role);
            if (!addToRole.Succeeded) 
            {
                errMessages.Add("Role does not exist");
                _logger.LogWarning("Role does not exist");
                return Result.Failure<SuccessCreateDto, IEnumerable<string>>(errMessages); 
            }

            _logger.LogInformation($"User (Email: {userToRegister.Email}) successfully created.");
            return Result.Success<SuccessCreateDto, IEnumerable<string>>(new SuccessCreateDto {Id = user.Id, Message = "User successfully created!" });
        }

        public async Task<Result<SuccessMessageDto, IEnumerable<string>>> UpdateUserDetailsAsync(string userId, UpdateUserDto updateUserDto)
        {
            _logger.LogInformation($"Updating user (Name: {updateUserDto.Name}).");

            var errMessages = new List<string>();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User does not exist.");
                errMessages.Add("User does not exist.");
                return Result.Failure<SuccessMessageDto, IEnumerable<string>>(errMessages);
            }

            user.Name = updateUserDto.Name;
            user.LastName = updateUserDto.LastName;
            user.DateOfBirth = updateUserDto.DateOfBirth ?? DateOnly.MinValue;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Something went wrong while updating user info.");
                errMessages.AddRange(result.Errors.Select(error => error.Description));
                return Result.Failure<SuccessMessageDto, IEnumerable<string>>(errMessages);
            }

            _logger.LogInformation("User succesfully updated.");
            return Result.Success<SuccessMessageDto, IEnumerable<string>>(new SuccessMessageDto { Message = "User succesfully updated!" });
        }

        public async Task<Result<SuccessMessageDto, IEnumerable<string>>> PasswordUpdateAsync(string userId, PasswordUpdateDto passwordUpdateDto)
        {
            _logger.LogInformation("Updating user password.");

            var errMessages = new List<string>();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User does not exist.");
                errMessages.Add("User does not exist.");
                return Result.Failure<SuccessMessageDto, IEnumerable<string>>(errMessages);
            }

            var updateResult = await _userManager.ChangePasswordAsync(user, passwordUpdateDto.OldPassword, passwordUpdateDto.NewPassword);
            if (!updateResult.Succeeded)
            {
                _logger.LogWarning("Failed to update user password.");
                errMessages.AddRange(updateResult.Errors.Select(error => error.Description));
                return Result.Failure<SuccessMessageDto, IEnumerable<string>>(errMessages);
            }

            _logger.LogInformation("User password succesfully updated.");
            return Result.Success<SuccessMessageDto, IEnumerable<string>>(new SuccessMessageDto { Message = "User password successfully updated!"} );
        }

        public async Task<Result<UserDto, string>> GetProfileAsync(string userId)
        {
            _logger.LogInformation("Retrieving user.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"User (ID: {userId}) does not exist");
                return Result.Failure<UserDto, string>("User does not exist.");
            }
               
            var userToReturn = new UserDto
            {
                UserName = user.UserName,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth
            };

            _logger.LogInformation($"User succesfully retrieved.");
            return Result.Success<UserDto, string>(userToReturn);
        }

        public async Task<Result<IEnumerable<BookRentHistory>, string>> GetUserRentHistoryByIdAsync(string userId)
        {
            _logger.LogInformation("Retrieving user rent history.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"User (ID: {userId}) does not exist.");
                return Result.Failure<IEnumerable<BookRentHistory>, string>("User does not exist.");
            }
                
            var userRentsHistory = await GetHistoryAsync(userId);

            _logger.LogInformation($"User rent history succesfully retrieved.");
            return Result.Success<IEnumerable<BookRentHistory>, string>(userRentsHistory);
        }

        public async Task<IEnumerable<UserDetailsDto>> GetUsersAsync()
        {
            _logger.LogInformation("Retrieving for all users.");
            var users = await _userManager.GetUsersInRoleAsync(Roles.User);

            var usersToReturn = _mapper.Map<List<UserDetailsDto>>(users);

            return usersToReturn;
        }

        private async Task<IEnumerable<BookRentHistory>> GetHistoryAsync(string userId)
        {
            var rents = await _rentRepository.GetRentsForUserAsync(userId);
            var rentsHistory = new List<BookRentHistory>();

            foreach (var rent in rents)
            {
                var book = await _bookRepository.GetBookByIdAsync(rent.BookId);
                var rentHistory = _mapper.Map<BookRentHistory>(book);
                rentHistory.DateRented = rent.DateRented;
                rentHistory.DateReturned = rent.DateReturned;

                rentsHistory.Add(rentHistory);
            }

            return rentsHistory;
        }
    }
}
