using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel.AccountVM
{
  public class OtherItemVM
  {
    public Guid Id { get; set; }
    public string Item { get; set; }
    public float Amount { get; set; }
    public Guid InvoiceId { get; set; }

  }
}
