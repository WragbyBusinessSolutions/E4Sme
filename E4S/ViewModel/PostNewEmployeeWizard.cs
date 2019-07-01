using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
  public class PostNewEmployeeWizard
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNo { get; set; }
    public string Gender { get; set; }
    public string MaritalStatus { get; set; }
    public DateTime DateOfBirth { get; set; }

    
    public Guid JobTitle { get; set; }
    public Guid Department { get; set; }
    public Guid JobCategory { get; set; }
    public Guid EmploymentStatus { get; set; }

    public Guid Branch { get; set; }
    public DateTime JoinedDate { get; set; }
    public DateTime StartDate { get; set; }

    public float Amount { get; set; }
    public string PayFrequency { get; set; }

  }
}
