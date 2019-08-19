using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class QuoteItem : BaseClass
  {
    public Guid Id { get; set; }
    public Guid ProductServiceId { get; set; }
    public virtual ProductService ProductService { get; set; }

    public Guid QuoteRecordId { get; set; }
    public virtual QuoteRecord QuoteRecord { get; set; }

    public float Quantity { get; set; }
    public float UnitCost { get; set; }
    public float TotalCost { get; set; }
  }
}
