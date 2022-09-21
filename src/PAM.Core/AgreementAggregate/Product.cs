using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAM.SharedKernel;
using PAM.SharedKernel.Interfaces;

namespace PAM.Core.AgreementAggregate;
public class Product : EntityBase, IAggregateRoot
{
  public string ProductNumber { get; set; }
  public string? Description { get; set; } = "";
  public decimal Price { get; set; } = 0m;

  public ProductGroup ProductGroup { get; set; }
  public List<Agreement> Agreements { get; set; }
}
