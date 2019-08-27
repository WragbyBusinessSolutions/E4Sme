using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class InvoiceOtherItem : BaseClass
  {
    public Guid Id { get; set; }

    public Guid OtherItemId { get; set; }
    public virtual OtherItem OtherItem { get; set; }

    public Guid InvoiceRecordId { get; set; }
    public virtual InvoiceRecord InvoiceRecord { get; set; }


    public float Amount { get; set; }
  }
}
