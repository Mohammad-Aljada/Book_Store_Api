using Book_Store_Core.Dto_s.BookDTO_s;
using Book_Store_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Dto_s.CategoryDTO_s
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<BookDTO> Books { get; set; } = new List<BookDTO>();

    }
}
