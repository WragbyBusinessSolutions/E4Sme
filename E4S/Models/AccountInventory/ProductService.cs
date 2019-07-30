using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class ProductService : BaseClass
  {
    public Guid Id { get; set; }
    public string ProductServiceName { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }

    public Guid VendorId { get; set; }

  }
}
