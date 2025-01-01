using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Address { get; set; }
        public string City { get; set; }
        public string OrderStatus { get; set; } = "Pending";
        public decimal TotalAmount { get; set; }
        public ICollection<OrderDetails> orderDetails { get; set; } = new HashSet<OrderDetails>();

        public void CalculateTotalAmount()
        {
            TotalAmount = orderDetails.Sum(order=>order.PriceTotal);
        }


    }
}
