﻿using E4S.Models.AccountInventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.ViewModel.AccountVM
{
  public class InvoiceListViewModel
  {
    public InvoiceRecord InvoiceRecord { get; set; }
    public Customer Customer { get; set; }
  }
}
