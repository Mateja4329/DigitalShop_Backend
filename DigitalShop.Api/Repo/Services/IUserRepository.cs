using DigitalShop.DTOs.User;
using DigitalShop.Entities;
using DigitalShop.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DigitalShop.Repo.Services
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string userEmail);
        Task<PaginatedList<User>> GetAllUsersAsync(UserQueryParameters queryParms);
    }
}
