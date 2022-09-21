using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAM.SharedKernel;
using PAM.SharedKernel.Interfaces;

namespace PAM.Core.AgreementAggregate;
public class ProductGroup : EntityBase, IAggregateRoot
{
  public string GroupCode { get; set; }
  public string? Description { get; set; } = "";

  public List<Product> Products { get; set; }
}
