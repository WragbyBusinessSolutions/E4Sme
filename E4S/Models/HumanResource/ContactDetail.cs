using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class ContactDetail : BaseClass
  {
    public Guid Id { get; set; }
    public Guid EmployeeDetailId { get; set; }
    public virtual EmployeeDetail EmployeeDetail { get; set; }

    public string AddressOne { get; set; }
    public string AddressTwo { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }
    public string Mobile { get; set; }
    public string HomeTelephone { get; set; }
    public string WorkTelephone { get; set; }
    public string WorkEmail { get; set; }
    public string OtherEmail { get; set; }

    public bool IsActive { get; set; }

  }
}
