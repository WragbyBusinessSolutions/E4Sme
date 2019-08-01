using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class Transaction : BaseClass
  {
    public Guid Id { get; set; }

    public string Description { get; set; }
    public string Type { get; set; }

    public float Amount { get; set; }
   // public 
  }
}
