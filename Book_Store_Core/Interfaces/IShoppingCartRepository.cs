
using Book_Store_Core.Dto_s.CartDTO_S;
using Book_Store_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task AddToCartAsync(int userId, int bookId, int quantity);
        Task RemoveFromCartAsync(int userId, int bookId);
        Task<IEnumerable<UserCartItemsDTO>> GetCartItemsAsync(int userId);
        Task ClearCartAsync(int userId);
        Task AddQuantityToCartAsync(int userId, CartQuantityUpdateDTO cartQuantity);
        Task RemoveQuantityFromCartAsync(int userId,  CartQuantityUpdateDTO cartQuantity);


    }
}
