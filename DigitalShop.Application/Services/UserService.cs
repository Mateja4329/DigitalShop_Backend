using BCrypt.Net;
using DigitalShop.Application.DTOs.User;
using DigitalShop.Application.Helpers;
using DigitalShop.Application.Mappings;
using DigitalShop.Application.Services.Interface;
using DigitalShop.Infrastructure.Entities.dbFilter;
using DigitalShop.Infrastructure.Repo.Interface;
using BC = BCrypt.Net.BCrypt;

namespace DigitalShop.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly AuthHelpers _helpService;
        public UserService(IUserRepository repository, AuthHelpers helpService)
        {
            _repository = repository;
            _helpService = helpService;
        }

        // POST ==========================================================
        // REGISTER -------------------------------------------------------
        public async Task<UserResponseDTO?> RegisterUserApp(UserCreateDTO userCreateDTO)
        {
            var checkUser = await _repository.GetUserByEmailAsync(userCreateDTO.Email);
            if (checkUser != null) return null;

            var passwordHash = BC.EnhancedHashPassword(userCreateDTO.Password, hashType: HashType.SHA512);
            var newUser = userCreateDTO.ToUserEntity(passwordHash);

            await _repository.AddUserAsync(newUser);
            return newUser.ToUserResponseDto();
        }
        // LOGIN ----------------------------------------------------------
        public async Task<String?> LogInUserApp(UserLoginDTO userLoginDTO)
        {
            var user = await _repository.GetUserByEmailAsync(userLoginDTO.Email);
            if (user == null) return null;

            var isPasswordValid = BC.EnhancedVerify(userLoginDTO.Password, user.Password, hashType: HashType.SHA512);
            if(!isPasswordValid) return null;

            var token = _helpService.GenerateJWTToken(user);
            return token;
        }
        // GET ==========================================================
        // ALL ----------------------------------------------------------
        public async Task<PaginatedList<UserResponseDTO>> GetAllUsersApp(UserQueryParameters query)
        {
            var filterOptions = new UserFilterOptions
            {
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                SearchUser = query.SearchUser,
                SearchUserEmail = query.SearchUserEmail
            };
            var (users, totalCount) = await _repository.GetAllUsersAsync(filterOptions);

            var dtoList = users.ToUserResponseDtolist();

            var paginatedDto = new PaginatedList<UserResponseDTO>(
                dtoList, 
                totalCount, 
                query.PageIndex, 
                query.PageSize
            );

            return paginatedDto;
        }
        // ONE ----------------------------------------------------------
        public async Task<UserResponseDTO?> GetUserApp(Guid userId)
        {
            var getUser = await _repository.GetUserAsync(userId);
            return getUser?.ToUserResponseDto();
        }
        // PUT ==========================================================
        public async Task<UserResponseDTO?> UpdateProfileApp(Guid userId, UserUpdateDTO request)
        {
            var updateProfile = await _repository.UpdateProfileAsync(userId, request.FirstName, request.LastName, request.PhoneNumber);
            return updateProfile?.ToUserResponseDto();
        }
        // DELETE =======================================================
        public async Task<UserResponseDTO?> DeleteUserApp(Guid userId)
        {
            var deleteAccount = await _repository.DeleteUserAsync(userId);
            return deleteAccount?.ToUserResponseDto();
        }
    }
}
