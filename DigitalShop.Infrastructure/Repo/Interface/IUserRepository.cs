using DigitalShop.Infrastructure.Entities;
using DigitalShop.Infrastructure.Entities.dbFilter;

namespace DigitalShop.Infrastructure.Repo.Interface
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string userEmail);
        Task<(List<User> User, int TotalCount)> GetAllUsersAsync(UserFilterOptions queryParams);
    }
}
