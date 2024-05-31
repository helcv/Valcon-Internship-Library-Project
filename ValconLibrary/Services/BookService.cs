using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ValconLibrary.Constants;
using ValconLibrary.DTOs;
using ValconLibrary.Entities;
using ValconLibrary.Interfaces;

namespace ValconLibrary.Services
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IRentRepository _rentRepository;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly ILogger<BookService> _logger;

        public BookService(IMapper mapper, IBookRepository bookRepository, 
                           IAuthorRepository authorRepository, IRentRepository rentRepository,
                           UserManager<UserIdentity> userManager, ILogger<BookService> logger)
        {
            _mapper = mapper;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _rentRepository = rentRepository;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result<SuccessCreateDto, string>> CreateBookAsync(CreateBookDto createBookDto)
        {
            _logger.LogInformation("Creating a book.");
            var authors = (await _authorRepository.GetAuthorsByIdsAsync(createBookDto.AuthorIds)).ToList();

            if (createBookDto.AuthorIds.Count != createBookDto.AuthorIds.Distinct().Count())
            {
                _logger.LogWarning("Duplicate author IDs are not allowed.");
                return Result.Failure<SuccessCreateDto, string>("Duplicate author IDs are not allowed.");
            }          
            
            if (authors.Count != createBookDto.AuthorIds.Count || !authors.Any())
            {
                _logger.LogWarning("Failed to find authors with provided ids.");
                return Result.Failure<SuccessCreateDto, string>("Failed to find authors with provided ids.");
            }             

            if (await _bookRepository.IsbnExists(createBookDto.ISBN))
            {
                _logger.LogWarning("Book with provided ISBN already exists.");
                return Result.Failure<SuccessCreateDto, string>("Book with provided ISBN already exists.");
            }               

            var bookToAdd = _mapper.Map<Book>(createBookDto);
            bookToAdd.CreatedAt = DateTime.UtcNow;
            bookToAdd.ModifiedAt = DateTime.UtcNow;
            bookToAdd.Authors = authors;

            await _bookRepository.AddBookAsync(bookToAdd);
            if(!await _bookRepository.SaveAllAsync())
            {
                _logger.LogWarning("Failed to add book.");
                return Result.Failure<SuccessCreateDto, string>("Failed to add book.");
            }        

            var successMessage = new SuccessCreateDto
            {
                Id = bookToAdd.Id.ToString(),
                Message = "Book succesfully created!"
            };

            _logger.LogInformation("Book succesfully created!");
            return Result.Success<SuccessCreateDto, string>(successMessage);
        }

        public async Task<Result<SuccessMessageDto, string>> UpdateBookAsync(Guid id, UpdateBookDto updateBookDto)
        {
            _logger.LogInformation($"Updating book (ID: {id})");
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning($"Book (ID: {id}) does not exist.");
                return Result.Failure<SuccessMessageDto, string>("Book with provided ID does not exist.");
            }       

            var authors = (await _authorRepository.GetAuthorsByIdsAsync(updateBookDto.AuthorIds)).ToList();

            if (updateBookDto.AuthorIds.Count != updateBookDto.AuthorIds.Distinct().Count())
            {
                _logger.LogWarning("Duplicate author IDs are not allowed.");
                return Result.Failure<SuccessMessageDto, string>("Duplicate author IDs are not allowed.");
            }     

            if (authors.Count != updateBookDto.AuthorIds.Count || !authors.Any())
            {
                _logger.LogWarning("Failed to find authors with provided ids.");
                return Result.Failure<SuccessMessageDto, string>("Failed to find authors with provided ids.");
            }          

            if (await _bookRepository.IsbnExists(updateBookDto.ISBN) && book.ISBN != updateBookDto.ISBN)
            {
                _logger.LogWarning("Book with provided ISBN already exists.");
                return Result.Failure<SuccessMessageDto, string>("Book with provided ISBN already exists.");
            }         

            book.Authors = authors;
            book.Title = updateBookDto.Title;
            book.ISBN = updateBookDto.ISBN;
            book.Genre = Enum.Parse<BookGenres>(updateBookDto.Genre);
            book.NumberOfPages = updateBookDto.NumberOfPages;
            book.PublishingYear = updateBookDto.PublishingYear;
            book.TotalCopies = updateBookDto.TotalCopies;

            _bookRepository.Update(book);
            if (!await _bookRepository.SaveAllAsync())
            {
                _logger.LogWarning("Failed to update book.");
                return Result.Failure<SuccessMessageDto, string>("Failed to update book");
            }          

            _logger.LogInformation("Book succesfully updated.");
            return Result.Success<SuccessMessageDto, string>(new SuccessMessageDto { Message = "Book succesfully updated." });
        }

        public async Task<Result<SuccessMessageDto, string>> DeleteBookAsync(Guid id)
        {
            _logger.LogInformation($"Deleting book (ID: {id})");
            if (!await _bookRepository.DeleteBookAsync(id))
            {
                _logger.LogWarning($"Failed to delete book (ID: {id}).");
                return Result.Failure<SuccessMessageDto, string>("Failed to delete book.");
            }            

            await _bookRepository.SaveAllAsync();
            _logger.LogInformation("Book succesfully deleted.");
            return Result.Success<SuccessMessageDto, string>(new SuccessMessageDto { Message = "Book succesfully deleted." });
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            _logger.LogInformation("Retrieving all books.");
            var books = await _bookRepository.GetAllBooks().ToListAsync();

            return _mapper.Map<List<BookDto>>(books);
        }

        public async Task<Result<BookDto, string>> GetBookByIdAsync(Guid id)
        {
            _logger.LogInformation($"Retrieving book (ID: {id}).");
            var book = await _bookRepository.GetBookByIdAsync(id);  
            if (book == null)
            {
                _logger.LogWarning($"Book (ID: {id}) does not exist.");
                return Result.Failure<BookDto, string>("Failed to find book.");
            }           

            var bookToReturn = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                Genre = book.Genre.ToString(),
                NumberOfPages = book.NumberOfPages,
                PublishingYear =book.PublishingYear,
                TotalCopies = book.TotalCopies,
                Authors = _mapper.Map<List<BookAuthorDto>>(book.Authors)
            };

            _logger.LogInformation("Book succesfully retrieved.");
            return Result.Success<BookDto, string>(bookToReturn);
        }

        public async Task<Result<IEnumerable<UserRentHistoryDto>, string>> GetRentHistoryForBookAsync(Guid bookId)
        {
            _logger.LogInformation($"Retrieving rent history for Book (ID: {bookId}).");
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null)
            {
                _logger.LogWarning($"Book (ID: {bookId}) does not exist.");
                return Result.Failure<IEnumerable<UserRentHistoryDto>, string>("Book does not exist.");
            }
                
            var rents = await _rentRepository.GetRentsForBookAsync(bookId);
            var rentsHistory = new List<UserRentHistoryDto>();

            foreach (var rent in rents)
            {
                var user = await _userManager.FindByIdAsync(rent.UserId);
                var rentHistory = new UserRentHistoryDto
                {
                    Id = rent.Id,
                    User = _mapper.Map<UserDto>(user),
                    DateRented = rent.DateRented,
                    DateReturned = rent.DateReturned
                };

                rentsHistory.Add(rentHistory);
            }

            _logger.LogInformation("Rent history succesfully returned.");
            return Result.Success<IEnumerable<UserRentHistoryDto>, string>(rentsHistory);
        }
    }
}
