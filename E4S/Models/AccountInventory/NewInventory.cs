using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class NewInventory : BaseClass
  {
    public Guid Id { get; set; }

    public Guid ProductServiceId { get; set; }
    public virtual ProductService ProductService { get; set; }

    public float UnitPrice { get; set; }
    public float TotalPrice { get; set; }
    public float Quantity { get; set; }
    public string Status { get; set; }


    public Guid VendorId { get; set; }

  }
}
