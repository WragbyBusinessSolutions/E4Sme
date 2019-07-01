using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace E4S.Models
{
  // Add profile data for application users by adding properties to the ApplicationUser class
  public class ApplicationUser : IdentityUser
  {
    public string UserRole { get; set; }

    public string EmployeeName { get; set; }

    public string Status { get; set; }

    public Guid OrganisationId { get; set; }
    public string OrganisationName { get; set; }

    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }

  }
}
