﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models
{
  public class Branch : BaseClass
  {
    public Guid Id { get; set; }
    public string BranchName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string PhoneNo { get; set; }
    public string Email { get; set; }
    public string Comment { get; set; }
    public bool IsActive { get; set; }
  }
}
