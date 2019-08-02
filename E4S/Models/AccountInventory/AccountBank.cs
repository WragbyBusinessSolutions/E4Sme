using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class AccountBank : BaseClass
  {
    public Guid Id { get; set; }
    public string AccountType { get; set; }
    public string AccountName { get; set; }
    public string AccountNo { get; set; }
    public string BankName { get; set; }
    public string Description { get; set; }


  }
}

