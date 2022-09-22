using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Ardalis.Specification;

namespace PAM.Core.AgreementAggregate.Specifications;
public class ActiveProductsSpec: Specification<Product>
{
  public ActiveProductsSpec()
{
  Query
      .Where(product => product.Active == true)
      .Include(product => product.ProductGroup);
}
}
