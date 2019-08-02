using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class Cashflow : BaseClass
  {
    public Guid Id { get; set; }

    //FlowType type: Paid Invoice, Paid Inventory, Other Expenses, Other Income. 
    //The reference also comes from the Id.
    public string FlowType { get; set; }
    public Guid FlowTypeId { get; set; }

    public string DebitCredit { get; set; }
    public Guid BankId { get; set; }


    public string FlowDetails { get; set; }
    public float Amount { get; set; }
    public float Tax { get; set; }


  }
}
