using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PAM.Core.AgreementAggregate.Specifications;
public class AgreementByIdWithProductAndGroupSpec
 : Specification<Agreement>, ISingleResultSpecification
{
  public AgreementByIdWithProductAndGroupSpec(int id)
  {
    Query
        .Where(agreement => agreement.Id == id)
        .Include(agreement => agreement.Product).ThenInclude(product => product.ProductGroup);
  }
}
