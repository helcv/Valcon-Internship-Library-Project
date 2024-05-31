using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
using ValconLibrary.Constants;
using ValconLibrary.DTOs;
using ValconLibrary.Entities;
using ValconLibrary.Interfaces;

namespace ValconLibrary.Services
{
    [ExcludeFromCodeCoverage]
    public class RentService : IRentService
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IBookRepository _bookRepository;
        private readonly IRentRepository _rentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RentService> _logger;

        public RentService(UserManager<UserIdentity> userManager, IBookRepository bookRepository, 
                           IRentRepository rentRepository, IUserRepository userRepository,
                           IMapper mapper, ILogger<RentService> logger)
        {
            _userManager = userManager;
            _bookRepository = bookRepository;
            _rentRepository = rentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<SuccessMessageDto, string>> RentBookAsync(RentBookDto rentBookDto)
        {
            _logger.LogInformation("Renting a book.");
            var identityUser = await _userManager.FindByIdAsync(rentBookDto.UserId);

            if (identityUser == null)
            {
                _logger.LogWarning($"User (ID: {rentBookDto.UserId}) does not exist.");
                return Result.Failure<SuccessMessageDto, string>("User does not exist.");
            }

            if (!await _userManager.IsInRoleAsync(identityUser, Roles.User))
            {
                _logger.LogWarning("User is not in expected role.");
                return Result.Failure<SuccessMessageDto, string>("You can not rent book for this user.");
            }              

            var userDb = await _userRepository.GetUserById(rentBookDto.UserId);

            var book = await _bookRepository.GetBookByIdAsync(rentBookDto.BookId);

            if (book == null)
            {
                _logger.LogWarning($"Book (ID: {book.Id}) does not exist.");
                return Result.Failure<SuccessMessageDto, string>("Book does not exist.");
            }
                
            if (!await IsAvailableAsync(book))
            {
                _logger.LogWarning($"Book (ID: {book.Id}) is not available.");
                return Result.Failure<SuccessMessageDto, string>("Book is not available");
            }           

            var rent = new RentBook
            {
                UserId = userDb.Id,
                User = userDb,
                BookId = book.Id,
                Book = book,
                DateRented = rentBookDto.DateRented ?? DateTime.UtcNow
            };

            await _rentRepository.AddRentAsync(rent);
            if(!await _rentRepository.SaveAllAsync())
            {
                _logger.LogWarning("Failed to create a rent.");
                return Result.Failure<SuccessMessageDto, string>("Failed to create a rent.");
            }
                

            _logger.LogInformation("Book succesfully rented.");
            return Result.Success<SuccessMessageDto, string>(new SuccessMessageDto { Message = "Rent succesfully created!" });
        }

        public async Task<Result<SuccessMessageDto, string>> ReturnBookAsync(ReturnBookDto returnBookDto)
        {
            _logger.LogInformation("Returning book.");
            var user = await _userRepository.GetUserById(returnBookDto.UserId);

            if (user == null)
            {
                _logger.LogWarning($"User (ID: {returnBookDto.BookId}) does not exist.");
                return Result.Failure<SuccessMessageDto, string>("User does not exist.");
            }
                
            var book = await _bookRepository.GetBookByIdAsync(returnBookDto.BookId);

            if (book == null)
            {
                _logger.LogWarning($"Book (ID: {returnBookDto.BookId}) does not exist.");
                return Result.Failure<SuccessMessageDto, string>("Book does not exist.");
            }          

            var rent = await _rentRepository.GetRentAsync(book.Id, user.Id);
            if (rent == null)
            {
                _logger.LogWarning("Provided book is not rented for provided user.");
                return Result.Failure<SuccessMessageDto, string>("Provided book is not rented for provided user.");
            }           

            rent.DateReturned = returnBookDto.DateReturned ?? DateTime.UtcNow;

            if (rent.DateReturned < rent.DateRented)
            {
                _logger.LogWarning("Rent date must be before return date.");
                return Result.Failure<SuccessMessageDto, string>("Rent date must be before return date.");
            }              

            _rentRepository.UpdateRentAsync(rent);
            if(! await _rentRepository.SaveAllAsync())
            {
                _logger.LogWarning("Failed to return a book.");
                return Result.Failure<SuccessMessageDto, string>("Failed to return book.");
            }

            _logger.LogInformation("Book succesfully returned.");
            return Result.Success<SuccessMessageDto, string>(new SuccessMessageDto { Message = "Book succesfully returned!" });
        }

        private async Task<bool> IsAvailableAsync(Book book)
        {
            var booksRented = await _rentRepository.GetNumberOfActiveRentedBooksAsync(book.Id);

            return booksRented < book.TotalCopies;
        }
    }
}
