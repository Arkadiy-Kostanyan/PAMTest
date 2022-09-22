using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PAM.Core.AgreementAggregate.Specifications;
public class ProductByIdWithGroupSpec : Specification<Product>, ISingleResultSpecification
{
  public ProductByIdWithGroupSpec(int id)
  {
    Query
        .Where(product => product.Id == id)
        .Include(product => product.ProductGroup);
  }
}
