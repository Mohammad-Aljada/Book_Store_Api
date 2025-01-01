using Book_Store_Core.Dto_s.InvoiceDTO_s;
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
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _context;

        public InvoiceRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<InvoiceDTO> CreateInvoiceAsync(Invoice invoice)
        {
            // Ensure the invoice is populated correctly before saving
            if (invoice == null)
            {
                throw new ArgumentNullException(nameof(invoice), "Invoice cannot be null");
            }

            // Add the invoice to the context and save changes
            var inv = _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            // Check if the invoice is correctly saved
            Console.WriteLine($"Invoice created with OrderId: {invoice.OrderId}, TotalAmount: {invoice.TotalAmount}");

            // After saving, adapt the entity to DTO and return it
            var invDto = inv.Entity.Adapt<InvoiceDTO>(); // Use inv.Entity to get the actual entity
            return invDto;
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync(int userId)
        {
            var invoices = await _context.Invoices
           .Include(i => i.InvoiceItems)
            .Include(i => i.Order)
            .Where(i=>i.Order.UserId==userId)// تضمين العناصر المرتبطة بكل فاتورة
           .ToListAsync();
            if (invoices == null)
            {
                return null;
            }
            return invoices;

        }

        public async Task<Invoice> GetInvoiceByIdAsync(int invoiceId, int userId)
        {
           var invoice =  await _context.Invoices
           .Include(i => i.InvoiceItems) // تضمين عناصر الفاتورة
           .Include(i => i.Order) // تضمين الطلب المرتبط
           .FirstOrDefaultAsync(i => i.Id == invoiceId && i.Order.UserId == userId); // التحقق من الفاتورة والمستخدم
        
            if (invoice == null)
            {
                return null;
            }
            return invoice;
        }
    }
}
