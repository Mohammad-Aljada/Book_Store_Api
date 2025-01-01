using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Dto_s.Order
{
    public class OrderDetailsDto
    {
        public int BookId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
