using Book_Store_Core.Dto_s.UserDTO_s;
using Book_Store_Core.Interfaces;
using Book_Store_Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Infrastructure.Repositories
{
    public class AuthRepositry : IAuthRepository
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        public AuthRepositry(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager  , IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }
        public async Task<string> RegisterAsync(ApplicationUser user, string password)
        {
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return "User Registered Successfully";
            }
            var errorMessages = result.Errors.Select(error => error.Description).ToList();
            return string.Join(", ", errorMessages);
        }

       



        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                return "Invalid Username or Password";
            }

            var result = await signInManager.PasswordSignInAsync(user, password, false, false);
            if (!result.Succeeded)
            {
                return null;
            }
            return GenerateToken(user);
        }
        // Generate password reset token
        public async Task<string> SendResetPasswordUrlAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return null;

            return await userManager.GeneratePasswordResetTokenAsync(user);
        }

        // Reset user password
        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return false;
            var tokenValidity = await userManager.VerifyUserTokenAsync(user, "Default", "ResetPassword", token);
            if (!tokenValidity)
            {
                return false;  // Invalid or expired token
            }

            var result = await userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded) {
                return false;
            }
            return true;
        }


        private string GenerateToken(ApplicationUser users)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub , users.FullName),
                new Claim(ClaimTypes.NameIdentifier , users.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["JWT:Issuer"],
                configuration["JWT:Audience"],
                claims,
                signingCredentials: cred,
                expires: DateTime.Now.AddMinutes(3000)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
      

    }
}
