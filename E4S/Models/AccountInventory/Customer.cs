using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class Customer : BaseClass
  {
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public string Address { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string State { get; set; }
    public string Country { get; set; }

    public string AccountNumber { get; set; }
    public string BankName { get; set; }
    public string AccountName { get; set; }
    public string SortCode { get; set; }

    public string Website { get; set; }
    public string Mobile { get; set; }
    public string Note { get; set; }
    public string Status { get; set; }


  }
}
