using E4S.Models.HumanResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
  public class EmployeeDetailsViewModel
  {
    public Guid Id { get; set; }
    public Guid JobTitleId { get; set; }
    public Guid EmploymentStatusId { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid JobCategoryId { get; set; }
    public Guid BranchId { get; set; }
    public DateTime JoinDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string ContractDetails { get; set; }

    //====== Salary ===========//
    public Guid PayGradeId { get; set; }
    public string PayFrequency { get; set; }
    public string Currency { get; set; }
    public float Amount { get; set; }
    public string Comments { get; set; }


    public EmployeeDetail EmployeeDetail { get; set; }
    public ContactDetail ContactDetail { get; set; }
    public List<EmergencyContact> EmergencyContacts { get; set; }
    public List<Dependant> Dependants { get; set; }


  }
}
