using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Dto_s.CartDTO_S
{
    public class UserCartItemsDTO
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }
}
