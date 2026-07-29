using DigitalShop.Infrastructure.Entities;
using DigitalShop.Infrastructure.Entities.dbFilter;

namespace DigitalShop.Infrastructure.Repo.Interface
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string userEmail);
        Task<(List<User> User, int TotalCount)> GetAllUsersAsync(UserFilterOptions queryParams);
        Task<User?> GetUserAsync(Guid userId);
        Task<User?> UpdateProfileAsync(Guid userId, string? newFirstName, string? newLastName, string? newPhoneNumber);
        Task<User?> DeleteUserAsync(Guid userId);
    }
}
