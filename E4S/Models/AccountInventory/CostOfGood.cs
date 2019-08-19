using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class CostOfGood : BaseClass
  {
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public float TotalCost { get; set; }
  }
}
