using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ValconLibrary.DTOs;
using ValconLibrary.Entities;
using ValconLibrary.Interfaces;

namespace ValconLibrary.Services
{

    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(IAuthorRepository authorRepo, IMapper mapper, ILogger<AuthorService> logger)
        {
            _authorRepo = authorRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<SuccessCreateDto, string>> CreateAuthorAsync(AuthorDto authorDto)
        {
            _logger.LogInformation("Creating new author.");
            var authorToAdd = new Author
            {
                Name = authorDto.Name,
                LastName = authorDto.LastName,
                YearOfBirth = authorDto.YearOfBirth,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            await _authorRepo.AddAuthorAsync(authorToAdd);
            if (!await _authorRepo.SaveAllAsync())
            {
                _logger.LogWarning("Failed to add author.");
                return Result.Failure<SuccessCreateDto, string>("Failed to add author.");
            }

            var successMessage = new SuccessCreateDto
            {
                Id = authorToAdd.Id.ToString(),
                Message = "Author succesfully created!"
            };

            _logger.LogInformation("Author succesfully created.");
            return Result.Success<SuccessCreateDto, string>(successMessage);
        }

        public async Task<Result<AuthorDetailsDto, string>> GetAuthorByIdAsync(Guid id)
        {
            _logger.LogInformation($"Retrieving author (ID: {id}).");
            var author = await _authorRepo.GetAuthorByIdAsync(id);
            if (author == null)
            {
                _logger.LogError($"Author (ID: {id}) does not exist.");
                return Result.Failure<AuthorDetailsDto, string>("Author does not exist.");
            }

            var authorToReturn = new AuthorDetailsDto
            {
                Id = author.Id,
                Name = author.Name,
                LastName = author.LastName,
                YearOfBirth = author.YearOfBirth,
                CreatedAt = author.CreatedAt,
                ModifiedAt = author.ModifiedAt
            };

            _logger.LogInformation("Author succesfully retrieved.");
            return Result.Success<AuthorDetailsDto, string>(authorToReturn);
        }

        public IEnumerable<AuthorDetailsDto> GetAllAuthors()
        {
            _logger.LogInformation("Retrieving all authors.");
            var authors = _authorRepo.GetAllAuthors().ToList();

            return _mapper.Map<List<AuthorDetailsDto>>(authors);
        }

        public async Task<Result<SuccessMessageDto, string>> DeleteAuthorAsync(Guid id)
        {
            _logger.LogInformation($"Deleting author (ID: {id}).");
            if (!await _authorRepo.DeleteAuthorAsync(id))
            {
                _logger.LogWarning($"Failed to delete author (ID: {id}).");
                return Result.Failure<SuccessMessageDto, string>("Failed to delete author");
            }

            await _authorRepo.SaveAllAsync();
            _logger.LogInformation("Author succesfully deleted.");
            return Result.Success<SuccessMessageDto, string>(new SuccessMessageDto { Message = "Author succesfully deleted." });
        }

        public async Task<Result<SuccessMessageDto, string>> UpdateAuthorAsync(Guid id, AuthorDto updateAuthorDto)
        {
            _logger.LogInformation($"Updating author (ID: {id}).");
            var author = await _authorRepo.GetAuthorByIdAsync(id);
            if (author == null)
            {
                _logger.LogWarning($"Author (ID: {id}) does not exist.");
                return Result.Failure<SuccessMessageDto, string>("Author does not exist");
            }

            author.Name = updateAuthorDto.Name;
            author.LastName = updateAuthorDto.LastName;
            author.YearOfBirth = updateAuthorDto.YearOfBirth;
            author.ModifiedAt = DateTime.UtcNow;

            _authorRepo.Update(author);
            if (!await _authorRepo.SaveAllAsync())
            {
                _logger.LogWarning("Failed to update author.");
                return Result.Failure<SuccessMessageDto, string>("Failed to update author.");
            }

            _logger.LogInformation("Author succesfully updated.");
            return Result.Success<SuccessMessageDto, string>(new SuccessMessageDto { Message = "Author succesfully updated." });
        }
    }
}
