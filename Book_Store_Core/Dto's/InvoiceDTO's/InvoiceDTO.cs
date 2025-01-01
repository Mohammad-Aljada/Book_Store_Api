using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Dto_s.InvoiceDTO_s
{
    public class InvoiceDTO
    {
        public int OrderId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int Payment_Type { get; set; }

        public string? UserName { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }

        public List<InvoiceItemDTO> InvoiceItems { get; set; } = new List<InvoiceItemDTO>();
    }
}
