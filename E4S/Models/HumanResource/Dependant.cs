using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class Dependant : BaseClass
  {
    public Guid Id { get; set; }
    public Guid EmployeeDetailId { get; set; }
    public virtual EmployeeDetail EmployeeDetail { get; set; }

    public string Name { get; set; }
    public string Relationship { get; set; }
    public string Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public bool IsActive { get; set; }

  }
}
