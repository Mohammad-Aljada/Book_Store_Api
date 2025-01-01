using Book_Store_Core.Dto_s.BookDTO_s;
using Book_Store_Core.Interfaces;
using Book_Store_Core.Models;
using Book_Store_Infrastructure.Data;
using Book_Store_Infrastructure.Helper;
using Book_Store_Infrastructure.Repositories;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book_Store_API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IBookRepository _bookRepository;

        public BooksController( AppDbContext context,   IBookRepository bookRepository)
        {
            this.context = context;
            _bookRepository = bookRepository;
        }

        // Create a new book
        [HttpPost("CreateBook")]
        
        public async Task<IActionResult> CreateBook([FromForm] CreateBookDTO createBookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                
                var createdBook = await _bookRepository.CreateBookAsync(createBookDto);
                var bookDto = createdBook.Adapt<BookDTO>(); // Map Book entity to BookDto
                 // Manually map CategoryName to the DTO

                return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, bookDto); // Return 201 response
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Update an existing book
        [HttpPut("UpdateBook")]
        public async Task<IActionResult> UpdateBook( [FromQuery] int id, [FromForm] UpdateBookDto updateBookDto)
        {
            try
            {
             
                // Find the existing book
                var existingBook = await context.Books.FindAsync(id);
                if (existingBook == null)
                    return NotFound(new { Message = "Book not found." });

                if (updateBookDto.image != null)
                {
                    FileSettings.DeleteImage(existingBook.ImageName); // Delete the old image
                    existingBook.ImageName = FileSettings.SaveImage(updateBookDto.image); // Save the new image
                }
                else
                {
                    ModelState.Remove("Image");
                }
                // Validate ModelState
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

               var Book = _bookRepository.UpdateBookAsync(id, updateBookDto);
                var updatedBookDto = existingBook.Adapt<BookDTO>();

                // Return the updated book as a DTO
                return Ok(updatedBookDto);
            }
            catch (Exception ex) {
              return StatusCode(500, $"Internal server error: {ex.Message}");

            }
        }

        // Get all books
        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _bookRepository.GetAllBooksAsync();
                var booksDto = books.Adapt<IEnumerable<BookDTO>>(); // Map Book to BookDto
                return Ok(booksDto); // Return JSON response
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Get book by Id
        [HttpGet("DetailsBook")]
        public async Task<IActionResult> GetBookById([FromQuery] int id)
        {
            try
            {
                var book = await _bookRepository.GetBookByIdAsync(id);
                if (book == null)
                {
                    return NotFound(); // Return 404 if book not found
                }
                var bookDto = book.Adapt<BookDTO>(); // Map Book to BookDto
                return Ok(bookDto); // Return JSON response
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Delete a book
        [HttpDelete("DeleteBook")]
        public async Task<IActionResult> DeleteBook( [FromQuery] int id)
        {
            try
            {
                var deleted = await _bookRepository.DeleteBookAsync(id);
                if (!deleted)
                {
                    return NotFound(); // Return 404 if book not found
                }
                return NoContent(); // Return 204 No Content if deletion is successful
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
