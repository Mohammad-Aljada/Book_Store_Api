using Book_Store_Core.Dto_s;
using Book_Store_Core.Dto_s.Order;
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
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            foreach (var orderDetail in order.orderDetails)
            {
                if (orderDetail.BookId == 0)
                {
                    throw new InvalidOperationException("BookId cannot be 0.");
                }

                var itemExists = await _context.Books
                    .AnyAsync(i => i.Id == orderDetail.BookId);
                if (!itemExists)
                {
                    throw new InvalidOperationException($"Item with ID {orderDetail.BookId} does not exist.");
                }
            }

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }


        public async Task<Order> GetOrderByIdAsync(int orderId, int userId)
        {
            return await _context.Orders
                .Include(o => o.orderDetails)
                    .ThenInclude(od => od.Book) 
                .Where(o => o.Id == orderId && o.UserId == userId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Order>> GetAllOrderAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.orderDetails)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> DeleteOrderAsync(int orderId, int userId)
        {
            var order = await _context.Orders
                .Where(o => o.Id == orderId && o.UserId == userId)
                .FirstOrDefaultAsync();

            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Order> GetOrderWithItemsAsync(int orderId , int userId)
        {
            return await _context.Orders
                       .Include(o => o.orderDetails)
                       .ThenInclude(od=>od.Book)// Ensure related items are included
                       .FirstOrDefaultAsync(o => o.Id == orderId&& o.UserId==userId);
        }


    }
}
