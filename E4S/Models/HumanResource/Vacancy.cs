using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class Vacancy : BaseClass
  {
    public Guid Id { get; set; }
    public Guid JobTitleId { get; set; }
    public virtual JobTitle JobTitle { get; set; }
    public string VacancyName { get; set; }
    public string HiringManager { get; set; }
    public int NumberOfPosition { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string ApplicationLink { get; set; }
    public DateTime VacancyCloseDate { get; set; }
    public bool IsActive { get; set; }

  }
}
