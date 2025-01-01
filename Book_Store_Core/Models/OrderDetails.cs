using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        public decimal Price { get; set; }

        public decimal Quantity { get; set; }
        public decimal PriceTotal => Quantity * Price;
        public Order Order { get; set; }

        public Book Book { get; set; }

    }
}
