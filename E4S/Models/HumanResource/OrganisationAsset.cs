using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class OrganisationAsset : BaseClass
  {
    public Guid Id { get; set; }
    public Guid EmployeeDetailId { get; set; }
    public string DeviceName { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string SerialNumber { get; set; }
    public string EmployeeName { get; set; }
    public string Status { get; set; }
    public bool IsAssigned { get; set; }

  }
}
