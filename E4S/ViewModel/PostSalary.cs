using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
  public class PostSalary
  {
    public Guid EmployeeId { get; set; }
    public Guid PayGrade { get; set; }
    public string PayFrequency { get; set; }
    public float Amount { get; set; }
    public string Currency { get; set; }

    public string BankName { get; set; }
    public string AccountName { get; set; }
    public string AccountNo { get; set; }


    public string Comments { get; set; }
  }
}
