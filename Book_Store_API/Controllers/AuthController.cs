using Book_Store_API.Helper;
using Book_Store_Core.Dto_s.UserDTO_s;
using Book_Store_Core.Interfaces;
using Book_Store_Core.Models;
using Book_Store_Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Book_Store_API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthRepository authRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this.authRepository = authRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(forgotPasswordDto.Email))
                    return BadRequest("Email is required.");

                
                var token = await authRepository.SendResetPasswordUrlAsync(forgotPasswordDto.Email);

                var resetLink = Url.Action("ResetPassword", "Auth", new { token, email = forgotPasswordDto.Email }, Request.Scheme);

                // Use EmailService to send email
                var email = new Email
                {
                    Recivers = forgotPasswordDto.Email,
                    Subject = "Password Reset Request",
                    Body = $"Click the link to reset your password: {resetLink}"
                };

                EmailService.SendEmail(email);

                return Ok($"Password reset link has been sent to your email + token is {token}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(resetPasswordDto.Email) ||
                    string.IsNullOrWhiteSpace(resetPasswordDto.Token) ||
                    string.IsNullOrWhiteSpace(resetPasswordDto.NewPassword))
                    return BadRequest("Email, token, and new password are required.");


                var result = await authRepository.ResetPasswordAsync(resetPasswordDto.Email, resetPasswordDto.Token, resetPasswordDto.NewPassword);
                if (!result)
                    return BadRequest("Password reset failed. Ensure the Password and token is vaild.");

                return Ok("Password has been reset successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return Unauthorized(new { Message = "Invalid Email or Password" });

                }
                var token = await authRepository.LoginAsync(loginDto.Email, loginDto.Password);
                if (token == null)
                {
                    return Unauthorized(new { Message = "Invalid Email or Password" });
                }
                return Ok(new UserDto()
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login {ex.Message}");
                return StatusCode(500, new { Message = "an expected error" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = new ApplicationUser
                {
                    UserName = registerDto.UserName,
                    FullName = registerDto.FullName,
                    Email = registerDto.Email

                };

                // Check if email already exists
                if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
                {
                    return BadRequest("Email is already in use.");
                }

                var result = await authRepository.RegisterAsync(user, registerDto.Password);
                if (result == null)
                {
                    return BadRequest(result);
                }

                return StatusCode(200 , "User Registered Successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        
    }
}
