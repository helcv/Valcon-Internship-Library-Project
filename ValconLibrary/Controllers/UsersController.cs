using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using ValconLibrary.Constants;
using ValconLibrary.DTOs;
using ValconLibrary.Interfaces;

namespace ValconLibrary.Controllers
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        [ProducesResponseType<SuccessMessageDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<IEnumerable<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.Librarian)]
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterDto registerDto)
        {
            var result = await _userService.CreateUserAsync(registerDto, Roles.User);
            if (result.IsFailure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Return all users
        /// </summary>
        [ProducesResponseType<IEnumerable<UserDetailsDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<IEnumerable<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.Librarian + "," + Roles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetUsersAsync();

            return Ok(result);
        }

        /// <summary>
        /// Return specific user rent history
        /// </summary>
        [ProducesResponseType<SuccessMessageDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<IEnumerable<BookRentHistory>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.Librarian)]
        [HttpGet("{id}/history")]
        public async Task<IActionResult> GetUserRentHistory(string id)
        {
            var result = await _userService.GetUserRentHistoryByIdAsync(id);
            if (result.IsFailure) return BadRequest(result.Error);

            return Ok(result.Value);
        }
    }
}
