using Book_Store_API.Helper;
using Book_Store_Core.Dto_s;
using Book_Store_Core.Dto_s.Order;
using Book_Store_Core.Interfaces;
using Book_Store_Core.Models;
using Book_Store_Infrastructure.Data;
using Book_Store_Infrastructure.Repositories;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Book_Store_API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository cartRepository;

        public OrdersController( AppDbContext context,  IOrderRepository orderRepository,   IShoppingCartRepository cartRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
            this.cartRepository = cartRepository;
        }
        [HttpPost("CreateOrder")]
        [Authorize]
        public async Task<IActionResult> AddOrderFromCart([FromBody] OrderDto OrderDto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            var userId = ExtractClaims.EtractUserId(token);

            if (!userId.HasValue)
            {
                return Unauthorized("Invalid user token.");
            }
         

            // Retrieve all cart items
            var cartItems = await cartRepository.GetCartItemsAsync(userId.Value);

            // Ensure that the cart is not empty
            if (!cartItems.Any())
            {
                return BadRequest("The cart is empty.");
            }

            var orderDetails = cartItems.Select(cartItem => new OrderDetails
            {
                BookId = cartItem.BookId,
                Quantity = (int)cartItem.Quantity,
                Price = (decimal)cartItem.Price
                
            }).ToList();

           
            var orderDetailsDto = ConvertOrderDetailsToDto(orderDetails);

            var NeworderDto = new OrderDto
            {
                Address = OrderDto.Address,
                City = OrderDto.City,
                OrderDetails = orderDetailsDto  
            };

            var order = MapOrderDtoToOrder(NeworderDto, userId.Value );
            order.CalculateTotalAmount();  

            var createdOrder = await _orderRepository.AddOrderAsync(order);
            await cartRepository.ClearCartAsync(userId.Value);



            return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrder.Id }, createdOrder);
        }
       

        [HttpGet("GetOrders")]
        [Authorize]
        public async Task<IActionResult> GetOrderById([FromQuery] int orderId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            var userId = ExtractClaims.EtractUserId(token);

            if (!userId.HasValue)
            {
                return Unauthorized("Invalid user token.");
            }

            var order = await _orderRepository.GetOrderByIdAsync(orderId, userId.Value);
            if (order == null) return NotFound("Order Is Empty.");

            order.CalculateTotalAmount(); 

            return Ok(order);
        }

        [HttpGet("GetAllOrder")]
        [Authorize]
        public async Task<IActionResult> GetAllOrder()
        {
            // التحقق من وجود التوكن
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            var userId = ExtractClaims.EtractUserId(token);

            if (!userId.HasValue)
            {
                return Unauthorized("Invalid user token.");
            }

            try
            {
                var orders = await _orderRepository.GetAllOrderAsync(userId.Value);

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                    WriteIndented = true 
                };

                var ordersJson = JsonSerializer.Serialize(orders, options);

                return Ok(ordersJson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }


        [HttpDelete("DeleteOrder")]
        [Authorize]
        public async Task<IActionResult> DeleteOrder([FromQuery] int orderId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            var userId = ExtractClaims.EtractUserId(token);

            if (!userId.HasValue)
            {
                return Unauthorized("Invalid user token.");
            }

            var isDeleted = await _orderRepository.DeleteOrderAsync(orderId, userId.Value);
            if (!isDeleted) return NotFound("Order not found or unauthorized.");

            return NoContent();
        }

        private List<OrderDetails> ConvertCartItemsToOrderDetails(IEnumerable<ShoppingCartItem> cartItems)
        {
            return cartItems.Select(cartItem => new OrderDetails
            {
                BookId = cartItem.BookId,
                Quantity = cartItem.Quantity,

            }).ToList();
        }


        private Order MapOrderDtoToOrder(OrderDto orderDto, int userId)
        {
           


            var order = new Order
            {
                UserId = userId,
                Address = orderDto.Address,
                City = orderDto.City,

                OrderStatus = orderDto.OrderStatus ?? "Pending",
                orderDetails = orderDto.OrderDetails.Select(od => new OrderDetails
                {
                    BookId = od.BookId,
                    Quantity = od.Quantity,
                    Price = od.Price
                }).ToList()
            };
            
            return order;
        }
        private List<OrderDetailsDto> ConvertOrderDetailsToDto(List<OrderDetails> orderDetails)
        {
            return orderDetails.Select(od => new OrderDetailsDto
            {
                BookId = od.BookId,
                Quantity = (int)od.Quantity,
                Price = od.Price
            }).ToList();
        }



    }
}
