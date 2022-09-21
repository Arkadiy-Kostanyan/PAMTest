using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PAM.Core.AgreementAggregate;
using PAM.Core.AgreementAggregate.Specifications;
using PAM.Infrastructure.Data;
using Xunit;

namespace PAM.IntegrationTests.Data;
public class EfAgreementCRUD : BaseEfRepoTestFixture
{
  EfRepository<ProductGroup> _grepository;
  EfRepository<Product> _prepository;
  EfRepository<Agreement> _repository;
  Product _product = new Product();
  ProductGroup _group = new ProductGroup();
  string _userId = "User1";

  public EfAgreementCRUD()
  {
    _grepository = new EfRepository<ProductGroup>(GetDBContext());
    _prepository = new EfRepository<Product>(GetDBContext());
    _repository = new EfRepository<Agreement>(GetDBContext());

    _group.GroupCode = "G1Code";
    _grepository.AddAsync(_group).GetAwaiter().GetResult();

    _group = _grepository.ListAsync().GetAwaiter().GetResult()
                    .FirstOrDefault();

    _product.ProductNumber = "P1Number";
    _product.ProductGroup = _group;
    _product.Price = 10m;
    _prepository.AddAsync(_product).GetAwaiter().GetResult();

    _product = _prepository.ListAsync().GetAwaiter().GetResult()
                    .FirstOrDefault();

    
  }

  private Agreement InitAgreement()
  {
    var agreement = new Agreement();
    agreement.UserId = _userId;
    agreement.SetProduct(_product);
    agreement.EffectiveDate = DateTime.Now;
    agreement.ExpirationDate = DateTime.Now.AddDays(30);
    agreement.NewPrice = 20m;
    agreement.Active = true;

    return agreement;
  }

  [Fact]
  public async Task AddsAgreementAndSetsId()
  {

    var agreement = InitAgreement();
    await _repository.AddAsync(agreement);
    // detach the item so we get a different instance
    _dbContext.Entry(agreement).State = EntityState.Detached;


    var newAgreement = (await _repository.ListAsync(new AgreementWithProductsAndGroupsSpec(_userId)))
                    .FirstOrDefault();

    Assert.Equal(agreement.UserId, newAgreement?.UserId);
    Assert.Equal(agreement.Product.ProductNumber, newAgreement?.Product.ProductNumber);
    Assert.Equal(agreement.Product.ProductGroup.GroupCode, newAgreement?.Product.ProductGroup.GroupCode);
    Assert.True(newAgreement?.Id > 0);
  }

  [Fact]
  public async Task DeletesAdreementAfterAddingIt()
  {
    // add an Agreement
    var agreement = InitAgreement();
    await _repository.AddAsync(agreement);

    // delete the item
    await _repository.DeleteAsync(agreement);

    // verify it's no longer there
    Assert.DoesNotContain(await _repository.ListAsync(),
        a => a.NewPrice == agreement.NewPrice);
  }

  [Fact]
  public async Task UpdatesAgreementAfterAddingIt()
  {
    // add an Agreement
    var agreement = InitAgreement();
    await _repository.AddAsync(agreement);

    // detach the item so we get a different instance
    _dbContext.Entry(agreement).State = EntityState.Detached;

    // fetch the item and update its NewPrice
    var newAgreement = (await _repository.ListAsync())
        .FirstOrDefault(a => a.NewPrice == agreement.NewPrice);
    if (newAgreement == null)
    {
      Assert.NotNull(newAgreement);
      return;
    }
    
    Assert.NotSame(agreement, newAgreement);
    decimal newprice = 300m;
    newAgreement.NewPrice = newprice;

    // Update the item
    await _repository.UpdateAsync(newAgreement);

    // Fetch the updated item
    var updatedItem = (await _repository.ListAsync())
        .FirstOrDefault(a => a.NewPrice == newprice);

    Assert.NotNull(updatedItem);
    Assert.NotEqual(agreement.NewPrice, updatedItem?.NewPrice);
    Assert.Equal(agreement.ExpirationDate, updatedItem?.ExpirationDate);
    Assert.Equal(agreement.Id, updatedItem?.Id);
  }
}

