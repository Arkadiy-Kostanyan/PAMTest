using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAM.SharedKernel;
using PAM.SharedKernel.Interfaces;

namespace PAM.Core.AgreementAggregate;
public class Agreement : EntityBase, IAggregateRoot
{
  public string UserId { get; set; }
  public DateTime EffectiveDate { get; set; }
  public DateTime? ExpirationDate { get; set; }
  public decimal ProductPrice { get; private set; }
  public decimal NewPrice { get; set; }
  public Product Product { get; private set; }

  public void SetProduct(Product product)
  {
    if(product is null)
      throw new ArgumentNullException(nameof(product));
    Product = product;
    ProductPrice = product.Price;
  }
}
