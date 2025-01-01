using Book_Store_Core.Dto_s.Order;
using Book_Store_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Infrastructure.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> AddOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int orderId, int userId);
        Task<IEnumerable<Order>> GetAllOrderAsync(int userId);
        Task<bool> DeleteOrderAsync(int orderId, int userId);

        Task<Order> GetOrderWithItemsAsync(int orderId , int userId);

    }
}
