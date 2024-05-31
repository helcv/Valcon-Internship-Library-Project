using ValconLibrary.Entities;

namespace ValconLibrary.Interfaces
{
    public interface IRentRepository
    {
        Task AddRentAsync(RentBook rent);
        void UpdateRentAsync(RentBook rent);
        Task<RentBook> GetRentAsync(Guid bookId, string userId);
        Task<List<RentBook>> GetRentsForUserAsync(string userId);
        Task<List<RentBook>> GetRentsForBookAsync(Guid bookId);
        Task<int> GetNumberOfActiveRentedBooksAsync(Guid bookId);
        Task<bool> SaveAllAsync();
    }
}
