using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class OperatingExpense : BaseClass
  {
    public Guid Id { get; set; }
    public Guid ExpenseId { get; set; }
    public virtual Expense Expense { get; set; }

    public string Description { get; set; }
    public float Amount { get; set; }


    public bool isPaid { get; set; }
    public Guid BankId { get; set; }
    public Guid ProductService { get; set; }
  }
}
