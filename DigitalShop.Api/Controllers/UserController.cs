using BCrypt.Net;
using DigitalShop.Application.DTOs.User;
using DigitalShop.Application.Helpers;
using DigitalShop.Application.Mappings;
using DigitalShop.Infrastructure.Repo.Interface;
using DigitalShop.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using BC = BCrypt.Net.BCrypt;

// ------------ Difference between Encryption and Hashing ------------
// Encryption: It is a two-way process. Imagine putting a message in a safe and locking it with a key.
// If a hacker steals the safe and finds your key (which must be somewhere on the server), he can unlock the safe
// and read the original message. Encryption is used for messages, credit card numbers and files.
//
// Hashing: It's a one-way process — like a meat grinder. You insert "mypassword123" (meat)
// and you get an unrecognizable string of characters (minced meat). There is no mathematical way
// to recreate a steak from ground meat. A hacker with a stolen database cannot obtain the original passwords.
//
// Here we will use BCrypt instead of SHA-512 because they are newer and they add "salt".
//
// Because this is a small project, I won't use the Three-Tier Architecture, then the business logic will be here.
// ---------------------------------------------------------------------

namespace DigitalShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthHelpers _helpService;

        public UserController(IUserRepository userRepository, AuthHelpers helpService)
        {
            _userRepository = userRepository;
            _helpService = helpService;
        }

        // ==================== POST ====================
        // ------------------ REGISTER ------------------
        [HttpPost("Register")]
        public async Task<ActionResult<UserResponseDTO>> RegisterUser(UserCreateDTO userCreateDTO, [FromServices] IValidator<UserCreateDTO> validator)
        {
            // First we validate
            var validationResult = await validator.ValidateAsync(userCreateDTO);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            // Then check if user already exists
            var existingUser = await _userRepository.GetUserByEmailAsync(userCreateDTO.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "User with this email already exists" });
            }

            // At the end we hash the password with BCrypt
            var passwordHash = BC.EnhancedHashPassword(userCreateDTO.Password, hashType: HashType.SHA512);
            var newUser = userCreateDTO.ToUserEntity(passwordHash);

            await _userRepository.AddUserAsync(newUser);

            var responseDto = newUser.ToUserResponseDto();
            var apiResponse = new ApiResponse(true, "User successfully registered", responseDto);

            return CreatedAtAction(nameof(RegisterUser), new { id = newUser.UserId }, apiResponse);
        }

        // ------------------ LOGIN ------------------
        [HttpPost("LogIn")]
        public async Task<ActionResult<UserResponseDTO>> LogInUser(UserLoginDTO userLoginDTO, [FromServices] IValidator<UserLoginDTO> validator)
        {
            // Validate
            var validationResult = await validator.ValidateAsync(userLoginDTO);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            // Now retrieve user by email
            var user = await _userRepository.GetUserByEmailAsync(userLoginDTO.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // Then verify password
            var isPasswordValid = BC.EnhancedVerify(userLoginDTO.Password, user.Password, hashType: HashType.SHA512);
            if (!isPasswordValid)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var token = _helpService.GenerateJWTToken(user);

            return Ok(new { token = token });
        }

        // ==================== GET ====================
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetUsers([FromQuery] UserQueryParameters queryParams, [FromServices] IValidator<UserQueryParameters> validator)
        {
            var validationResult = await validator.ValidateAsync(queryParams);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var paginatedUsers = await _userRepository.GetAllUsersAsync(queryParams);
            var dtoList = paginatedUsers.Items.ToUserResponseDtolist();
            var paginatedDto = new PaginatedList<UserResponseDTO>(dtoList, paginatedUsers.TotalCount, paginatedUsers.PageIndex, queryParams.PageSize);
            return Ok(new ApiResponse(true, "Users retrieved successfully", paginatedDto));
        }
    }
}
