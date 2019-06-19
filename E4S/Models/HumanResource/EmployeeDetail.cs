using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.HumanResource
{
  public class EmployeeDetail : BaseClass
  {
    public Guid Id { get; set; }
    public string EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string ImageUrl { get; set; }
    public string OtherId { get; set; }
    public string Gender { get; set; }
    public string MaritalStatus { get; set; }
    public string Nationality { get; set; }
    public DateTime DateOfBirth { get; set; }
    public bool IsActive { get; set; }
    public string EmployeeStatus { get; set; }
    public Guid UserId { get; set; }
  }
}
