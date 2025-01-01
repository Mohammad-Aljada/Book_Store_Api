using Book_Store_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Dto_s.Order
{
   public class OrderDto
    {

        public string? OrderStatus { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public List<OrderDetailsDto> OrderDetails  { get; set; } = new List<OrderDetailsDto>();

     
    }
}
