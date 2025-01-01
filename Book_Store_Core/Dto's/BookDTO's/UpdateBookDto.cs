using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Dto_s.BookDTO_s
{
    public  class UpdateBookDto
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public IFormFile? image {  get; set; }
        public string? ImageName { get; set; }
        public string? Description { get; set; }

        public int? CategoryId { get; set; }
    }
}
