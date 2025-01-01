using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Dto_s.UserDTO_s
{
    public class LoginDTO
    {
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
