using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class InvoiceItem : BaseClass
  {
    public Guid Id { get; set; }
    public Guid ProductServiceId {get;set;}
    public virtual ProductService ProductService { get; set; }

    public Guid InvoiceRecordId { get; set; }
    public virtual InvoiceRecord InvoiceRecord { get; set; }

    public float Quantity { get; set; }
    public float UnitCost { get; set; }
    public float TotalCost { get; set; }

  }
}
