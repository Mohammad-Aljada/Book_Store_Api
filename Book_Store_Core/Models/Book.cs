using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; } 
        public int Price { get; set; }
        public int Stock { get; set; }
        
        public string ImageName { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<OrderDetails> Details { get; set; } = new HashSet<OrderDetails>();
    }
}
