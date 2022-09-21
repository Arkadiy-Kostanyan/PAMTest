using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Ardalis.Specification;

namespace PAM.Core.AgreementAggregate.Specifications;
public class AgreementWithProductsAndGroupsSpec : Specification<Agreement>
{
  public AgreementWithProductsAndGroupsSpec(string userId)
{
  Query
      .Where(agreement => agreement.UserId == userId)
      .Include(agreement => agreement.Product).ThenInclude(product => product.ProductGroup);
}
}
