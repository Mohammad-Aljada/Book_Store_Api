using Book_Store_Core.Dto_s.BookDTO_s;
using Book_Store_Core.Dto_s.CategoryDTO_s;
using Book_Store_Core.Helper;
using Book_Store_Core.Interfaces;
using Book_Store_Core.Models;
using Book_Store_Infrastructure.Data;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all books
        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            try
            {
                var books = await _context.Books
            .Include(b => b.Category)
    .ToListAsync();

                return books;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logger here)
                throw new Exception("An error occurred while fetching the books.", ex);
            }
        }

        // Get book by Id
        public async Task<Book> GetBookByIdAsync(int id)
        {
            try
            {
                // Get the book along with the category
                var book = await _context.Books
                    .Include(b => b.Category)  // Include category data
                    .FirstOrDefaultAsync(b => b.Id == id);
                return book; // Fetch book by Id
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logger here)
                throw new Exception($"An error occurred while fetching the book with ID {id}.", ex);
            }
        }

        // Create a new book
        public async Task<Book> CreateBookAsync(CreateBookDTO bookdto)
        {
            try
            {
                // Save the image and get the image file name
                if (bookdto.Image != null)
                {
                    bookdto.ImageName = FileSettings.SaveImage(bookdto.Image); // Save image and assign image name
                }

                var book = bookdto.Adapt<Book>();

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return book;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the book.", ex);
            }
        }

        // Update an existing book
        public async Task<Book> UpdateBookAsync(int id , UpdateBookDto bookDto)
        {
            try
            {
                // Find the existing book
                var existingBook = await _context.Books.FindAsync(id);
                if (existingBook == null)
                    return null; // Return null if book doesn't exist

                // Update only provided fields
                if (!string.IsNullOrWhiteSpace(bookDto.Title))
                    existingBook.Title = bookDto.Title;

                if (!string.IsNullOrWhiteSpace(bookDto.Author))
                    existingBook.Author = bookDto.Author;

                if (!string.IsNullOrWhiteSpace(bookDto.Description))
                    existingBook.Description = bookDto.Description;

                if (!string.IsNullOrWhiteSpace(bookDto.ISBN))
                    existingBook.ISBN = bookDto.ISBN;

                existingBook.Price = bookDto.Price > 0 ? bookDto.Price : existingBook.Price;
                existingBook.Stock = bookDto.Stock > 0 ? bookDto.Stock : existingBook.Stock;

                // Save changes

                await _context.SaveChangesAsync();
                // Map DTO to the existing book

                return existingBook ;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the book with ID {id}.", ex);
            }
        }

        // Delete a book
        public async Task<bool> DeleteBookAsync(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null) return false;

                // Delete the image associated with the book
                FileSettings.DeleteImage(book.ImageName);

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the book with ID {id}.", ex);
            }
        }


    }
}
