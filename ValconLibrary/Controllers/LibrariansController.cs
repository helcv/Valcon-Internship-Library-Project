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
    public class LibrariansController : ControllerBase
    {
        private readonly IUserService _userService;

        public LibrariansController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Register new librarian
        /// </summary>
        [ProducesResponseType<UserTokenDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<IEnumerable<string>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> RegisterLibrarian(RegisterDto registerDto)
        {
            var result = await _userService.CreateUserAsync(registerDto, Roles.Librarian);
            if (result.IsFailure) return BadRequest(result.Error);

            return Ok(result.Value);
        }
    }
}
