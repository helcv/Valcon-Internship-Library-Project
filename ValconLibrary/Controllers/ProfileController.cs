using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using ValconLibrary.Constants;
using ValconLibrary.DTOs;
using ValconLibrary.Extensions;
using ValconLibrary.Interfaces;

namespace ValconLibrary.Controllers
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;

        public ProfileController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _contextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Return user informations
        /// </summary>
        [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<IEnumerable<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserProfile()
        {
            var currUser = _contextAccessor.HttpContext.User;
            var currUserId = currUser.GetUserId();

            var result = await _userService.GetProfileAsync(currUserId);
            if (result.IsFailure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Update user informations
        /// </summary>
        [ProducesResponseType<SuccessMessageDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<IEnumerable<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpPut("details")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            var currUser = _contextAccessor.HttpContext.User;
            var currUserId = currUser.GetUserId();

            var result = await _userService.UpdateUserDetailsAsync(currUserId, updateUserDto);
            if (result.IsFailure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Update user password
        /// </summary>
        [ProducesResponseType<SuccessMessageDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<IEnumerable<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpPut("password")]
        public async Task<IActionResult> UpdateUserPassword(PasswordUpdateDto passwordUpdateDto)
        {
            var currUser = _contextAccessor.HttpContext.User;
            var currUserId = currUser.GetUserId();

            var result = await _userService.PasswordUpdateAsync(currUserId, passwordUpdateDto);
            if (result.IsFailure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Return user rent history
        /// </summary>
        [ProducesResponseType<SuccessMessageDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<IEnumerable<BookRentHistory>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.User)]
        [HttpGet("history")]
        public async Task<IActionResult> GetUserRentHistory()
        {
            var currUser = _contextAccessor.HttpContext.User;
            var currUserId = currUser.GetUserId();

            var result = await _userService.GetUserRentHistoryByIdAsync(currUserId);
            if (result.IsFailure) return BadRequest(result.Error);

            return Ok(result.Value);
        }
    }
}
