
using Book_Store_Core.Dto_s.CartDTO_S;
using Book_Store_Core.Interfaces;
using Book_Store_Core.Models;
using Book_Store_Infrastructure.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Infrastructure.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly AppDbContext _context;

        public ShoppingCartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddToCartAsync(int userId, int bookId, int quantity)
        {
            var cartItem = await _context.ShoppingCartItems
                .FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new ShoppingCartItem { UserId = userId, BookId = bookId, Quantity = quantity };
                _context.ShoppingCartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(int userId, int bookId)
        {
            var cartItem = await _context.ShoppingCartItems
                .FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);

            if (cartItem != null)
            {
                _context.ShoppingCartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<UserCartItemsDTO>> GetCartItemsAsync(int userId)
        {
            var cartItems = await _context.ShoppingCartItems
       .Where(x => x.UserId == userId)
       .Include(x => x.Book) // Include related Book entity
       .Select(x => new UserCartItemsDTO
       {
           BookId = x.Book.Id, // BookId from Book
           BookTitle = x.Book.Title, // Title from Book
           Price = x.Book.Price, // Price from Book
           Quantity = x.Quantity // Quantity from ShoppingCartItem
       })
       .ToListAsync(); // Execute the query and return the list
   return cartItems;
        }

        public async Task ClearCartAsync(int userId)
        {
            var cartItems = _context.ShoppingCartItems.Where(x => x.UserId == userId);
            _context.ShoppingCartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
        public async Task AddQuantityToCartAsync(int userId, CartQuantityUpdateDTO cartQuantity)
        {
            var cartItem = await _context.ShoppingCartItems
                .FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == cartQuantity.BookId);

            if (cartItem != null)
            {
                cartItem.Quantity += cartQuantity.Quantity; // Add the specified quantity to the current quantity
                _context.ShoppingCartItems.Update(cartItem);
                await _context.SaveChangesAsync(); // Save changes to the database
            }
            else
            {
                await _context.SaveChangesAsync();
            }
            
            
        }
        public async Task RemoveQuantityFromCartAsync(int userId, CartQuantityUpdateDTO cartQuantity)
        {
            var cartItem = await _context.ShoppingCartItems
       .FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == cartQuantity.BookId);

            if (cartItem == null)
            {
                throw new Exception("Cart item not found.");
            }

            // Decrement quantity or remove item
            cartItem.Quantity -= cartQuantity.Quantity;

            if (cartItem.Quantity <= 0)
            {
                _context.ShoppingCartItems.Remove(cartItem);
            }
            else
            {
                _context.ShoppingCartItems.Update(cartItem);
            }

            await _context.SaveChangesAsync();


        }

    }
}
