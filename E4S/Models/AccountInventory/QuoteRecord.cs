using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class QuoteRecord : BaseClass
  {
    public Guid Id { get; set; }
    public int QuoteNo { get; set; }
    public Guid CustomerId { get; set; }

    public float SubTotal { get; set; }
    public float Tax { get; set; }
    public float Total { get; set; }

    public DateTime QuoteDate { get; set; }

  }
}
