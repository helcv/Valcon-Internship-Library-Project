using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace ValconLibrary.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name).Value;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
