using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class Transaction : BaseClass
  {
    public Guid Id { get; set; }

    //Transaction type: Invoice, Inventory, Other Expenses, Other Income. 
    //The reference also comes from the Id.
    public string TransactionType { get; set; }
    public Guid TransactionId { get; set; }

    public string DebitCredit { get; set; }
    public Guid BankId { get; set; }

    public string TransactionDetails { get; set; }
    public string Type { get; set; }

    public float Amount { get; set; }

  }
}
