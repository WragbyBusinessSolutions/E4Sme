using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class StockRecord : BaseClass
  {
    public Guid Id {get;set;}

    public Guid ProductServiceId { get; set; }
    public virtual ProductService ProductService { get; set; }

    public float AllTimeQuantity { get; set; }
    public float QuantitySold { get; set; }
    public float QuantityRemain { get; set; }

    public float CostPrice { get; set; }
  }
}
