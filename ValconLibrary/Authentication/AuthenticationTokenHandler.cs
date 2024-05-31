using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ValconLibrary.Entities;

namespace ValconLibrary.Authentication
{
    [ExcludeFromCodeCoverage]
    public class AuthenticationTokenHandler : IAuthenticationTokenHandler
    {
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly ILogger<AuthenticationTokenHandler> _logger;

        public AuthenticationTokenHandler(IConfiguration config, UserManager<UserIdentity> userManager, ILogger<AuthenticationTokenHandler> logger)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            _userManager = userManager;
            _logger = logger;
        }


        public async Task<string>  CreateToken(UserIdentity user)
        {
            _logger.LogInformation($"Trying to create JWT token for User - {user.Email}");
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id)
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            _logger.LogInformation("Token succesfully created.");
            return tokenHandler.WriteToken(token);
        }
    }
}
