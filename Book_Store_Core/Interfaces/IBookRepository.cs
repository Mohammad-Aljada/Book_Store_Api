using Book_Store_Core.Dto_s.BookDTO_s;
using Book_Store_Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Interfaces
{
    public interface IBookRepository
    {
        Task<Book> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> CreateBookAsync(CreateBookDTO bookdto);
        Task<Book> UpdateBookAsync(int id , UpdateBookDto updateBookDto); 
        Task<bool> DeleteBookAsync(int id);

    }
}
