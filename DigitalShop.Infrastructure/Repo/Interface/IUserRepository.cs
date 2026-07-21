using DigitalShop.Application.DTOs.User;
using DigitalShop.Infrastructure.Entities;
using DigitalShop.Application.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DigitalShop.Infrastructure.Repo.Interface
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string userEmail);
        Task<PaginatedList<User>> GetAllUsersAsync(UserQueryParameters queryParms);
    }
}
