using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using ValconLibrary.Constants;
using ValconLibrary.DTOs;
using ValconLibrary.Interfaces;
using ValconLibrary.Services;

namespace ValconLibrary.Controllers
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        /// <summary>
        /// Create new author
        /// </summary>
        [ProducesResponseType<SuccessMessageDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.Librarian)]
        [HttpPost]
        public async Task<IActionResult> AddAuthor(AuthorDto authorDto)
        {
            var result = await _authorService.CreateAuthorAsync(authorDto);
            if (result.IsFailure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Update author info
        /// </summary>
        [ProducesResponseType<SuccessMessageDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.Librarian)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody]AuthorDto authorDto)
        {
            var result = await _authorService.UpdateAuthorAsync(id, authorDto);
            if (result.IsFailure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Get author by Id
        /// </summary>
        [ProducesResponseType<AuthorDetailsDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.Librarian)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAuthorById(Guid Id)
        {
            var result = await _authorService.GetAuthorByIdAsync(Id);
            if (result.IsFailure) return NotFound(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Delete author by Id
        /// </summary>
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.Librarian)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAuthorById(Guid Id)
        {
            var result = await _authorService.DeleteAuthorAsync(Id);
            if (result.IsFailure) return NotFound(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Return all authors
        /// </summary>
        [ProducesResponseType<IEnumerable<AuthorDetailsDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.Librarian)]
        [HttpGet]
        public IActionResult GetAllAuthors()
        {
            var result = _authorService.GetAllAuthors();

            return Ok(result);
        }
    }
}
