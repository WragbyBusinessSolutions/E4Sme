using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
  public class PostNewLeave
  {
    public Guid Id { get; set; }
    public string EmployeeName { get; set; }
    public string LeaveTitle { get; set; }
    public string Description { get; set; }
    public DateTime LeaveStartDate { get; set; }
    public DateTime LeaveEndDate { get; set; }
    public DateTime LeaveApprovedDate { get; set; }
    public string Status { get; set; }


  }
}
