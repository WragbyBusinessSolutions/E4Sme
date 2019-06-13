using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E4S.Models.AccountInventory
{
  public class Estimate : BaseClass
  {
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string InvoiveId { get; set; }

    public string EstimateTitle { get; set; }
    public string EstimateId { get; set; }
    public int EstimateNo { get; set; }

    public DateTime EstimateDate { get; set; }
    public DateTime ExpireDate { get; set; }

    public string POSO { get; set; }
    public string Subheading { get; set; }
    public string Footer { get; set; }
    public string Memo { get; set; }


    public float Total { get; set; }
    public float GrandTotal { get; set; }
    public string Status { get; set; }

  }
}
