using Book_Store_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<string> RegisterAsync(ApplicationUser user, string password);
        Task<string> LoginAsync(string email, string password);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
        Task<string> SendResetPasswordUrlAsync(string email);
   
    }
}
