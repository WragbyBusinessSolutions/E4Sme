using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountViewModels
{
  public class RegisterViewModel
  {
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    [Display(Name = "User Role")]
    public string UserRole { get; set; }

    [Display(Name = "Employee Name")]
    public string EmployeeName { get; set; }

    [Display(Name = "Staus")]
    public string Status { get; set; }

    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    [Display (Name = "Organisation")]
    public Guid OrganisationId { get; set; }

    
  }
}
