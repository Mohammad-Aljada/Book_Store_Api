using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; } = new HashSet<Book>();
        public DateTime CreatedAt { get; set; }
    }
}
