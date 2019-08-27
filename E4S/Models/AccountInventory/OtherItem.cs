using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class OtherItem : BaseClass
  {
    public Guid Id { get; set; }
    public string Item { get; set; }
    public float Amount { get; set; }
    
  }
}
