using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAM.Core.AgreementAggregate;
using Xunit;

namespace PAM.UnitTests.Core.AgreementAggregate;
public class Agreement_SetProduct
{
  private Agreement _agreement = new Agreement();

  [Fact]
  public void SetProductPrice()
  {
    decimal price = 20m;

    var _testItem = new Product
    {
      Price = price
    };

    _agreement.SetProduct(_testItem);

    Assert.Equal(_testItem, _agreement.Product);
    Assert.Equal(price, _agreement.ProductPrice);
  }

  [Fact]
  public void ThrowsExceptionGivenNullItem()
  {
#nullable disable
    Action action = () => _agreement.SetProduct(null);
#nullable enable

    var ex = Assert.Throws<ArgumentNullException>(action);
    
    Assert.Equal("product", ex.ParamName);
  }
}
