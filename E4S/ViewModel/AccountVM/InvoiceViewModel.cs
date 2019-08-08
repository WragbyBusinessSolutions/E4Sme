using E4S.Models;
using E4S.Models.AccountInventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel.AccountVM
{
  public class InvoiceViewModel
  {
    public Guid Id { get; set; }
    public int QuoteNo { get; set; }

    public Customer Customer { get; set; }
    public Organisation Organisation { get; set; }

    public List<InvoiceItem> QuoteItems { get; set; }

    public string InoviceTitle { get; set; }
    public float Tax { get; set; }
    public float SubTotal { get; set; }
    public float Total { get; set; }

  }
}
