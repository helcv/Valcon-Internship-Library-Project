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
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Create new book
        /// </summary>
        [ProducesResponseType<SuccessMessageDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.Librarian)]
        [HttpPost]
        public async Task<IActionResult> AddAuthor(CreateBookDto createBookDto)
        {
            var result = await _bookService.CreateBookAsync(createBookDto);
            if (result.IsFailure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Return all books
        /// </summary>
        [ProducesResponseType<IEnumerable<BookDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.Librarian)]
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var result = await _bookService.GetAllBooksAsync();

            return Ok(result);
        }

        /// <summary>
        /// Get book by Id
        /// </summary>
        [ProducesResponseType<BookDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.Librarian)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBookById(Guid Id)
        {
            var result = await _bookService.GetBookByIdAsync(Id);
            if (result.IsFailure) return NotFound(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Update book info
        /// </summary>
        [ProducesResponseType<SuccessMessageDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.Librarian)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] UpdateBookDto updateBookDto)
        {
            var result = await _bookService.UpdateBookAsync(id, updateBookDto);
            if (result.IsFailure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Delete book by Id
        /// </summary>
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.Librarian)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBookById(Guid Id)
        {
            var result = await _bookService.DeleteBookAsync(Id);
            if (result.IsFailure) return NotFound(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Return rent history for specific book
        /// </summary>
        [ProducesResponseType<IEnumerable<UserRentHistoryDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.Librarian + "," + Roles.Admin)]
        [HttpGet("{id:guid}/history")]
        public async Task<IActionResult> GetBookRentHistory(Guid id)
        {
            var result = await _bookService.GetRentHistoryForBookAsync(id);
            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }
    }
}
