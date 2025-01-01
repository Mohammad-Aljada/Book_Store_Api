using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_Core.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public int Payment_Type { get; set; }
        public string UserName  { get; set; }
        
        public ICollection<InvoiceItem> InvoiceItems { get; set; } = new HashSet<InvoiceItem>();
        public decimal TotalAmount { get; set; }
    }
}
