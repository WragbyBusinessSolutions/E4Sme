using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
  public class HRDashboardViewModel
  {
    public int VacancyNo { get; set; }
    public int CandidateNo { get; set; }
    public int PendingLeave { get; set; }
    public int TotalEmployee { get; set; }

    public List<LatestEmployeeVM> LatestEmployeeVMs { get; set; }
    public List<LeaveDashVM> LeaveDashVMs { get; set; }
    public List<RecentApplication> RecentApplications { get; set; }
  }

  public class LatestEmployeeVM
  {
    public Guid Id { get; set; }
    public string EmployeeName { get; set; }
    public string Department { get; set; }
    public string JobTitle { get; set; }
    public string IsActive { get; set; }
  }

  public class LeaveDashVM
  {
    public Guid Id { get; set; }
    public string EmployeeName { get; set; }
    public string LeaveRewuest { get; set; }
    public string Days { get; set; }
    public string IsApprove { get; set; }
  }

  public class RecentApplication
  {
    public Guid Id { get; set; }
    public string Vacancy { get; set; }
    public string CandidateName { get; set; }
    public string Email { get; set; }
    public string HiringManager { get; set; }
  }
}
