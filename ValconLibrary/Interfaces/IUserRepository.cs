using ValconLibrary.Entities;

namespace ValconLibrary.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User> GetUserById(string id);
        Task<bool> SaveAllAsync();
    }
}
