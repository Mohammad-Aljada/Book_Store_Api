using Book_Store_Core.Dto_s.InvoiceDTO_s;
using Book_Store_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<InvoiceDTO> CreateInvoiceAsync(Invoice invoice);
        Task<Invoice> GetInvoiceByIdAsync(int invoiceId , int userId);
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync(int userId);
    
    }
}
