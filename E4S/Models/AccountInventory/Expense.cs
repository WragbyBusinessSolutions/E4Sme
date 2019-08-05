using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class Expense : BaseClass
  {
    public Guid Id { get; set; }
    public string ExpenseType { get; set; }
    public string Description { get; set; }

  }
}
