using DigitalShop.Infrastructure.Data;
using DigitalShop.Application.DTOs.User;
using DigitalShop.Infrastructure.Entities;
using DigitalShop.Application.Helpers;
using DigitalShop.Infrastructure.Repo.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace DigitalShop.Infrastructure.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext dataContext;

        public UserRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        // POST ==============================================
        // REGISTER ---------------------------------------------
        public async Task<User> AddUserAsync(User user)
        {
            await dataContext.Users.AddAsync(user);
            await dataContext.SaveChangesAsync();
            return user;
        }

        // LOGIN ---------------------------------------------
        public async Task<User?> GetUserByEmailAsync(string userEmail)
        {
            return await dataContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        }

        // GET ================================================
        public async Task<PaginatedList<User>> GetAllUsersAsync(UserQueryParameters queryParams)
        {
            // First we need to initialize the query as IQueryable (no db exe yet)
            var query = dataContext.Users.AsNoTracking().AsQueryable();

            // Now we apply text and property filters
            if (!string.IsNullOrWhiteSpace(queryParams.SearchUser))
            {
                query = query
                    .Where(p => 
                    (p.FirstName + " " + p.LastName).ToLower().Contains(queryParams.SearchUser.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(queryParams.SearchUserEmail))
            {
                query = query.Where(p => p.Email == queryParams.SearchUserEmail);
            }

            var users = await query
                .OrderBy(u => u.UserId)
                .Skip((queryParams.PageIndex - 1) * queryParams.PageSize) // (2 - 1) * 10 = We skip the first 10 users (who are on the first page)
                .Take(queryParams.PageSize)                  // Get the next 10 users (from 11 to 20)
                .ToListAsync();

            var count = await query.CountAsync(); // Just count how many total users you have, don't extract data

            return new PaginatedList<User>(users, count, queryParams.PageIndex, queryParams.PageSize);
        }
    }
}
