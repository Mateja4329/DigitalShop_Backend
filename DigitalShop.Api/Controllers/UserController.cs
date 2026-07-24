using BCrypt.Net;
using DigitalShop.Application.DTOs.User;
using DigitalShop.Application.Helpers;
using DigitalShop.Application.Mappings;
using DigitalShop.Application.Services.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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

            // Then check if token already exists
            var newUser = await _userService.RegisterUserApp(userCreateDTO);
            if (newUser == null)
            {
                return BadRequest(new { message = "User with this email already exists" });
            }

            var apiResponse = new ApiResponse(true, "User successfully registered", newUser);

            return CreatedAtAction(nameof(RegisterUser), new { id = newUser.UserId }, apiResponse);
        }

        // ------------------ LOGIN ------------------
        [HttpPost("LogIn")]
        public async Task<IActionResult> LogInUser(UserLoginDTO userLoginDTO, [FromServices] IValidator<UserLoginDTO> validator)
        {
            // Validate
            var validationResult = await validator.ValidateAsync(userLoginDTO);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            // Now retrieve token by email
            var token = await _userService.LogInUserApp(userLoginDTO);
            if (token == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

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

            var paginatedDto = await _userService.GetAllUsersApp(queryParams);
            
            return Ok(new ApiResponse(true, "Users retrieved successfully", paginatedDto));
        }
    }
}
