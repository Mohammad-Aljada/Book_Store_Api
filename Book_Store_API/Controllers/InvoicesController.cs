using Book_Store_API.Helper;
using Book_Store_Core.Dto_s.InvoiceDTO_s;
using Book_Store_Core.Interfaces;
using Book_Store_Core.Models;
using Book_Store_Infrastructure.Data;
using Book_Store_Infrastructure.Repositories;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Book_Store_API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public InvoicesController(IInvoiceRepository invoiceRepository , IOrderRepository orderRepository , UserManager<ApplicationUser> userManager) {
            _invoiceRepository = invoiceRepository;
             _orderRepository = orderRepository;
            _userManager = userManager;
        }
        [HttpGet("GetAllInvoices")]
        [Authorize]
        public async Task<IActionResult> GetAllInvoices()
        {
            try
            {
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
                // استدعاء الطريقة من الـ Repository
                var invoices = await _invoiceRepository.GetAllInvoicesAsync(userId.Value);
                if (invoices == null || !invoices.Any()) {
                    return NotFound(new {message = "Invoice For This User Is Empty"});
                }

                // تحويل البيانات من Entity إلى DTO باستخدام Mapster
                var orders = await _orderRepository.GetAllOrderAsync(userId.Value); // استرجاع كل الطلبات للمستخدم

                // تحويل الفواتير إلى InvoiceDTO مع إضافة address و city من order
                var invoiceDTOs = invoices.Select(invoice =>
                {
                    var invoiceDto = invoice.Adapt<InvoiceDTO>();

                    // البحث عن الطلب المرتبط بالفاتورة
                    var order = orders.FirstOrDefault(o => o.Id == invoice.OrderId);

                    if (order != null)
                    {
                        // إضافة عنوان ومدينة من order
                        invoiceDto.Address = order.Address;  // افترض أن Address موجود في Order
                        invoiceDto.City = order.City;        // افترض أن City موجود في Order
                    }

                    return invoiceDto;
                }).ToList();



                return Ok(new { sucess = true, message = "Get Invoice success", data = invoiceDTOs });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("CreateInvoice")]
        [Authorize]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceDTO invoiceDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

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

                // Check if the order exists and fetch its details
                var order = await _orderRepository.GetOrderWithItemsAsync(invoiceDTO.OrderId, userId.Value);
                if (order == null)
                {
                    return NotFound($"Order with ID {invoiceDTO.OrderId} does not exist.");
                }

                // Check if orderDetails is null or empty
                if (order.orderDetails == null)
                {
                    return BadRequest("The order details are null.");
                }

                if (!order.orderDetails.Any())
                {
                    return BadRequest("The order has no items associated with it.");
                }

                var totalamount = order.TotalAmount;
                var Address = order.Address;
                var City = order.City;

                // Ensure that InvoiceDTO is properly mapped to Invoice
                var invoice = invoiceDTO.Adapt<Invoice>();  // Mapping InvoiceDTO to Invoice

                // Add additional properties if needed
                invoice.InvoiceDate = DateTime.Now;  
                invoice.TotalAmount = totalamount;

                // Build InvoiceItems from OrderItems
                var invoiceItems = order.orderDetails.Select(item => new InvoiceItem
                {
                    BookId = item.BookId,
                    BookTitle = item.Book.Title,
                    Price = item.Price,
                    Quantity = (int)item.Quantity
                }).ToList();
              

                var user = await _userManager.FindByIdAsync(userId.Value.ToString());
                if (user != null)
                {
                    invoice.UserName = user.FullName; // إضافة اسم المستخدم إلى الفاتورة
                }
                // Add the InvoiceItems to the Invoice
                invoice.InvoiceItems = invoiceItems;

                // Add the invoice to the database
                var inv = await _invoiceRepository.CreateInvoiceAsync(invoice);

                // Map the saved Invoice entity to InvoiceDTO for the response
                var invoiceResponse = inv.Adapt<InvoiceDTO>(); 
                invoiceResponse.Address = Address;
                invoiceResponse.City = City;
                

                return Ok(new { success = true, message = "Create Invoice success", data = invoiceResponse });
            }
            catch (Exception ex)
            {
                // Debugging: log the exception message
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetInvoiceById")]
        [Authorize]
        public async Task<IActionResult> GetInvoiceById([FromQuery]int invoiceId)
        {
            try
            {
                // استخراج التوكن من الهيدر
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Token is missing.");
                }

                // التحقق واستخراج معرف المستخدم من التوكن
                var userId = ExtractClaims.EtractUserId(token);
                if (!userId.HasValue)
                {
                    return Unauthorized("Invalid user token.");
                }

                // استدعاء الطريقة من المستودع
                var invoice = await _invoiceRepository.GetInvoiceByIdAsync(invoiceId, userId.Value);

                if (invoice == null)
                {
                    return NotFound($"Invoice with ID {invoiceId} not found for the user.");
                }
                var orders = await _orderRepository.GetAllOrderAsync(userId.Value);
                var order = orders.FirstOrDefault(o => o.Id == invoice.OrderId);

                // تحويل الكيان إلى DTO باستخدام Mapster
                var invoiceDTO = invoice.Adapt<InvoiceDTO>();

                if (order != null)
                {
                    // إضافة عنوان ومدينة من order
                    invoiceDTO.Address = order.Address;  // افترض أن Address موجود في Order
                    invoiceDTO.City = order.City;        // افترض أن City موجود في Order
                }

                return Ok(new {sucess=true ,message = "Get Invoice success" , data=invoiceDTO});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
