using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class InvoiceRecord : BaseClass
  {
    public Guid Id { get; set; }

    public int InvoiceNo { get; set; }
    public int QuoteNo { get; set; }

    public Guid CustomerId { get; set; }

    public float SubTotal { get; set; }
    public float Tax { get; set; }
    public float Total { get; set; }

    public string InvoiceTitle { get; set; }
    public DateTime DueDate { get; set; }

    public string PaymentStatus { get; set; }
  }
}
