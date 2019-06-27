using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class InstitutionQualification : BaseClass
  {
    public Guid Id { get; set; }
    public Guid EmployeeDetailId { get; set; }
    public virtual EmployeeDetail EmployeeDetail { get; set; }


    public string Degree { get; set; }
    public string Grade { get; set; }
    public string Institution { get; set; }
    public string CourseOfStudy { get; set; }
    public string YearCompleted { get; set; }
    public string ImageURL { get; set; }

  }
}
