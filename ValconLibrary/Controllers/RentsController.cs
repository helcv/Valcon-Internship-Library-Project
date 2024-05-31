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
    [Route("api/")]
    public class RentsController : ControllerBase
    {
        private readonly IRentService _rentService;

        public RentsController(IRentService rentService)
        {
            _rentService = rentService;
        }

        /// <summary>
        /// Rent book
        /// </summary>
        [ProducesResponseType<SuccessMessageDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.Librarian)]
        [HttpPost("rent-book")]
        public async Task<IActionResult> RentBook(RentBookDto rentBookDto)
        {
            var result = await _rentService.RentBookAsync(rentBookDto);
            if (result.IsFailure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Return book
        /// </summary>
        [ProducesResponseType<SuccessMessageDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.Librarian)]
        [HttpPost("return-book")]
        public async Task<IActionResult> ReturnBook(ReturnBookDto returnBookDto)
        {
            var result = await _rentService.ReturnBookAsync(returnBookDto);
            if (result.IsFailure) return BadRequest(result.Error);

            return Ok(result.Value);
        }
    }
}
