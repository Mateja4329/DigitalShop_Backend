using DigitalShop.Application.DTOs.User;
using DigitalShop.Application.Helpers;

namespace DigitalShop.Application.Services.Interface
{
    public interface IUserService
    {
        Task<UserResponseDTO?> RegisterUserApp(UserCreateDTO userCreateDTO);
        Task<String?> LogInUserApp(UserLoginDTO userLoginDTO);
        Task<PaginatedList<UserResponseDTO>> GetAllUsersApp(UserQueryParameters query);
        Task<UserResponseDTO?> GetUserApp(Guid userId);
        Task<UserResponseDTO?> UpdateProfileApp(Guid userId, UserUpdateDTO request);
        Task<UserResponseDTO?> DeleteUserApp(Guid userId);
    }
}
