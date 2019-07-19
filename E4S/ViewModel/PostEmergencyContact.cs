using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel
{
  public class PostEmergencyContact
  {
    public string AId { get; set; }
    [Required]
    public string Name { get; set;}
    [Required]
    public string Relationship { get; set; }
    [Required]
    public string HomeTelephone { get; set; }
    [Required]
    public string Address { get; set; }
  }
}
