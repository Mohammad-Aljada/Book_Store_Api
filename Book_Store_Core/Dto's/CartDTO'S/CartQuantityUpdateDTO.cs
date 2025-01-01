using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Dto_s.CartDTO_S
{
    public class CartQuantityUpdateDTO
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}
