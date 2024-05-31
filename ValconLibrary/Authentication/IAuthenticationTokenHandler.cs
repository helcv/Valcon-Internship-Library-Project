using ValconLibrary.Entities;

namespace ValconLibrary.Authentication
{
    public interface IAuthenticationTokenHandler
    {
        Task<string> CreateToken(UserIdentity user);
    }
}
