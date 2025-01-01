using Book_Store_API.Helper;
using Book_Store_Core.Dto_s.CartDTO_S;
using Book_Store_Core.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Book_Store_API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {

        private readonly IShoppingCartRepository _repository;

        public CartsController(IShoppingCartRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("addToCart")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] UserCartItemsDTO cartItemDto)
        {
            try
            {
                // Extract token
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token is missing.");
                }

                // Validate and extract user ID from token
                var userId = ExtractClaims.EtractUserId(token);
                if (!userId.HasValue)
                {
                    return Unauthorized("Invalid user token.");
                }

                // Proceed with adding to cart
                await _repository.AddToCartAsync(userId.Value, cartItemDto.BookId, cartItemDto.Quantity);
                return Ok(new { message = "Item added to cart." });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("removeInCart")]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart([FromQuery] int bookId)
        {
            try
            {
                // Extract token
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token is missing.");
                }

                // Validate and extract user ID from token
                var userId = ExtractClaims.EtractUserId(token);
                if (!userId.HasValue)
                {
                    return Unauthorized("Invalid user token.");
                }

                // Proceed with removing from cart
                await _repository.RemoveFromCartAsync(userId.Value, bookId);
                return Ok(new { message = "Item removed from cart." });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpPost("clearCart")]
        [Authorize]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                // Extract user ID from token
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var userId = ExtractClaims.EtractUserId(token);

                if (!userId.HasValue)
                {
                    return Unauthorized("Invalid user token.");
                }

                // Clear the cart
                await _repository.ClearCartAsync(userId.Value);

                return Ok(new { message = "Cart has been cleared." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("GetitemsCart")]
        [Authorize]
        public async Task<IActionResult> GetCartItems()
        {
            try
            {
                // Extract user ID from token
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var userId = ExtractClaims.EtractUserId(token);

                if (!userId.HasValue)
                {
                    return Unauthorized("Invalid user token.");
                }

                // Get all items from the cart
                var cartItems = await _repository.GetCartItemsAsync(userId.Value);
                
                if (!cartItems.Any())
                {
                    return NotFound(new { message = "Cart is empty." });
                }

                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        // Add Quantity to Cart
        [HttpPost("AddQuantity")]
        [Authorize]
        public async Task<IActionResult> AddQuantityToCart([FromBody] CartQuantityUpdateDTO model)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var userId = ExtractClaims.EtractUserId(token);

                if (!userId.HasValue)
                {
                    return Unauthorized("Invalid user token.");
                }
                // Call repository method to add quantity to cart
                await _repository.AddQuantityToCartAsync(userId.Value, model);

                return Ok(new { message = "Quantity for Item is increased."  });
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                return StatusCode(500, new { Message = "An error occurred while adding quantity.", Error = ex.Message });
            }
        }

        // Remove Quantity from Cart
        [HttpPost("RemoveQuantity")]
        [Authorize]
        public async Task<IActionResult> RemoveQuantityFromCart([FromBody] CartQuantityUpdateDTO model)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var userId = ExtractClaims.EtractUserId(token);

                if (!userId.HasValue)
                {
                    return Unauthorized("Invalid user token.");
                }
                // Call repository method to remove quantity from cart
                await _repository.RemoveQuantityFromCartAsync(userId.Value, model);

                return Ok(new { message = "Quantity for Item is Decrease." });
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                return StatusCode(500, new { Message = "An error occurred while removing quantity.", Error = ex.Message });
            }
        }



    }
}
