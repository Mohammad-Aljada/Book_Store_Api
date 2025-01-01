using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
            public string FullName { get; set; }
            public string? Address { get; set; }
            public string? City { get; set; }
            public string? PhoneNumber { get; set; }
            public string? ImageName { get; set; }

        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();

    }
}
